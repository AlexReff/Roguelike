using GoRogue;
using Microsoft.Xna.Framework;
using Roguelike.Entities.Items;
using Roguelike.Entities.Monsters;
using Roguelike.Interfaces;
using Roguelike.Models;
using Roguelike.Skills;
using Roguelike.Systems;
using SadConsole;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roguelike.Entities
{
    internal partial class Actor : MyBasicEntity
    {
        private static Dictionary<uint, Actor> _actors;
        public static List<Actor> AllActors { get { return _actors.Values.ToList(); } }
        public static Actor GetActor(uint id)
        {
            if (_actors.ContainsKey(id))
            {
                return _actors[id];
            }

            return null;
        }

        static Actor()
        {
            _actors = new Dictionary<uint, Actor>();
        }

        protected int? _time;
        public virtual int Time
        {
            get
            {
                if (_time.HasValue)
                {
                    var val = _time.Value;
                    _time = null;
                    return val;
                }
                return ActionSpeed;
            }
        }

        public string Description { get; set; }

        /// <summary>
        /// Which direction the actor is currently facing
        /// </summary>
        public Direction FacingDirection { get; set; }
        public XYZRelativeDirection VisionDirection { get; set; }

        public ActorBody Body { get; set; }

        public Actor(
            string name,
            double maxHealth,
            double maxMana,
            int strength,
            int agility,
            int stamina,
            int willpower,
            int intelligence,
            int vitae,
            int actionSpeed,
            int moveSpeed,
            double awareness,
            double innerFovAwareness,
            bool hasVision,
            double fovViewAngle,
            XYZRelativeDirection visionDirection,
            ActorBody body,
            Color fgColor,
            Color bgColor,
            char icon,
            Coord position,
            int layer,
            bool isWalkable,
            bool isTransparent
            ) : base(name, fgColor, bgColor, icon, position, layer, isWalkable, isTransparent)
        {
            CurrentLevel = 1;
            Inventory = new List<Item>();
            Skills = new List<Skill>();
            FacingDirection = Direction.UP;
            VisionDirection = XYZRelativeDirection.Forward;

            MaxHealth = Health = maxHealth;
            MaxMana = Mana = maxMana;

            Strength = strength;
            Agility = agility;
            Stamina = stamina;
            Willpower = willpower;
            Intelligence = intelligence;
            Vitae = vitae;

            ActionSpeed = actionSpeed;
            MoveSpeed = moveSpeed;

            InnerFOVAwareness = innerFovAwareness;
            Awareness = awareness;
            HasVision = hasVision;
            FOVViewAngle = fovViewAngle;
            VisionDirection = VisionDirection;
            Body = body;

            if (Body != null)
            {
                Body.Parent = this;
            }

            _actors.Add(this.ID, this);
        }

        public bool MoveBump(Direction direction)
        {
            Coord target = Position + direction;
            FacingDirection = direction;

            //var feedbackMsg = "";

            Actor otherActor = MyGame.World.CurrentMap.GetEntityAt<Actor>(target);
            Item item = MyGame.World.CurrentMap.GetEntityAt<Item>(target);
            
            bool allowMovement = true;

            if (otherActor != null)
            {
                //PlayerMessageManager.Instance.AddMessage(new PlayerMessage($"Encountered a {otherActor.Name}", MessageCategory.Notification));

                if (this.IsHostileTo(otherActor))
                {
                    BumpAttack(otherActor);
                    return true;
                }

                allowMovement = false;
            }

            if (allowMovement && MyGame.World.CurrentMap.IsTileWalkable(target))
            {
                if (item != null)
                {
                    if (this is Player)
                    {
                        //PlayerMessageManager.Instance.AddMessage(new PlayerMessage($"Encountered a {item.Name}", MessageCategory.Notification));

                        if (item is Currency && MyGame.GameSettings.GoldAutoPickup)
                        {
                            PickupItem(item);
                        }
                    }

                    allowMovement = true;
                }

                if (allowMovement)
                {
                    //DebugManager.Instance.AddMessage(new DebugMessage($"Actor {Glyph}, Walking to: {target}", DebugSource.System));

                    Position += direction;
                    return true;
                }
                else
                {
                    //DebugManager.Instance.AddMessage(new DebugMessage($"Actor {Glyph}, failed walking to: {target}", DebugSource.System));
                    //return false;
                }
            }
            else
            {
                //DebugManager.Instance.AddMessage(new DebugMessage($"Actor {Glyph}, Unable to walk to non-walkable: {target}", DebugSource.System));
                //ImmediateFeedbackManager.Instance.AddMessage($"Cannot walk {direction.ToString().Replace("_", "").ToProperCase()}");
                //feedbackMsg = $"Cannot walk {direction.ToString().Replace("_", "").ToProperCase()}";
                //return false;

                //// Check for the presence of a door
                //TileDoor door = GameLoop.World.CurrentMap.GetTileAt<TileDoor>(Position + positionChange);
                //// if there's a door here,
                //// try to use it
                //if (door != null)
                //{
                //    GameLoop.CommandManager.UseDoor(this, door);
                //    return true;
                //}
            }

            //ImmediateFeedbackManager.Instance.AddMessage(feedbackMsg);
            //MyGame.UIManager.GameScreen.MapScreen.UpdateFOV();
            return false;
        }

        public void Die()
        {
            // drop full inventory on death
            DropAllItems();

            // drop all currency on death
            DropCurrency(this.Currency);

            IsDead = true;

            if (this is Player)
            {
                EventManager.Instance.InvokeActorDied(this);

                DebugManager.Instance.AddMessage("");
                DebugManager.Instance.AddMessage("[c:r f:red]Player died! Reset the game to continue.[c:u]");
                PlayerMessageManager.Instance.AddMessage("");
                PlayerMessageManager.Instance.AddMessage("[c:r f:red]You died! Game over.");
                MyGame.CommandManager.GameOver();
            }
            else
            {
                var colorStr = $"{this.ForegroundColor.R},{this.ForegroundColor.G},{this.ForegroundColor.B}";
                PlayerMessageManager.Instance.AddMessage(new PlayerMessage($"[c:r f:{colorStr}]{this.Name} [c:r f:red]died[c:u]", MessageCategory.Combat));
                this.CurrentMap.RemoveEntity(this);

                EventManager.Instance.InvokeActorDied(this);
            }

            //this.CurrentMap.RemoveEntity(this);
        }
    }
}
