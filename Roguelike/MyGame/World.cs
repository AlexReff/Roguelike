using GoRogue;
using GoRogue.GameFramework;
using GoRogue.MapGeneration;
using GoRogue.MapViews;
using GoRogue.Random;
using Microsoft.Xna.Framework;
using Roguelike.Entities;
using Roguelike.Entities.Items;
using Roguelike.Entities.Monsters;
using Roguelike.Helpers;
using SadConsole;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike
{
    class World
    {
        public GameMap CurrentMap { get; set; }

        // player data
        public Player Player
        {
            get
            {
                return CurrentMap.ControlledGameObject;
            }
        }

        public World(): this(GenerateDungeon(MyGame.GameSettings.MapWidth, MyGame.GameSettings.MapHeight))
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
            QuickGenerators.GenerateDungeonMazeMap(tempMap, minRooms: 3, maxRooms: 5, roomMinSize: 8, roomMaxSize: 22);
            map.ApplyTerrainOverlay(tempMap, SpawnTerrainCreator(tempMap));

            Coord posToSpawn;
            // Spawn a few mock enemies
            for (int i = 0; i < 10; i++)
            {
                posToSpawn = map.WalkabilityView.RandomPosition(true);
                var existingMonster = map.GetEntityAt<Monster>(posToSpawn);
                if (existingMonster == null)
                {
                    var dragon = new Dragon(posToSpawn);
                    map.AddEntity(dragon);
                    if (Helpers.Helpers.RandomGenerator.NextBoolean())
                    {
                        dragon.AddCurrency(Helpers.Helpers.RandomGenerator.NextDouble() * 50);
                    }
                }
            }
            for (int i = 0; i < 10; i++)
            {
                posToSpawn = map.WalkabilityView.RandomPosition(true);
                var existingMonster = map.GetEntityAt<Monster>(posToSpawn);
                if (existingMonster == null)
                {
                    var goblin = new Goblin(posToSpawn);
                    map.AddEntity(goblin);
                    if (Helpers.Helpers.RandomGenerator.NextBoolean())
                    {
                        goblin.AddCurrency(Helpers.Helpers.RandomGenerator.NextDouble() * 10);
                    }
                }
            }
            for (int i = 0; i < 10; i++)
            {
                posToSpawn = map.WalkabilityView.RandomPosition(true);
                var existingPlayer = map.GetEntityAt<Player>(posToSpawn);
                if (existingPlayer == null)
                {
                    var item = new Item("Pebble", Color.White, Color.Transparent, (char)249, posToSpawn);
                    map.AddEntity(item);
                }
            }

            // Spawn player
            posToSpawn = map.WalkabilityView.RandomPosition(true);
            map.ControlledGameObject = new Player(posToSpawn);
            map.AddEntity(map.ControlledGameObject);

            return map;
        }

        private static Func<Coord, bool, IGameObject> SpawnTerrainCreator(ArrayMap<bool> map) => (Coord position, bool mapGenValue) => SpawnTerrain(map, position, mapGenValue);

        private static IGameObject SpawnTerrain(ArrayMap<bool> map, Coord position, bool mapGenValue)
        {
            if (mapGenValue) // Floor
                return new Floor(position);
            else             // Wall
                return new Wall(map, position);
        }
    }
}
