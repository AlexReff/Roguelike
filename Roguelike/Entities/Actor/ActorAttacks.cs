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


        public IEnumerable<Weapon> EquippedWeapons
        {
            get
            {
                return Body.Hands.FindAll(m => m.IsHoldingItem && m.HeldItem is Weapon).Select(m => (Weapon)m.HeldItem);
            }
        }

        public void StartBumpAttack(Actor target)
        {
            //get the default 'attack' and attempt to default-target hit the enemy
            //var bestAttack = GetDefaultAttack(target);
            //DoAttack(target, bestAttack);

            //DoAttack(target);
            QueueAttack(target);
        }

        public void QueueAttack(Actor target)
        {
            //if (this is Player || target is Player || MyGame.World.Player.VisibleActors.Contains(target))
            //{
            //    PlayerMessageManager.Instance.AddMessage($"{Name} swings at {target.Name}({target.State})");
            //}
            State = ActorState.Attacking;
            QueuedActions.Enqueue(new AttackBumpAction(this, target));
            MyGame.Karma.AddAfterLast(KarmaActionSpeed, this);

            //if (target is Player)
            //{
            //    MyGame.CommandManager.EndPlayerTurn();
            //}
            //TODO: 'attack swing speed': math based on weapon weight/size, str/agi
        }

        /// <summary>
        /// Performs an attack. TODO: add attack data
        /// </summary>
        public void ResolveAttack(Actor target)
        {
            //if (this is Player || target is Player || MyGame.World.Player.VisibleActors.Contains(target))
            //{
            //    PlayerMessageManager.Instance.AddMessage($"{Name} barely misses {target.Name}({target.State})");
            //}

            target.InterruptQueuedActions = true;
            EventManager.Instance.InvokeActorAttacked(this, target);
            State = ActorState.Recovering;
            
            //MyGame.Karma.Add(this);
        }

        ///// <summary>
        ///// Attempts to perform the selected attack
        ///// </summary>
        //public void DoAttack(Actor target, AttackAttempt attack)
        //{
        //    if (this is Player)
        //    {
        //        PlayerMessageManager.Instance.AddMessage(new PlayerMessage($"Attacked {target.Name}: {attack.Damage} dmg", MessageCategory.Notification));
        //    }
        //    else
        //    {
        //        DebugManager.Instance.AddMessage($"{this.Name} attacked {target.Name}: {attack.Damage} dmg");
        //    }

        //    var diceRoll = Helpers.RandomGenerator.NextDouble() * 100; //0-100.0 random value
        //    if (diceRoll <= 0.001)
        //    {
        //        //catastrophic failure
        //        //possible outcomes: serious damage to weapon, stumble?
        //        MyGame.CommandManager.DestroyEquippedWeapon(attack.Weapon.EquippedBy, attack.Weapon);
        //        //needs mitigation (luck?)
        //    }
        //    else if (diceRoll <= 1)
        //    {
        //        //total failure?
        //    }
        //    else if (diceRoll <= 15)
        //    {
        //        //grazing?
        //    }
        //    else if (diceRoll >= 99)
        //    {
        //        //critical+perfect rolls?
        //    }
        //    else if (diceRoll >= 95)
        //    {
        //        //critical?
        //    }

        //    var dmgRnd = Helpers.RandomGenerator.NextDouble();

        //    target.Health -= attack.Damage;
        //    this.Mana -= attack.ManaCost;

        //    if (attack.DoEffectTarget != null)
        //    {
        //        attack.DoEffectTarget(target);
        //    }
        //    if (attack.DoEffectCaster != null)
        //    {
        //        attack.DoEffectCaster(this);
        //    }
        //}

        //public AttackAttempt GetDefaultAttack(Actor target)
        //{
        //    /* eventually...
        //     * this will need to determine
        //     * what is the 'best' way to either 
        //     * 'kill' or 'neutralize' the enemy
        //     * things will need to be factored in,
        //     * such as... armor value, whether it's 'lifecritical'
        //     * whether we get bonuses to specific things, etc...
        //     * will need to assign 'costs' to different targets
        //     */
        //    /*
        //     * for now,
        //     * find the 'best' weapon equipped,
        //     * target the enemy's torso
        //     * (maybe player bump attack should always auto attack torso? encourage more interaction?)
        //     */

        //    List<object> AvailableAttackSkills = new List<object>();

        //    if (EquippedWeapons.Any())
        //    {
        //        List<Weapon> Weps = this.EquippedWeapons.ToList();
        //        //get the 'best' weapon?
        //    }
        //    if (this.Body.Hands.Any())
        //    {
        //        //
        //    }
        //    if (this.Body.Feet.Any())
        //    {
        //        //
        //    }

        //    var result = new AttackAttempt("Default Attack", 0, 20, target.Body.Limbs[0].Name, 1, null, null);
        //    //if (CanAttack(result, target))
        //    //{
        //    //    return result;
        //    //}

        //    //TODO: Select a new attack
        //    return result;
        //}

        //public List<AttackSkill> GetAttackSkills()
        //{
        //    List<AttackSkill> skills = new List<AttackSkill>();

        //    if (EquippedWeapons.Any())
        //    {
        //        List<Weapon> Weps = this.EquippedWeapons.ToList();
        //        //get the 'best' weapon?
        //    }
        //    if (this.Body.Hands.Any())
        //    {
        //        //
        //    }
        //    if (this.Body.Feet.Any())
        //    {
        //        //
        //    }

        //    return skills;
        //}

        //public bool CanAttack(AttackInstance targetAttack, Actor target)
        //{
        //    if (this.CurrentMap.DistanceMeasurement.Calculate(this.Position, target.Position) < (1 + targetAttack.Range))
        //    {
        //        return true;
        //    }

        //    return false;
        //}

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
