using System.Collections.Generic;
using GoRogue;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Roguelike.Attacks;
using Roguelike.Entities.Interfaces;
using Roguelike.Helpers;
using Roguelike.Models;
using Roguelike.Spells;
using SadConsole;

namespace Roguelike.Entities.Monsters
{
    internal class Goblin : Monster, IHasBody, IHasVision, ICanAttack
    {
        public ActorBody Body { get; set; }
        public double FOVRadius { get; set; }
        public List<Attack> Attacks { get; set; }
        //public List<Spell> Spells { get; set; }
        public XYZRelativeDirection VisionDirection { get; set; }

        public Goblin(Coord position) : this(position, "Goblin")
        {
            //
        }

        public Goblin(Coord position, string name) : base(MyGame.GameSettings.GoblinGlyphColor, Color.Transparent, 'g', position, (int)MapLayer.MONSTERS, isWalkable: true, isTransparent: true)
        {
            VisionDirection = XYZRelativeDirection.Forward;

            Name = name;
            MaxHealth = Health = 40;
            Mana = 0;
            FOVRadius = 2;

            Strength = 7;
            Agility = 6;
            Stamina = 7;
            Willpower = 5;
            Intelligence = 4;
            Vitae = 0;

            Body = ActorBody.HumanoidBody(MyGame.GameSettings.GoblinGlyphColor);// new ActorBody();
            Attacks = new List<Attack>();
        }

        public bool CanAttack(uint attackId, Actor target)
        {
            return false;
        }

        //public bool CastSpell(uint spellId, Actor target)
        //{
        //    return false;
        //}

        public bool DoAttack(uint attackId, Actor target)
        {
            return false;
        }

        public Attack GetAttack(uint attackId)
        {
            return null;
        }

        public Attack GetBestAttack(Actor target)
        {
            return null;
        }

        //public Spell GetSpell(uint spellId)
        //{
        //    return null;
        //}
    }
}
