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
        public IdleInPlaceAction() : base("IdleInPlace")
        {
            Conditions.Add("AtTargetPosition", true);
            Effects.Add("IdleInPlace", true);
        }

        public override bool IsCompleted(NPC actor)
        {
            return actor.AtTargetPosition;
        }

        /// <summary>
        /// AKA "Check Procedural Preconditions"
        /// </summary>
        /// <param name="actor"></param>
        /// <returns></returns>
        public override bool IsValid(NPC actor)
        {
            return actor.TargetPosition != null && !actor.AtTargetPosition;
        }

        public override bool Perform(NPC actor)
        {
            MyGame.Karma.Add(actor.ActionSpeed, actor);
            return true;
        }
    }
}
