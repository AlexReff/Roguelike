using Microsoft.Xna.Framework;
using Roguelike.Helpers;
using SadConsole;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Consoles
{
    internal class PlayerMessageConsole : Console
    {
        private Console BackgroundConsole;
        private Console OutputConsole;

        private Color BackgroundColor;
        private Color BorderColor;

        private int _maxLines;
        private readonly Queue<string> _lines;

        public PlayerMessageConsole(int width, int height, Color backgroundColor, Color borderColor) : base(width, height)
        {
            Width = width;
            Height = height;
            IsVisible = true;
            BackgroundColor = backgroundColor;
            BorderColor = borderColor;

            BackgroundConsole = new BorderedBackgroundConsole(width, height, "Events", BackgroundColor, BorderColor);
            BackgroundConsole.Position = new Point(0, 0);

            OutputConsole = new ScrollingConsole(width - 2, height - 1);
            //OutputConsole.Font = Global.FontDefault.Master.GetFont(Font.FontSizes.Half);
            OutputConsole.Position = new Point(1, 1);
            OutputConsole.Cursor.Position = new Point(0, OutputConsole.Height - 1);

            Children.Add(BackgroundConsole);
            Children.Add(OutputConsole);

            _maxLines = Height;
            _lines = new Queue<string>(PlayerMessageManager.Instance.Messages);

            foreach (var msg in PlayerMessageManager.Instance.Messages)
            {
                AddMessage(msg);
            }

            PlayerMessageManager.Instance.Subscribe("messageConsole", AddMessage);
        }

        public void AddMessage(string message)
        {
            _lines.Enqueue(message);
            // when exceeding the max number of lines remove the oldest one
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
