using GoRogue;
using Roguelike.Attacks;
using Roguelike.Entities.Items;
using Roguelike.JSON;
using Roguelike.Karma.Actions;
using Roguelike.Systems;
using Roguelike.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Roguelike.Entities
{
    internal partial class Actor
    {
        ///// <summary>
        ///// What an actor attacks with if no weapon is equipped. eg fists, claws, teeth
        ///// </summary>
        //public object DefaultWeapon { get; set; }
        public Actor CurrentTarget { get; set; }
        public bool UnderAttack { get { return _lastAttackedTime.HasValue && MyGame.Karma.CurrentTime - _lastAttackedTime.Value < Karma.KarmaSchedule.TicksPerMinute / 2; } }
        private long? _lastAttackedTime;


        public IEnumerable<Weapon> EquippedWeapons
        {
            get
            {
                return Body.Hands.FindAll(m => m.IsHoldingItem && m.HeldItem is Weapon).Select(m => (Weapon)m.HeldItem);
            }
        }

        /// <summary>
        /// Executes an attack, and returns the outgoing result for the defender to resolve.
        /// Only modifies the attacker. ResolveDefense modifies the defender
        /// </summary>
        public AttackInstance ResolveAttack(Actor target)
        {
            //if (this is Player || target is Player || MyGame.World.Player.VisibleActors.Contains(target))
            //{
            //    PlayerMessageManager.Instance.AddMessage($"{Name} barely misses {target.Name}({target.State})");
            //}
            // TODO: do attack math things
            long dmg = (long)Math.Round(Helpers.RandomGenerator.NextDouble() * (Strength + Agility) * .8);
            
            if (dmg > 0 && (target is Player || this is Player))
            {
                DebugManager.Instance.AddMessage($"{Name} attacks {target.Name} for {dmg}dmg");
            }

            EventManager.Instance.InvokeActorAttacked(this, target);
            State = ActorState.Recovering;

            AttackInstance attack = new AttackInstance();

            return attack;
        }

        /// <summary>
        /// Resolve an incoming attack, modify health and status as needed.
        /// </summary>
        public void ResolveDefense(Actor attacker, AttackInstance attack)
        {
            InterruptQueuedActions = true;
            SensesHostiles = true;

            if (CurrentTarget == null)
            {
                CurrentTarget = attacker;
            }

            switch (attack.DamageType)
            {
                default:
                    Health -= attack.DamageValue;
                    break;
            }

            // process an incoming attack

            _lastAttackedTime = MyGame.Karma.CurrentTime;
        }

        public bool IsHostileTo(Actor otherActor)
        {
            ////make everyone hostile to the player, for debugging purposes
            //if (this is Player || otherActor is Player)
            //{
            //    return true;
            //}

            if (Data.Factions[Faction].HostileWith.Contains(otherActor.Faction) ||
                Data.Factions[otherActor.Faction].HostileWith.Contains(Faction))
            {
                return true;
            }

            return false;
        }

    }
}
