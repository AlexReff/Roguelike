using GoRogue;
using Roguelike.Karma.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roguelike.Entities
{
    internal partial class Actor
    {
        /// <summary>
        /// Used for things like turn->move, or turn->attack.
        /// Generally, for actions that require one user input but generate successive sub-actions
        /// </summary>
        public Queue<MultiStageAction> QueuedActions { get; }
        public KarmaAction CurrentAction { get; set; }

        /// <summary>
        /// This bool will be set when an event happens to the actor before the actor could finish all queued actions
        /// </summary>
        public bool InterruptQueuedActions { get; set; }

        /// <summary>
        /// Standard value for scheduling future actions after finishing an action or not performing an action
        /// </summary>
        /// <returns>Ticks</returns>
        public long KarmaReactionSpeed
        {
            get
            {
                return (long)Math.Floor(ActionSpeed * 2);
            }
        }

        /// <summary>
        /// Standard value for scheduling movement. Default is 'walk'
        /// </summary>
        /// <returns>Ticks</returns>
        public long KarmaMoveSpeed
        {
            get
            {
                return (long)Math.Floor(MoveSpeed * 6);
            }
        }

        /// <summary>
        /// Standard value for scheduling actions, attacks, etc
        /// </summary>
        /// <returns>Ticks</returns>
        public long KarmaActionSpeed
        {
            get
            {
                return (long)Math.Floor(ActionSpeed * 3);
            }
        }

        #region Commands
        // commands are directly from command mgr, user input
        // UI manager -> command manager -> Command$$$()

        /// <summary>
        /// Master function for moving an actor
        /// </summary>
        /// <returns>True if something happened, false if nothing happened</returns>
        public bool CommandMove(Direction direction)
        {
            InterruptQueuedActions = false;
            // if needed, this will turn the actor (scheduling)
            // then schedule the move action as needed
            CommandFaceDirection(direction);

            Coord target = Position + direction;
            if (!CurrentMap.WalkabilityView[target])
            {
                // can't walk to this position
                return false;
            }

            var actor = CurrentMap.GetEntityAt<Actor>(target);
            if (actor != null)
            {
                if (IsHostileTo(actor))
                {
                    //Command Attack
                    return CommandBumpAttack(direction);
                }
            }

            // spot is walkable and no hostile in place

            // do the move
            StartMove(direction);
            //DoMove(direction);
            return true;
        }

        /// <summary>
        /// Command a turn
        /// </summary>
        /// <returns></returns>
        public bool CommandFaceDirection(Direction direction)
        {
            return StartTurn(direction);
        }

        /// <summary>
        /// Command an attack
        /// </summary>
        /// <returns>True if a bump attack was attempted, false if nothing happened</returns>
        public bool CommandBumpAttack(Direction direction)
        {
            // do the attack
            Coord target = Position + direction;
            //FacingDirection = direction;

            Actor otherActor = CurrentMap.GetEntityAt<Actor>(target);

            if (otherActor != null)
            {
                // if an enemy is in the way, kill it!
                // could be refactored to move around or through the enemy if there is a 'higher priority' move or attack order (in future versions)
                if (this.IsHostileTo(otherActor))
                {
                    StartBumpAttack(otherActor);
                    return true;
                }
            }

            return false;
        }

        #endregion Commands
    }
}
