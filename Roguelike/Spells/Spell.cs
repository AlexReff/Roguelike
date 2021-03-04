using GoRogue;
using Microsoft.Xna.Framework;
using Roguelike.Attacks;
using Roguelike.Entities;
using Roguelike.Helpers;
using SadConsole;
using SadConsole.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Spells
{
    public class Spell : IHasID
    {
        public uint ID { get; set; }
        public string Name { get; set; }
        public double BaseManaCost { get; set; }
        public double Range { get; set; }
        public List<TargetType> TargetTypes { get; set; }
        /// <summary>
        /// Determines whether this spell can affect 
        /// </summary>
        public Func<Actor, bool> CanHit { get; set; }

        //public Spell(Color foreground, Color background, string name, char glyph, Coord position, int weight = 1, int condition = 100, int width = 1, int height = 1) : base(foreground, background, glyph, position, (int)MapLayer.ITEMS, isWalkable: true, isTransparent: true)
        public Spell()
        {
            ID = Helpers.Helpers.IDGenerator.UseID();
        }
        //public Item(Color foreground, Color background, string name, char glyph, Coord position, int weight = 1, int condition = 100, int width = 1, int height = 1) : base(foreground, background, glyph, position, (int)MapLayer.ITEMS, isWalkable: true, isTransparent: true)

    }
}
