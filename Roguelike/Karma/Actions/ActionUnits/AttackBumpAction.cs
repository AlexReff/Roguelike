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
        private Actor _actionCreatedTarget;

        public AttackBumpAction(Actor attacker, Direction dir) : base(attacker)
        {
            _dir = dir;
            _targetPos = attacker.Position + dir;
            _actionCreatedTarget = attacker.CurrentMap.GetEntityAt<Actor>(_targetPos);
        }

        public override long GetDelay()
        {
            // delay between starting an attack and the attack landing
            if (!_started)
            {
                return Actor.KarmaReactionSpeed;
            }
            if (Actor.State == ActorState.Moving)
            {
                return Actor.KarmaMoveSpeed;
            }
            return Actor.KarmaActionSpeed;
        }

        public override void Perform()
        {
            // check to make sure the target still exists
            Actor target = Actor.CurrentMap.GetEntityAt<Actor>(_targetPos);
            if (_actionCreatedTarget != null && (target == null || target.ID != _actionCreatedTarget.ID))
            {
                // we lost whatever target we had...
                BecameInvalid = true;
                return;
            }

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
                    BecameInvalid = true;
                }
                else
                {
                    // resolve the attack
                    var attack = Actor.ResolveAttack(target);
                    target.ResolveDefense(Actor, attack);
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
                    BecameInvalid = true;
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
        }
    }
}
