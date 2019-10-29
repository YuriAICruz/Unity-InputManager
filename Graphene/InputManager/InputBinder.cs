using System;
using System.Collections.Generic;
using Graphene.InputManager.ComboSystem;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace Graphene.InputManager
{
    [Serializable]
    public class InputBinder
    {
        public List<InputKey> key = new List<InputKey>();
        public List<KeyCode> value = new List<KeyCode>();

        public void AddOrUpdateBind(InputKey key, KeyCode value)
        {
            if (Exist(key))
                this.value[this.key.IndexOf(key)] = value;
            else
            {
                this.key.Add(key);
                this.value.Add(value);
            }
        }

        public bool Exist(InputKey key)
        {
            return this.key.Contains(key);
        }

        public KeyCode Get(InputKey key)
        {
            if (Exist(key))
                return this.value[this.key.IndexOf(key)];

            return KeyCode.None;
        }
    }
}