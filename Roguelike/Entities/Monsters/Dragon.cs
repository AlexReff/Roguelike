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
    internal class Dragon : Monster, IHasBody, IHasVision, ICanAttack, ICastsSpells
    {
        public ActorBody Body { get; set; }
        public int FOVRadius { get; set; }
        public List<Attack> Attacks { get; set; }
        public List<Spell> Spells { get; set; }
        public XYZRelativeDirection VisionDirection { get; set; }

        public Dragon(Coord position) : this(position, "Dragon")
        {
        }

        public Dragon(Coord position, string name) : base(MyGame.GameSettings.DragonGlyphColor, Color.Black, 'd', position, (int)MapLayer.MONSTERS, isWalkable: true, isTransparent: false)
        {
            Body = ActorBody.HumanoidBody(MyGame.GameSettings.DragonGlyphColor);
            Attacks = new List<Attack>();
            Spells = new List<Spell>();
            VisionDirection = XYZRelativeDirection.Forward;

            Name = name;
            MaxHealth = Health = 200;
            Mana = 100;
            FOVRadius = 4;

            Strength = 20;
            Agility = 7;
            Stamina = 20;
            Willpower = 14;
            Intelligence = 10;
            Vitae = 30;
        }

        public bool CanAttack(uint attackId, Actor target)
        {
            return false;
        }

        public bool CastSpell(uint spellId, Actor target)
        {
            return false;
        }

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

        public Spell GetSpell(uint spellId)
        {
            return null;
        }

        public Direction GetVisionDirection()
        {
            return null;
        }
    }
}
