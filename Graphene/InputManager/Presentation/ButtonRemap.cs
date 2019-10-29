using System;
using Graphene.InputManager.ComboSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Graphene.InputManager.Presentation
{
    enum Binding
    {
        None = 0,
        Console = 1,
        Keyboard = 2
    }

    public class ButtonRemap : MonoBehaviour
    {
        public Text title;
        public Text keyConsole;
        public Text keyKeyboard;
        public Button remapConsole;
        public Button remapKeyboard;

        private Binding _binding;

        private bool _listenersAdded;
        private InputBinder _binder;

        private InputKey _key;


        public void Setup(InputKey key, InputData inputData)
        {
            _key = key;
            _binder = inputData.InputBinder;

            UpdateInfo();
            
            if (!_listenersAdded)
            {
                remapConsole.onClick.AddListener(RebindConsoleButton);
                remapKeyboard.onClick.AddListener(RebindKeyboardButton);

                _listenersAdded = true;
            }

            _binding = Binding.None;
        }

        void UpdateInfo()
        {
            title.text = _key.ToString();
            
            keyConsole.gameObject.SetActive(false);
            remapConsole.gameObject.SetActive(false);
            
            keyKeyboard.text = _binder.Get(_key).ToString();
            
            _binding = Binding.None;
        }

        private void RebindConsoleButton()
        {
            keyConsole.text = "Mapping";
            _binding = Binding.Console;
        }

        private void RebindKeyboardButton()
        {
            keyKeyboard.text = "Mapping";
            _binding = Binding.Keyboard;
        }


        void OnGUI()
        {
            if (_binding == Binding.None) return;

            var e = Event.current;
            if (e.isKey)
            {
                Debug.Log("Detected key code: " + e.keyCode);
                MapTo(e.keyCode);
            }
            else if (e.isMouse)
            {
                Debug.Log("Detected Mouse key code: " + e.button + " " + e.mousePosition);
                MapTo(e.keyCode);
            }
        }

        private void MapTo(KeyCode keyCode)
        {
            if (keyCode == KeyCode.None) return;

            switch (_binding)
            {
                case Binding.Console:
                    break;
                case Binding.Keyboard:
                    _binder.AddOrUpdateBind(_key, keyCode);
                    UpdateInfo();
                    break;
            }
        }
    }
}