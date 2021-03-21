using GoRogue;
using Microsoft.Xna.Framework;
using Roguelike.Interfaces;
using SadConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roguelike.Entities
{
    internal abstract class MyBasicEntity : BasicEntity
    {
        private static Dictionary<uint, MyBasicEntity> _entities;
        public static List<MyBasicEntity> AllEntities { get { return _entities.Values.ToList(); } }
        public static MyBasicEntity GetEntity(uint id)
        {
            if (_entities.ContainsKey(id))
            {
                return _entities[id];
            }

            return null;
        }

        static MyBasicEntity()
        {
            _entities = new Dictionary<uint, MyBasicEntity>();
        }

        private static readonly IDGenerator IDGenerator = new IDGenerator();

        public new uint ID { get; }
        public new GameMap CurrentMap { get { return (GameMap)base.CurrentMap; } }
        public Color ForegroundColor { get; set; }
        public Color BackgroundColor { get; set; }
        public int Glyph { get; set; }

        public double Mass { get; set; }
        public double Volume { get; set; }

        public MyBasicEntity(string name, double mass, double volume, Color foreground, Color background, int glyph, Coord position, int layer, bool isWalkable, bool isTransparent) : this(name, foreground, background, glyph, position, layer, isWalkable, isTransparent)
        {
            Mass = mass;
            Volume = volume;
        }

        public MyBasicEntity(string name, Color foreground, Color background, int glyph, Coord position, int layer, bool isWalkable, bool isTransparent) : this(foreground, background, glyph, position, layer, isWalkable, isTransparent)
        {
            Name = name;
        }

        public MyBasicEntity(Color foreground, Color background, int glyph, Coord position, int layer, bool isWalkable, bool isTransparent) : base(foreground, background, glyph, position, layer, isWalkable, isTransparent)
        {
            ID = IDGenerator.UseID();
            ForegroundColor = foreground;
            BackgroundColor = background;
            Glyph = glyph;
             
            _entities.Add(this.ID, this);
        }

        /// <summary>
        /// Sets the glyph of the first cell of the animation
        /// </summary>
        /// <param name="glyph"></param>
        public void SetGlyph(int glyph)
        {
            this.Animation.Cells[0].Glyph = Glyph = glyph;
        }
    }
}
