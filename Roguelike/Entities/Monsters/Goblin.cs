using System.Collections.Generic;
using GoRogue;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Roguelike.Attacks;
using Roguelike.Helpers;
using Roguelike.Models;
using Roguelike.Spells;
using SadConsole;

namespace Roguelike.Entities.Monsters
{
    internal class Goblin : Monster
    {
        public Goblin(Coord position) : this(position, "Goblin")
        {
            //
        }

        public Goblin(Coord position, string name) : base(MyGame.GameSettings.GoblinGlyphColor, Color.Transparent, 'g', position, (int)MapLayer.MONSTERS, isWalkable: true, isTransparent: true)
        {
            VisionDirection = XYZRelativeDirection.Forward;

            Name = name;
            MaxHealth = 40;
            Health = 40;
            Mana = 0;
            FOVRadius = 2;

            Strength = 7;
            Agility = 6;
            Stamina = 7;
            Willpower = 5;
            Intelligence = 4;
            Vitae = 0;

            Body = ActorBody.HumanoidBody(MyGame.GameSettings.GoblinGlyphColor);
        }
    }
}
