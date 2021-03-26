using GoRogue;
using Roguelike.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Karma.Actions
{
    internal class AttackBumpAction : ActionUnit
    {
        private Direction _dir;
        private bool _started;
        private Coord _targetPos;

        public AttackBumpAction(Actor attacker, Direction dir) : base(attacker)
        {
            _dir = dir;
            _targetPos = attacker.Position + dir;
        }

        public override long GetDelay()
        {
            // delay between starting an attack and the attack landing
            return Actor.KarmaActionSpeed;
        }

        public override void Perform()
        {
            // check to make sure the target still exists
            Actor target = Actor.CurrentMap.GetEntityAt<Actor>(_targetPos);

            if (target != null && Actor.IsHostileTo(target))
            {
                // there is a hostile target on the target space
                if (!_started)
                {
                    // begin swinging at the enemy
                    _started = true;
                    Actor.State = ActorState.Attacking;
                }
                else if (Actor.State == ActorState.Moving)
                {
                    // we were trying to move, now there's an enemy!
                    IsComplete = true;
                }
                else
                {
                    // resolve the attack
                    Actor.ResolveAttack(target);
                    Actor.State = ActorState.Recovering;
                    IsComplete = true;
                }
            }
            else
            {
                // no enemy found
                if (!_started)
                {
                    // begin moving to the spot
                    _started = true;
                    Actor.State = ActorState.Moving;
                }
                else if (Actor.State == ActorState.Attacking)
                {
                    // we were trying to attack, but the enemy is no longer there!
                    IsComplete = true;
                }
                else
                {
                    // finish the move
                    if (Actor.CanMove(_dir))
                    {
                        Actor.DoMove(_dir);
                    }
                    IsComplete = true;
                }
            }
            
            //if (target == null || !Actor.IsHostileTo(target))
            //{
            //    // no valid target, bail out of this action
            //    IsComplete = true;
            //    return;
            //}

            //if (!_started)
            //{
            //    // start the attack
            //    Actor.State = ActorState.Attacking;
            //    _started = true;
            //}
            //else
            //{
            //    // TODO: math to determine if this attack is interrupted or completes
            //    //if (Actor.InterruptQueuedActions && mathResult) { IsComplete = true; return; }

            //    // resolve the attack
            //    Actor.ResolveAttack(target);
            //    Actor.State = ActorState.Recovering;
            //    IsComplete = true;
            //}
        }
    }
}
