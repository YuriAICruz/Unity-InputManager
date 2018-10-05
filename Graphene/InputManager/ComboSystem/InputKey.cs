using System;

namespace Graphene.InputManager.ComboSystem
{
    [Flags]
    public enum InputKey
    {
        Null = 0,
        Button_A = 2,
        Button_B = 4,
        Button_X = 8,
        Button_Y = 16,
        Button_RB = 32,
        Button_RT = 64,
        Button_RS,
        Button_LB = 128,
        Button_LT = 256,
        Button_LS,
        Button_Start = 512,
        Button_Select = 1024,
        Button_DPad_Up = 2048,
        Button_DPad_Down = 4096,
        Button_DPad_Left = 8192,
        Button_DPad_Right = 16384,
    }
}