using GoRogue;
using Roguelike.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Roguelike.Systems
{
    internal class EventManager
    {
        /// <summary>
        /// Fired when any actor moves
        /// </summary>
        public event Action<Actor, Coord, Coord> ActorMovedEvent;
        /// <summary>
        /// Fired when an actor dies
        /// </summary>
        public event Action<Actor> ActorDiedEvent;
        /// <summary>
        /// Fired when an actor's health value was modified
        /// </summary>
        public event Action<Actor, double> ActorHealthChangedEvent;
        /// <summary>
        /// Fires when an actor attacks another actor. TODO: Include attack data as needed
        /// </summary>
        public event Action<Actor, Actor> ActorAttackEvent;
        /// <summary>
        /// Fires every time the player's turn ends
        /// </summary>
        public event Action PlayerTurnEndedEvent;
        /// <summary>
        /// Fires when the player is created
        /// </summary>
        public event Action<Player> PlayerSpawnedEvent;

        //

        private EventManager()
        {
            //
        }

        public void Reset()
        {
            ActorMovedEvent = null;
            ActorAttackEvent = null;
            ActorDiedEvent = null;
            ActorHealthChangedEvent = null;
            PlayerSpawnedEvent = null;
            PlayerTurnEndedEvent = null;
        }

        public void InvokeActorAttacked(Actor attacker, Actor target)
        {
            if (ActorAttackEvent != null)
            {
                ActorAttackEvent(attacker, target);
            }
        }

        public void InvokeActorMoved(Actor actor, Coord oldPosition, Coord newPosition)
        {
            if (ActorMovedEvent != null)
            {
                ActorMovedEvent(actor, oldPosition, newPosition);
            }
        }

        public void InvokeActorDied(Actor actor)
        {
            if (ActorDiedEvent != null)
            {
                ActorDiedEvent(actor);
            }
            MyGame.World.CurrentMap.RemoveEntity(actor);
        }

        public void InvokeActorHealthChanged(Actor actor, double prevHealth)
        {
            if (ActorHealthChangedEvent != null)
            {
                ActorHealthChangedEvent(actor, prevHealth);
            }
        }

        public void InvokePlayerTurnEnded()
        {
            if (PlayerTurnEndedEvent != null)
            {
                PlayerTurnEndedEvent();
            }
        }

        public void InvokePlayerSpawn(Player player)
        {
            if (PlayerSpawnedEvent != null)
            {
                PlayerSpawnedEvent(player);
            }
        }


        // static

        private static readonly EventManager instance = new EventManager();

        // Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
        static EventManager() { }

        public static EventManager Instance { get { return instance; } }
    }
}
