using GoRogue;
using Roguelike.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Karma.Actions
{
    /// <summary>
    /// Stand still, chill, be cool
    /// </summary>
    internal class IdleInPlaceAction : KarmaAction
    {
        public IdleInPlaceAction(NPC actor, double cost) : base("IdleInPlace", actor, cost)
        {
            Conditions.Add("AtTargetPosition", true);
            Effects.Add("IdleInPlace", true);
        }

        public override Coord? GetTargetPosition()
        {
            return Actor.SpawnPoint;
        }

        public override bool IsCompleted()
        {
            return (Actor as NPC).AtTargetPosition;
        }

        /// <summary>
        /// AKA "Check Procedural Preconditions"
        /// </summary>
        public override bool IsValid()
        {
            return (Actor as NPC).TargetPosition != null && !(Actor as NPC).AtTargetPosition;
        }

        public override bool Perform()
        {
            return false;
        }

        public override double GetRange()
        {
            return .5;
        }
    }
}
