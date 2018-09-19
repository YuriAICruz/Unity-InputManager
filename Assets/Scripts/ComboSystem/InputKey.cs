using System;

namespace Shooter.InputManage.ComboSystem
{
    [Flags]
    public enum InputKey
    {
        Null = 0,
        Deffend = 2,
        Dodge = 4,
        Run = 8,
        Interact = 16,
        Attack = 32,
        Map = 64
    }
}