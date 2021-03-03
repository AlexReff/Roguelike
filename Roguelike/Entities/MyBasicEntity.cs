using GoRogue;
using Microsoft.Xna.Framework;
using SadConsole;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Entities
{
    public class MyBasicEntity : BasicEntity
    {
        public MyBasicEntity(Color foreground, Color background, int glyph, Coord position, int layer, bool isWalkable, bool isTransparent) : base(foreground, background, glyph, position, layer, isWalkable, isTransparent)
        {
            //
        }
    }
}
