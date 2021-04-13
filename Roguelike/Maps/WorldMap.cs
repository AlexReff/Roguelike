using GoRogue;
using GoRogue.MapViews;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roguelike.Maps
{
    internal class WorldMap
    {
        private const float oceanCutoff = .1f;

        public int Width { get; protected set; }
        public int Height { get; protected set; }

        public Point CityOne { get; set; }

        public bool[,] Ocean { get; set; }
        public bool[,] Land { get; set; }
        public bool[,] Mountain { get; set; }
        public bool[,] Lakes { get; set; }

        public float[,] HeightMap { get; set; }
        public float[,] TempMap { get; set; }

        // WorldMap -> Region -> Zone -> MapTile
        public Region[,] Regions { get; set; }

        public List<River> Rivers { get; set; }


        private HashSet<Point> OceanVisitedPoints { get; set; }
        private HashSet<Point> IslandVisitedPoints { get; set; }

        public WorldMap(int width, int height)
        {
            Width = width;
            Height = height;

            Ocean = new bool[Width, Height];
            Land = new bool[Width, Height];
            Mountain = new bool[Width, Height];
            Lakes = new bool[Width, Height];

            HeightMap = new float[Width, Height];
            TempMap = new float[Width, Height];

            Regions = new Region[Width, Height]; //new ArrayMap2D<Region>(Width, Height);
            Rivers = new List<River>();

            //for (int x = 0; x < Width; x++)
            //{
            //    for (int y = 0; y < Height; y++)
            //    {
            //        Regions[x, y] = new Region(Ocean[x, y], Mountain[x, y], Land[x, y], HeightMap[x, y], TempMap[x, y]);
            //    }
            //}
        }

        public void CarveOcean()
        {
            OceanVisitedPoints = new HashSet<Point>();
            SpreadOcean(0, 0);

            // remove baby islands near edges
            // identify each 'island' and remove all except for the largest (main island)

            // index is islandrefs::value, value is size
            List<int> IslandSizes = new List<int>();
            Dictionary<Point, int> IslandRefs = new Dictionary<Point, int>();
            IslandVisitedPoints = new HashSet<Point>();

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (Ocean[x, y])
                    {
                        continue;
                    }

                    Point thisGroundPoint = new Point(x, y);

                    if (IslandRefs.ContainsKey(thisGroundPoint))
                    {
                        // this cell has already been processed
                        continue;
                    }

                    var newIslandIdx = IslandSizes.Count;
                    IslandSizes.Add(1);

                    //HashSet<Point> visited = new HashSet<Point>();
                    IslandVisitedPoints.Clear();
                    IEnumerable<Point> points = GetIsland(thisGroundPoint);

                    foreach (var point in points)
                    {
                        if (!IslandRefs.ContainsKey(point))
                        {
                            IslandRefs.Add(point, newIslandIdx);
                            IslandSizes[newIslandIdx]++;
                        }
                    }
                }
            }

            int maxSize = IslandSizes.Max();
            int largestIslandIdx = IslandSizes.IndexOf(maxSize);
            foreach (var islandRef in IslandRefs)
            {
                if (islandRef.Value == largestIslandIdx)
                {
                    continue;
                }

                Ocean[islandRef.Key.X, islandRef.Key.Y] = true;
            }
        }

        private IEnumerable<Point> GetIsland(Point point)
        {
            if (IslandVisitedPoints.Contains(point))
            {
                yield break;
            }

            if (Ocean[point.X, point.Y])
            {
                yield break;
            }

            //var points = new List<Point>() { point };
            yield return point;
            
            IslandVisitedPoints.Add(point);

            if (point.X > 0)
            {
                Point left = new Point(point.X - 1, point.Y);
                var leftPoints = GetIsland(left);
                foreach (var pt in leftPoints)
                {
                    yield return pt;
                }
            }
            if (point.Y > 0)
            {
                Point top = new Point(point.X, point.Y - 1);
                var topPoints = GetIsland(top);
                //points.AddRange(topPoints);
                foreach (var pt in topPoints)
                {
                    yield return pt;
                }
            }
            if (point.X < Width - 1)
            {
                Point right = new Point(point.X + 1, point.Y);
                var rightPoints = GetIsland(right);
                //points.AddRange(rightPoints);
                foreach (var pt in rightPoints)
                {
                    yield return pt;
                }
            }
            if (point.Y < Height - 1)
            {
                Point btm = new Point(point.X, point.Y + 1);
                var btmPoints = GetIsland(btm);
                //points.AddRange(btmPoints);
                foreach (var pt in btmPoints)
                {
                    yield return pt;
                }
            }

            yield break;
        }

        private void SpreadOcean(int x, int y)
        {
            var thisPoint = new Point(x, y);
            if (OceanVisitedPoints.Contains(thisPoint))
            {
                return;
            }
            
            OceanVisitedPoints.Add(thisPoint);

            if (HeightMap[x, y] / 255 > oceanCutoff)
            {
                return;
            }

            Ocean[x, y] = true;

            if (x + 1 < Width)
            {
                SpreadOcean(x + 1, y);
            }
            if (y + 1 < Height)
            {
                SpreadOcean(x, y + 1);
            }
            if (x > 0)
            {
                SpreadOcean(x - 1, y);
            }
            if (y > 0)
            {
                SpreadOcean(x, y - 1);
            }
        }


        // currently doesn't work very well for the size of maps we use
        // will have to revisit this at another zoom level to generate decently usable rivers
        public River CreateRiver(Point start, MyRandom rng)
        {
            River river = new River(start);

            Point closest = start;
            float closestPoint = Width * Height;
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (Ocean[x, y] || Lakes[x, y])
                    {
                        var yDiff = Math.Abs(y - start.Y);
                        var xDiff = Math.Abs(x - start.X);
                        float dist = (float)Math.Sqrt((float)Math.Pow(xDiff, 2) + (float)Math.Pow(yDiff, 2));
                        if (dist < closestPoint)
                        {
                            closestPoint = dist;
                            closest = new Point(x, y);
                        }
                    }
                }
            }

            //we now know where the closest body of water is relative to the starting point
            //start at both points and noisily work our way from each
            List<Point> path = new List<Point>();
            Stack<Point> endPoints = new Stack<Point>();
            path.Add(closest);

            Point negPoint = closest;
            Point posPoint = start;

            while (posPoint != negPoint)
            {
                Direction targetDir = Direction.GetDirection(posPoint, negPoint);
                Direction reverseDir = Direction.GetDirection(negPoint, posPoint);

                // starting point
                var rnd = rng.NextDouble();
                if (rnd < .35)
                {
                    // detour one way or the other
                    if (rng.NextBoolean())
                    {
                        //go left
                        targetDir++;
                    }
                    else
                    {
                        //go right
                        targetDir--;
                    }
                }
                Point nextPoint = posPoint + targetDir;
                path.Add(nextPoint);
                posPoint = nextPoint;

                if (nextPoint != negPoint)
                {
                    // ending point
                    var nextRnd = rng.NextDouble();
                    if (nextRnd < .35)
                    {
                        // detour one way or the other
                        if (rng.NextBoolean())
                        {
                            reverseDir++;
                        }
                        else
                        {
                            reverseDir--;
                        }
                    }
                    Point prevPoint = negPoint + reverseDir;
                    endPoints.Push(prevPoint);
                    negPoint = prevPoint;
                }
            }

            while (endPoints.Count > 0)
            {
                river.Steps.Add(endPoints.Pop());
            }

            //int x = terrainRng.Next(0, map.Width);
            //int y = terrainRng.Next(0, map.Height);

            Rivers.Add(river);
            return river;
        }
    }
}
