using GoRogue;
using Roguelike.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Karma.Actions
{
    internal abstract class KarmaAction
    {
        public NPC NPC { get; }

        /// <summary>
        /// Ref_Actions::Name
        /// </summary>
        public string Name { get; }
        
        public double Cost { get; set; }

        public Dictionary<string, object> Conditions { get; }
        public Dictionary<string, object> Effects { get; }

        /// <summary>
        /// Perform the current action for the given actor
        /// Perform must also Karma-Schedule the actor
        /// </summary>
        /// <returns></returns>
        public abstract bool Perform();

        /// <summary>
        /// Checks preconditions to see if this action is still 'valid' for the given actor
        /// </summary>
        /// <returns></returns>
        public abstract bool IsValid();

        /// <summary>
        /// Maybe not needed (success/fail callbacks instead?)
        /// </summary>
        /// <returns></returns>
        public abstract bool IsCompleted();

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
        public virtual bool IsInValidRange() => true;


        public KarmaAction(string name, NPC actor, double cost)
        {
            NPC = actor;
            Name = name;
            Cost = cost;

            Conditions = new Dictionary<string, object>();
            Effects = new Dictionary<string, object>();

            //Actions.Add(name, this);
        }
        
        //protected Action<IReGoapAction<T, W>> doneCallback;
        //protected Action<IReGoapAction<T, W>> failCallback;
        //protected IReGoapAction<T, W> previousAction;
        //protected IReGoapAction<T, W> nextAction;
    }
}
