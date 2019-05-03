using UnityEngine;

namespace Graphene.InputManager
{
    [RequireComponent(typeof(InputTester))]
    public class Tester : MonoBehaviour
    {
        private InputTester _input;
        
        private void Start()
        {
            _input = GetComponent<InputTester>();
            _input.debug = true;
            _input.Init();
        }
    }

    class InputTester : InputSystem
    {
        
    }
}