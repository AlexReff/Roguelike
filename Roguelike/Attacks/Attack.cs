using GoRogue;
using Roguelike.Spells;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Attacks
{
    public class Attack : IHasID
    {
        public uint ID { get; set; }
        public string Name { get; set; }
        public double ManaCost { get; set; }
        public List<TargetType> TargetTypes { get; set; }
        public int Range { get; set; }

        public Attack()
        {
            ID = Helpers.Helpers.IDGenerator.UseID();
        }
    }
}
