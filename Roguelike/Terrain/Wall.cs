using GoRogue;
using Microsoft.Xna.Framework;
using SadConsole;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Terrain
{
    class Wall : BasicTerrain
    {
        public Wall(Color foregroundColor, Color backgroundColor, char glyph, Coord position) : base(foregroundColor, backgroundColor, glyph, position, isWalkable: false, isTransparent: false)
        {
            //
        }
    }
}
