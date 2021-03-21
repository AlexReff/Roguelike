using Microsoft.Xna.Framework;
using SadConsole;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Consoles
{
    internal class MapGameBackgroundConsole : Console
    {
        private Color BackgroundColor;
        private Color BorderColor;

        private string Title;

        public MapGameBackgroundConsole(int width, int height, Color backgroundColor, Color? borderColor) : base(width, height)
        {
            IsVisible = true;
            BackgroundColor = backgroundColor;
            BorderColor = borderColor ?? Color.Transparent;

            Title = "Local";

            var bgRect = new Rectangle(0, 0, width, height);

            Helpers.DrawBorderBgTitle(this, bgRect, Title, BackgroundColor, BorderColor);
        }
    }
}
