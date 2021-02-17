using GoRogue;
using GoRogue.GameFramework;
using GoRogue.MapGeneration;
using GoRogue.MapViews;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Roguelike.Helpers;
using SadConsole;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Consoles
{
    class GameConsole : SadConsole.Console
    {
        private static readonly Dictionary<Keys, Direction> s_movementDirectionMapping = new Dictionary<Keys, Direction>
        {
            { Keys.NumPad7, Direction.UP_LEFT }, { Keys.NumPad8, Direction.UP }, { Keys.NumPad9, Direction.UP_RIGHT },
            { Keys.NumPad4, Direction.LEFT }, { Keys.NumPad6, Direction.RIGHT },
            { Keys.NumPad1, Direction.DOWN_LEFT }, { Keys.NumPad2, Direction.DOWN }, { Keys.NumPad3, Direction.DOWN_RIGHT },
            { Keys.Up, Direction.UP }, { Keys.Down, Direction.DOWN }, { Keys.Left, Direction.LEFT }, { Keys.Right, Direction.RIGHT },
            { Keys.Q, Direction.UP_LEFT }, { Keys.W, Direction.UP }, { Keys.E, Direction.UP_RIGHT },
            { Keys.A, Direction.LEFT }, { Keys.D, Direction.RIGHT },
            { Keys.Z, Direction.DOWN_LEFT }, { Keys.X, Direction.DOWN }, { Keys.C, Direction.DOWN_RIGHT }
        };

        //private Console ContainerConsole;
        private GameMapConsole MapScreen;
        private BorderedBackgroundConsole MapBackground;
        private DebugConsole DebugScreen;

        private const int MapWidth = 100;
        private const int MapHeight = 100;
        private const int MapScreenWidth = 60;
        private const int MapScreenHeight = 40;
        private const int DebugConsoleWidth = MapScreenWidth + 2;

        private static Point MapScreenBgPosition = new Point(0, 0);
        private static Point MapScreenPosition = new Point(MapScreenBgPosition.X + 1, MapScreenBgPosition.Y + 1);

        private Color MapScreenBorderColor = Color.DarkSeaGreen;
        private Color MapScreenBgColor = Color.Black;
        private Color DebugScreenBorderColor = Color.CadetBlue;
        private Color DebugScreenBgColor = Color.Black;

        //private Action<Keys> MoveDebounceWrapper;

        public GameConsole(int width, int height, Font font) : base(width, height)
        {
            IsVisible = true;
            IsFocused = true;
            Width = width;
            Height = height;
            UseKeyboard = true;

            this.FillWithRandomGarbage();

            // Add all screens/elements from background -> foreground

            // Add border+background behind GameMap
            MapBackground = new TitledConsole(MapScreenWidth + 2, MapScreenHeight + 2, "Map", MapScreenBgColor, MapScreenBorderColor);
            MapBackground.Position = MapScreenBgPosition;
            Children.Add(MapBackground);

            // GameMap
            MapScreen = new GameMapConsole(MapWidth, MapHeight, MapScreenWidth, MapScreenHeight, font);
            MapScreen.Position = MapScreenPosition;
            Children.Add(MapScreen);

            // Debug Console
            Point dbgPos = new Point(MapBackground.Position.X, MapScreenBgPosition.Y + MapBackground.Height);
            DebugScreen = new DebugConsole(DebugConsoleWidth, height - (MapBackground.Height + MapBackground.Position.Y), DebugScreenBgColor, DebugScreenBorderColor);
            //Point dbgPos = new Point(MapBackground.Position.X + MapBackground.Width + 1, MapScreenBgPosition.Y);
            //DebugScreen = new DebugConsole(DebugConsoleWidth, MapBackground.Height), DebugScreenBgColor, DebugScreenBorderColor);
            DebugScreen.Position = dbgPos;
            Children.Add(DebugScreen);

            //Action<Keys> dirAction = (key) =>
            //{
            //    MapScreen.Map.ControlledGameObject.Position += s_movementDirectionMapping[key];
            //};
            //MoveDebounceWrapper = dirAction.DebounceKeyHandler(700);
        }

        //private void MoveDebounce(Direction dir)
        //{
        //    MapScreen.Map.ControlledGameObject.Position += dir;
        //}

        public override bool ProcessKeyboard(SadConsole.Input.Keyboard info)
        {
            Direction moveDirection = Direction.NONE;
            //Keys keyMatch = 0;

            // Simplified way to check if any key we care about is pressed and set movement direction.
            foreach (Keys key in s_movementDirectionMapping.Keys)
            {
                if (info.IsKeyPressed(key))
                {
                    moveDirection = s_movementDirectionMapping[key];
                    //keyMatch = key;
                    break;
                }
            }

            if (moveDirection != Direction.NONE)
            {
                //Roguelike.Helpers.Helpers.Debounce<Direction>(MoveDebounce(moveDirection));

                //MapScreen.Map.ControlledGameObject.Position += moveDirection;
                MovePlayer(moveDirection);
                //MoveDebounceWrapper(moveDirection);
                //MoveDebounceWrapper(keyMatch);
                return true;
            }

            // Process more player-related hotkeys

            return base.ProcessKeyboard(info);
        }

        private void MovePlayer(Direction moveDirection)
        {
            DebugManager.Instance.AddMessage(new DebugMessage("User Attempted Move: " + moveDirection.ToString(), DebugSource.User));
            MapScreen.Map.ControlledGameObject.Position += moveDirection;
        }
    }
}
