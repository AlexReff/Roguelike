using Microsoft.Xna.Framework;
using SadConsole;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Consoles
{
    internal class BorderedBackgroundConsole : ContainerConsole
    {
        private SadConsole.Console DrawConsole;

        private Color BackgroundColor;
        private Color BorderColor;

        private string Title;

        public BorderedBackgroundConsole(int width, int height, string title, Color backgroundColor, Color? borderColor)
        {
            IsVisible = true;
            Width = width;
            Height = height;
            Title = title;
            BackgroundColor = backgroundColor;

            var bgRect = new Rectangle(0, 0, width, height);
            var bgWidth = width;
            var bgHeight = height;

            DrawConsole = new SadConsole.Console(width, height);
            DrawConsole.Position = new Point(0, 0);

            if (borderColor != null && borderColor.HasValue)
            {
                BorderColor = borderColor.Value;
                DrawConsole.Fill(bgRect, Color.White, BorderColor, 0, 0);

                // Inset the background rect
                bgRect = new Rectangle(1, 1, width - 2, height - 2);
                bgWidth = bgWidth - 1;
                bgHeight = bgHeight - 1;
            }

            DrawConsole.Fill(bgRect, Color.White, BackgroundColor, 0, 0);

            if (!string.IsNullOrEmpty(Title))
            {
                int widthCenterPoint = (int)Math.Floor(width / 2.0);
                int center = widthCenterPoint - (int)Math.Floor(Title.Length / 2.0);
                DrawConsole.Print(center, 0, Title, new Color(94, 194, 121), new Color(81, 89, 152));
            }

            Children.Add(DrawConsole);
        }
    }
}
