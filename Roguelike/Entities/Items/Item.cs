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
        private static readonly IDGenerator IDGenerator = new IDGenerator();

        private static Dictionary<uint, Item> _items;
        public static List<Item> AllItems { get { return _items.Values.ToList(); } }
        public static Item GetItem(uint id)
        {
            if (_items.ContainsKey(id))
            {
                return _items[id];
            }

            return null;
        }

        static Item()
        {
            _items = new Dictionary<uint, Item>();
        }


        public new uint ID { get; private set; }

        private int _durability;
        public int Durability {
            get { return _durability; }
            set
            {
                _durability += value;
                if (_durability <= 0)
                    Destroy();
            }
        }
        
        public double Weight { get; set; }

        //public WeaponType WeaponType { get; set; }

        /// <summary>
        /// Item without a Coord position (does not start on the map)
        /// </summary>
        public Item(string name, Color foreground, Color background, char glyph, int weight = 1, int condition = 100) : this(name, foreground, background, glyph, new Coord(0,0), weight, condition)
        {
            Destroy();
        }


        public Item(string name, Color foreground, Color background, char glyph, Coord position, int weight = 1, int durability = 100) : base(name, foreground, background, glyph, position, (int)MapLayer.ITEMS, isWalkable: true, isTransparent: true)
        {
            // assign the object's fields to the parameters set in the constructor
            Animation.CurrentFrame[0].Foreground = foreground;
            Animation.CurrentFrame[0].Background = background;
            Animation.CurrentFrame[0].Glyph = glyph;
            //Weight = weight;
            //Condition = condition;
            Durability = durability;

            //Attacks = new List<AttackSkill>();

            ID = IDGenerator.UseID();

            _items.Add(this.ID, this);
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
