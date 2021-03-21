using GoRogue;
using Microsoft.Xna.Framework;
using SadConsole;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Entities
{
    class Floor : BasicTerrain
    {
        private static readonly int[] FloorCharacters = { 0, 39, 46 };

        public Floor(Coord position) : base(new Color(211, 211, 211, 75), Color.Black, ' ', position, isWalkable: true, isTransparent: true)
        {
            var floorChar = (int)System.Math.Floor(Helpers.RandomGenerator.NextDouble() * FloorCharacters.Length);
            Glyph = FloorCharacters[floorChar];
        }
    }
}
