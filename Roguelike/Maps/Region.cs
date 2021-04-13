using GoRogue.MapViews;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Maps
{
    internal class Region
    {
        /// <summary>
        /// Number of Zones
        /// </summary>
        public static readonly int RegionWidth = 16;
        /// <summary>
        /// Number of Zones
        /// </summary>
        public static readonly int RegionHeight = 16;

        // currently represents a 64x64 grid of tiles
        //public ArrayMap2D<Zone> Zones { get; }
        public Zone[,] Zones { get; set; }

        public RegionType RegionType { get; set; }
        public float HeightMap { get; set; }
        public float Temperature { get; set; }

        public Region(bool isOcean, bool isMountain, bool isLand, float heightVal, float tempVal)
        {
            //Zones = new ArrayMap2D<Zone>(RegionWidth, RegionHeight);
            Zones = new Zone[RegionWidth, RegionHeight];

            HeightMap = heightVal;
            Temperature = tempVal;

            if (isOcean)
            {
                RegionType = RegionType.Ocean;
            }
            else if (isMountain)
            {
                RegionType = RegionType.Mountain;
            }
            else if (isLand)
            {
                RegionType = RegionType.Land;
            }
        }
    }

    internal enum RegionType
    {
        Ocean,
        Land,
        Mountain,
    }
}
