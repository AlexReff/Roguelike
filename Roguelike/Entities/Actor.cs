using GoRogue;
using Microsoft.Xna.Framework;
using Roguelike.Helpers;
using SadConsole;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Entities
{
    internal class Actor : BasicEntity
    {
        private char Glyph;

        public Actor(Color fgColor, Color bgColor, char icon, Coord position, int layer, bool isWalkable, bool isTransparent)
            : base(fgColor, bgColor, icon, position, layer, isWalkable, isTransparent)
        {
            //FOVRadius = 10;
            Glyph = icon;
        }

        public Actor(Color fgColor, Color bgColor, char icon, Coord position, MapLayer layer, bool isWalkable, bool isTransparent)
            : this(fgColor, bgColor, icon, position, (int)layer, isWalkable, isTransparent) { }

        public bool MoveBy(Direction direction)
        {
            Coord target = Position + direction;
            if (MyGame.World.CurrentMap.IsTileWalkable(target))
            {
                DebugManager.Instance.AddMessage(new DebugMessage($"Actor {Glyph}, Walking to: {target}", DebugSource.System));
                if (this is Player)
                {
                    PlayerMessageManager.Instance.AddMessage(new PlayerMessage($"Moved to: {target}", MessageCategory.Movement));
                }
                //Monster monster = GameLoop.World.CurrentMap.GetEntityAt<Monster>(Position + positionChange);
                //Item item = GameLoop.World.CurrentMap.GetEntityAt<Item>(Position + positionChange);
                //if (monster != null)
                //{
                //    GameLoop.CommandManager.Attack(this, monster);
                //    return true;
                //}
                //// if there's an item here,
                //// try to pick it up
                //else if (item != null)
                //{
                //    GameLoop.CommandManager.Pickup(this, item);
                //    return true;
                //}

                Position += direction;
                return true;
            }
            else
            {
                DebugManager.Instance.AddMessage(new DebugMessage($"Actor {Glyph}, Unable to walk to: {target}", DebugSource.System));
                //// Check for the presence of a door
                //TileDoor door = GameLoop.World.CurrentMap.GetTileAt<TileDoor>(Position + positionChange);
                //// if there's a door here,
                //// try to use it
                //if (door != null)
                //{
                //    GameLoop.CommandManager.UseDoor(this, door);
                //    return true;
                //}
                return false;
            }
        }

        //// Attempts to move to a location
        //public bool MoveTo(Point positionTarget, bool skipAllChecks = false)
        //{
        //    if (skipAllChecks)
        //    {
        //        Position = positionTarget;
        //        return true;
        //    }

        //    return true;
        //}
    }
}
