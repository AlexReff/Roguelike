using GoRogue;
using Roguelike.Karma;
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
        public bool SensesHostiles { get; set; }

        //

        /// <summary>
        /// This bool will be set when an event happens to the actor before the actor could finish all queued actions. Currently disabled
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
                //return (long)Math.Floor(ActionSpeed);
                return (long)Math.Floor((22 / (2 * ActionSpeed + 15) - .07) * KarmaSchedule.TicksPerSecond);
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
                //for coordinates (1, 1.5), (10, 1), (100, .1), functions are...
                // 1.69444 − 0.437946 ln ( 0.369299 x + 1.18961 )
                // 2.125 - .625x^(0.255273)
                /*
                 * 30/(x+16) - .15 = TicksPerSecond
                 */
                switch (MoveRate)
                {
                    case ActorMoveRate.Crouching:
                        return (long)Math.Floor((35 / (MoveSpeed + 20) - .15) * KarmaSchedule.TicksPerSecond * 1.4);
                    case ActorMoveRate.Jogging:
                        return (long)Math.Floor((20 / (MoveSpeed + 12) - .12) * KarmaSchedule.TicksPerSecond);
                    case ActorMoveRate.Sprinting:
                        return (long)Math.Floor((18 / (MoveSpeed + 13) - .14) * KarmaSchedule.TicksPerSecond);
                    case ActorMoveRate.Walking:
                    default:
                        return (long)Math.Floor((30 / (MoveSpeed + 16) - .15) * KarmaSchedule.TicksPerSecond);
                }
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
                return (long)Math.Floor((20 / (ActionSpeed + 11) - .14) * KarmaSchedule.TicksPerSecond);
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
        /// <summary>
        /// Queue's a turn and a move (no attack)
        /// </summary>
        public void QueueActionTurnAndMove(Direction direction)
        {
            ActionQueue.Enqueue(new TurnAction(this, direction));
            ActionQueue.Enqueue(new MoveDirectionAction(this, direction));
        }

        public void QueueTurn(Direction dir)
        {
            ActionQueue.Enqueue(new TurnAction(this, dir));
        }

        public void QueueBumpAttack(Direction dir)
        {
            ActionQueue.Enqueue(new AttackBumpAction(this, dir));
        }

        public void QueueTurnAndBumpAttack(Direction direction)
        {
            //// see if there is a hostile target, and set it.
            //// when the bump attack starts, check if we have a target,
            //// if we have a target, but there is no target, cancel and return control
            //var pos = Position + direction;
            //var hostile = CurrentMap.GetEntityAt<Actor>(pos);
            //if (hostile != null && IsHostileTo(hostile))
            //{
            //    CurrentTarget = hostile;
            //}
            QueueTurn(direction);
            QueueBumpAttack(direction);
        }

        #endregion Commands
    }
}
