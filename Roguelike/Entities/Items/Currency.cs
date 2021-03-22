using GoRogue;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Entities.Items
{
    internal class Currency : Item
    {
        public int Amount { get; set; }

        public Currency(int amount, Coord position) : base("Gold", Color.Gold, Color.Transparent, '$', position)
        {
            Amount = amount;
        }
    }
}
