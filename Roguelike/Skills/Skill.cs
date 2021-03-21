using Roguelike.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Skills
{
    internal class Skill
    {
        public string Name { get; set; }
        
        public virtual double Damage { get; }

        public virtual bool CanUse(Actor caster, Actor target) => true;
    }
}
