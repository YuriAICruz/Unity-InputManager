using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Graphene.InputManager.ComboSystem;
using Graphene.Utils;
using UnityEngine;

namespace Graphene.InputManager
{
    public class ZeldaLikeInputDispatcher : InputSystem
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
        
        protected Dictionary<ComboChecker, Action> _comboAssembly;
        
        public event Action MapToggle;

        public event Action<bool> Deffend;
        public event Action Dodge;
        public event Action Interact;
        public bool IsRunning;
        public bool IsDeffending;

        private Coroutine _charge;

        public ZeldaLikeInputDispatcher(MonoBehaviour mono)
        {
            // OverheadSlash = Attack
            // ChargedSlash = Charge Attack 

            // TrueChargedSlashCombo = Attack >> Direction + Attack >> Direction + Attack

            _comboAssembly = new Dictionary<ComboChecker, Action>()
            {
                // Dodge
                {
                    new ComboChecker(
                        new List<InputEvent>
                        {
                            new InputEvent()
                            {
                                down = false,
                                input = InputKey.Button_B
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
                        new List<InputEvent>
                        {
                            new InputEvent()
                            {
                                down = true,
                                hold = true,
                                holdTime = 0.6f,
                                input = InputKey.Button_RB
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
                        new List<InputEvent>
                        {
                            new InputEvent()
                            {
                                down = false,
                                input = InputKey.Button_RB
                            },
                            new InputEvent()
                            {
                                down = false,
                                input = InputKey.Button_RB,
                                betweenMinTime = 0.3f,
                                betweenMaxTime = 0.6f
                            },
                            new InputEvent()
                            {
                                down = false,
                                input = InputKey.Button_RB,
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
                        new List<InputEvent>
                        {
                            new InputEvent()
                            {
                                down = false,
                                input = InputKey.Button_RB
                            },
                            new InputEvent()
                            {
                                down = false,
                                input = InputKey.Button_RB,
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
                        new List<InputEvent>
                        {
                            new InputEvent()
                            {
                                down = false,
                                input = InputKey.Button_RB,
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

        protected override void GetInputs()
        {
            if (Input.GetButtonDown("Fire1"))
                EnqueueInput(InputKey.Button_RB);
            if (Input.GetButtonUp("Fire1"))
                EnqueueInput(InputKey.Button_RB, false);

            if (Input.GetButtonDown("Fire2"))
                EnqueueInput(InputKey.Button_A);
            if (Input.GetButtonUp("Fire2"))
                EnqueueInput(InputKey.Button_A, false);

            if (Input.GetButtonDown("Dodge"))
                EnqueueInput(InputKey.Button_B);
            if (Input.GetButtonUp("Dodge"))
                EnqueueInput(InputKey.Button_B, false);

            if (Input.GetButtonDown("Interact"))
                EnqueueInput(InputKey.Button_Y);
            if (Input.GetButtonUp("Interact"))
                EnqueueInput(InputKey.Button_Y, false);

            if (Input.GetButtonDown("Run"))
            {
                EnqueueInput(InputKey.Button_X);
                IsRunning = true;
            }
            if (Input.GetButtonUp("Run"))
            {
                EnqueueInput(InputKey.Button_X, false);
                IsRunning = false;
            }
            
            if (Input.GetButtonDown("Map"))
            {
                EnqueueInput(InputKey.Button_RT);
                if (MapToggle != null) MapToggle();
            }
            if (Input.GetButtonUp("Map"))
            {
                EnqueueInput(InputKey.Button_RT, false);
            }
            
            if (Input.GetButtonDown("Deffend"))
            {
                EnqueueInput(InputKey.Button_A);
//                 IsDeffending = true;
//                 if (Deffend != null) Deffend(IsDeffending);
            }
            if (Input.GetButtonUp("Deffend"))
            {
                EnqueueInput(InputKey.Button_A, false);
//                IsDeffending = false;
//                if (Deffend != null) Deffend(IsDeffending);
            }

            _leftStickDirection.x = Input.GetAxis("Horizontal");
            _leftStickDirection.y = Input.GetAxis("Vertical");

            if (LeftStick != null) LeftStick(_leftStickDirection);
        }
    }
}