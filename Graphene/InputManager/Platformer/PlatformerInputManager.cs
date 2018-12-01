using System;
using System.Collections.Generic;
using Graphene.InputManager.ComboSystem;
using Graphene.Utils;
using UnityEngine;

namespace Graphene.InputManager.Platformer
{
    [Serializable]
    public class PlatformerInputManager : InputManager.InputSystem
    {
        private Coroutine _update;
        public event Action Interact, Pause, Attack, AttackSeq, Jump, Dodge, LockOn, LockOff;

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