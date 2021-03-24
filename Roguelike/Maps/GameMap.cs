using System;
using System.Collections.Generic;
using System.Linq;
using GoRogue;
using GoRogue.GameFramework;
using GoRogue.MapGeneration;
using GoRogue.MapViews;
using Microsoft.Xna.Framework;
using Roguelike.Consoles;
using Roguelike.Entities;
using Roguelike.Entities.Items;
using Roguelike.Interfaces;
using Roguelike.JSON;
using Roguelike.Maps;
using Roguelike.Models;
using Roguelike.Systems;
using SadConsole;
using SadConsole.Entities;

namespace Roguelike
{
    public enum MapLayer
    {
        TERRAIN,
        ITEMS,
        MONSTERS,
        PLAYER
    }

    internal class GameMap : BasicMap
    {
        // Handles the changing of tile/entity visiblity as appropriate based on Map.FOV.
        public FOVVisibilityHandler FovVisibilityHandler { get; }

        // Since we'll want to access the player as our Player type, create a property to do the cast for us.  The cast must succeed thanks to the ControlledGameObjectTypeCheck
        // implemented in the constructor.
        public new Player ControlledGameObject
        {
            get => (Player)base.ControlledGameObject;
            set => base.ControlledGameObject = value;
        }

        public GameMap(int width, int height)
            // Allow multiple items on the same location only on the items layer.  This example uses 8-way movement, so Chebyshev distance is selected.
            : base(width, height, Enum.GetNames(typeof(MapLayer)).Length - 1, Distance.CHEBYSHEV, entityLayersSupportingMultipleItems: LayerMasker.DEFAULT.Mask(new[] { (int)MapLayer.ITEMS, (int)MapLayer.MONSTERS }))
        {
            ControlledGameObjectChanged += ControlledGameObjectTypeCheck<Player>; // Make sure we don't accidentally assign anything that isn't a Player type to ControlledGameObject
            FovVisibilityHandler = new PlayerFOVVisibilityHandler(this, ColorAnsi.BlackBright); //new DefaultFOVVisibilityHandler(this, ColorAnsi.BlackBright);
        }

        public void AddPlayer(Player player)
        {
            AddEntity(player);
            ControlledGameObject = player;

            //this must be last
            EventManager.Instance.InvokePlayerSpawn(player);
        }

        public bool AddEntity(MyBasicEntity entity)
        {
            var result = base.AddEntity(entity);

            if (entity is Actor)
            {
                if (entity is Player)
                {
                    MyGame.Karma.Add(0, entity as Actor);
                }
                else
                {
                    MyGame.Karma.Add(entity as Actor);
                }
                (entity as Actor).AddedToMap();
            }

            return result;
        }

        public bool RemoveEntity(MyBasicEntity entity)
        {
            if (entity is Actor)
            {
                MyGame.Karma.RemoveAll(entity as Actor);
            }

            return base.RemoveEntity(entity);
        }

        public bool IsTileWalkable(Point point)
        {
            if (point.X < 0 || point.Y < 0 || point.X >= Width || point.Y >= Height)
                return false;

            return WalkabilityView[point.Y * Width + point.X];
        }

        public T GetEntityAt<T>(Point location) where T : MyBasicEntity
        {
            return GetEntitiesAt<T>(location).FirstOrDefault();
        }

        public IEnumerable<T> GetEntitiesAt<T>(Point location) where T : MyBasicEntity
        {
            return Entities.GetItems(location).OfType<T>();
        }

        public void CenterOnActor(Actor actor)
        {
            if (this.Renderers.Count > 0 && this.Renderers[0] is GameConsole)
                ((GameConsole)this.Renderers[0]).CenterOnActor(actor);
        }

        public void GenerateDungeon()
        {
            // Generate map via GoRogue, and update the real map with appropriate terrain.
            var tempMap = new ArrayMap<bool>(Width, Height);
            //QuickGenerators.GenerateDungeonMazeMap(tempMap, minRooms: 3, maxRooms: 5, roomMinSize: 8, roomMaxSize: 22);
            QuickGenerators.GenerateDungeonMazeMap(tempMap, minRooms: 1, maxRooms: 1, roomMinSize: 16, roomMaxSize: 24);
            ApplyTerrainOverlay(tempMap, SpawnTerrainCreator(tempMap));

            Coord posToSpawn;
            // Spawn a few mock enemies
            for (int i = 0; i < 10; i++)
            {
                posToSpawn = WalkabilityView.RandomPosition(true);
                var existingActor = GetEntityAt<Actor>(posToSpawn);
                if (existingActor == null)
                {
                    var dragon = new NPC("dragon", posToSpawn);
                    var added = AddEntity(dragon);
                    if (Helpers.RandomGenerator.NextBoolean() || Helpers.RandomGenerator.NextBoolean())
                    {
                        dragon.AddCurrency((int)Math.Floor(Helpers.RandomGenerator.NextDouble() * 50));
                    }
                }
                else
                {
                    i--;
                }
            }
            for (int i = 0; i < 10; i++)
            {
                posToSpawn = WalkabilityView.RandomPosition(true);
                var existingActor = GetEntityAt<Actor>(posToSpawn);
                if (existingActor == null)
                {
                    var goblin = new NPC("goblin", posToSpawn);
                    var added = AddEntity(goblin);
                    if (Helpers.RandomGenerator.NextBoolean() || Helpers.RandomGenerator.NextBoolean())
                    {
                        goblin.AddCurrency((int)Math.Floor(Helpers.RandomGenerator.NextDouble() * 10));
                    }
                }
                else
                {
                    i--;
                }
            }
            for (int i = 0; i < 10; i++)
            {
                posToSpawn = WalkabilityView.RandomPosition(true);
                var existingActor = GetEntityAt<Actor>(posToSpawn);
                if (existingActor == null)
                {
                    var item = new Item("Pebble", Color.White, Color.Transparent, (char)249, posToSpawn);
                    AddEntity(item);
                }
                else
                {
                    i--;
                }
            }

            // Spawn player
            posToSpawn = WalkabilityView.RandomPosition(true);

            AddPlayer(new Player(posToSpawn));
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
