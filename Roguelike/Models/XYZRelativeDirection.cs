using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Models
{
    [Flags]
    public enum XYZRelativeDirection
    {
        Forward = 1,
        Backward = 2,
        Left = 4,
        Right = 8,
        Up = 16,
        Down = 32,
    }
}
