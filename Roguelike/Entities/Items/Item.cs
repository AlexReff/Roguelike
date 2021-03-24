using GoRogue;
using Microsoft.Xna.Framework;
using SadConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roguelike.Entities.Items
{
    /// <summary>
    /// Represents an item sitting on the ground
    /// </summary>
    internal class Item : MyBasicEntity
    {
        // static
        //private static Dictionary<long, Item> _items;
        //public static List<Item> AllItems { get { return _items.Values.ToList(); } }
        //public static Item GetItem(long id)
        //{
        //    if (_items.ContainsKey(id))
        //    {
        //        return _items[id];
        //    }

        //    return null;
        //}

        //static Item()
        //{
        //    _items = new Dictionary<long, Item>();
        //}

        // non-static

        private int _durability;
        public int Durability {
            get { return _durability; }
            set
            {
                _durability = value;
                if (_durability <= 0)
                    Destroy();
            }
        }
        
        /// <summary>
        /// Specific to backpack-carry weight, not necessarily 'mass' or kg
        /// </summary>
        public double Weight { get; set; }

        /// <summary>
        /// Whether or not this item can be dropped on the ground, or if it just gets destroyed
        /// </summary>
        public bool IsDroppable { get; }

        /// <summary>
        /// Item without a Coord position (does not start on the map)
        /// </summary>
        public Item(string name, Color foreground, Color background, char glyph, int weight = 1, int condition = 100) : this(name, foreground, background, glyph, new Coord(0,0), weight, condition)
        {
            Destroy();
        }

        /// <summary>
        /// Create an item with a map position
        /// </summary>
        public Item(string name, Color foreground, Color background, char glyph, Coord position, double weight = 1, int durability = 100) : base(name, foreground, background, glyph, position, (int)MapLayer.ITEMS, isWalkable: true, isTransparent: true)
        {
            // assign the object's fields to the parameters set in the constructor
            Animation.CurrentFrame[0].Foreground = foreground;
            Animation.CurrentFrame[0].Background = background;
            Animation.CurrentFrame[0].Glyph = glyph;

            Durability = durability;
            Weight = weight;
            IsDroppable = true;

            //_items.Add(this.ID, this);
        }

        /// <summary>
        /// Remove this Entity from the CurrentMap (eg character picked up this item)
        /// </summary>
        public virtual void Destroy()
        {
            if (this.CurrentMap != null)
            {
                this.CurrentMap.RemoveEntity(this);
            }
        }
    }
}
