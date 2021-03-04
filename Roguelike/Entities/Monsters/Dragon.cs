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
    internal class Dragon : Monster
    {
        public Dragon(Coord position) : this(position, "Dragon")
        {
        }

        public Dragon(Coord position, string name) : base(MyGame.GameSettings.DragonGlyphColor, Color.Black, 'd', position, (int)MapLayer.MONSTERS, isWalkable: true, isTransparent: false)
        {
            Body = ActorBody.HumanoidBody(MyGame.GameSettings.DragonGlyphColor);

            Name = name;
            MaxHealth = 200;
            Health = 200;
            Mana = 100;
            FOVRadius = 4;

            Strength = 20;
            Agility = 7;
            Stamina = 20;
            Willpower = 14;
            Intelligence = 10;
            Vitae = 30;
        }
    }
}
