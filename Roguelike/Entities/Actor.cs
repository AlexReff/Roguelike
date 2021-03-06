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
using System.Linq;
using System.Text;

namespace Roguelike.Entities
{
    public class Actor : MyBasicEntity
    {
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

        private int _currentLevel;
        public int CurrentLevel
        {
            get { return _currentLevel; }
            set
            {
                _currentLevel = System.Math.Max(1, value);
            }
        }

        private double _currency;
        public double Currency
        {
            get { return _currency; }
            set
            {
                var prevCurrency = _currency;
                _currency = System.Math.Max(0, System.Math.Round(value, 2));
                if (this is Player)
                {
                    PlayerMessageManager.Instance.AddMessage(new PlayerMessage($"Acquired {_currency - prevCurrency} gold", MessageCategory.Notification));
                }
            }
        }

        public double MaxHealth { get; protected set; }

        private double _health;
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

        private double _mana;
        public double Mana
        {
            get { return _mana; }
            set
            {
                //cannot have negative mana
                _mana = System.Math.Max(0, value);
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
                    //PlayerMessageManager.Instance.AddMessage(new PlayerMessage($"Encountered a {otherActor.Name}", MessageCategory.Notification));

                    if (this.IsHostileTo(otherActor))
                    {
                        BumpAttack(otherActor);
                    }

                    allowMovement = false;
                }
                else if (item != null)
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

        public IEnumerable<Weapon> GetEquippedWeapons()
        {
            return Body.Hands.FindAll(m => m.IsHoldingItem && m.HeldItem is Weapon).Select(m => (Weapon)m.HeldItem);
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

            /* 
             * 1-100 potential attributes/skill?
             * 
             */
            var attackResult = Helpers.Helpers.RandomGenerator.NextDouble();
            if (attackResult <= 0.001)
            {
                //catastrophic failure

                //needs mitigation (luck?)
            }
            else if (attackResult <= 0.01)
            {
                //failure?
            }
            else if (attackResult <= 0.1)
            {
                //miss?
            }

            var dmgRnd = Helpers.Helpers.RandomGenerator.NextDouble();

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

        public double ResolveAttack()
        {
            return 0;
        }

        public AttackInstance GetBestAttack(Actor target)
        {
            var result = new AttackInstance("Normal Attack", 0, 20, target.Body.Limbs[0].Name, 1, null, null);
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

        public void DropAllItems()
        {
            foreach (var item in Inventory)
            {
                DropItem(item);
            }
        }

        public void DropItem(uint id)
        {
            var item = Inventory.FirstOrDefault((inv) => inv.ID == id);
            if (item != null && item.ID == id)
            {
                DropItem(item);
            }
        }

        public void DropItem(Item item)
        {
            item.Position = Position;
            this.CurrentMap.AddEntity(item);
            Inventory.Remove(item);
        }

        public void AddItem(Item item)
        {
            if (item is Currency)
            {
                this.Currency += ((Currency)item).Amount;
            }
            else
            {
                this.Inventory.Add(item);
            }
        }

        public void PickupItem(Item item)
        {
            if (item.CurrentMap != null)
            {
                item.CurrentMap.RemoveEntity(item);
            }
            AddItem(item);
        }

        public void AddCurrency(double amount)
        {
            if (amount > 0)
            {
                Currency += amount;
            }
        }

        public void DropCurrency(double amount)
        {
            if (Currency >= amount)
            {
                Currency -= amount;
                this.CurrentMap.AddEntity(new Currency(amount, Position));
            }
            else if (Currency > 0)
            {
                this.CurrentMap.AddEntity(new Currency(Currency, Position));
                Currency = 0;
            }
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

            // drop full inventory on death
            DropAllItems();

            // drop all currency on death
            DropCurrency(double.MaxValue);

            this.CurrentMap.RemoveEntity(this);
        }
    }
}
