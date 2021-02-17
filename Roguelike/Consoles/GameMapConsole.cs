using GoRogue;
using GoRogue.GameFramework;
using GoRogue.MapGeneration;
using GoRogue.MapViews;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SadConsole;
using XnaRect = Microsoft.Xna.Framework.Rectangle;

namespace Roguelike
{
    internal class GameMapConsole : ContainerConsole
    {
        private static readonly char[] FloorCharacters = { (char) 0, (char)39, (char)44, (char)46 };
        public GameMap Map { get; }
        public ScrollingConsole MapRenderer { get; }
        //Generate a map and display it.  Could just as easily pass it into
        public GameMapConsole(int mapWidth, int mapHeight, int viewportWidth, int viewportHeight, Font font)
        {
            Width = viewportWidth;
            Height = viewportHeight;
            Map = GenerateDungeon(mapWidth, mapHeight);

            // Get a console that's set up to render the map, and add it as a child of this container so it renders
            MapRenderer = Map.CreateRenderer(new XnaRect(0, 0, viewportWidth, viewportHeight), font /*SadConsole.Global.FontDefault /*SadConsole.Global.Fonts["Anno"].GetFont(Font.FontSizes.One)*/);
            //MapRenderer.Position = position;
            Children.Add(MapRenderer);
            //Map.ControlledGameObject.IsFocused = true; // Set player to receive input, since in this example the player handles movement

            // Set up to recalculate FOV and set camera position appropriately when the player moves.  Also make sure we hook the new
            // Player if that object is reassigned.
            Map.ControlledGameObject.Moved += Player_Moved;
            Map.ControlledGameObjectChanged += ControlledGameObjectChanged;

            // Calculate initial FOV and center camera
            Map.CalculateFOV(Map.ControlledGameObject.Position, Map.ControlledGameObject.FOVRadius, Radius.SQUARE);
            MapRenderer.CenterViewPortOnPoint(Map.ControlledGameObject.Position);
        }

        public void SetFont(Font font)
        {
            MapRenderer.Font = font;
        }

        private void ControlledGameObjectChanged(object s, ControlledGameObjectChangedArgs e)
        {
            if (e.OldObject != null)
                e.OldObject.Moved -= Player_Moved;

            ((BasicMap)s).ControlledGameObject.Moved += Player_Moved;
        }

        private static GameMap GenerateDungeon(int width, int height)
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

        private void Player_Moved(object sender, ItemMovedEventArgs<IGameObject> e)
        {
            Map.CalculateFOV(Map.ControlledGameObject.Position, Map.ControlledGameObject.FOVRadius, Radius.CIRCLE);
            MapRenderer.CenterViewPortOnPoint(Map.ControlledGameObject.Position);
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
                return new BasicTerrain(Color.White, Color.Black, (char) 219, position, isWalkable: false, isTransparent: false);
        }
    }
}
