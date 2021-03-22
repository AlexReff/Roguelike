using GoRogue;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Karma
{
    internal struct KarmaCombinedState : IKarmaCombinedState
    {
        //actor state
        public Coord? TargetPosition { get; set; }

        public bool AtTargetPosition { get; set; }

        public bool IdleInPlace { get; set; }

        public bool AttackHostile { get; set; }

        //world state


        public KarmaCombinedState(IKarmaGoalState actorState, IKarmaWorldState worldState) : this(actorState.TargetPosition, actorState.AtTargetPosition, actorState.IdleInPlace, actorState.AttackHostile)
        {
            //
        }

        public KarmaCombinedState(Coord? targetPosition, bool atTargetPosition, bool idleInPlace, bool attackHostile)
        {
            this.TargetPosition = targetPosition;
            this.AtTargetPosition = atTargetPosition;
            this.IdleInPlace = idleInPlace;
            this.AttackHostile = attackHostile;
        }

        public KarmaCombinedState Clone()
        {
            Coord? targetPos = null;
            if (TargetPosition.HasValue)
            {
                targetPos = new Coord(TargetPosition.Value.X, TargetPosition.Value.Y);
            }
            return new KarmaCombinedState(targetPos, AtTargetPosition, IdleInPlace, AttackHostile);
        }
    }

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
