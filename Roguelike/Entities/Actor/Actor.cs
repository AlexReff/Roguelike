using GoRogue;
using GoRogue.Pathing;
using Microsoft.Xna.Framework;
using Roguelike.Entities.Items;
using Roguelike.Interfaces;
using Roguelike.JSON;
using Roguelike.Karma.Actions;
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
        // static
        //private static Dictionary<long, Actor> _actors;
        //public static List<Actor> AllActors { get { return _actors.Values.ToList(); } }
        //public static Actor GetActor(long id)
        //{
        //    if (_actors.ContainsKey(id))
        //    {
        //        return _actors[id];
        //    }

        //    return null;
        //}

        //static Actor()
        //{
        //    _actors = new Dictionary<long, Actor>();
        //}

        // non-static

        public string Faction { get; set; }
        public string Description { get; set; }

        /// <summary>
        /// Which direction the actor is currently facing
        /// </summary>
        public Direction FacingDirection { get; set; }
        public XYZRelativeDirection VisionDirection { get; set; }
        public Coord SpawnPoint { get; set; }

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
            double actionSpeed,
            double moveSpeed,
            double awareness,
            double innerFovAwareness,
            bool hasVision,
            double fovViewAngle,
            string faction,
            XYZRelativeDirection visionDirection,
            string bodyType,
            Color fgColor,
            Color bgColor,
            char icon,
            Coord position,
            int layer,
            bool isWalkable,
            bool isTransparent
            ) : base(name, fgColor, bgColor, icon, position, layer, isWalkable, isTransparent)
        {
            SpawnPoint = position;
            State = ActorState.Idle;

            CurrentLevel = 1;
            ActionQueue = new Queue<ActionUnit>();
            InterruptQueuedActions = false;

            VisibleActors = new HashSet<Actor>();
            VisibleEnemies = new HashSet<Actor>();

            Inventory = new List<Item>();
            Skills = new List<Skill>();
            FacingDirection = Direction.UP;
            VisionDirection = visionDirection;

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

            Faction = faction;

            switch (bodyType)
            {
                case "Humanoid":
                default:
                    Body = ActorBody.HumanoidBody(fgColor);
                    break;
            }

            if (Body != null)
            {
                Body.Parent = this;
            }

            //_actors.Add(this.ID, this);
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
                MyGame.Karma.Stop();
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
