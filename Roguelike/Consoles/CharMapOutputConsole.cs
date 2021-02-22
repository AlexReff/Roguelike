using Microsoft.Xna.Framework;
using Roguelike.Helpers;
using SadConsole;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Consoles
{
    internal class CharMapOutputConsole : Console
    {
        /// <summary>
        /// 2 Additional rows - 1 for the legend, 1 for the mouseover output on the last row
        /// </summary>
        public static readonly int AdditionalHeight = 2;
        /// <summary>
        /// 1 Additional column for the legend
        /// </summary>
        public static readonly int AdditionalWidth = 1;

        private Point LastMouseOverPos;

        public CharMapOutputConsole(int width, int height) : base(width + AdditionalWidth, height + AdditionalHeight)
        {
            PopulateCharacterMapConsole();

            MouseMove += Handle_MouseMove;
        }

        private void Handle_MouseMove(object sender, SadConsole.Input.MouseEventArgs e)
        {
            var thisPoint = e.MouseState.CellPosition;
            if (thisPoint == LastMouseOverPos)
            {
                return;
            }

            LastMouseOverPos = thisPoint;

            if (e.MouseState.IsOnConsole)
            {
                var pos = e.MouseState.CellPosition;
                if (pos.X > 0 && pos.X < Width
                    && pos.Y > 0 && pos.Y < Height - 1)
                {
                    int thisIndx = GetIndexAtPos(pos.X, pos.Y);
                    var thisChar = (char)thisIndx;

                    string paddedIndx = $"{thisIndx}".PadLeft(3);
                    string strX = $"{pos.X - 1}".PadLeft(2);
                    string strY = $"{pos.Y - 1}".PadLeft(2);
                    string output = $"{thisChar} {paddedIndx} ({strX},{strY})";

                    var strMiddle = (output.Length / 2.0);
                    var consoleMiddle = (Width / 2.0);
                    int targetMiddle = (int)System.Math.Floor(consoleMiddle - strMiddle);

                    DrawLine(new Point(0, Height - 1), new Point(Width - 1, Height - 1), Color.Black, Color.Black);
                    Print(targetMiddle, Height - 1, output, Color.White, Color.Black);

                    DebugManager.Instance.AddMessage(output);
                }
            }
        }

        private void PopulateCharacterMapConsole()
        {
            //Draw a black line over the top of the legend (colors in 0,0 spot)
            DrawLine(new Point(0, 0), new Point(Width - 1, 0), Color.Black, Color.Black);

            //Draw a black line over the mouseover text area
            DrawLine(new Point(0, Height - 1), new Point(Width - 1, Height - 1), Color.Black, Color.Black);

            for (int x = 0; x < Width - AdditionalWidth; x++)
            {
                //Convert to Hexadecimal since we can't output 2 numbers in a single space in SadConsole
                string thisChar = System.Convert.ToByte(x).ToString("x");
                Print(x + 1, 0, thisChar, Color.White, Color.Black);
                Print(0, x + 1, thisChar, Color.White, Color.Black);
            }

            for (int x = 0; x < Width - AdditionalWidth; x++)
            {
                for (int y = 0; y < Height - AdditionalHeight; y++)
                {
                    Print(x + 1, y + 1, GetCharAtPos(x + 1, y + 1).ToString(), Color.White, Color.Black);
                }
            }
        }

        public char GetCharAtPos(Point pos) => GetCharAtPos(pos.X, pos.Y);

        public char GetCharAtPos(int x, int y) => (char)GetIndexAtPos(x, y);

        public int GetIndexAtPos(Point pos) => GetIndexAtPos(pos.X, pos.Y);
        
        public int GetIndexAtPos(int x, int y)
        {
            //Legend takes up the 0,0 row/column, so -1 to get accurate index
            return (y - 1) * (Width - AdditionalWidth) + (x - 1);
        }
    }
}
