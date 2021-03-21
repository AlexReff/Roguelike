using GoRogue;
using Microsoft.Xna.Framework;
using Roguelike.Models;
using Roguelike.JSON;
using Roguelike.Weapons;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Entities.Items
{
    internal class Weapon : Item
    {
        public Hand EquippedBy { get; set; }
        public Materials Material { get; set; }
        //public WeaponFamily WeaponType { get; set; }
        public WeaponQuality WeaponQuality { get; set; }
        public double Length { get; set; }

        public Weapon(string name, Color foreground, Color background, char glyph) : base(name, foreground, background, glyph)
        {
            //
        }

        public Weapon(string name, Color foreground, Color background, char glyph, Coord position) : base(name, foreground, background, glyph, position)
        {
            //
        }
    }
}
