using GoRogue;
using GoRogue.Random;
using Microsoft.Xna.Framework;
using Roguelike.Entities;
using Roguelike.Models;
using SadConsole;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Roguelike.Helpers
{
    public static class Helpers
    {
        public static IDGenerator IDGenerator = new IDGenerator();
        
        public static SadConsoleRandomGenerator RandomGenerator = new SadConsoleRandomGenerator();

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

        public static void DrawBorderBgTitle(this Console console, Microsoft.Xna.Framework.Rectangle area, string title, Color backgroundColor, Color borderColor)
        {
            var bgArea = new Microsoft.Xna.Framework.Rectangle(area.X + 1, area.Y + 1, area.Width - 2, area.Height - 2);

            console.Fill(area, Color.White, borderColor, 0, 0);

            console.Fill(bgArea, Color.White, backgroundColor, 0, 0);

            if (!string.IsNullOrEmpty(title))
            {
                int widthCenterPoint = (int)System.Math.Floor(area.Width / 2.0);
                int center = widthCenterPoint - (int)System.Math.Floor(title.Length / 2.0);
                console.Print(center, 0, title, new Color(94, 194, 121), new Color(81, 89, 152));
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
