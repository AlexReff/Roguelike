using GoRogue;
using GoRogue.MapViews;
using Microsoft.Xna.Framework;
using SadConsole;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Entities
{
    class Wall : BasicTerrain
    {
        private static readonly int[] WallCharacters =
        {
            //Bits are: WSEN
            206, //0000 island
            182, //0001                 north
            205, //0010            east
            200, //0011            east north
            186, //0100      south
            182, //0101      south      north
            213, //0110      south east
            204, //0111      south east north
            205, //1000 west
            189, //1001 west            north
            205, //1010 west       east
            208, //1011 west       east north
            183, //1100 west south
            185, //1101 west south      north
            203, //1110 west south east
            206, //1111 west south east north
        };

        public Wall(ArrayMap<bool> map, Coord position) : base(Color.White, Color.Black, (char)219, position, isWalkable: false, isTransparent: false)
        {
            int wallCharIndex = 0;
            Coord northNeighbor = new Coord(position.X, position.Y - 1);
            if (map.Contains(northNeighbor))
            {
                if (!map[northNeighbor])
                {
                    if (CanBeSeenByPlayer(map, northNeighbor))
                    {
                        //North neighbor exists
                        wallCharIndex += 1;
                    }
                }
            }
            Coord eastNeighbor = new Coord(position.X + 1, position.Y);
            if (map.Contains(eastNeighbor))
            {
                if (!map[eastNeighbor])
                {
                    if (CanBeSeenByPlayer(map, eastNeighbor))
                    {
                        //East neighbor exists
                        wallCharIndex += 2;
                    }
                }
            }
            Coord southNeighbor = new Coord(position.X, position.Y + 1);
            if (map.Contains(southNeighbor))
            {
                if (!map[southNeighbor])
                {
                    if (CanBeSeenByPlayer(map, southNeighbor))
                    {
                        //South neighbor exists
                        wallCharIndex += 4;
                    }
                }
            }
            Coord westNeighbor = new Coord(position.X - 1, position.Y);
            if (map.Contains(westNeighbor))
            {
                if (!map[westNeighbor])
                {
                    if (CanBeSeenByPlayer(map, westNeighbor))
                    {
                        //West neighbor exists
                        wallCharIndex += 8;
                    }
                }
            }

            Glyph = WallCharacters[wallCharIndex];
        }

        public static bool CanBeSeenByPlayer(ArrayMap<bool> map, Coord position)
        {
            if (map.Contains(position) && map[position])
            {
                return true;
            }

            if (map.Contains(position.X, position.Y + 1))
            {
                if (map[position.X, position.Y + 1])
                {
                    return true;
                }
            }
            if (map.Contains(position.X + 1, position.Y))
            {
                if (map[position.X + 1, position.Y])
                {
                    return true;
                }
            }
            if (map.Contains(position.X, position.Y - 1))
            {
                if (map[position.X, position.Y - 1])
                {
                    return true;
                }
            }
            if (map.Contains(position.X - 1, position.Y))
            {
                if (map[position.X - 1, position.Y])
                {
                    return true;
                }
            }
            if (map.Contains(position.X + 1, position.Y + 1))
            {
                if (map[position.X + 1, position.Y + 1])
                {
                    return true;
                }
            }
            if (map.Contains(position.X + 1, position.Y - 1))
            {
                if (map[position.X + 1, position.Y - 1])
                {
                    return true;
                }
            }
            if (map.Contains(position.X - 1, position.Y + 1))
            {
                if (map[position.X - 1, position.Y + 1])
                {
                    return true;
                }
            }
            if (map.Contains(position.X - 1, position.Y + 1))
            {
                if (map[position.X - 1, position.Y + 1])
                {
                    return true;
                }
            }
            if (map.Contains(position.X - 1, position.Y - 1))
            {
                if (map[position.X - 1, position.Y - 1])
                {
                    return true;
                }
            }

            return false;
        }
    }
}

