using GoRogue;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Entities.Items
{
    public class Currency : Item
    {
        public double Amount { get; protected set; }

        public Currency(double amount, Coord position) : base("Gold", Color.Gold, Color.Transparent, '$', position)
        {
            Amount = amount;
        }
    }
}
