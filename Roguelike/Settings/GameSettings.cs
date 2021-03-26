using GoRogue;
using Microsoft.Xna.Framework;
using Roguelike.Systems;
using SadConsole.Themes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Settings
{
    internal class GameSettings
    {
        public Radius FOVRadiusType = Radius.CIRCLE;

        public string GameFont = "FlyingMage";

        public readonly int ZoneSize = 64; // width/height in TILES
        public readonly int RegionSize = 32; // width/height in ZONES
        public readonly int WorldWidth = 24; // width in REGIONS
        public readonly int WorldHeight = 32; // height in REGIONS

        public int ZoneRenderRange = 2; // aoe of zones to render

        public int FPSLimit = 60;

        /// <summary>
        /// The width of the entire screen/window
        /// </summary>
        public int GameWidth = 75;
        /// <summary>
        /// The height of the entire screen/window
        /// </summary>
        public int GameHeight = 50;

        /// <summary>
        /// The width of the 'Game' screen sub-area
        /// </summary>
        public int MapScreenWidth = 45;
        /// <summary>
        /// The height of the 'Game' screen sub-area
        /// </summary>
        public int MapScreenHeight = 25;

        /// <summary>
        /// The width of the in-game generated Dungeon
        /// </summary>
        public int MapWidth = 60;
        /// <summary>
        /// The height of the in-game generated Dungeon
        /// </summary>
        public int MapHeight = 50;

        public int GameMenuWidth = 18;

        public int DebugConsoleWidth = 40;
        public int DebugHeight = 25;

        public ButtonTheme ButtonTheme = new MyButtonTheme();
        public Colors ButtonColors = MyButtonTheme.MyButtonColors();

        public Color MapScreenBorderColor = Color.DarkSeaGreen;
        public Color MapScreenBgColor = Color.Black;
        public Color DebugScreenBorderColor = Color.CadetBlue;
        public Color DebugScreenBgColor = Color.Black;

        public Color PlayerCharacterGlyphColor = Color.MediumSpringGreen;
        public Color DragonGlyphColor = Color.MediumVioletRed;
        public Color GoblinGlyphColor = Color.DarkGreen;

        public bool GoldAutoPickup = true;

        public bool EnableCharMapOutput = true;
        public bool EnableDebugOutput = true;

        public GameSettings()
        {
            ////GameFont = "Martin";
            ////GameFont = "Cooz";
            //GameFont = "FlyingMage";

            //FOVRadiusType = Radius.CIRCLE;

            ////GameWidth = 120;
            ////GameHeight = 70;
            //GameWidth = 75;
            //GameHeight = 50;

            ////MapScreenWidth = 60;
            ////MapScreenHeight = 40;
            //MapScreenWidth = 45;
            //MapScreenHeight = 25;

            //MapWidth = 60;
            //MapHeight = 50;

            //GameMenuWidth = 18;

            //DebugConsoleWidth = 62;

            //ButtonTheme = new MyButtonTheme();
            //ButtonColors = MyButtonTheme.MyButtonColors();

            //MapScreenBorderColor = Color.DarkSeaGreen;
            //MapScreenBgColor = Color.Black;

            //DebugScreenBorderColor = Color.CadetBlue;
            //DebugScreenBgColor = Color.Black;

            //PlayerCharacterGlyphColor = Color.MediumSpringGreen;
            
            //DragonGlyphColor = Color.MediumVioletRed;
            //GoblinGlyphColor = Color.DarkGreen;

            //GoldAutoPickup = true;

            //EnsureValidSettings();
        }

        //private void EnsureValidSettings()
        //{
        //    if (MapWidth < MapScreenWidth || MapHeight < MapScreenHeight)
        //    {
        //        DebugManager.Instance.AddMessage(new DebugMessage($"Invalid map/screen dimensions: Map({MapWidth},{MapHeight}), MapScreen({MapScreenWidth},{MapScreenHeight})", DebugSource.System));
        //    }
        //}
    }
}
