using GoRogue;
using Roguelike.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Roguelike.Karma.Actions
{
    internal class TurnAction : ActionUnit
    {
        private Direction _dir;
        private Queue<Direction> _steps;
        private bool _started;

        public TurnAction(Actor actor, Direction dir) : base(actor)
        {
            _dir = dir;
            _steps = new Queue<Direction>();
        }

        public override long GetDelay() => Actor.KarmaTurnSpeed;

        public override void Perform()
        {
            if (Actor.FacingDirection == _dir)
            {
                BecameInvalid = true;
                return;
            }

            if (!_started)
            {
                _steps = GetSteps();
                _started = true;
            }

            if (_steps.Count == 0)
            {
                BecameInvalid = true;
                return;
            }

            Actor.State = ActorState.Turning;
            Actor.FacingDirection = _steps.Dequeue();

            if (Actor.FacingDirection == _dir || _steps.Count == 0)
            {
                IsComplete = true;
                return;
            }
        }

        private Queue<Direction> GetSteps()
        {
            var clockwise = new List<Direction>() { Actor.FacingDirection + 1 };
            var counterCw = new List<Direction>() { Actor.FacingDirection - 1 };

            List<Direction> turnSteps;

            while (true)
            {
                if (clockwise[clockwise.Count - 1] == _dir)
                {
                    turnSteps = clockwise;
                    break;
                }
                else if (counterCw[counterCw.Count - 1] == _dir)
                {
                    turnSteps = counterCw;
                    break;
                }
                clockwise.Add(clockwise[clockwise.Count - 1] + 1);
                counterCw.Add(counterCw[counterCw.Count - 1] - 1);
            }

            return new Queue<Direction>(turnSteps);
        }
    }
}
