using GoRogue;
using Roguelike.Karma;
using Roguelike.Karma.Actions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Entities
{
    internal partial class NPC : IKarmaGoalState
    {
        //Karma-related goal values
        public Coord? TargetPosition { get; set; }
        
        public bool AtTargetPosition { get { return TargetPosition != null && Position != null && TargetPosition == Position; } }

        /// <summary>
        /// Goal to idle in a specific place (TargetPosition required)
        /// </summary>
        public bool IdleInPlace { get; set; }

        /// <summary>
        /// Attack nearest (or biggest threat?) hostile
        /// </summary>
        public bool AttackHostile { get; set; }

        //public bool CanSeePlayer { get; set; }
        //public bool AlertedToPlayer { get; set; }
        //public bool NearPlayer { get; set; }
        //public bool NextToPlayer { get; set; }
        //public bool InAttackRangeOfPlayer { get; set; }
        //public bool Fleeing { get; set; }
        //public bool IsSafeDistance { get; set; }
    }
}
