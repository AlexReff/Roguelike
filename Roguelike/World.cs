using GoRogue;
using GoRogue.GameFramework;
using GoRogue.MapGeneration;
using GoRogue.MapViews;
using Microsoft.Xna.Framework;
using Roguelike.Entities;
using Roguelike.Helpers;
using SadConsole;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike
{
    class World
    {
        private static readonly char[] FloorCharacters = { (char)0, (char)39, (char)44, (char)46 };

        public GameMap CurrentMap { get; set; }

        // player data
        public Player Player
        {
            get
            {
                return CurrentMap.ControlledGameObject;
            }
        }

        public World(): this(GenerateDungeon(UIManager.MapWidth, UIManager.MapHeight))
        {
        }

        public World(GameMap map)
        {
            CurrentMap = map;
            MyGame.UIManager.SetGameMap(CurrentMap);
        }

        public static GameMap GenerateDungeon(int width, int height)
        {
            // Same size as screen, but we set up to center the camera on the player so expanding beyond this should work fine with no other changes.
            var map = new GameMap(width, height);

            // Generate map via GoRogue, and update the real map with appropriate terrain.
            var tempMap = new ArrayMap<bool>(map.Width, map.Height);
            QuickGenerators.GenerateDungeonMazeMap(tempMap, minRooms: 10, maxRooms: 20, roomMinSize: 8, roomMaxSize: 16);
            map.ApplyTerrainOverlay(tempMap, SpawnTerrain);

            Coord posToSpawn;
            // Spawn a few mock enemies
            for (int i = 0; i < 10; i++)
            {
                posToSpawn = map.WalkabilityView.RandomPosition(true); // Get a location that is walkable
                var goblin = new BasicEntity(Color.Red, Color.Transparent, 'g', posToSpawn, (int)MapLayer.MONSTERS, isWalkable: false, isTransparent: true);
                map.AddEntity(goblin);
            }

            // Spawn player
            posToSpawn = map.WalkabilityView.RandomPosition(true);
            map.ControlledGameObject = new Player(posToSpawn);
            map.AddEntity(map.ControlledGameObject);

            return map;
        }

        private static IGameObject SpawnTerrain(Coord position, bool mapGenValue)
        {
            GoRogue.Random.SadConsoleRandomGenerator rnd = new GoRogue.Random.SadConsoleRandomGenerator();
            var floorChar = (int)System.Math.Floor(rnd.NextDouble() * FloorCharacters.Length);
            // Floor or wall.  This could use the Factory system, or instantiate Floor and Wall classes, or something else if you prefer;
            // this simplistic if-else is just used for example
            if (mapGenValue) // Floor
                return new BasicTerrain(Color.LightGray, Color.Black, FloorCharacters[floorChar], position, isWalkable: true, isTransparent: true);
            else             // Wall
                return new BasicTerrain(Color.White, Color.Black, (char)219, position, isWalkable: false, isTransparent: false);
        }
    }
}
