using Roguelike.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Entities.Interfaces
{
    internal interface IHasBody
    {
        ActorBody Body { get; set; }
    }
}
