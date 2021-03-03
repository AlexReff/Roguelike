using Microsoft.Xna.Framework;
using Roguelike.Helpers;
using SadConsole;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Consoles
{
    internal class DebugConsole : Console
    {
        //private Console BackgroundConsole;
        private ScrollingConsole OutputConsole;

        private Color BackgroundColor;
        private Color BorderColor;

        //private List<string> messages;
        private readonly Queue<string> _lines;
        private int _maxLines;

        public DebugConsole(int width, int height, Color backgroundColor, Color borderColor) : base(width, height)
        {
            BackgroundColor = backgroundColor;
            BorderColor = borderColor;

            //BackgroundConsole = new BorderedBackgroundConsole(width, height, "Debug", BackgroundColor, BorderColor);
            //BackgroundConsole.Position = new Point(0, 0);
            this.DrawBorderBgTitle(new Rectangle(0, 0, width, height), "Debug", BackgroundColor, BorderColor);

            OutputConsole = new ScrollingConsole(width - 2, height - 1);
            //OutputConsole.Font = Global.FontDefault.Master.GetFont(Font.FontSizes.Half);
            OutputConsole.Position = new Point(1, 1);
            OutputConsole.Cursor.Position = new Point(0, OutputConsole.Height - 1);

            //Children.Add(BackgroundConsole);
            Children.Add(OutputConsole);

            _maxLines = Height;
            _lines = new Queue<string>();

            foreach (var msg in DebugManager.Instance.Messages)
            {
                AddMessage(msg);
            }

            DebugManager.Instance.Subscribe("debugConsole", AddMessage);
        }

        public void AddMessage(string message)
        {
            _lines.Enqueue(message);
            if (_lines.Count > _maxLines)
            {
                _lines.Dequeue();
            }
            // Move the cursor to the last line and print the message.
            OutputConsole.Cursor.Position = new Point(0, System.Math.Max(OutputConsole.Height, _lines.Count + 1));
            OutputConsole.Cursor.Print(message + "\n");
        }
    }
}
