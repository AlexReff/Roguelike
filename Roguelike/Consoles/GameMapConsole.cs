using GoRogue;
using GoRogue.GameFramework;
using GoRogue.MapGeneration;
using GoRogue.MapViews;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Entities;
using SadConsole;
using XnaRect = Microsoft.Xna.Framework.Rectangle;

namespace Roguelike
{
    internal class GameMapConsole : ContainerConsole
    {
        //private int MapWidth, MapHeight;

        public GameMap Map { get; private set; }
        public ScrollingConsole MapRenderer { get; private set; }

        public GameMapConsole(/*int mapWidth, int mapHeight,*/ int viewportWidth, int viewportHeight, Font font)
        {
            Width = viewportWidth;
            Height = viewportHeight;
            //MapWidth = mapWidth;
            //MapHeight = mapHeight;
            Font = font;
        }

        public void SetGameMap(GameMap map)
        {
            Map = map;

            Map.ControlledGameObject.Moved += Player_Moved;
            Map.ControlledGameObjectChanged += ControlledGameObjectChanged;

            MapRenderer = Map.CreateRenderer(new XnaRect(0, 0, Width, Height), Font /*SadConsole.Global.FontDefault /*SadConsole.Global.Fonts["Anno"].GetFont(Font.FontSizes.One)*/);
            Children.Add(MapRenderer);

            Map.CalculateFOV(Map.ControlledGameObject.Position, Map.ControlledGameObject.FOVRadius, Radius.SQUARE);
            MapRenderer.CenterViewPortOnPoint(Map.ControlledGameObject.Position);
        }

        private void ControlledGameObjectChanged(object s, ControlledGameObjectChangedArgs e)
        {
            if (e.OldObject != null)
                e.OldObject.Moved -= Player_Moved;

            ((BasicMap)s).ControlledGameObject.Moved += Player_Moved;
        }

        //private static GameMap GenerateDungeon(int width, int height)
        //{
        //    // Same size as screen, but we set up to center the camera on the player so expanding beyond this should work fine with no other changes.
        //    var map = new GameMap(width, height);

        //    // Generate map via GoRogue, and update the real map with appropriate terrain.
        //    var tempMap = new ArrayMap<bool>(map.Width, map.Height);
        //    QuickGenerators.GenerateDungeonMazeMap(tempMap, minRooms: 10, maxRooms: 20, roomMinSize: 8, roomMaxSize: 16);
        //    map.ApplyTerrainOverlay(tempMap, SpawnTerrain);

        //    Coord posToSpawn;
        //    // Spawn a few mock enemies
        //    for (int i = 0; i < 10; i++)
        //    {
        //        posToSpawn = map.WalkabilityView.RandomPosition(true); // Get a location that is walkable
        //        var goblin = new BasicEntity(Color.Red, Color.Transparent, 'g', posToSpawn, (int)MapLayer.MONSTERS, isWalkable: false, isTransparent: true);
        //        map.AddEntity(goblin);
        //    }

        //    // Spawn player
        //    posToSpawn = map.WalkabilityView.RandomPosition(true);
        //    map.ControlledGameObject = new Player(posToSpawn);
        //    map.AddEntity(map.ControlledGameObject);

        //    return map;
        //}

        private void Player_Moved(object sender, ItemMovedEventArgs<IGameObject> e)
        {
            Map.CalculateFOV(Map.ControlledGameObject.Position, Map.ControlledGameObject.FOVRadius, Radius.CIRCLE);
            MapRenderer.CenterViewPortOnPoint(Map.ControlledGameObject.Position);
        }

        //private static IGameObject SpawnTerrain(Coord position, bool mapGenValue)
        //{
        //    GoRogue.Random.SadConsoleRandomGenerator rnd = new GoRogue.Random.SadConsoleRandomGenerator();
        //    var floorChar = (int)System.Math.Floor(rnd.NextDouble() * FloorCharacters.Length);
        //    // Floor or wall.  This could use the Factory system, or instantiate Floor and Wall classes, or something else if you prefer;
        //    // this simplistic if-else is just used for example
        //    if (mapGenValue) // Floor
        //        return new BasicTerrain(Color.LightGray, Color.Black, FloorCharacters[floorChar], position, isWalkable: true, isTransparent: true);
        //    else             // Wall
        //        return new BasicTerrain(Color.White, Color.Black, (char) 219, position, isWalkable: false, isTransparent: false);
        //}
    }
}
