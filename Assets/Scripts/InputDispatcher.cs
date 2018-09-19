using System;
using System.Collections;
using System.IO;
using System.Threading;
using UnityEngine;

namespace Shooter.InputManage
{
    public class InputDispatcher
    {
        public event Action<Vector2> RightStick;
        private Vector2 _rightStickDirection;
        public event Action<Vector2> LeftStick;
        private Vector2 _leftStickDirection;
        public event Action Shoot;
        public event Action Bomb;

        private Thread _inputThread;

        public InputDispatcher(MonoBehaviour mono)
        {
            mono.StartCoroutine(Update());
        }

        void GetInputs()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (Shoot != null) Shoot();
            }
            if (Input.GetButtonDown("Fire2"))
            {
                if (Bomb != null) Bomb();
            }
                
            _leftStickDirection.x = Input.GetAxis("Horizontal");
            _leftStickDirection.y = Input.GetAxis("Vertical");
                
            if (LeftStick != null) LeftStick(_leftStickDirection);
        }

        IEnumerator Update()
        {
            while (true)
            {
                GetInputs();
                yield return new WaitForChangedResult();
            }
        }
        
    }
}