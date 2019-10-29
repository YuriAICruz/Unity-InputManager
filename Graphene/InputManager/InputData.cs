using System.Collections.Generic;
using Graphene.InputManager.ComboSystem;
using UnityEngine;

namespace Graphene.InputManager
{
    [CreateAssetMenu(fileName = "InputData", menuName = "InputSystem/InputData", order = 1)]
    public class InputData : ScriptableObject
    {
        public List<ComboChecker> Inputs;

        public InputBinder InputBinder;
    }
}