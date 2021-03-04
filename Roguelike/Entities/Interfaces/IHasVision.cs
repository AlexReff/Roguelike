using GoRogue;
using Roguelike.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Entities.Interfaces
{
    public interface IHasVision
    {
        double FOVRadius { get; set; }
        XYZRelativeDirection VisionDirection { get; set; }
    }
}
