using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Maps
{
    internal class River
    {
        public List<Point> Steps { get; set; }

        public River()
        {
            Steps = new List<Point>();
        }

        public River(Point start) : this()
        {
            Steps.Add(start);
        }
    }
}
