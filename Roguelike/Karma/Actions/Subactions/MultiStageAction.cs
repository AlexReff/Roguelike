using Roguelike.Entities;
using System;
using System.Collections.Generic;
using System.Text;

/*
 * While KarmAction is designed specifically for the Karma AI system,
 * MultiStageAction is like a 'child' designed for Actors  to handle 
 * the concept of having  sequences of states instead of immediate actions,
 * eg 'attacking' before attack lands and 'turning' while turning
 * this can potentially open up more strategy or combat options 
 * (eg use turn momentum to do a roundhouse kick as you face another target)
*/

namespace Roguelike.Karma.Actions
{
    internal abstract class MultiStageAction
    {
        public Actor Actor { get; set; }
        public bool Interruptable { get; set; }
        public bool IsComplete { get; set; }
        
        /// <summary>
        /// Must schedule the Actor (or call a function that schedules the actor?)
        /// </summary>
        public abstract void Perform();

        /// <summary>
        /// If this action runs, and is not complete after,
        /// how long to delay the schedule of this actor for repeat/next stage
        /// </summary>
        /// <returns>Karma Schedule time offset</returns>
        public abstract long GetDelay();

        public MultiStageAction(Actor actor)
        {
            Actor = actor;
        }
    }
}
