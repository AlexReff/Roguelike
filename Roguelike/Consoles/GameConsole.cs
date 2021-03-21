using GoRogue;
using GoRogue.GameFramework;
using GoRogue.MapGeneration;
using GoRogue.MapViews;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Roguelike.Entities;
using Roguelike.Systems;
using SadConsole;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Consoles
{
    class GameConsole : Console
    {
        public GameMapConsole MapScreen { get; private set; }
        public GameMenuConsole ControlsConsole { get; private set; }
        public EffectsScreenConsole EffectsScreen { get; private set; }

        private MapGameBackgroundConsole MapBackground;
        private PlayerMessageConsole MessageScreen;
        private DebugConsole DebugScreen;
        private CharMapOutputConsole CharMapScreen;
        private StatsMenuConsole StatsMenu;

        private static Point MapScreenBgPosition = new Point(0, 0);
        //private static Point MapScreenPosition = new Point(MapScreenBgPosition.X + 1, MapScreenBgPosition.Y + 1);
        private static Point MapScreenPosition = new Point(1, 1);

        public GameConsole(int width, int height, Font font) : base(width, height)
        {
            this.FillWithRandomGarbage();

            // Add all screens/elements from background -> foreground

            // Add border+background behind GameMap
            MapBackground = new MapGameBackgroundConsole(MyGame.GameSettings.MapScreenWidth + 2, MyGame.GameSettings.MapScreenHeight + 2, MyGame.GameSettings.MapScreenBgColor, MyGame.GameSettings.MapScreenBorderColor);
            MapBackground.Position = MapScreenBgPosition;
            Children.Add(MapBackground);

            // GameMap
            MapScreen = new GameMapConsole(MyGame.GameSettings.MapScreenWidth, MyGame.GameSettings.MapScreenHeight, font/*.Master.GetFont(Font.FontSizes.Two)*/);
            MapScreen.Position = MapScreenPosition;
            Children.Add(MapScreen);

            EffectsScreen = new EffectsScreenConsole(MapScreen.Width, MapScreen.Height);
            EffectsScreen.Position = MapScreenPosition;
            Children.Add(EffectsScreen);

            // User Message Console
            Point msgPos = new Point(0, MapBackground.Height);
            MessageScreen = new PlayerMessageConsole(MapBackground.Width, height - MapBackground.Height, MyGame.GameSettings.DebugScreenBgColor, MyGame.GameSettings.DebugScreenBorderColor);
            MessageScreen.Position = msgPos;
            Children.Add(MessageScreen);

            Point ctrlsPos = new Point(MapBackground.Position.X + MapBackground.Width, MapBackground.Height + MapScreenBgPosition.Y);
            ControlsConsole = new GameMenuConsole(MyGame.GameSettings.GameMenuWidth, MessageScreen.Height);
            ControlsConsole.Position = ctrlsPos;
            Children.Add(ControlsConsole);

            Point statsMenuPos = new Point(MapBackground.Position.X + MapBackground.Width, 0);
            StatsMenu = new StatsMenuConsole(width - MapBackground.Width, MapBackground.Height);
            StatsMenu.Position = statsMenuPos;
            Children.Add(StatsMenu);

            //SubscribeToEvents();

            //DEBUG/DEVELOPER THINGS BELOW

            //// Debug Console
            //Point dbgPos = new Point(0, MapBackground.Position.Y + MapBackground.Height + 1);
            //DebugScreen = new DebugConsole(MyGame.GameSettings.DebugConsoleWidth, Height - dbgPos.Y, MyGame.GameSettings.DebugScreenBgColor, MyGame.GameSettings.DebugScreenBorderColor);
            //DebugScreen.Position = dbgPos;
            //Children.Add(DebugScreen);

            //// Character map for easy reference
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
