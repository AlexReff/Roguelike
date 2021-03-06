using SadConsole;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System;
using Roguelike.Entities;

namespace Roguelike.Attacks
{
    public enum TargetType
    {
        TargetActor,
        TargetEntity,
        TargetItem,
        TargetGround,
        TargetSelf,
    }

    public class AttackType
    {
        public string Name { get; set; }
        public double DamageMultiplier { get; set; }
        public Action<Actor> DoEffectAttacker { get; set; }
        public Action<Actor> DoEffectTarget { get; set; }
    }

    //public struct AttackInstance : IHasID
    //{
    //    public uint ID { get; set; }
    //    public string Name { get; set; }
    //    public double ManaCost { get; set; }
    //    public double Damage { get; set; }
    //    public double Range { get; set; }

    //    /// <summary>
    //    /// Any action that occurs to the 'caster' as a result of this attack completing
    //    /// </summary>
    //    public Action<Actor> DoEffectCaster { get; set; }

    //    /// <summary>
    //    /// Any (non-numeric-damage) action that occurs to the target
    //    /// </summary>
    //    public Action<Actor> DoEffectTarget { get; set; }

    //    public AttackInstance(string name, double manaCost, double damage, double range, Action<Actor> targetEffect, Action<Actor> casterEffect)
    //    {
    //        ID = Helpers.Helpers.IDGenerator.UseID();
    //        Name = name;
    //        ManaCost = manaCost;
    //        Damage = damage;
    //        Range = range;
    //        DoEffectTarget = targetEffect;
    //        DoEffectCaster = casterEffect;
    //    }
    //}
}
