using Microsoft.Xna.Framework;
using Roguelike.Maps;
using Roguelike.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using Troschuetz.Random.Generators;

namespace Roguelike
{
    internal class WorldGen
    {
        private WorldMap map;
        public WorldMap Map { get { return map; } }

        private const int SeedWordCount = 3;

        private const int totalRivers = 5;

        //private const float fpNoiseFactor = .031f;
        private const float fpNoiseFactor = .19f;
        private const int fpNoiseRepeats = 3;

        private const float tempNoiseFactor = .04f;

        private const float heightMapFactor = .11f;

        // first-pass heightmap cutoff points
        private const float fpCutoffOcean = .1f;
        private const float fpCutoffLake = .1f;
        //private const float fpCutoffPeak = .58f;
        private const float fpCutoffPeak = .8f;
        private const float fpCutoffSand = .15f;

        private const float cityOneTempMin = 297;
        private const float cityOneTempMax = 303;

        // render cutoff points
        private const float rndrCutoffOcean = fpCutoffLake;
        private const float rndrCutoffPeak = fpCutoffPeak;
        private const float rndrCutoffSand = fpCutoffSand;

        private const float tempMin = 250;
        private const float tempMax = 320;

        private readonly Color GroundBrownColor = new Color(135, 62, 35);
        private readonly Color GroundGreenColor = Color.DarkGreen;

        public string MasterSeed { get; private set; }
        public Dictionary<RNGPools, MyRandom> RNG { get; private set; }

        public WorldGen()
        {
            RNG = new Dictionary<RNGPools, MyRandom>();

            GenerateSeed();

            PopulateRNG();
        }

        public void ProcessMap(SadConsole.Console console)
        {
            FillConsole(console);

            var width = console.Width;
            var height = console.Height;

            map = new WorldMap(width, height);

            var terrainRng = RNG[RNGPools.Terrain];

            SimplexNoise noise = new SimplexNoise(terrainRng.Next().GetHashCode());

            // start with the first pass as the initial values
            //var map.HeightMap = noise.Calc2D(width, height, fpNoiseFactor);
            map.HeightMap = noise.Calc2D(width, height, fpNoiseFactor);

            for (int i = 1; i < fpNoiseRepeats; i++)
            {
                //SimplexNoise nextNoise = new SimplexNoise(terrainRng.Next().GetHashCode());
                noise.Seed = terrainRng.Next().GetHashCode();

                var thisMap = noise.Calc2D(width, height, fpNoiseFactor);

                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        //map.HeightMap[x, y] *= thisMap[x, y];
                        map.HeightMap[x, y] += thisMap[x, y];
                        map.HeightMap[x, y] /= 2;
                    }
                }
            }

            // apply a gradient to remove edges and designate oceans
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    map.HeightMap[x, y] *= ((float)Math.Sin(((float)x / (float)(width - 1)) * Math.PI)) * ((float)Math.Sin(((float)y / (float)(height - 1)) * Math.PI));
                }
            }

            // designate lake and land
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    float hMapVal = map.HeightMap[x, y] / 255;

                    // designate land & lake
                    if (hMapVal > fpCutoffOcean)
                    {
                        map.Regions[x, y].RegionType = RegionType.Land;
                    }
                    else
                    {
                        map.Regions[x, y].RegionType = RegionType.Lake;
                    }
                }
            }

            // turns ocean 'lake's into ocean
            // designate ocean spaces
            map.CarveOcean();

            // remove lakes - will be created later
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (map.Regions[x, y].RegionType == RegionType.Lake)
                    {
                        map.Regions[x, y].RegionType = RegionType.Land;
                    }
                }
            }

            // we now have an ocean map

            // now designate correct(?) heightmap
            noise.Seed = terrainRng.Next().GetHashCode();
            map.HeightMap = noise.Calc2D(width, height, heightMapFactor);

            ////noise.Seed = terrainRng.Next().GetHashCode();
            ////var hm2 = noise.Calc2D(width, height, heightMapFactor);
            //for (int x = 0; x < width; x++)
            //{
            //    for (int y = 0; y < height; y++)
            //    {
            //        float heightParsed = map.HeightMap[x, y] / 255;
            //        //float multParsed = hm2[x, y] / 255;

            //        if (heightParsed > fpCutoffPeak)
            //        {
            //            if (map.Regions[x, y].RegionType != RegionType.Ocean)
            //            {
            //                map.Regions[x, y].RegionType = RegionType.Mountain;
            //            }
            //        }

            //        //heightParsed *= multParsed;

            //        //if (heightParsed <= fpCutoffLake)
            //        //{
            //        //    //map.HeightMap[x, y] = 0;
            //        //    //map.Lakes[x, y] = true;
            //        //    map.Regions[x, y].RegionType = RegionType.Lake;
            //        //}
            //        //else if (map.HeightMap[x, y] < fpCutoffPeak)
            //        //{
            //        //    //map.Land[x, y] = true;
            //        //    map.Regions[x, y].RegionType = RegionType.Land;
            //        //}
            //        //else
            //        //{
            //        //    //map.Mountain[x, y] = true;
            //        //    map.Regions[x, y].RegionType = RegionType.Mountain;
            //        //}
            //    }
            //}

            //int numRivers = 0;
            //int failedTries = 0;
            //int maxFails = 30;
            //while (numRivers < totalRivers && failedTries <= maxFails)
            //{
            //    int x = terrainRng.Next(0, map.Width);
            //    int y = terrainRng.Next(0, map.Height);
            //    if (map.Mountain[x, y] || map.Lakes[x, y])
            //    {
            //        //we found a starting position for a river
            //        River river = map.CreateRiver(new Point(x, y), terrainRng);
            //        //foreach (var step in river.Steps)
            //        //{
            //        //    Draw
            //        //}
            //    }
            //    else
            //    {
            //        //find a new point
            //        failedTries++;
            //        continue;
            //    }
            //}

            //var tempMap = GetTempatureGradient(width, height, tempMin, tempMax);
            map.TempMap = GetTempatureGradient(width, height, 0, 255);

            noise.Seed = terrainRng.Next().GetHashCode();
            //var tempNoise = noise.Calc2D(width, height, .02f);
            var tempNoise = noise.Calc2D(width, height, tempNoiseFactor);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    //0.0-1.0f
                    float clampedValue = tempNoise[x, y] / 255;

                    //0.0-0.2
                    float parsedValue = clampedValue / 5;

                    //.9-1.1
                    parsedValue += 0.9f;

                    //modulate temperature by ~10% via noise
                    map.TempMap[x, y] *= parsedValue;

                    //account for height. higher elevation = colder temperature

                    ////approaching mountain level, we need to start drastically cooling off
                    //var heightVal = map.HeightMap[x, y] / 255f;
                    //if (!map.Ocean[x, y] && heightVal > rndrCutoffPeak - .05)
                    //{
                    //    var over = heightVal - rndrCutoffPeak + .05;
                    //    var range = 1 - rndrCutoffPeak;
                    //    var percentOver = (float)over / (float)range;

                    //    if (percentOver < .1)
                    //    {
                    //        //cool down a little
                    //        //0.0001 - .1
                    //        map.TempMap[x, y] *= (float)(.90 - percentOver);
                    //    }
                    //    else //if (percentOver < .5)
                    //    {
                    //        //cool down a lot
                    //        //.1-.5
                    //        map.TempMap[x, y] *= (float)(.70 - percentOver);
                    //    }
                    //    //else //highest number around .62ish
                    //    //{
                    //    //    //freeze
                    //    //    //tempMap[x, y] *= Math.Max((float).001, (float)(.5 - percentOver));
                    //    //}
                    //}
                }
            }

            // we now have a temperature gradient and a height map

            // find a west-coast space to place the 'first' starting city
            List<Point> validSpawnPoints = new List<Point>();
            for (int y = height * 3 / 4; y < height * 4 / 5; y++)
            {
                for (int x = 1; x < width / 3; x++)
                {
                    // if this is an ocean spot,
                    // or if there is not an ocean spot to the left,
                    // skip this spot
                    if (map.Regions[x, y].RegionType == RegionType.Ocean || map.Regions[x - 1, y].RegionType != RegionType.Ocean)
                    {
                        continue;
                    }

                    // if there is not a land spot to the north, do not use this spot
                    // this is due to the eastern/southern mountain range that must
                    // accompany the starter town
                    if (map.Regions[x, y - 1].RegionType == RegionType.Ocean)
                    {
                        continue;
                    }

                    validSpawnPoints.Add(new Point(x, y));

                    //float floatVal = map.TempMap[x, y] / 255;

                    //var range = tempMax - tempMin;
                    //var temp = floatVal * range + tempMin;
                    //// we found the left-most point
                    //if (temp <= cityOneTempMax && temp >= cityOneTempMin)
                    //{
                    //    //valid point
                    //    validSpawnPoints.Add(new Point(x, y));
                    //}
                }
            }

            // no valid spawn point, search higher on the coast
            if (!validSpawnPoints.Any())
            {
                for (int y = height * 4 / 5; y > height / 2; y--)
                {
                    for (int x = 1; x < width / 4; x++)
                    {
                        // if this is an ocean spot,
                        // or if there is not an ocean spot to the left,
                        // skip this spot
                        if (map.Regions[x, y].RegionType == RegionType.Ocean || map.Regions[x - 1, y].RegionType != RegionType.Ocean)
                        {
                            continue;
                        }

                        // if there is not a land spot to the north, do not use this spot
                        // this is due to the eastern/southern mountain range that must
                        // accompany the starter town
                        if (map.Regions[x, y - 1].RegionType == RegionType.Ocean)
                        {
                            continue;
                        }

                        validSpawnPoints.Add(new Point(x, y));
                    }
                }
            }
            
            if (!validSpawnPoints.Any())
            {
                throw new Exception("Unable to locate any valid tiles for City 1");
            }
            else
            {
                var randSpawn = terrainRng.Next(0, validSpawnPoints.Count - 1);
                map.CityOne = validSpawnPoints[randSpawn];

                // we need to generate a coast-following-mountain-range
                // this will help regulate temperature in the 'primary' city
                // making it more desirable, and safer
                Point belowStarterCity = new Point(map.CityOne.X, map.CityOne.Y + 1);
                map.Regions[belowStarterCity.X, belowStarterCity.Y].RegionType = RegionType.Mountain;

                //carve the mountains below the city leading to the ocean
                Point leftMost = new Point(belowStarterCity.X - 1, belowStarterCity.Y);
                while (leftMost.X >= 0
                    && map.Regions[leftMost.X, leftMost.Y].RegionType != RegionType.Ocean
                    && map.Regions[leftMost.X, leftMost.Y - 1].RegionType != RegionType.Ocean
                    )
                {
                    map.Regions[leftMost.X, leftMost.Y].RegionType = RegionType.Mountain;
                    leftMost = new Point(leftMost.X - 1, leftMost.Y);
                }

                //note: leftMost is now on the ocean tile, not the last land tile

                Point rightStarterCity = new Point(map.CityOne.X + 1, map.CityOne.Y);
                //Point aboveRightStarter = new Point(rightStarterCity.X, rightStarterCity.Y - 1);
                Point belowRightStarter = new Point(rightStarterCity.X, rightStarterCity.Y + 1);

                map.Regions[rightStarterCity.X, rightStarterCity.Y].RegionType = RegionType.Mountain;
                //map.Regions[aboveRightStarter.X, aboveRightStarter.Y].RegionType = RegionType.Mountain;
                map.Regions[belowRightStarter.X, belowRightStarter.Y].RegionType = RegionType.Mountain;
            }

            Draw(console);
            //DrawDebug(console);
            //DrawTemp(console);
        }

        private float[,] GetTempatureGradient(int width, int height, float minTemp, float maxTemp)
        {
            float[,] temps = new float[width, height];

            //var middlePercent = .2f;

            var range = maxTemp - minTemp;
            var step = range / (float)height;

            for (int y = 0; y < height; y++) 
            {
                float temp = minTemp + (y * step);

                //float percent = (y + 1) / (float)height;
                //if (percent < middlePercent || percent > 1 - middlePercent)
                //{
                //    //outside the range of normal
                //}

                if (y == height - 1)
                {
                    //floating point math is wonky
                    temp = maxTemp;
                }
                for (int x = 0; x < width; x++)
                {
                    temps[x, y] = temp;
                }
            }

            return temps;
        }

        public string GenerateSeed()
        {
            // used to grab words at random
            MyRandom _rand = new MyRandom(Guid.NewGuid().GetHashCode());

            var allWords = JSON.Data.Words;
            List<string> words = new List<string>();
            while (words.Count < SeedWordCount)
            {
                var idx = _rand.Next(0, allWords.Count - 1);
                var word = allWords[idx];
                if (!string.IsNullOrEmpty(word) && !words.Contains(word))
                {
                    words.Add(word);
                }
            }

            MasterSeed = string.Join(" ", words);

            return MasterSeed;
        }

        private void DrawMountain(SadConsole.Console console, int x, int y)
        {
            char val = (char)30;
            Color fgColor = GroundBrownColor;
            //Color fgColor = baseColor;
            //if (alpha < .9f)
            //{
            //    fgColor = new Color(baseColor.R, baseColor.G, baseColor.B, alpha);
            //}
            //alpha == > mountainCutoff, so we need to scale it by subtracting the cutoff, then 
            //var newAlpha = (alpha - rndrCutoffPeak) * (1 / (1 - rndrCutoffPeak));
            
            Color bgColor = GroundGreenColor; //new Color(80, 70, 40);
            console.Print(x, y, val.ToString(), fgColor, bgColor);
        }

        private void DrawMountain(SadConsole.Console console, int x, int y, float alpha)
        {
            char val = (char)219;
            Color baseColor = Color.White;
            //Color fgColor = baseColor;
            //if (alpha < .9f)
            //{
            //    fgColor = new Color(baseColor.R, baseColor.G, baseColor.B, alpha);
            //}
            //alpha == > mountainCutoff, so we need to scale it by subtracting the cutoff, then 
            //var newAlpha = (alpha - rndrCutoffPeak) * (1 / (1 - rndrCutoffPeak));
            
            Color fgColor = new Color(baseColor.R, baseColor.G, baseColor.B, alpha);
            Color bgColor = Color.LightGray; //new Color(80, 70, 40);
            console.Print(x, y, val.ToString(), fgColor, bgColor);
        }

        private void DrawSand(SadConsole.Console console, int x, int y, float alpha)
        {
            char val = (char)219;
            Color baseColor = Color.Tan;
            Color fgColor = new Color(baseColor.R, baseColor.G, baseColor.B, alpha * 2);
            //var newAlpha = (alpha - rndrCutoffSand) * (1 / (1 - (rndrCutoffPeak - rndrCutoffSand)));

            //Color fgColor = new Color(baseColor.R, baseColor.G, baseColor.B, newAlpha);
            Color bgColor = new Color(80, 70, 40);
            console.Print(x, y, val.ToString(), fgColor, bgColor);
        }

        private void DrawGround(SadConsole.Console console)
        {
            var noise = new SimplexNoise(RNG[RNGPools.Drawing].Next().GetHashCode());
            var field = noise.Calc2D(console.Width, console.Height, .15f);
            var bubbleField = noise.Calc2D(console.Width, console.Height, .2f);

            for (int x = 0; x < console.Width; x++)
            {
                for (int y = 0; y < console.Height; y++)
                {
                    if (map.Regions[x, y].RegionType == RegionType.Land)
                    {
                        char val = (char)219;
                        Color bgColor = GroundGreenColor;
                        Color baseColor = GroundBrownColor;
                        
                        //0.0-1.0
                        float fVal = field[x, y] / 255;
                        fVal = (fVal * 2 / 3);

                        //if (RNG[RNGPools.Drawing].NextBoolean())
                        //{
                        var nFVal = bubbleField[x, y] / 255;
                        if (nFVal > .7)
                        {
                            val = (char)129;
                        }
                        else if (nFVal > .4)
                        {
                            val = (char)37;
                        }
                        else
                        {
                            val = (char)39;
                        }
                        //}
                        //else
                        //{
                        //    //Color temp = bgColor;
                        //    //bgColor = baseColor;
                        //    //baseColor = temp;
                        //}

                        Color fgColor = new Color(baseColor.R, baseColor.G, baseColor.B, (int)(fVal * 255));

                        console.Print(x, y, val.ToString(), fgColor, bgColor);
                    }
                }
            }
        }

        private void DrawGround(SadConsole.Console console, int x, int y, float alpha)
        {
            char val = (char)219;
            Color baseColor = Color.DarkGreen;
            //Color fgColor = new Color(baseColor.R, baseColor.G, baseColor.B, alpha);
            //var newAlpha = (alpha - rndrCutoffPeak) * (1 / (1 - rndrCutoffPeak));
            var newAlpha = (alpha - rndrCutoffSand) * (1 / (1 - (rndrCutoffPeak - rndrCutoffSand)));

            Color fgColor = new Color(baseColor.R, baseColor.G, baseColor.B, newAlpha);
            Color bgColor = new Color(80, 70, 40);
            console.Print(x, y, val.ToString(), fgColor, bgColor);
        }

        private void DrawRiver(SadConsole.Console console, int x, int y)
        {
            char val = (char)219;
            Color fgColor = Color.DeepSkyBlue;
            //Color fgColor = new Color(baseColor.R, baseColor.G, baseColor.B, 1f);
            //Color bgColor = new Color(80, 70, 40);
            Color bgColor = Color.Transparent;
            
            console.Print(x, y, val.ToString(), fgColor, bgColor);
        }

        private void DrawLake(SadConsole.Console console, int x, int y)
        {
            char val = (char)219;
            Color baseColor = Color.DarkBlue;
            Color fgColor = new Color(baseColor.R, baseColor.G, baseColor.B, 1f);
            //Color bgColor = new Color(80, 70, 40);
            Color bgColor = Color.White;
            console.Print(x, y, val.ToString(), fgColor, bgColor);
        }

        private void DrawOcean(SadConsole.Console console)
        {
            var noise = new SimplexNoise(RNG[RNGPools.Drawing].Next().GetHashCode());
            var field = noise.Calc2D(console.Width, console.Height, .15f);
            var bubbleField = noise.Calc2D(console.Width, console.Height, .2f);

            for (int x = 0; x < console.Width; x++)
            {
                for (int y = 0; y < console.Height; y++)
                {
                    if (map.Regions[x, y].RegionType == RegionType.Ocean)
                    {
                        //char val = (char)156;
                        char val = (char)32;
                        if (RNG[RNGPools.Drawing].NextBoolean())
                        {
                            var nFVal = bubbleField[x, y] / 255;
                            if (nFVal > .66)
                            {
                                val = (char)248;
                            }
                            else if (nFVal > .33)
                            {
                                val = (char)249;
                            }
                            else
                            {
                                val = (char)250;
                            }
                        }
                        Color bgColor = Color.MidnightBlue;
                        Color baseColor = Color.White;

                        //0.0-1.0
                        float fVal = field[x, y] / 255;
                        fVal = (fVal / 2) + .1f;

                        Color fgColor = new Color(baseColor.R, baseColor.G, baseColor.B, fVal);

                        console.Print(x, y, val.ToString(), fgColor, bgColor);
                    }
                }
            }
        }

        private void FillConsole(SadConsole.Console console)
        {
            var fillFg = Color.Aquamarine;
            var fillBg = Color.Aqua;
            //var fillChar = (char)176;
            var fillChar = (char)219;

            console.Fill(fillFg, fillBg, fillChar);
        }

        public void Draw(SadConsole.Console console)
        {
            DrawOcean(console);
            DrawGround(console);

            // draw the result to the screen
            for (int x = 0; x < console.Width; x++)
            {
                for (int y = 0; y < console.Height; y++)
                {
                    Point thisPt = new Point(x, y);
                    if (map.CityOne == thisPt)
                    {
                        continue;
                    }
                    switch (map.Regions[x, y].RegionType)
                    {
                        case RegionType.Lake:
                            DrawLake(console, x, y);
                            break;
                        case RegionType.Mountain:
                            DrawMountain(console, x, y);
                            break;
                        case RegionType.Land:
                            // draw patterned ground
                            // DrawGround(console);

                            //float floatVal = map.HeightMap[x, y] / 255;
                            //if (floatVal < rndrCutoffSand)
                            //{
                            //    DrawSand(console, x, y, floatVal);
                            //}
                            //else if (floatVal > rndrCutoffPeak)
                            //{
                            //    DrawMountain(console, x, y, floatVal);
                            //}
                            //else
                            //{
                            //    DrawGround(console, x, y, floatVal);
                            //}
                            break;
                        case RegionType.Ocean:
                        case RegionType.CityOne:
                        case RegionType.CityTwo:
                        case RegionType.CityThree:
                        default:
                            continue;
                    }
                }
            }

            DrawCities(console);

            //if (map.Rivers.Any())
            //{
            //    foreach (var river in map.Rivers)
            //    {
            //        foreach (var step in river.Steps)
            //        {
            //            DrawRiver(console, step.X, step.Y);
            //        }
            //    }
            //}
        }

        public void DrawOld(SadConsole.Console console)
        {
            DrawOcean(console);

            // draw the result to the screen
            for (int x = 0; x < console.Width; x++)
            {
                for (int y = 0; y < console.Height; y++)
                {
                    Point thisPt = new Point(x, y);
                    if (map.CityOne == thisPt)
                    {
                        continue;
                    }
                    if (map.Regions[x, y].RegionType == RegionType.Ocean)
                    {
                        //DrawOcean(console, x, y);
                        continue;
                    }
                    else if (map.Regions[x, y].RegionType == RegionType.Lake)
                    {
                        DrawLake(console, x, y);
                    }
                    else
                    {
                        float floatVal = map.HeightMap[x, y] / 255;
                        if (floatVal < rndrCutoffSand)
                        {
                            DrawSand(console, x, y, floatVal);
                        }
                        else if (floatVal > rndrCutoffPeak)
                        {
                            DrawMountain(console, x, y, floatVal);
                        }
                        else
                        {
                            DrawGround(console, x, y, floatVal);
                        }
                    }
                }
            }

            DrawCities(console);

            //if (map.Rivers.Any())
            //{
            //    foreach (var river in map.Rivers)
            //    {

            //        foreach (var step in river.Steps)
            //        {
            //            DrawRiver(console, step.X, step.Y);
            //        }
            //    }
            //}
        }

        public void DrawCities(SadConsole.Console console)
        {
            if (map.CityOne == null)
            {
                return;
            }
            char val = (char)197;
            Color baseColor = Color.White;
            //Color fgColor = new Color(baseColor.R, baseColor.G, baseColor.B, 1);
            //Color bgColor = new Color(80, 70, 40);
            Color bgColor = Color.Red;
            console.Print(map.CityOne.X, map.CityOne.Y, val.ToString(), baseColor, bgColor);
        }

        public void DrawDebug(SadConsole.Console console)
        {
            DrawOcean(console);

            // draw the result to the screen
            for (int x = 0; x < console.Width; x++)
            {
                for (int y = 0; y < console.Height; y++)
                {
                    if (map.Regions[x, y].RegionType == RegionType.Ocean)
                    {
                        //DrawOcean(console, x, y);
                        continue;
                    }
                    var point = new Point(x, y);
                    if (map.CityOne == point)
                    {
                        continue;
                    }

                    float floatVal = map.HeightMap[x, y] / 255;

                    // float floatVal = map.HeightMap[x, y] / 255;

                    char val = (char)219;

                    // instead of white on black, make rainbow!
                    Color fgColor = Helpers.HSL2RGB(floatVal, 0.5, 0.5);
                    Color bgColor = Color.White;

                    // label possible temperature spawn points
                    // debugging
                    float tempVal = map.TempMap[x, y] / 255;
                    var range = tempMax - tempMin;
                    var temp = tempVal * range + tempMin;
                    if (x > 0 && temp <= cityOneTempMax && temp >= cityOneTempMin)
                    {
                        if (map.Regions[x - 1, y].RegionType == RegionType.Ocean)
                        {
                            fgColor = Color.Green;
                        }
                    }

                    console.Print(x, y, val.ToString(), fgColor, bgColor);
                }
            }

            DrawCities(console);
        }

        public void DrawTemp(SadConsole.Console console)
        {
            for (int x = 0; x < console.Width; x++)
            {
                for (int y = 0; y < console.Height; y++)
                {
                    float floatVal = map.TempMap[x, y] / 255;

                    var range = tempMax - tempMin;
                    var temp = floatVal * range + tempMin;
                    //float heightVal = map.HeightMap[x, y] / 255;
                    //if (heightVal <= rndrCutoffOcean)
                    //{
                    //    continue;
                    //}
                    char val = (char)219;
                    Color fgColor = Color.Red;
                    Color bgColor = Color.White;
                    //if (floatVal < .1)
                    //{
                    //    fgColor = Color.White;
                    //}
                    //else if (floatVal < .2)
                    //{
                    //    fgColor = Color.LightBlue;
                    //}
                    //else if (floatVal < .3)
                    //{
                    //    fgColor = Color.LightCyan;
                    //}
                    //else if (floatVal < .4)
                    //{
                    //    fgColor = Color.Blue;
                    //}
                    //else if (floatVal < .5)
                    //{
                    //    fgColor = Color.Aqua;
                    //}
                    //else if (floatVal < .6)
                    //{
                    //    fgColor = Color.Yellow;
                    //}
                    //else if (floatVal < .7)
                    //{
                    //    fgColor = Color.Orange;
                    //}
                    //else if (floatVal < .8)
                    //{
                    //    fgColor = Color.DarkOrange;
                    //}
                    //else if (floatVal < .9)
                    //{
                    //    fgColor = Color.OrangeRed;
                    //}

                    if (temp < 273)
                    {
                        fgColor = Color.White;
                    }
                    else if (temp < 282)
                    {
                        fgColor = Color.Blue;
                    }
                    else if (temp < 291)
                    {
                        fgColor = Color.Green;
                    }
                    else if (temp < 300)
                    {
                        fgColor = Color.Yellow;
                    }
                    else if (temp < 305)
                    {
                        fgColor = Color.Orange;
                    }
                    //else if (floatVal < .18)
                    //{
                    //    fgColor = Color.Silver;
                    //}
                    //else if (floatVal < .35)
                    //{
                    //    fgColor = Color.Blue;
                    //}
                    //else if (floatVal < .65)
                    //{
                    //    fgColor = Color.Green;
                    //}
                    //else if (floatVal < .8)
                    //{
                    //    fgColor = Color.Orange;
                    //}

                    //Color baseColor = Color.Red;
                    //Color fgColor = new Color(baseColor.R, baseColor.G, baseColor.B, floatVal);
                    ////Color bgColor = new Color(80, 70, 40);
                    //Color bgColor = Color.Blue;

                    //fgColor = new Color(fgColor.R, fgColor.G, fgColor.B, floatVal);
                    console.Print(x, y, val.ToString(), fgColor, bgColor);
                }
            }
        }

        public void ProcessMap_orig(SadConsole.Console console)
        {
            FillConsole(console);

            //var thisRnd = RNG[RNGPools.Master]
            var terrainRng = RNG[RNGPools.Terrain];

            SimplexNoise noise = new SimplexNoise(terrainRng.Next().GetHashCode());

            var worldField = noise.Calc2D(console.Width, console.Height, .2f);
            
            int border = 1;

            float[,] squareGradient = Helpers.GenerateSquareGradient(console.Width, console.Height);

            float[,] groundSpaces = new float[console.Width, console.Height];

            // fill in base ground rectangle
            for (int x = border; x < console.Width - border; x++)
            {
                for (int y = border; y < console.Height - border; y++)
                {
                    var floatVal = worldField[x, y] / 255;
                    groundSpaces[x, y] = Math.Max(0, floatVal - squareGradient[x, y]);
                }
            }

            //// draw the result to the screen
            //for (int x = 0; x < console.Width; x++)
            //{
            //    for (int y = 0; y < console.Height; y++)
            //    {
            //        //DrawGround(console, x, y, groundSpaces[x, y]);
            //        if (groundSpaces[x, y] < 0.05f)
            //        {
            //            DrawOcean(console, x, y);
            //        }
            //        else
            //        {
            //            DrawGround(console, x, y, groundSpaces[x, y]);
            //        }
            //    }
            //}

            bool[,] land = new bool[console.Width, console.Height];

            // designate land vs ocean
            for (int x = 0; x < console.Width; x++)
            {
                for (int y = 0; y < console.Height; y++)
                {
                    if (groundSpaces[x, y] < 0.05f)
                    {
                        //ocean
                        land[x, y] = false;
                    }
                    else
                    {
                        land[x, y] = true;
                    }
                }
            }

            // heightmap noise
            //SimplexNoise heightNoise = new SimplexNoise(terrainRng.Next().GetHashCode());
            //var islandHeight = heightNoise.Calc2D(console.Width, console.Height, .09f);
            noise.Seed = terrainRng.Next().GetHashCode();
            var islandHeight = noise.Calc2D(console.Width, console.Height, .09f);

            //HashSet<Point> MountainPeaks = new HashSet<Point>();
            //bool[,] mountains = new bool[console.Width, console.Height];
            //for (int x = 0; x < console.Width; x++)
            //{
            //    for (int y = 0; y < console.Height; y++)
            //    {
            //        if (land[x, y])
            //        {
            //            var floatVal = islandHeight[x, y] / 255;
            //            if (floatVal > peakCutoff)
            //            {
            //                //MountainPeaks.Add(new Point(x, y));
            //                //mountains[x, y] = true;
            //            }
            //        }
            //    }
            //}

            Draw(console);

            //// create a non-ocean rectangle
            //float islandEdgePercent = .08f;
            //int leftCutoff = (int)Math.Floor(islandEdgePercent * console.Width);
            //int rightCutoff = console.Width - leftCutoff;// (int)Math.Floor(islandEdgePercent * console.Width);
            //int topCutoff = (int)Math.Floor(islandEdgePercent * console.Height);
            //int btmCutoff = console.Height - topCutoff; // (int)Math.Floor(islandEdgePercent * console.Width);

            //for (int x = leftCutoff; x <= rightCutoff; x++)
            //{
            //    for (int y = topCutoff; y <= btmCutoff; y++)
            //    {
            //        groundSpaces[x, y] = 1;
            //        DrawGround(console, x, y, 1);
            //    }
            //}

            //// cut corners off the edges
            //int squareSize = 3;
            //for (int x = 0; x < squareSize; x++)
            //{
            //    for (int y = 0; y < squareSize; y++)
            //    {
            //        var left = x + border;
            //        var right = console.Width - border - x - 1;
            //        var top = y + border;
            //        var bottom = console.Height - border - y - 1;

            //        groundSpaces[left, top] = 0;
            //        console.Print(left, top, fillChar.ToString(), fillFg, fillBg);

            //        groundSpaces[right, top] = 0;
            //        console.Print(right, top, fillChar.ToString(), fillFg, fillBg);

            //        //groundSpaces[left, bottom] = 0;
            //        //console.Print(left, bottom, fillChar.ToString(), fillFg, fillBg);

            //        groundSpaces[right, bottom] = 0;
            //        console.Print(right, bottom, fillChar.ToString(), fillFg, fillBg);
            //    }
            //}

            //float oceanCutoff = .1f;
            //for (int x = 0; x < console.Width; x++)
            //{
            //    for (int y = 0; y < console.Height; y++)
            //    {
            //        if (groundSpaces[x, y] < oceanCutoff)
            //        {
            //            groundSpaces[x, y] = 0;
            //            //DrawOcean(console, x, y);
            //        }
            //    }
            //}
        }

        private void PopulateRNG()
        {
            RNG.Clear();
            var pools = Enum.GetValues(typeof(RNGPools));
            var masterRng = new MyRandom(MasterSeed.GetHashCode());
            foreach (var poolObj in pools)
            {
                var pool = (RNGPools)poolObj;
                if (pool == RNGPools.Master)
                {
                    RNG.Add(pool, masterRng);
                }
                else
                {
                    var seed = masterRng.Next().GetHashCode();
                    var rng = new MyRandom(seed);

                    RNG.Add(pool, rng);
                }
            }
        }

    }

    internal enum RNGPools
    {
        Master,
        Terrain,
        Drawing,
    }
}
