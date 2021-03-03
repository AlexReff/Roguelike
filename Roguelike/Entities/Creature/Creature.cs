using GoRogue;
using Microsoft.Xna.Framework;
using Roguelike.Attacks;
using Roguelike.Entities.Interfaces;
using Roguelike.Models;
using Roguelike.Spells;
using SadConsole;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Entities.Monsters
{
    public abstract class Creature : Actor
    {
        /// <summary>
        /// Represents a 'critter' or other animal-like being that is not classified as a 'monster' eg deers, squirrels, rats, mice, wolves, turtles
        /// </summary>
        public Creature(Color foreground, Color background, char glyph, Coord position, int layer, bool isWalkable, bool isTransparent) : base(foreground, background, glyph, position, layer, isWalkable, isTransparent)
        {
        }
    }
}
