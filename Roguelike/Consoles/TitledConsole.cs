using Microsoft.Xna.Framework;
using SadConsole;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Consoles
{
    class TitledConsole : BorderedBackgroundConsole
    {
        public string Title { get; private set; }
        private SadConsole.Console OverlayConsole;

        public TitledConsole(int width, int height, string title, Color backgroundColor, Color? borderColor) : base(width, height, backgroundColor, borderColor)
        {
            OverlayConsole = new SadConsole.Console(width, 1);
            OverlayConsole.Position = new Point(0, 0);
            SetTitle(title.Align(HorizontalAlignment.Center, width));
        }

        public void SetTitle(string title)
        {
            this.Title = title;
            OverlayConsole.Print(0, 0, this.Title);
        }
    }
}
