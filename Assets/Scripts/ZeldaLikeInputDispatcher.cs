using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Debuging;
using Shooter.InputManage.ComboSystem;
using UnityEngine;
using Utils;

namespace Shooter.InputManage
{
    public class ZeldaLikeInputDispatcher
    {
        public event Action<Vector2> RightStick;
        private Vector2 _rightStickDirection;
        public event Action<Vector2> LeftStick;
        private Vector2 _leftStickDirection;

        public event Action OverheadSlash;
        public event Action RisingSlash;
        public event Action ChargedSlash;
        public event Action WideSlash;

        public event Action TrueChargedSlashCombo;
        public event Action TrueChargedSlashComboFinal;
        public event Action FowardLungingAttackCombo;
        public event Action FowardLungingAttackComboFinal;
        public event Action StationaryCombo;
        public event Action StationaryComboFinal;
        
        public event Action MapToggle;

        public event Action<bool> Deffend;
        public event Action Dodge;
        public event Action Interact;
        public bool IsRunning;
        public bool IsDeffending;

        private Dictionary<ComboChecker, Action> _combos;

        private Thread _inputThread;
        private bool _blocked;

        private Coroutine _charge;


        private Queue<Coroutine> _checkInputRoutine = new Queue<Coroutine>();

        public ZeldaLikeInputDispatcher(MonoBehaviour mono)
        {
            // OverheadSlash = Attack
            // ChargedSlash = Charge Attack 

            // TrueChargedSlashCombo = Attack >> Direction + Attack >> Direction + Attack

            _combos = new Dictionary<ComboChecker, Action>()
            {
                // Dodge
                {
                    new ComboChecker(
                        new[]
                        {
                            new InputEvent()
                            {
                                down = false,
                                input = InputKey.Dodge
                            },
                        }
                    ),
                    () =>
                    {
                        if (Dodge != null) Dodge();
                    }
                },
                

                // -- Charged Attacks -- 

                // ChargedSlash
                {
                    new ComboChecker(
                        new[]
                        {
                            new InputEvent()
                            {
                                down = true,
                                hold = true,
                                holdTime = 0.6f,
                                input = InputKey.Attack
                            }
                        }
                    ),
                    () =>
                    {
                        if (ChargedSlash != null) ChargedSlash();
                    }
                },

                // -- Combos --
                
                // TrueChargedSlashComboFinal
                {
                    new ComboChecker(
                        new[]
                        {
                            new InputEvent()
                            {
                                down = false,
                                input = InputKey.Attack
                            },
                            new InputEvent()
                            {
                                down = false,
                                input = InputKey.Attack,
                                betweenMinTime = 0.3f,
                                betweenMaxTime = 0.6f
                            },
                            new InputEvent()
                            {
                                down = false,
                                input = InputKey.Attack,
                                betweenMinTime = 0.25f,
                                betweenMaxTime = 0.6f
                            }
                        }
                    ),
                    () =>
                    {
                        if (TrueChargedSlashComboFinal != null) TrueChargedSlashComboFinal();
                    }
                },

                // TrueChargedSlashCombo
                {
                    new ComboChecker(
                        new[]
                        {
                            new InputEvent()
                            {
                                down = false,
                                input = InputKey.Attack
                            },
                            new InputEvent()
                            {
                                down = false,
                                input = InputKey.Attack,
                                betweenMinTime = 0.3f,
                                betweenMaxTime = 0.6f
                            }
                        }
                    ),
                    () =>
                    {
                        if (TrueChargedSlashCombo != null) TrueChargedSlashCombo();
                    }
                },

                // -- Single Attacks -- 

                // OverheadSlash
                {
                    new ComboChecker(
                        new[]
                        {
                            new InputEvent()
                            {
                                down = false,
                                input = InputKey.Attack,
                                betweenMinTime = 1.5f
                            },
                        }
                    ),
                    () =>
                    {
                        if (OverheadSlash != null) OverheadSlash();
                    }
                }
            };

            mono.StartCoroutine(Update());
        }

        private void GetInputs()
        {
            if (Input.GetButtonDown("Fire1"))
                EnqueueInput(InputKey.Attack);
            if (Input.GetButtonUp("Fire1"))
                EnqueueInput(InputKey.Attack, false);

            if (Input.GetButtonDown("Fire2"))
                EnqueueInput(InputKey.Deffend);
            if (Input.GetButtonUp("Fire2"))
                EnqueueInput(InputKey.Deffend, false);

            if (Input.GetButtonDown("Dodge"))
                EnqueueInput(InputKey.Dodge);
            if (Input.GetButtonUp("Dodge"))
                EnqueueInput(InputKey.Dodge, false);

            if (Input.GetButtonDown("Interact"))
                EnqueueInput(InputKey.Interact);
            if (Input.GetButtonUp("Interact"))
                EnqueueInput(InputKey.Interact, false);

            if (Input.GetButtonDown("Run"))
            {
                EnqueueInput(InputKey.Run);
                IsRunning = true;
            }
            if (Input.GetButtonUp("Run"))
            {
                EnqueueInput(InputKey.Run, false);
                IsRunning = false;
            }
            
            if (Input.GetButtonDown("Map"))
            {
                EnqueueInput(InputKey.Map);
                if (MapToggle != null) MapToggle();
            }
            if (Input.GetButtonUp("Map"))
            {
                EnqueueInput(InputKey.Map, false);
            }
            
            if (Input.GetButtonDown("Deffend"))
            {
                EnqueueInput(InputKey.Deffend);
//                 IsDeffending = true;
//                 if (Deffend != null) Deffend(IsDeffending);
            }
            if (Input.GetButtonUp("Deffend"))
            {
                EnqueueInput(InputKey.Deffend, false);
//                IsDeffending = false;
//                if (Deffend != null) Deffend(IsDeffending);
            }

            _leftStickDirection.x = Input.GetAxis("Horizontal");
            _leftStickDirection.y = Input.GetAxis("Vertical");

            if (LeftStick != null) LeftStick(_leftStickDirection);
        }

        private IEnumerator CheckInput(InputEvent input)
        {
            foreach (var combo in _combos)
            {
                var act = combo.Value;
                combo.Key.CheckCombo(input, (res) =>
                {
                    if (res == ComboChecker.State.Fail) return;
                    
                    KillInputsRoutine();
                    
                    //if (res == ComboChecker.State.Waiting) return;
                    
                    act();
                });

                yield return new WaitForChangedResult();
            }
        }

        private IEnumerator Update()
        {
            while (true)
            {
                if (_blocked)
                {
                    yield return new WaitForChangedResult();
                    continue;
                }

                GetInputs();

                yield return new WaitForChangedResult();
            }
        }

        private void EnqueueInput(InputKey input, bool down = true)
        {
            var ipt = new InputEvent()
            {
                input = input,
                down = down
            };

            // KillInputsRoutine();

            _checkInputRoutine.Enqueue(GlobalCoroutineManager.Instance.StartCoroutine(CheckInput(ipt)));
        }

        private void KillInputsRoutine()
        {
            if (_checkInputRoutine.Count == 0) return;

            for (int i = 0, n= _checkInputRoutine.Count; i < n; i++)
            {
                GlobalCoroutineManager.Instance.StopCoroutine(_checkInputRoutine.Dequeue());
            }
        }

        public void UnblockInputs()
        {
            _blocked = false;
        }

        public void BlockInputs()
        {
            _blocked = true;
        }
    }
}