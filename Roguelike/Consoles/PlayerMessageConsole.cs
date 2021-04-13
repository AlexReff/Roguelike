using Microsoft.Xna.Framework;
using Roguelike.Systems;
using SadConsole;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Consoles
{
    internal class PlayerMessageConsole : Console
    {
        private Console OutputConsole;

        private Color BackgroundColor;
        private Color BorderColor;

        private int _maxLines;
        private readonly Queue<string> _lines;

        private string lastMessage;
        private int lastMessageRepeatCount;

        public PlayerMessageConsole(int width, int height, Color backgroundColor, Color borderColor) : base(width, height)
        {
            BackgroundColor = backgroundColor;
            BorderColor = borderColor;

            this.DrawBorderBgTitle(new Rectangle(0, 0, width, height), "Events", BackgroundColor, BorderColor);

            OutputConsole = new ScrollingConsole((width * 2) - 3, height - 1);
            OutputConsole.Position = new Point(2, 1);
            OutputConsole.Cursor.Position = new Point(0, OutputConsole.Height - 1);

            OutputConsole.Font = SadConsole.Global.Fonts["CP850"].GetFont(Font.FontSizes.One);

            Children.Add(OutputConsole);

            _maxLines = Height;
            _lines = new Queue<string>();
            lastMessageRepeatCount = 0;

            foreach (var msg in PlayerMessageManager.Instance.Messages)
            {
                AddMessage(msg);
            }

            PlayerMessageManager.Instance.Subscribe("messageConsole", AddMessage);
        }

        public void AddMessage(string message)
        {
            if (message == lastMessage)
            {
                OutputConsole.Cursor.Position = new Point(0, OutputConsole.Cursor.Position.Y - 1);
                OutputConsole.Cursor.Print(message + $" (x{++lastMessageRepeatCount})\n");
            }
            else
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
                lastMessage = message;
                lastMessageRepeatCount = 1;
            }
        }
    }
}
