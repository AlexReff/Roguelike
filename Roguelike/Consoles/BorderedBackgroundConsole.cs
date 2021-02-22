using Microsoft.Xna.Framework;
using SadConsole;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Consoles
{
    internal class BorderedBackgroundConsole : Console
    {
        private Color BackgroundColor;
        private Color BorderColor;

        private string Title;

        public BorderedBackgroundConsole(int width, int height, string title, Color backgroundColor, Color? borderColor) : base(width, height)
        {
            IsVisible = true;
            Title = title;
            BackgroundColor = backgroundColor;

            var bgRect = new Rectangle(0, 0, width, height);

            if (borderColor != null && borderColor.HasValue)
            {
                BorderColor = borderColor.Value;
                Fill(bgRect, Color.White, BorderColor, 0, 0);

                // Inset the background rect
                bgRect = new Rectangle(1, 1, width - 2, height - 2);
            }

            Fill(bgRect, Color.White, BackgroundColor, 0, 0);

            if (!string.IsNullOrEmpty(Title))
            {
                int widthCenterPoint = (int)System.Math.Floor(width / 2.0);
                int center = widthCenterPoint - (int)System.Math.Floor(Title.Length / 2.0);
                Print(center, 0, Title, new Color(94, 194, 121), new Color(81, 89, 152));
            }
        }
    }
}
