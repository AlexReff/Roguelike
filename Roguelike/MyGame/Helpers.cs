using GoRogue;
using GoRogue.Random;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Entities;
using Roguelike.Models;
using SadConsole;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Roguelike
{
    internal static class Helpers
    {
        public static readonly SadConsoleRandomGenerator RandomGenerator = new SadConsoleRandomGenerator();

        public static readonly TextInfo enUsText = new CultureInfo("en-US", false).TextInfo;

        public static List<string> GetJsonList(this JsonElement arrayField, string columnName)
        {
            List<string> list = new List<string>();

            if (arrayField.GetArrayLength() > 0)
            {
                var enumer = arrayField.EnumerateArray();
                while (enumer.MoveNext())
                {
                    string val = enumer.Current.GetProperty(columnName).GetString();
                    if (!string.IsNullOrEmpty(val))
                    {
                        list.Add(val);
                    }
                }
            }

            return list;
        }


        // Given H,S,L in range of 0-1
        // Returns a Color (RGB struct) in range of 0-255
        public static Color HSL2RGB(double h, double sl, double l)
        {
            double v;
            double r, g, b;

            r = l;
            g = l;
            b = l;

            v = (l <= 0.5) ? (l * (1.0 + sl)) : (l + sl - l * sl);

            if (v > 0)
            {
                double m;
                double sv;
                int sextant;
                double fract, vsf, mid1, mid2;

                m = l + l - v;
                sv = (v - m) / v;
                h *= 6.0;

                sextant = (int)h;
                fract = h - sextant;
                vsf = v * sv * fract;
                mid1 = m + vsf;
                mid2 = v - vsf;

                switch (sextant)
                {
                    case 0:
                        r = v;
                        g = mid1;
                        b = m;
                        break;
                    case 1:
                        r = mid2;
                        g = v;
                        b = m;
                        break;
                    case 2:
                        r = m;
                        g = v;
                        b = mid1;
                        break;
                    case 3:
                        r = m;
                        g = mid2;
                        b = v;
                        break;
                    case 4:
                        r = mid1;
                        g = m;
                        b = v;
                        break;
                    case 5:
                        r = v;
                        g = m;
                        b = mid2;
                        break;
                }
            }

            Color rgb = new Color(0, 0, 0, 1f);

            rgb.R = Convert.ToByte(r * 255.0f);
            rgb.G = Convert.ToByte(g * 255.0f);
            rgb.B = Convert.ToByte(b * 255.0f);

            return rgb;
        }

        public static float[,] GenerateSquareGradient(int width, int height)
        {
            int halfWidth = width / 2;
            int halfHeight = height / 2;
            float[,] gradient = new float[width,height];

            for (int i = 0; i < width; i++)
            {
                //gradient[i] = new float[height];

                for (int j = 0; j < height; j++)
                {
                    int x = i;
                    int y = j;

                    float val;

                    x = x > halfWidth ? width - x : x;
                    y = y > halfHeight ? height - y : y;

                    int smaller = x < y ? x : y;
                    val = smaller / (float)halfWidth;

                    //colorValue *= colorValue * colorValue;
                    val += val;
                    if (val > 1)
                    {
                        val = 1;
                    }
                    val = 1 - val;
                    gradient[i,j] = val; //new Color(colorValue, colorValue, colorValue);
                }
            }

            return gradient;

            //int width = 500;
            //int height = 500;
            //int halfWidth = width / 2;
            //int halfHeight = height / 2;

            //Color[][] gradient = new Color[width][];

            //for (int i = 0; i < width; i++)
            //{
            //    gradient[i] = new Color[height];

            //    for (int j = 0; j < height; j++)
            //    {
            //        int x = i;
            //        int y = j;

            //        float colorValue;

            //        x = x > halfWidth ? width - x : x;
            //        y = y > halfHeight ? height - y : y;

            //        int smaller = x < y ? x : y;
            //        colorValue = smaller / (float)halfWidth;

            //        colorValue = 1 - colorValue;
            //        colorValue *= colorValue * colorValue;
            //        gradient[i][j] = new Color(colorValue, colorValue, colorValue);
            //    }
            //}

            //return gradient;
        }

        public static float[,] GenerateCustomGradient(int width, int height)
        {
            //int halfWidth = width / 2;
            //int halfHeight = height / 2;
            float[,] gradient = new float[width,height];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    int x = i;
                    int y = j;

                    float val;

                    val = ((float)Math.Sin(((float)x / (float)width) * Math.PI)) * ((float)Math.Sin(((float)y / (float)height) * Math.PI));
                    //val = ((float)Math.Sin(((float)x / (float)height) * Math.PI)) * ((float)Math.Sin(((float)y / (float)width) * Math.PI));
                    //f(x,y) = ((float)Math.Sin(((float)x/(float)WIDTH) * Math.PI) ) * ((float)Math.Sin(((float)y / (float)WIDTH) * Math.PI) )

                    gradient[i, j] = val;

                    //x = x > halfWidth ? width - x : x;
                    //y = y > halfHeight ? height - y : y;

                    //int smaller = x < y ? x : y;
                    //val = smaller / (float)halfWidth;

                    ////colorValue *= colorValue * colorValue;
                    //val += val;
                    //if (val > 1)
                    //{
                    //    val = 1;
                    //}
                    //val = 1 - val;
                    //gradient[i,j] = val; //new Color(colorValue, colorValue, colorValue);
                }
            }

            return gradient;
        }

        public static List<Coord> GetNeighbors(this Coord center)
        {
            List<Coord> vals = new List<Coord>()
            {
                new Coord(center.X    , center.Y + 1),
                new Coord(center.X + 1, center.Y),
                new Coord(center.X + 1, center.Y + 1),
                new Coord(center.X    , center.Y - 1),
                new Coord(center.X - 1, center.Y),
                new Coord(center.X - 1, center.Y - 1),
                new Coord(center.X - 1, center.Y + 1),
                new Coord(center.X + 1, center.Y - 1),
            };
            return vals.Where(m => m.X >= 0 && m.Y >= 0).ToList();
        }

        public static bool IsVowel(char vowel)
        {
            return vowel == 'a'
                | vowel == 'e'
                | vowel == 'i'
                | vowel == 'o'
                | vowel == 'u'
                | vowel == 'A'
                | vowel == 'E'
                | vowel == 'I'
                | vowel == 'O'
                | vowel == 'U'
                ;
        }

        public static string ToProperCase(this string input) {
            return enUsText.ToTitleCase(input);
        }

        public static void DrawBorderBgTitle(this SadConsole.Console console, Microsoft.Xna.Framework.Rectangle area, string title, Color backgroundColor, Color borderColor)
        {
            console.Fill(area, borderColor, backgroundColor, 0, 0);

            for (int x = area.Left + 1; x < area.Right - 1; x++)
            {
                console.SetGlyph(x, area.Top, (char)196); //left-right
                console.SetGlyph(x, area.Bottom - 1, (char)196); //left-right
            }
            for (int y = area.Top + 1; y < area.Bottom - 1; y++)
            {
                console.SetGlyph(area.Left, y, (char)179); //up-down
                console.SetGlyph(area.Right - 1, y, (char)179); //up-down
            }

            console.SetGlyph(area.Left, area.Top, (char)218); //top left
            console.SetGlyph(area.Right - 1, area.Top, (char)191); //top right
            console.SetGlyph(area.Left, area.Bottom - 1, (char)192); //bottom left
            console.SetGlyph(area.Right - 1, area.Bottom - 1, (char)217); //bottom right

            if (!string.IsNullOrEmpty(title))
            {
                string framedTitle = (char)180 + title + (char)195;
                int widthCenterPoint = (int)System.Math.Floor(area.Width / 2.0);
                int center = widthCenterPoint - (int)System.Math.Floor(framedTitle.Length / 2.0);
                console.Print(center, 0, framedTitle, borderColor, backgroundColor);
            }
        }

        /// <summary>
        /// Gets the FOV degree of an entity that has vision
        /// </summary>
        /// <returns>FOV for map where 0 == RIGHT, 90 == DOWN, 270 == UP</returns>
        public static double GetFOVDegree(Actor entity)
        {
            if (entity == null)
            {
                return 0;
            }

            double angle = 0;

            Direction facingDirection = entity.FacingDirection;

            if (facingDirection == Direction.RIGHT)
            {
                angle = 0;
            }
            else if (facingDirection == Direction.DOWN_RIGHT)
            {
                angle = 45;
            }
            else if (facingDirection == Direction.DOWN)
            {
                angle = 90;
            }
            else if (facingDirection == Direction.DOWN_LEFT)
            {
                angle = 135;
            }
            else if (facingDirection == Direction.LEFT)
            {
                angle = 180;
            }
            else if (facingDirection == Direction.UP_LEFT)
            {
                angle = 225;
            }
            else if (facingDirection == Direction.UP)
            {
                angle = 270;
            }
            else if (facingDirection == Direction.UP_RIGHT)
            {
                angle = 315;
            }

            XYZRelativeDirection visionDirection = entity.VisionDirection;

            if (visionDirection.HasFlag(XYZRelativeDirection.Right))
            {
                if (visionDirection.HasFlag(XYZRelativeDirection.Forward))
                {
                    angle += 45;
                }
                else if (visionDirection.HasFlag(XYZRelativeDirection.Backward))
                {
                    angle += 135;
                }
                else
                {
                    angle += 90;
                }
            }
            else if (visionDirection.HasFlag(XYZRelativeDirection.Left))
            {
                if (visionDirection.HasFlag(XYZRelativeDirection.Forward))
                {
                    angle -= 45;
                }
                else if (visionDirection.HasFlag(XYZRelativeDirection.Backward))
                {
                    angle -= 135;
                }
                else
                {
                    angle -= 90;
                }
            }
            //else if (visionDirection.HasFlag(XYZRelativeDirection.Forward))
            //{
            //    angle += 0;
            //}
            else if (visionDirection.HasFlag(XYZRelativeDirection.Backward))
            {
                angle += 180;
            }


            return angle;
        }

        public static System.Action<T> Debounce<T>(this System.Action<T> func, int milliseconds = 300)
        {
            CancellationTokenSource? cancelTokenSource = null;

            return arg =>
            {
                cancelTokenSource?.Cancel();
                cancelTokenSource = new CancellationTokenSource();

                Task.Delay(milliseconds, cancelTokenSource.Token)
                    .ContinueWith(t =>
                    {
                        if (t.IsCompletedSuccessfully)
                        {
                            func(arg);
                        }
                    }, TaskScheduler.Default);
            };
        }

        public static System.Action<T> DebounceKeyHandler<T>(this System.Action<T> func, int milliseconds = 300) where T : struct
        {
            T? lastValue = null;
            CancellationTokenSource? cancelTokenSource = null;

            return arg =>
            {
                if (lastValue != null && lastValue.HasValue && EqualityComparer<T>.Default.Equals(lastValue.Value, arg))
                {
                    //Same key pressed, key still bound so timeout has not finished yet
                    return;
                }

                // New Key pressed
                func(arg);
                lastValue = arg;
                cancelTokenSource?.Cancel();
                cancelTokenSource = new CancellationTokenSource();

                Task.Delay(milliseconds, cancelTokenSource.Token)
                    .ContinueWith(t =>
                    {
                        if (t.IsCompletedSuccessfully)
                            lastValue = null;
                    }, TaskScheduler.Default);

                //Task
                //.Delay(milliseconds, cancelTokenSource.Token)
                //    .ContinueWith(t =>
                //    {
                //        if (t.IsCompletedSuccessfully)
                //        {
                //            func(arg);
                //        }
                //    }, TaskScheduler.Default);
            };
        }
    }
}
