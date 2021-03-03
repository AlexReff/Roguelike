using Roguelike.Spells;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Entities.Interfaces
{
    public interface ICastsSpells
    {
        List<Spell> Spells { get; set; }

        bool CastSpell(uint spellId, Actor target);
        Spell GetSpell(uint spellId);
    }
}
