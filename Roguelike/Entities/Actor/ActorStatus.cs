using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Entities
{
    internal partial class Actor
    {
        public bool IsVampire { get; set; }
        public bool IsWerewolf { get; set; }
        public bool IsOffBalance { get; set; }
        public bool IsStunned { get; set; }
        public bool IsEnraged { get; set; }
    }
}
