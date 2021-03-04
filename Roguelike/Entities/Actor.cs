using GoRogue;
using Microsoft.Xna.Framework;
using Roguelike.Entities.Items;
using Roguelike.Entities.Monsters;
using Roguelike.Helpers;
using SadConsole;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Entities
{
    public class Actor : MyBasicEntity
    {
        public char Glyph { get; protected set; }
        public bool HasVision { get; protected set; }
        public double MoveSpeed { get; protected set; }
        public int ActionSpeed { get; protected set; }

        /// <summary>
        /// Which direction the actor is currently facing
        /// </summary>
        public Direction FacingDirection { get; set; }

        public double MaxHealth { get; set; }
        public double Health { get; set; }
        public double Mana { get; set; }
        
        // Attributes
        public int Strength { get; protected set; }
        public int Agility { get; protected set; }
        public int Stamina { get; protected set; }
        public int Willpower { get; protected set; }
        public int Intelligence { get; protected set; }
        public int Vitae { get; protected set; }

        public List<Item> Inventory { get; protected set; }

        // Attack related conditions
        public bool IsOffBalance { get; protected set; }
        public bool IsStunned { get; protected set; }
        public bool IsEnraged { get; protected set; }


        public Actor(Color fgColor, Color bgColor, char icon, Coord position, int layer, bool isWalkable, bool isTransparent)
            : base(fgColor, bgColor, icon, position, layer, isWalkable, isTransparent)
        {
            Glyph = icon;
            Inventory = new List<Item>();
        }

        public Actor(Color fgColor, Color bgColor, char icon, Coord position, MapLayer layer, bool isWalkable, bool isTransparent)
            : this(fgColor, bgColor, icon, position, (int)layer, isWalkable, isTransparent) { }

        public bool MoveBy(Direction direction)
        {
            Coord target = Position + direction;
            if (MyGame.World.CurrentMap.IsTileWalkable(target))
            {
                bool allowMovement = true;

                //Monsters are marked WALKABLE to allow for pathfinding
                Monster monster = MyGame.World.CurrentMap.GetEntityAt<Monster>(target);
                Item item = MyGame.World.CurrentMap.GetEntityAt<Item>(target);
                if (monster != null)
                {
                    PlayerMessageManager.Instance.AddMessage(new PlayerMessage($"Encountered a {monster.Name}", MessageCategory.Notification));
                    //handle monster interaction
                    allowMovement = false;
                }
                else if (item != null)
                {
                    PlayerMessageManager.Instance.AddMessage(new PlayerMessage($"Encountered a {item.Name}", MessageCategory.Notification));
                    //handle item interaction
                    allowMovement = true;
                }

                if (allowMovement)
                {
                    DebugManager.Instance.AddMessage(new DebugMessage($"Actor {Glyph}, Walking to: {target}", DebugSource.System));

                    Position += direction;
                    return true;
                }
                else
                {
                    DebugManager.Instance.AddMessage(new DebugMessage($"Actor {Glyph}, failed walking to: {target}", DebugSource.System));
                    return false;
                }
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
