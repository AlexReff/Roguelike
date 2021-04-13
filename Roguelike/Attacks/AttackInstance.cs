using Roguelike.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Attacks
{
    internal struct AttackInstance
    {
        public DamageType DamageType { get; }
        public double DamageValue { get; }
        public Limb TargetLimb { get; }

        public AttackInstance(DamageType type, double val, Limb limb)
        {
            DamageType = type;
            DamageValue = val;
            TargetLimb = limb;
        }
    }
}
