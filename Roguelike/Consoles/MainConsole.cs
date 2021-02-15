using GoRogue;
using GoRogue.GameFramework;
using GoRogue.MapGeneration;
using GoRogue.MapViews;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SadConsole;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Consoles
{
    class MainConsole : ContainerConsole
    {
        private Console ContainerConsole;
        private GameMapConsole MapScreen;
        private BorderedBackgroundConsole MapBackground;

        private const int MapWidth = 100;
        private const int MapHeight = 100;
        private const int MapScreenWidth = 60;
        private const int MapScreenHeight = 40;

        private static Point MapScreenPosition = new Point(1, 8);
        private static Point MapScreenBgPosition = new Point(MapScreenPosition.X - 1, MapScreenPosition.Y - 1);

        private Color MapScreenBorderColor = Color.BurlyWood;
        private Color MapScreenBgColor = Color.Black;
        
        public MainConsole(int width, int height, Font font)
        {
            IsVisible = true;
            
            ContainerConsole = new Console(width, height);
            ContainerConsole.FillWithRandomGarbage();

            Children.Add(ContainerConsole);

            // Add all screens/elements from background -> foreground
            MapBackground = new BorderedBackgroundConsole(MapScreenWidth + 2, MapScreenHeight + 2, MapScreenBgPosition, MapScreenBgColor, MapScreenBorderColor);
            ContainerConsole.Children.Add(MapBackground);

            MapScreen = new GameMapConsole(MapWidth, MapHeight, MapScreenWidth, MapScreenHeight, MapScreenPosition, font);
            ContainerConsole.Children.Add(MapScreen);

            //Parent = SadConsole.Global.CurrentScreen;
            SadConsole.Global.CurrentScreen = this;
        }

        public void SetFont(Font font)
        {
            SadConsole.Global.FontDefault = font;
            Global.FontDefault.ResizeGraphicsDeviceManager(Global.GraphicsDeviceManager, Global.FontDefault.Size.X, Global.FontDefault.Size.Y, 0, 0);
            MapScreen.SetFont(font);
            MapBackground.SetFont(font);
            ContainerConsole.Font = font;
        }
    }
}
