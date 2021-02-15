using Microsoft.Xna.Framework;
using SadConsole;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Consoles
{
    public class BorderedBackgroundConsole : ContainerConsole
    {
        private Console BackgroundConsole;
        private Console BorderConsole;

        private Color BackgroundColor;
        private Color BorderColor;

        public BorderedBackgroundConsole(int width, int height, Point position, Color backgroundColor, Color? borderColor)
        {
            IsVisible = true;
            var bgRect = new Rectangle(0, 0, width, height);
            var bgWidth = width;
            var bgHeight = height;

            if (borderColor != null && borderColor.HasValue)
            {
                BorderColor = borderColor.Value;
                BorderConsole = new Console(width, height);
                BorderConsole.Fill(bgRect, Color.Black, BorderColor, 0, 0);
                BorderConsole.Position = position;

                Children.Add(BorderConsole);

                bgRect = new Rectangle(1, 1, width - 2, height - 2);
                bgWidth = bgWidth - 1;
                bgHeight = bgHeight - 1;
            }

            BackgroundColor = backgroundColor;

            BackgroundConsole = new Console(bgWidth, bgHeight);
            BackgroundConsole.Fill(bgRect, Color.Black, BackgroundColor, 0, 0);

            BackgroundConsole.Position = position;

            Children.Add(BackgroundConsole);
        }

        public void SetFont(Font font)
        {
            BackgroundConsole.Font = font;
            BorderConsole.Font = font;
        }
    }
}
