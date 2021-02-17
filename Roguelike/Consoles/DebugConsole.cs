using Microsoft.Xna.Framework;
using Roguelike.Helpers;
using SadConsole;
using SadConsole.Controls;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Consoles
{
    public class DebugConsole : SadConsole.Console
    {
        private SadConsole.Console BackgroundConsole;
        private SadConsole.Console OutputConsole;

        private Color BackgroundColor;
        private Color BorderColor;

        private List<string> messages;
        private readonly Queue<string> _lines;

        public DebugConsole(int width, int height, Color backgroundColor, Color borderColor) : base(width, height)
        {
            IsVisible = true;
            BackgroundColor = backgroundColor;
            BorderColor = borderColor;
            
            _lines = new Queue<string>(DebugManager.Instance.Messages);
            messages = new List<string>(DebugManager.Instance.Messages); 

            DebugManager.Instance.Subscribe("debugConsole", AddMessage);

            BackgroundConsole = new BorderedBackgroundConsole(width, height, BackgroundColor, BorderColor);
            BackgroundConsole.Position = new Point(0, 0);

            OutputConsole = new ScrollingConsole(width - 2, height - 1);
            //OutputConsole.Font = SadConsole.Global.FontDefault.Master.GetFont(Font.FontSizes.Half);
            OutputConsole.Position = new Point(1, 1);

            Children.Add(BackgroundConsole);
            Children.Add(OutputConsole);

            foreach (var msg in messages)
            {
                AddMessage(msg);
            }
        }

        public void AddMessage(string message)
        {
            _lines.Enqueue(message);
            // when exceeding the max number of lines remove the oldest one
            //if (_lines.Count > _maxLines)
            //{
            //    _lines.Dequeue();
            //}
            // Move the cursor to the last line and print the message.
            OutputConsole.Cursor.Position = new Point(0, _lines.Count + 1);
            OutputConsole.Cursor.Print(message + "\n");
        }
    }
}
