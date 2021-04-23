using GoRogue;
using GoRogue.GameFramework;
using GoRogue.MapGeneration;
using GoRogue.MapViews;
using GoRogue.Random;
using Microsoft.Xna.Framework;
using Roguelike.Entities;
using Roguelike.Entities.Items;
using Roguelike.Karma;
using Roguelike.Maps;
using Roguelike.Systems;
using SadConsole;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike
{
    /// <summary>
    /// Requires UIManager, Karma
    /// </summary>
    class World
    {
        public WorldMap WorldMap { get; set; }
        public GameMap CurrentMap { get; set; }

        public Player Player
        {
            get
            {
                return CurrentMap.ControlledGameObject;
            }
        }
        
        public World() : this(GenerateDungeon(MyGame.GameSettings.MapWidth, MyGame.GameSettings.MapHeight)) { }

        public World(GameMap map)
        {
            CurrentMap = map;
            MyGame.UIManager.SetGameMap(CurrentMap);
        }

        public static GameMap GenerateDungeon(int width, int height)
        {
            var map = new GameMap(width, height);
            map.GenerateDungeon();
            return map;
        }
    }
}
