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

        public override bool IsCompleted()
        {
            return NPC.AtTargetPosition;
        }

        /// <summary>
        /// AKA "Check Procedural Preconditions"
        /// </summary>
        public override bool IsValid()
        {
            return NPC.TargetPosition != null && !NPC.AtTargetPosition;
        }

        public override bool Perform()
        {
            MyGame.Karma.Add(NPC.ActionSpeed, NPC);
            return true;
        }
    }
}
