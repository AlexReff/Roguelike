using GoRogue;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Roguelike.Helpers
{
    public static partial class Helpers
    {
        public static Action<T> Debounce<T>(this Action<T> func, int milliseconds = 300)
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
        public static Action<T> DebounceKeyHandler<T>(this Action<T> func, int milliseconds = 300) where T : struct
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
