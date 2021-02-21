using GoRogue;
using GoRogue.GameFramework;
using GoRogue.MapGeneration;
using GoRogue.MapViews;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Roguelike.Entities;
using Roguelike.Helpers;
using SadConsole;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Consoles
{
    class GameConsole : Console
    {
        public GameMapConsole MapScreen { get; private set; }

        private BorderedBackgroundConsole MapBackground;
        private DebugConsole DebugScreen;
        private PlayerMessageConsole MessageScreen;

        private static readonly int DebugConsoleWidth = UIManager.MapScreenWidth + 2;

        private static Point MapScreenBgPosition = new Point(0, 0);
        private static Point MapScreenPosition = new Point(MapScreenBgPosition.X + 1, MapScreenBgPosition.Y + 1);

        private Color MapScreenBorderColor = Color.DarkSeaGreen;
        private Color MapScreenBgColor = Color.Black;
        private Color DebugScreenBorderColor = Color.CadetBlue;
        private Color DebugScreenBgColor = Color.Black;

        public GameConsole(int width, int height, Font font) : base(width, height)
        {
            IsVisible = true;
            //IsFocused = true;
            Width = width;
            Height = height;
            //UseKeyboard = true;

            this.FillWithRandomGarbage();

            // Add all screens/elements from background -> foreground

            // Add border+background behind GameMap
            MapBackground = new BorderedBackgroundConsole(UIManager.MapScreenWidth + 2, UIManager.MapScreenHeight + 2, "Map", MapScreenBgColor, MapScreenBorderColor);
            MapBackground.Position = MapScreenBgPosition;
            Children.Add(MapBackground);

            // GameMap
            MapScreen = new GameMapConsole(/*UIManager.MapWidth, UIManager.MapHeight,*/ UIManager.MapScreenWidth, UIManager.MapScreenHeight, font);
            MapScreen.Position = MapScreenPosition;
            Children.Add(MapScreen);

            // Debug Console
            Point dbgPos = new Point(MapBackground.Position.X, MapScreenBgPosition.Y + MapBackground.Height);
            DebugScreen = new DebugConsole(DebugConsoleWidth, height - (MapBackground.Height + MapBackground.Position.Y), DebugScreenBgColor, DebugScreenBorderColor);
            DebugScreen.Position = dbgPos;
            Children.Add(DebugScreen);

            // User Message Console
            Point msgPos = new Point(MapBackground.Position.X + MapBackground.Width, MapScreenBgPosition.Y);
            MessageScreen = new PlayerMessageConsole(30, (int)System.Math.Floor(MapBackground.Height / 2.0), DebugScreenBgColor, DebugScreenBorderColor);
            MessageScreen.Position = msgPos;
            Children.Add(MessageScreen);
        }

        public void SetMap(GameMap map)
        {
            MapScreen.SetGameMap(map);
        }

        //public override bool ProcessKeyboard(SadConsole.Input.Keyboard info)
        //{
        //    Direction moveDirection = Direction.NONE;
        //    //Keys keyMatch = 0;

        //    // Simplified way to check if any key we care about is pressed and set movement direction.
        //    foreach (Keys key in s_movementDirectionMapping.Keys)
        //    {
        //        if (info.IsKeyPressed(key))
        //        {
        //            moveDirection = s_movementDirectionMapping[key];
        //            //keyMatch = key;
        //            break;
        //        }
        //    }

        //    if (moveDirection != Direction.NONE)
        //    {
        //        MovePlayer(moveDirection);
        //        return true;
        //    }

        //    // Process more player-related hotkeys

        //    return base.ProcessKeyboard(info);
        //}

        //private void MovePlayer(Direction moveDirection)
        //{
        //    DebugManager.Instance.AddMessage(new DebugMessage("User Attempted Move: " + moveDirection.ToString(), DebugSource.User));
        //    MapScreen.Map.ControlledGameObject.Position += moveDirection;
        //}
        public void CenterOnActor(Actor actor)
        {
            MapScreen.MapRenderer.CenterViewPortOnPoint(actor.Position);
        }
    }
}
