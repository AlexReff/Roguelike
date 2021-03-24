using GoRogue;
using GoRogue.Pathing;
using Roguelike.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Karma.Actions
{
    internal class GetInRangeAction : KarmaAction
    {
        private double _range;

        public GetInRangeAction(NPC actor, double range) : base("GetInRange", actor, 0)
        {
            _range = range;
        }

        public override Coord? GetTargetPosition()
        {
            return (Actor as NPC).TargetPosition;
        }

        public override bool IsCompleted()
        {
            //measure distance to see if we are in range
            if ((Actor as NPC).TargetPosition.HasValue)
            {
                return Distance.EUCLIDEAN.Calculate(Actor.Position, (Actor as NPC).TargetPosition.Value) <= _range;
            }

            // return true to cancel this action - we don't have a target!
            return true;
        }

        public override bool IsValid()
        {
            return (Actor as NPC).TargetPosition.HasValue && !IsCompleted();
        }

        public override bool Perform()
        {
            if (IsCompleted())
            {
                return true;
            }

            Direction targetDir = Actor.GetNextStepInPath(GetTargetPosition().Value);

            if (targetDir != null)
            {
                return Actor.CommandMove(targetDir);
            }

            return false;
        }
    }
}
