using GoRogue;
using Roguelike.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Attacks
{
    public struct AttackInstance : IHasID
    {
        public uint ID { get; set; }
        public string Name { get; set; }
        public double ManaCost { get; set; }
        public double Damage { get; set; }
        public double Range { get; set; }
        public string LimbTargetName { get; set; }

        /// <summary>
        /// Any action that occurs to the 'caster' as a result of this attack completing
        /// </summary>
        public Action<Actor> DoEffectCaster { get; set; }

        /// <summary>
        /// Any (non-numeric-damage) action that occurs to the target
        /// </summary>
        public Action<Actor> DoEffectTarget { get; set; }

        public AttackInstance(string name, double manaCost, double damage, string targetLimbName, double range, Action<Actor> targetEffect, Action<Actor> casterEffect)
        {
            ID = Helpers.Helpers.IDGenerator.UseID();
            Name = name;
            ManaCost = manaCost;
            Damage = damage;
            Range = range;
            LimbTargetName = targetLimbName;
            DoEffectTarget = targetEffect;
            DoEffectCaster = casterEffect;
        }
    }
}
