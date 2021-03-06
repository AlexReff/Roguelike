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
        public GameMenuConsole ControlsConsole { get; private set; }

        private BorderedBackgroundConsole MapBackground;
        private PlayerMessageConsole MessageScreen;
        private DebugConsole DebugScreen;
        private CharMapOutputConsole CharMapScreen;

        private static Point MapScreenBgPosition = new Point(0, 0);
        private static Point MapScreenPosition = new Point(MapScreenBgPosition.X + 1, MapScreenBgPosition.Y + 1);

        public GameConsole(int width, int height, Font font) : base(width, height)
        {
            this.FillWithRandomGarbage();

            // Add all screens/elements from background -> foreground

            // Add border+background behind GameMap
            MapBackground = new BorderedBackgroundConsole(MyGame.GameSettings.MapScreenWidth + 2, MyGame.GameSettings.MapScreenHeight + 2, "Map", MyGame.GameSettings.MapScreenBgColor, MyGame.GameSettings.MapScreenBorderColor);
            MapBackground.Position = MapScreenBgPosition;
            Children.Add(MapBackground);

            // GameMap
            MapScreen = new GameMapConsole(MyGame.GameSettings.MapScreenWidth, MyGame.GameSettings.MapScreenHeight, font);
            MapScreen.Position = MapScreenPosition;
            Children.Add(MapScreen);

            // User Message Console
            Point msgPos = new Point(MapBackground.Position.X + MapBackground.Width, MapScreenBgPosition.Y);
            MessageScreen = new PlayerMessageConsole(30, (int)System.Math.Floor(MapBackground.Height / 2.0), MyGame.GameSettings.DebugScreenBgColor, MyGame.GameSettings.DebugScreenBorderColor);
            MessageScreen.Position = msgPos;
            Children.Add(MessageScreen);

            Point ctrlsPos = new Point(MapBackground.Position.X + MapBackground.Width, MapScreenBgPosition.Y + MessageScreen.Height);
            ControlsConsole = new GameMenuConsole(30, 30);
            ControlsConsole.Position = ctrlsPos;
            Children.Add(ControlsConsole);

            //DEBUG/DEVELOPER THINGS BELOW

            // Debug Console
            //Point dbgPos = new Point(width - MyGame.GameSettings.DebugConsoleWidth, 0);
            Point dbgPos = new Point(0, MapBackground.Position.Y + MapBackground.Height + 10);
            //DebugScreen = new DebugConsole(MyGame.GameSettings.DebugConsoleWidth, 24, MyGame.GameSettings.DebugScreenBgColor, MyGame.GameSettings.DebugScreenBorderColor);
            DebugScreen = new DebugConsole(MyGame.GameSettings.DebugConsoleWidth, Height - dbgPos.Y, MyGame.GameSettings.DebugScreenBgColor, MyGame.GameSettings.DebugScreenBorderColor);
            DebugScreen.Position = dbgPos;
            Children.Add(DebugScreen);

            // Character map for easy reference
            Point chrMapPos = new Point(Width - Font.Columns - CharMapOutputConsole.AdditionalWidth, Height - Font.Rows - CharMapOutputConsole.AdditionalHeight);
            CharMapScreen = new CharMapOutputConsole(Font.Columns, Font.Rows);
            CharMapScreen.Position = chrMapPos;
            Children.Add(CharMapScreen);
        }

        public void SetMap(GameMap map)
        {
            MapScreen.SetGameMap(map);
        }

        public void CenterOnActor(Actor actor)
        {
            MapScreen.MapRenderer.CenterViewPortOnPoint(actor.Position);
        }
    }
}
