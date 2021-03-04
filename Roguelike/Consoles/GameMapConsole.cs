using GoRogue;
using GoRogue.GameFramework;
using GoRogue.MapGeneration;
using GoRogue.MapViews;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Entities;
using Roguelike.Helpers;
using SadConsole;
using XnaRect = Microsoft.Xna.Framework.Rectangle;

namespace Roguelike
{
    internal class GameMapConsole : ContainerConsole
    {
        public GameMap Map { get; private set; }
        public ScrollingConsole MapRenderer { get; private set; }

        public GameMapConsole(int viewportWidth, int viewportHeight, Font font)
        {
            Font = font;
            Width = viewportWidth;
            Height = viewportHeight;
        }

        public void SetGameMap(GameMap map)
        {
            Map = map;

            Map.ControlledGameObject.Moved += Player_Moved;
            Map.ControlledGameObjectChanged += ControlledGameObjectChanged;

            MapRenderer = Map.CreateRenderer(new XnaRect(0, 0, Width, Height), Font /*SadConsole.Global.FontDefault /*SadConsole.Global.Fonts["Anno"].GetFont(Font.FontSizes.One)*/);
            Children.Add(MapRenderer);

            UpdateFOV();
        }

        private void ControlledGameObjectChanged(object s, ControlledGameObjectChangedArgs e)
        {
            if (e.OldObject != null)
                e.OldObject.Moved -= Player_Moved;

            ((GameMap)s).ControlledGameObject.Moved += Player_Moved;
        }

        private void Player_Moved(object sender, ItemMovedEventArgs<IGameObject> e)
        {
            Player player = (Player)e.Item;
            DebugManager.Instance.AddMessage(new DebugMessage($"Player_Moved triggered: {player.Glyph}->{e.NewPosition}", DebugSource.System));

            UpdateFOV(player);
        }

        public void UpdateFOV(Player player = null)
        {
            double deg = Helpers.Helpers.GetFOVDegree(player ?? MyGame.World?.Player ?? null);
            Map.CalculateFOV(Map.ControlledGameObject.Position, Map.ControlledGameObject.FOVRadius, Radius.CIRCLE, deg, 200);
            MapRenderer.CenterViewPortOnPoint(Map.ControlledGameObject.Position);
        }
    }
}
