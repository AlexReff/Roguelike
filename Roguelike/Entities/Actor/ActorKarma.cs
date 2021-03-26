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
        public Queue<ActionUnit> ActionQueue { get; }
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

        /// <summary>
        /// Turn rate in ticks/45deg
        /// 0 = instant turn.
        /// 1 = 1 ticks / 45 deg turn, 2 ticks / 90 deg.
        /// 2 = 2 ticks / 45 deg turn, 4 ticks / 90 deg.
        /// </summary>
        public long KarmaTurnSpeed
        {
            get
            {
                return 1;
            }
        }

        #region Commands
        // commands are directly from command mgr, user input
        // UI manager -> command manager -> Command$$$()

        /// <summary>
        /// Queue's a turn and a move (no attack)
        /// </summary>
        public void QueueActionTurnAndMove(Direction direction)
        {
            ActionQueue.Enqueue(new TurnAction(this, direction));
            ActionQueue.Enqueue(new MoveDirectionAction(this, direction));

            ////InterruptQueuedActions = false;

            //// if needed, this will turn the actor (scheduling)
            //// then schedule the move action as needed
            //CommandFaceDirection(direction);

            //Coord target = Position + direction;
            //if (!CurrentMap.WalkabilityView[target])
            //{
            //    // can't walk to this position
            //    return false;
            //}

            //var actor = CurrentMap.GetEntityAt<Actor>(target);
            //if (actor != null)
            //{
            //    if (IsHostileTo(actor))
            //    {
            //        //Command Attack
            //        return QueueBumpAttack(direction);
            //    }
            //}

            //// spot is walkable and no hostile in place

            //// do the move
            //StartMove(direction);
            ////DoMove(direction);
            //return true;
        }

        //<returns>True if a turn has been scheduled, false if no turn required or no action scheduled</returns>
        /// <summary>
        /// Adds a 'turn' action to the queue
        /// </summary>
        public void QueueTurn(Direction dir)
        {
            ActionQueue.Enqueue(new TurnAction(this, dir));


            //if (FacingDirection == dir)
            //{
            //    // already facing this direction
            //    State = ActorState.Idle;
            //    return false;
            //}

            //State = ActorState.Turning;
            // TurnAction will repeat until at the target direction
            //ActionQueue.Enqueue(new TurnAction(this, dir));
            //MyGame.Karma.AddAfterLast(1, this);

            //var clockwise = new List<Direction>() { FacingDirection + 1 };
            //var counterCw = new List<Direction>() { FacingDirection - 1 };

            //List<Direction> turnSteps;

            //while (true)
            //{
            //    if (clockwise[clockwise.Count - 1] == dir)
            //    {
            //        turnSteps = clockwise;
            //        break;
            //    }
            //    else if (counterCw[counterCw.Count - 1] == dir)
            //    {
            //        turnSteps = counterCw;
            //        break;
            //    }
            //    clockwise.Add(clockwise[clockwise.Count - 1] + 1);
            //    counterCw.Add(counterCw[counterCw.Count - 1] - 1);
            //}

            //foreach (var turn in turnSteps)
            //{
            //    QueuedActions.Enqueue(new TurnAction(this, turn));
            //    MyGame.Karma.AddAfterLast(1, this);
            //}

            //return true;
        }

        ///// <summary>
        ///// Command a turn
        ///// </summary>
        ///// <returns></returns>
        //public bool CommandFaceDirection(Direction direction)
        //{
        //    return QueueTurn(direction);
        //}

        /// <summary>
        /// Command an attack
        /// </summary>
        /// <returns>True if a bump attack was attempted, false if nothing happened</returns>
        public void QueueTurnAndBumpAttack(Direction direction)
        {
            QueueTurn(direction);
            QueueBumpAttack(direction);

            // do the attack
            //Coord target = Position + direction;
            //FacingDirection = direction;

            //Actor otherActor = CurrentMap.GetEntityAt<Actor>(target);

            //if (otherActor != null)
            //{
            //    // if an enemy is in the way, kill it!
            //    // could be refactored to move around or through the enemy if there is a 'higher priority' move or attack order (in future versions)
            //    if (this.IsHostileTo(otherActor))
            //    {
            //        StartBumpAttack(otherActor);
            //        return true;
            //    }
            //}

            //return false;
        }

        public void QueueBumpAttack(Direction dir)
        {
            ActionQueue.Enqueue(new AttackBumpAction(this, dir));
        }

        #endregion Commands
    }
}
