using GoRogue;
using Roguelike.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Karma.Actions
{
    /// <summary>
    /// KarmaAction Perform() must Queue ActionUnits
    /// </summary>
    internal abstract class KarmaAction
    {
        public Actor Actor { get; }

        /// <summary>
        /// Ref_Actions::Name
        /// </summary>
        public string Name { get; }
        
        public double Cost { get; }

        public Dictionary<string, object> Conditions { get; }
        public Dictionary<string, object> Effects { get; }

        /// <summary>
        /// Perform queues one or more ActionUnit on the Actor.
        /// These units will be sequentially completed until returning to this action (if IsCompleted==false)
        /// </summary>
        /// <returns>TRUE if at least one ActionUnit was queued, FALSE if nothing happens</returns>
        public abstract bool Perform();

        /// <summary>
        /// Checks preconditions to see if this action is still 'valid' for the given actor. Does not fail for out of range (use IsInValidRange to move actor)
        /// </summary>
        /// <returns></returns>
        public abstract bool IsValid();

        /// <summary>
        /// Maybe not needed (success/fail callbacks instead?)
        /// </summary>
        /// <returns></returns>
        public abstract bool IsCompleted();

        /// <summary>
        /// Should be used to initially determine where the NPC is trying to go
        /// </summary>
        /// <returns>Null if irrelevant, eg infinite range or cast-on-self</returns>
        public virtual Coord? GetTargetPosition() => null;

        /// <summary>
        /// Needs to be called when action preconditions become invalid
        /// </summary>
        public virtual void Invalidate() { }

        /// <summary>
        /// Resets the action to 'default' state, in case there are variables or other things that need to be cleared when re-used in a new plan
        /// </summary>
        public virtual void Reset() { }

        /// <summary>
        /// If this fails, the AI will attempt to get in range before running this action
        /// </summary>
        /// <returns>False if this actor requires movement before being able to do this action</returns>
        public virtual bool IsInValidRange()
        {
            if (!(Actor is NPC))
            {
                return true;
            }

            NPC npc = Actor as NPC;
            
            Coord? targetPos = null;
            if (npc.TargetPosition.HasValue)
            {
                targetPos = npc.TargetPosition.Value;
            }
            else if (npc.CurrentAction != null)
            {
                var actPos = npc.CurrentAction.GetTargetPosition();
                if (actPos.HasValue)
                {
                    targetPos = actPos.Value;
                }
            }

            if (targetPos != null && targetPos.HasValue)
            {
                return Distance.EUCLIDEAN.Calculate(Actor.Position, targetPos.Value) <= GetRange();
            }

            // There is no 'target position', so default return 'true' since there is no possible range to calculate
            return true;
        }

        /// <summary>
        /// Returns how far 'in distance' from the npc's targetPosition we need to be to use this action.
        /// Defaults to 1.5 (all 8 neighboring squares). 1.0 would be just N,E,S,W squares. 0.5 for on-square.
        /// </summary>
        public virtual double GetRange() => 1.5;


        public KarmaAction(string name, Actor actor, double cost)
        {
            Actor = actor;
            Name = name;
            Cost = cost;

            Conditions = new Dictionary<string, object>();
            Effects = new Dictionary<string, object>();
        }
    }
}
