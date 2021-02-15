using System;

namespace Roguelike
{
    internal class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new MyGame())
                game.Run();
        }
    }
}
