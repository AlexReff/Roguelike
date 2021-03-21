using Roguelike.Skills;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Entities
{
    internal partial class Actor
    {
        public List<Skill> Skills { get; set; }

        //public bool CanCastSpell(Skill skill, Actor target)
        //{
        //    //Spell spell = GetSpell(spellId);
        //    //if (spell == null || spell.ID != spellId)
        //    //{
        //    //    DebugManager.Instance.AddMessage(new DebugMessage($"Player::CastSpell: Invalid ID: {spellId}", DebugSource.System));
        //    //    return false;
        //    //}
        //    //if (this.CurrentMap.DistanceMeasurement.Calculate(this.Position, target.Position) < (1 + spell.Range))
        //    //{
        //    //    return true;
        //    //}

        //    return false;
        //}
    }
}
