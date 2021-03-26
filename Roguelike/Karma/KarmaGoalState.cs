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
        public bool IdleInArea { get; set; }

        public bool SensesHostiles { get; set; }
        public bool AttackHostiles { get; set; }
        public bool UnderAttack { get; set; }

        //world state


        public KarmaCombinedState(IKarmaGoalState actorState, IKarmaWorldState worldState) : this(actorState.TargetPosition, actorState.AtTargetPosition, actorState.IdleInPlace, actorState.IdleInArea, actorState.AttackHostiles, actorState.SensesHostiles, actorState.UnderAttack)
        {
            //
        }

        public KarmaCombinedState(Coord? targetPosition, bool atTargetPosition, bool idleInPlace, bool idleInArea, bool attackHostiles, bool sensesHostiles, bool underAttack)
        {
            this.TargetPosition = targetPosition;
            this.AtTargetPosition = atTargetPosition;
            this.IdleInPlace = idleInPlace;
            this.IdleInArea = idleInArea;
            this.AttackHostiles = attackHostiles;
            this.SensesHostiles = sensesHostiles;
            this.UnderAttack = underAttack;
        }

        public KarmaCombinedState Clone()
        {
            // create new structs/classes
            Coord? targetPos = null;
            if (TargetPosition.HasValue)
            {
                targetPos = new Coord(TargetPosition.Value.X, TargetPosition.Value.Y);
            }
            return new KarmaCombinedState(targetPos, AtTargetPosition, IdleInPlace, IdleInArea, AttackHostiles, SensesHostiles, UnderAttack);
        }
    }

    internal interface IKarmaGoalState
    {
        public Coord? TargetPosition { get; }

        // conditions
        public bool SensesHostiles { get; }
        public bool UnderAttack { get; }
        public bool AtTargetPosition { get; }

        // goals
        public bool IdleInPlace { get; }
        public bool IdleInArea { get; set; }
        public bool AttackHostiles { get; }

    }

    internal interface IKarmaWorldState
    {
        //
    }

    internal interface IKarmaCombinedState : IKarmaGoalState, IKarmaWorldState
    {
        //potentially add 'faction' state at some point?
    }

}
