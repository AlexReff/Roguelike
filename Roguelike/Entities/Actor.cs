using GoRogue;
using Microsoft.Xna.Framework;
using Roguelike.Attacks;
using Roguelike.Entities.Items;
using Roguelike.Entities.Monsters;
using Roguelike.Helpers;
using Roguelike.Models;
using Roguelike.Spells;
using SadConsole;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Entities
{
    public class Actor : MyBasicEntity
    {
        public int CurrentLevel { get; set; }
        public char Glyph { get; protected set; }
        public bool HasVision { get; protected set; }
        public double FOVRadius { get; protected set; }
        public XYZRelativeDirection VisionDirection { get; protected set; }
        public double MoveSpeed { get; protected set; }
        public int ActionSpeed { get; protected set; }

        /// <summary>
        /// Which direction the actor is currently facing
        /// </summary>
        public Direction FacingDirection { get; protected set; }
        public ActorBody Body { get; protected set; }

        public double MaxHealth { get; protected set; }

        private double _health { get; set; }
        public double Health
        {
            get { return _health; }
            set
            {
                _health = value;
                if (_health <= 0)
                    Die();
            }
        }

        private double _mana { get; set; }
        public double Mana
        {
            get { return _mana; }
            set
            {
                //cannot have negative mana
                _mana = System.Math.Max(0.0, value);
            }
        }

        // Attributes
        public int Strength { get; protected set; }
        public int Agility { get; protected set; }
        public int Stamina { get; protected set; }
        public int Willpower { get; protected set; }
        public int Intelligence { get; protected set; }
        public int Vitae { get; protected set; }

        public List<Item> Inventory { get; protected set; }

        public List<Spell> Spells { get; set; }

        // Attack related conditions
        //public List<AttackInstance> Attacks { get; protected set; }
        public bool IsOffBalance { get; protected set; }
        public bool IsStunned { get; protected set; }
        public bool IsEnraged { get; protected set; }


        public Actor(Color fgColor, Color bgColor, char icon, Coord position, int layer, bool isWalkable, bool isTransparent)
            : base(fgColor, bgColor, icon, position, layer, isWalkable, isTransparent)
        {
            Glyph = icon;
            CurrentLevel = 1;
            Inventory = new List<Item>();
            Spells = new List<Spell>();
            FacingDirection = Direction.UP;
            VisionDirection = XYZRelativeDirection.Forward;
        }

        public Actor(Color fgColor, Color bgColor, char icon, Coord position, MapLayer layer, bool isWalkable, bool isTransparent)
            : this(fgColor, bgColor, icon, position, (int)layer, isWalkable, isTransparent) { }

        public bool MoveBy(Direction direction)
        {
            Coord target = Position + direction;
            FacingDirection = direction;

            if (MyGame.World.CurrentMap.IsTileWalkable(target))
            {
                bool allowMovement = true;

                Actor otherActor = MyGame.World.CurrentMap.GetEntityAt<Actor>(target);
                Item item = MyGame.World.CurrentMap.GetEntityAt<Item>(target);
                if (otherActor != null)
                {
                    PlayerMessageManager.Instance.AddMessage(new PlayerMessage($"Encountered a {otherActor.Name}", MessageCategory.Notification));

                    if (this.IsHostileTo(otherActor))
                    {
                        BumpAttack(otherActor);
                    }

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
                    //return false;
                }
            }
            else
            {
                DebugManager.Instance.AddMessage(new DebugMessage($"Actor {Glyph}, Unable to walk to: {target}", DebugSource.System));
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

            MyGame.UIManager.GameScreen.MapScreen.UpdateFOV();
            return false;
        }

        public void BumpAttack(Actor target)
        {
            var bestAttack = GetBestAttack(target);
            DoAttack(target, bestAttack);
        }

        /// <summary>
        /// Performs the selected attack, DOES NO VALIDITY CHECKING
        /// </summary>
        public void DoAttack(Actor target, AttackInstance attack)
        {
            if (this is Player)
            {
                PlayerMessageManager.Instance.AddMessage(new PlayerMessage($"Attacked {target.Name}: {attack.Damage} dmg", MessageCategory.Notification));
            }
            else
            {
                DebugManager.Instance.AddMessage($"{this.Name} attacked {target.Name}: {attack.Damage} dmg");
            }

            target.Health -= attack.Damage;
            this.Mana -= attack.ManaCost;

            if (attack.DoEffectTarget != null)
            {
                attack.DoEffectTarget(target);
            }
            if (attack.DoEffectCaster != null)
            {
                attack.DoEffectCaster(this);
            }
        }

        public AttackInstance GetBestAttack(Actor target)
        {
            var result = new AttackInstance("Normal Attack", 0, 20, 1, null, null);
            if (CanAttack(result, target))
            {
                return result;
            }

            //TODO: Select a new attack
            return result;
        }

        public bool CanAttack(AttackInstance targetAttack, Actor target)
        {
            if (this.CurrentMap.DistanceMeasurement.Calculate(this.Position, target.Position) < (1 + targetAttack.Range))
            {
                return true;
            }

            return false;
        }

        public bool CanCastSpell(Spell spell, Actor target)
        {
            //Spell spell = GetSpell(spellId);
            //if (spell == null || spell.ID != spellId)
            //{
            //    DebugManager.Instance.AddMessage(new DebugMessage($"Player::CastSpell: Invalid ID: {spellId}", DebugSource.System));
            //    return false;
            //}
            if (this.CurrentMap.DistanceMeasurement.Calculate(this.Position, target.Position) < (1 + spell.Range))
            {
                return true;
            }

            return false;
        }

        public bool IsHostileTo(Actor otherActor)
        {
            //make everyone hostile to the player, for debugging purposes
            if (this is Player)
            {
                return true;
            }

            //everyone hates monsters, for debugging purposes
            if (otherActor is Monster)
            {
                return true;
            }

            //TODO: implement whenever factions + reputation exists
            return false;
        }

        public void Die()
        {
            if (this is Player)
            {
                DebugManager.Instance.AddMessage("Player died! Reset the game to continue.");
            }
            else
            {
                PlayerMessageManager.Instance.AddMessage(new PlayerMessage($"{this.Name} died", MessageCategory.Combat));
            }

            this.CurrentMap.RemoveEntity(this);
        }
    }
}
