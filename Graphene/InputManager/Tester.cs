using UnityEngine;

namespace Graphene.InputManager
{
    public class Tester : MonoBehaviour
    {
        private InputTester _input;
        
        private void Start()
        {
            _input = new InputTester();
            _input.debug = true;
            _input.Init();
        }
    }

    class InputTester : InputSystem
    {
        
    }
}