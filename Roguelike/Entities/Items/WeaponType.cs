using GoRogue;
using Microsoft.Xna.Framework;
using Roguelike.Attacks;
using Roguelike.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Entities.Items
{
    public class WeaponType : IHasID
    {
        public uint ID { get; protected set; }
        public string Name { get; protected set; }
        public WeaponBase WeaponBase { get; protected set; }

        protected WeaponType()
        {
            ID = Helpers.Helpers.IDGenerator.UseID();
        }
    }

    public class Weapon : Item
    {
        public Hand EquippedBy { get; set; }
        public WeaponBase WeaponBase { get; protected set; }

        public Weapon(string name, Color foreground, Color background, char glyph) : base(name, foreground, background, glyph)
        {
            //
        }
    }
}
