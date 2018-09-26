using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Graphene.InputManager.ComboSystem;
using Graphene.Utils;
using UnityEngine;

namespace Graphene.InputManager
{
    [Serializable]
    public abstract class InputSystem
    {
        private Queue<Coroutine> _checkInputRoutine = new Queue<Coroutine>();

        //protected Dictionary<ComboChecker, Action> _comboAssembly;

        public event Action<Vector2> Left_Axis, Right_Axis;

        protected InputData _inputData;

        private Coroutine _update;

        protected bool _blocked;
        private Vector2 _lastDpad;

        protected void EnqueueInput(InputKey input, bool down = true)
        {
            var ipt = new InputEvent()
            {
                input = input,
                down = down
            };

            // KillInputsRoutine();

            _checkInputRoutine.Enqueue(GlobalCoroutineManager.Instance.StartCoroutine(CheckInput(ipt)));
        }

        public virtual void Init()
        {
            _update = GlobalCoroutineManager.Instance.StartCoroutine(Update());

            var path = "Input/InputData";
            _inputData = Resources.Load<InputData>(path);

            if (_inputData == null)
            {
                Debug.LogError("No Input Data file, please create on on 'Resources/" + path + "'\nusing Create 'InputSystem/Combo'");
                throw new NullReferenceException();
            }
        }

        protected virtual void ExecuteCombo(int id)
        {
        }


        private IEnumerator CheckInput(InputEvent input)
        {
            Debug.Log(input);
            foreach (var combo in _inputData.Inputs)
            {
                combo.CheckCombo(input, (res) => Execute(res, combo));

                // yield return new WaitForChangedResult();
            }
            yield return null;
        }

        private void Execute(ComboChecker.State res, ComboChecker combo)
        {
            if (res == ComboChecker.State.Fail) return;

            KillInputsRoutine();

            //if (res == ComboChecker.State.Waiting) return;

            ExecuteCombo(combo.Id);
        }

        private void KillInputsRoutine()
        {
            if (_checkInputRoutine.Count == 0) return;

            for (int i = 0, n = _checkInputRoutine.Count; i < n; i++)
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

        protected IEnumerator Update()
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


//        Button_A = 2,
//        Button_B = 4,
//        Button_X = 8,
//        Button_Y = 16,
//        Button_RB = 32,
//        Button_RT = 64,
//        Button_LB = 128,
//        Button_LT = 256,
//        Button_Start = 512,
//        Button_Select = 1024,
//        Button_DPad_Up = 2048,
//        Button_DPad_Down = 4096,
//        Button_DPad_Left = 8192,
//        Button_DPad_Right = 16384,

        protected virtual void GetInputs()
        {
            if (Input.GetButtonDown("Button_A"))
                EnqueueInput(InputKey.Button_A);
            if (Input.GetButtonUp("Button_A"))
                EnqueueInput(InputKey.Button_A, false);

            if (Input.GetButtonDown("Button_B"))
                EnqueueInput(InputKey.Button_B);
            if (Input.GetButtonUp("Button_B"))
                EnqueueInput(InputKey.Button_B, false);

            if (Input.GetButtonDown("Button_X"))
                EnqueueInput(InputKey.Button_X);
            if (Input.GetButtonUp("Button_X"))
                EnqueueInput(InputKey.Button_X, false);

            if (Input.GetButtonDown("Button_Y"))
                EnqueueInput(InputKey.Button_Y);
            if (Input.GetButtonUp("Button_Y"))
                EnqueueInput(InputKey.Button_Y, false);

            if (Input.GetButtonDown("Button_RB"))
                EnqueueInput(InputKey.Button_RB);
            if (Input.GetButtonUp("Button_RB"))
                EnqueueInput(InputKey.Button_RB, false);

            if (Input.GetButtonDown("Button_RT"))
                EnqueueInput(InputKey.Button_RT);
            if (Input.GetButtonUp("Button_RT"))
                EnqueueInput(InputKey.Button_RT, false);

            if (Input.GetButtonDown("Button_LB"))
                EnqueueInput(InputKey.Button_LB);
            if (Input.GetButtonUp("Button_LB"))
                EnqueueInput(InputKey.Button_LB, false);

            if (Input.GetButtonDown("Button_LT"))
                EnqueueInput(InputKey.Button_LT);
            if (Input.GetButtonUp("Button_LT"))
                EnqueueInput(InputKey.Button_LT, false);

            if (Input.GetButtonDown("Button_Start"))
                EnqueueInput(InputKey.Button_Start);
            if (Input.GetButtonUp("Button_Start"))
                EnqueueInput(InputKey.Button_Start, false);

            if (Input.GetButtonDown("Button_Select"))
                EnqueueInput(InputKey.Button_A);
            if (Input.GetButtonUp("Button_Select"))
                EnqueueInput(InputKey.Button_A, false);

            var dpad = new Vector2(Input.GetAxisRaw("Button_DPad_Horizontal"), Input.GetAxisRaw("Button_DPad_Vertical"));

            if (dpad.x > 0)
            {
                EnqueueInput(InputKey.Button_DPad_Right);
            }
            else if (dpad.x < 0)
            {
                EnqueueInput(InputKey.Button_DPad_Left);
            }
            else
            {
                if (_lastDpad.x > 0)
                {
                    EnqueueInput(InputKey.Button_DPad_Right, false);
                }
                else if (_lastDpad.x < 0)
                {
                    EnqueueInput(InputKey.Button_DPad_Left, false);
                }
            }

            if (dpad.y > 0)
            {
                EnqueueInput(InputKey.Button_DPad_Up);
            }
            else if (dpad.y < 0)
            {
                EnqueueInput(InputKey.Button_DPad_Down);
            }
            else
            {
                if (_lastDpad.y > 0)
                {
                    EnqueueInput(InputKey.Button_DPad_Up, false);
                }
                else if (_lastDpad.y < 0)
                {
                    EnqueueInput(InputKey.Button_DPad_Down, false);
                }
            }

            _lastDpad = dpad;

            if (Left_Axis != null)
            {
                Left_Axis(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));
            }
            if (Right_Axis != null)
            {
                Right_Axis(new Vector2(Input.GetAxisRaw("Right_Stick_Horizontal"), Input.GetAxisRaw("Right_Stick_Vertical")));
            }
        }

        public void OnDestroy()
        {
            if (_update != null)
            {
                GlobalCoroutineManager.Instance.StopCoroutine(_update);
            }
        }
    }
}