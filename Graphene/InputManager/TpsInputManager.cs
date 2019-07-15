using System;
using UnityEngine;

namespace Graphene.InputManager
{
    public class TpsInputManager: InputManager.InputSystem
    {
        public event Action Interact, Pause, Attack, AttackSeq, Jump, Dodge, LockOn, LockOff;

        public override void Init()
        {
            base.Init();
        }

        protected override void ExecuteCombo(int id)
        {
            switch (id)
            {
                case 1:
                    LockOn?.Invoke();
                    break;
                case 0:
                    LockOff?.Invoke();
                    break;
                case 2:
                    Interact?.Invoke();
                    break;
                case 3:
                    Pause?.Invoke();
                    break;
                case 4:
                    Jump?.Invoke();
                    break;
                case 5:
                    Dodge?.Invoke();
                    break;
                case 11:
                    Attack?.Invoke();
                    break;
                case 10:
                    AttackSeq?.Invoke();
                    break;
            }
        }
    }
}