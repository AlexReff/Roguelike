using Roguelike.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Interfaces
{
    internal interface IBehavior
    {
        public bool Act(Actor actor);
    }
}
