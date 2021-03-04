using Microsoft.Xna.Framework;
using Roguelike.Helpers;
using SadConsole.Themes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Settings
{
    class GameSettings
    {
        //public static readonly string GameFont = "Isenhertz"; //possible candidate, needs some remapping, not usable for text
        //public static readonly string GameFont = "Phoebus"; //decent, needs glyph filled-in full opacity
        //public static readonly string GameFont = "Tigrex3d"; //ok, needs some remappings
        // public static readonly string GameFont = "Anno"; //not bad, a little blocky
        public string GameFont { get; private set; }//= "Martin"; //good but needs 'solid' glyph updated (done)

        /// <summary>
        /// The width of the entire screen/window
        /// </summary>
        public int GameWidth { get; private set; }
        /// <summary>
        /// The height of the entire screen/window
        /// </summary>
        public int GameHeight { get; private set; }
        /// <summary>
        /// The width of the 'Game' screen sub-area
        /// </summary>
        public int MapScreenWidth { get; private set; }
        /// <summary>
        /// The height of the 'Game' screen sub-area
        /// </summary>
        public int MapScreenHeight { get; private set; }
        /// <summary>
        /// The width of the in-game generated Dungeon
        /// </summary>
        public int MapWidth { get; private set; }
        /// <summary>
        /// The height of the in-game generated Dungeon
        /// </summary>
        public int MapHeight { get; private set; }
        public int GameMenuWidth { get; private set; }

        public int DebugConsoleWidth { get; private set; }

        public ButtonTheme ButtonTheme { get; private set; }
        public Colors ButtonColors { get; private set; }

        public Color MapScreenBorderColor { get; private set; }
        public Color MapScreenBgColor { get; private set; }
        public Color DebugScreenBorderColor { get; private set; }
        public Color DebugScreenBgColor { get; private set; }

        public Color PlayerCharacterGlyphColor { get; private set; }
        public Color DragonGlyphColor { get; private set; }
        public Color GoblinGlyphColor { get; private set; }

        public GameSettings()
        {
            //GameFont = "Martin";
            //GameFont = "Cooz";
            GameFont = "FlyingMage";

            GameWidth = 120;
            GameHeight = 60;

            MapScreenWidth = 60;
            MapScreenHeight = 40;

            MapWidth = 60;
            MapHeight = 50;

            GameMenuWidth = 18;

            DebugConsoleWidth = 28;

            ButtonTheme = new MyButtonTheme();
            ButtonColors = MyButtonTheme.MyButtonColors();

            MapScreenBorderColor = Color.DarkSeaGreen;
            MapScreenBgColor = Color.Black;

            DebugScreenBorderColor = Color.CadetBlue;
            DebugScreenBgColor = Color.Black;

            PlayerCharacterGlyphColor = Color.MediumSpringGreen;
            
            DragonGlyphColor = Color.MediumVioletRed;
            GoblinGlyphColor = Color.DarkGreen;

            EnsureValidSettings();
        }

        private void EnsureValidSettings()
        {
            if (MapWidth < MapScreenWidth || MapHeight < MapScreenHeight)
            {
                DebugManager.Instance.AddMessage(new DebugMessage($"Invalid map/screen dimensions: Map({MapWidth},{MapHeight}), MapScreen({MapScreenWidth},{MapScreenHeight})", DebugSource.System));
            }
        }
    }
}
