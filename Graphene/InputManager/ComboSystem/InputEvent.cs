﻿using System;
using UnityEngine;

namespace Graphene.InputManager.ComboSystem
{
    [Serializable]
    public class InputEvent
    {
        public InputKey input;
        public KeyCode keyboardBind = KeyCode.None;
        public KeyCode consoleBind = KeyCode.None;
        public float betweenMaxTime = 1.2f;
        public float betweenMinTime = 0.2f;
        public bool down;
        public bool hold;
        public float holdTime;

        public override string ToString()
        {
            return "(" + input + " down:" + down + ")";
        }
    }
}