using GoRogue.MapViews;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Maps
{
    internal class Zone
    {
        /// <summary>
        /// Number of Tiles
        /// </summary>
        public static readonly int ZoneWidth = 64;
        /// <summary>
        /// Number of Tiles
        /// </summary>
        public static readonly int ZoneHeight = 64;

        public ArrayMap2D<MapTile> Tiles { get; }

        public Zone()
        {
            Tiles = new ArrayMap2D<MapTile>(ZoneWidth, ZoneHeight);
        }
    }
}
