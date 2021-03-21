using Roguelike.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Karma.Actions
{
    internal class GoToAction : KarmaAction
    {
        public GoToAction() : base("GoToAction")
        {
            Conditions.Add("AtTargetPosition", false);
            Effects.Add("AtTargetPosition", true);
        }

        public override bool IsCompleted(NPC actor)
        {
            return actor.AtTargetPosition;
        }

        public override bool IsValid(NPC actor)
        {
            return true;
        }

        public override bool Perform(NPC actor)
        {
            return true;
        }
    }
}
