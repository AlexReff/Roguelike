using GoRogue;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Karma
{
    internal interface IKarmaGoalState
    {
        public Coord? TargetPosition { get; }
        public bool AtTargetPosition { get; }

        //goals
        public bool IdleInPlace { get; }
        public bool AttackHostile { get; }

        //public bool CanSeePlayer { get; set; }
        //public bool AlertedToPlayer { get; set; }
        //public bool NearPlayer { get; set; }
        //public bool NextToPlayer { get; set; }
        //public bool InAttackRangeOfPlayer { get; set; }
        
        //public bool Fleeing { get; set; }
        //public bool IsSafeDistance { get; set; }

        /*
         * IsHungry
         * IsTerrified
         */
    }

    internal interface IKarmaWorldState
    {
        //
    }

    internal interface IKarmaCombinedState : IKarmaGoalState, IKarmaWorldState
    {
        //potentially add 'faction' state at some point?
    }

    //internal class KarmaGoalState : IKarmaGoalState
    //{
    //    public Coord? TargetPosition { get; set; }
    //    public Coord? CurrentPosition { get; set; }
    //    public bool AtTargetPosition { get { return TargetPosition != null && CurrentPosition != null && TargetPosition == CurrentPosition; } }
    //    public bool IdleInPlace { get; set; }

    //    //public bool CanSeePlayer { get; set; }
    //    //public bool AlertedToPlayer { get; set; }
    //    //public bool NearPlayer { get; set; }
    //    //public bool NextToPlayer { get; set; }
    //    //public bool InAttackRangeOfPlayer { get; set; }
    //    //public bool Fleeing { get; set; }
    //    //public bool IsSafeDistance { get; set; }

    //    public KarmaGoalState()
    //    {
    //        //
    //    }
    //}
}
