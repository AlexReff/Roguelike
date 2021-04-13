using Microsoft.Xna.Framework;
using Roguelike.Systems;
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

        private Console DrawConsole;

        private Point LastMouseOverPos;

        public CharMapOutputConsole(int width, int height) : base(width + AdditionalWidth, height + AdditionalHeight)
        {
            DrawConsole = new Console(width, height);
            //DrawConsole = new Console(width, height, SadConsole.Global.Fonts["Guybrush"].GetFont(Font.FontSizes.One));
            //DrawConsole = new Console(width, height, SadConsole.Global.Fonts["CP850"].GetFont(Font.FontSizes.One));
            Children.Add(DrawConsole);
            //DrawConsole.FillWithRandomGarbage();

            //Font = SadConsole.Global.Fonts["CP850"].GetFont(Font.FontSizes.One);
            PopulateCharacterMapConsole();

            DrawConsole.MouseMove += Handle_MouseMove;
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
                if (pos.X > 0 && pos.X < DrawConsole.Width
                    && pos.Y > 0 && pos.Y < DrawConsole.Height - 1)
                {
                    int thisIndx = GetIndexAtPos(pos.X, pos.Y);
                    var thisChar = (char)thisIndx;

                    string paddedIndx = $"{thisIndx}".PadLeft(3);
                    string strX = $"{pos.X - 1}".PadLeft(2);
                    string strY = $"{pos.Y - 1}".PadLeft(2);
                    string output = $"{thisChar} {paddedIndx} ({strX},{strY})";

                    var strMiddle = (output.Length / 2.0);
                    var consoleMiddle = (DrawConsole.Width / 2.0);
                    int targetMiddle = (int)System.Math.Floor(consoleMiddle - strMiddle);

                    DrawConsole.DrawLine(new Point(0, DrawConsole.Height - 1), new Point(DrawConsole.Width - 1, DrawConsole.Height - 1), Color.Black, Color.Black);
                    DrawConsole.Print(targetMiddle, DrawConsole.Height - 1, output, Color.White, Color.Black);

                    DebugManager.Instance.AddMessage(output);
                }
            }
        }

        private void PopulateCharacterMapConsole()
        {
            //Draw a black line over the top of the legend (colors in 0,0 spot)
            DrawConsole.DrawLine(new Point(0, 0), new Point(Width - 1, 0), Color.Black, Color.Black);

            //Draw a black line over the mouseover text area
            DrawConsole.DrawLine(new Point(0, DrawConsole.Height - 1), new Point(DrawConsole.Width - 1, DrawConsole.Height - 1), Color.Black, Color.Black);

            for (int x = 0; x < DrawConsole.Width - AdditionalWidth; x++)
            {
                //Convert to Hexadecimal since we can't output 2 numbers in a single space in SadConsole
                string thisChar = System.Convert.ToByte(x).ToString("x");
                DrawConsole.Print(x + 1, 0, thisChar, Color.White, Color.Black);
                DrawConsole.Print(0, x + 1, thisChar, Color.White, Color.Black);
            }

            for (int x = 0; x < DrawConsole.Width - AdditionalWidth; x++)
            {
                for (int y = 0; y < DrawConsole.Height - AdditionalHeight; y++)
                {
                    DrawConsole.Print(x + 1, y + 1, GetCharAtPos(x + 1, y + 1).ToString(), Color.White, Color.Black);
                }
            }
        }

        public char GetCharAtPos(Point pos) => GetCharAtPos(pos.X, pos.Y);

        public char GetCharAtPos(int x, int y) => (char)GetIndexAtPos(x, y);

        public int GetIndexAtPos(Point pos) => GetIndexAtPos(pos.X, pos.Y);
        
        public int GetIndexAtPos(int x, int y)
        {
            //Legend takes up the 0,0 row/column, so -1 to get accurate index
            return (y - 1) * (DrawConsole.Width - AdditionalWidth) + (x - 1);
        }
    }
}
