using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Models
{
    [Flags]
    public enum XYZRelativeDirection
    {
        Forward  = 1 << 0,
        Backward = 1 << 1,
        Left     = 1 << 2,
        Right    = 1 << 3,
        Up       = 1 << 4,
        Down     = 1 << 5,
    }
}
