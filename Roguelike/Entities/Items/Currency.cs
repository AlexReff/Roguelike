using GoRogue;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Entities.Items
{
    internal class Currency : Item
    {
        public double Amount { get; set; }

        public Currency(double amount, Coord position) : base("Gold", Color.Gold, Color.Transparent, '$', position)
        {
            Amount = amount;
        }
    }
}
