using GoRogue;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Entities.Items.Weapons
{
    public class Sword : Item
    {
        public Sword(string name) : base(name, Color.Silver, Color.Transparent, 's')
        {
            
        }

        public Sword(string name, Coord position) : base(name, Color.Silver, Color.Transparent, 's', position)
        {

        }
    }
}
