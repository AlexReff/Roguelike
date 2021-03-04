using GoRogue;
using Microsoft.Xna.Framework;
using Roguelike.Attacks;
using Roguelike.Models;
using Roguelike.Spells;
using SadConsole;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Entities.Monsters
{
    /// <summary>
    /// Represents a typically-hostile 'creature' of some kind, not affiliated with intelligence-recognized races (eg not a human/elf)
    /// Animals typically should be 'Creature's rather than Monsters.
    /// </summary>
    public abstract class Monster : Actor //, IHasBody, IHasVision, ICanAttack, ICastsSpells
    {
        public Monster(Color foreground, Color background, char glyph, Coord position, int layer, bool isWalkable, bool isTransparent) : base(foreground, background, glyph, position, layer, isWalkable, isTransparent)
        {
            //Body = new ActorBody();
            //Attacks = new List<Attack>();
            //Spells = new List<Spell>();
        }

        //public ActorBody Body { get; set; }
        //public int FOVRadius { get; set; }
        //public List<Attack> Attacks { get; set; }
        //public List<Spell> Spells { get; set; }

        //public abstract bool CanAttack(uint attackId, Actor target);

        //public abstract bool CastSpell(uint spellId, Actor target);

        //public abstract bool DoAttack(uint attackId, Actor target);

        //public abstract Attack GetAttack(uint attackId);

        //public abstract Attack GetBestAttack(Actor target);

        //public abstract Spell GetSpell(uint spellId);

        //public abstract Direction GetVisionDirection();
    }
}
