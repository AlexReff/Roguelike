using GoRogue;
using Roguelike.Spells;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Attacks
{
    public class AttackSkill : IHasID
    {
        public uint ID { get; set; }
        public string Name { get; set; }
        public double BaseManaCost { get; set; }
        public List<TargetType> TargetTypes { get; set; }
        public int Range { get; set; }

        public AttackSkill()
        {
            ID = Helpers.Helpers.IDGenerator.UseID();
        }
    }
}
