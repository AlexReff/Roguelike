using GoRogue;
using Roguelike.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Karma.Actions
{
    internal abstract class KarmaAction
    {
        //static
        public static Dictionary<string, KarmaAction> Actions { get; private set; }

        static KarmaAction()
        {
            Actions = new Dictionary<string, KarmaAction>();
        }



        //non-static

        /// <summary>
        /// ID used to match available actions from json
        /// </summary>
        public string Name { get; }
        public Dictionary<string, object> Conditions { get; }
        public Dictionary<string, object> Effects { get; }

        /// <summary>
        /// Perform the current action for the given actor
        /// </summary>
        /// <param name="actor"></param>
        /// <returns></returns>
        public abstract bool Perform(NPC actor);

        /// <summary>
        /// Checks preconditions to see if this action is still 'valid' for the given actor
        /// </summary>
        /// <param name="actor"></param>
        /// <returns></returns>
        public abstract bool IsValid(NPC actor);

        /// <summary>
        /// Maybe not needed (success/fail callbacks instead?)
        /// </summary>
        /// <param name="actor"></param>
        /// <returns></returns>
        public abstract bool IsCompleted(NPC actor);

        /// <summary>
        /// Needs to be called when action preconditions become invalid
        /// </summary>
        /// <param name="actor"></param>
        public virtual void Invalidate(NPC actor) { }


        //public abstract KarmaGoalState GetEffects()

        public KarmaAction(string name)
        {
            Name = name;

            Conditions = new Dictionary<string, object>();
            Effects = new Dictionary<string, object>();

            Actions.Add(name, this);
        }
        
        //protected Action<IReGoapAction<T, W>> doneCallback;
        //protected Action<IReGoapAction<T, W>> failCallback;
        //protected IReGoapAction<T, W> previousAction;
        //protected IReGoapAction<T, W> nextAction;
    }
}
