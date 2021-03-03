using SadConsole.Themes;
using SadConsole;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Roguelike
{
    class MyButtonTheme : ButtonTheme
    {
        private static Color ForegroundColor = Color.White;
        private static Color BackgroundColor = Color.Black;

        public MyButtonTheme()
        {
            this.EndCharacterLeft = '-';
            this.EndCharacterRight = (char)16;
            this.ShowEnds = true;
            this.Normal = new Cell(ForegroundColor, BackgroundColor);
        }

        public static Colors MyButtonColors()
        {
            var baseTheme = SadConsole.Themes.Colors.CreateAnsi();

            baseTheme.Appearance_ControlNormal = new Cell(Color.White, Color.Black);
            baseTheme.Appearance_ControlOver = new Cell(Color.White, new Color(0x4e, 0x07, 0x07));

            baseTheme.Appearance_ControlFocused = baseTheme.Appearance_ControlNormal;
            baseTheme.Appearance_ControlSelected = baseTheme.Appearance_ControlNormal;
            baseTheme.Appearance_ControlMouseDown = baseTheme.Appearance_ControlOver;

            return baseTheme;
        }
    }
}
