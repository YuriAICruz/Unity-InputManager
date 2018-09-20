using System;

namespace Graphene.InputManager.ComboSystem
{
    [Serializable]
    public class InputEvent
    {
        public InputKey input;
        public float betweenMaxTime = 1.2f;
        public float betweenMinTime = 0.2f;
        public bool down;
        public bool hold;
        public float holdTime;
    }
}