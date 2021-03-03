using GoRogue;
using Microsoft.Xna.Framework;
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
