using Roguelike.Entities;
using System;
using System.Collections.Generic;
using System.Text;

/*
 * While KarmAction is designed specifically for the Karma AI system,
 * ActionUnit represents any singular 'thing' an Actor can do, since Karma
 * now queue's all actions instead of actions being immediate,
 * we can utilize this to treat ticks more like time and less like 'turns'
 * this can potentially open up more strategy or combat options 
 * Examples:
 * - use turn momentum to do a roundhouse kick as you face another target
 * - player can use single input to 'movebump' a direction, which will queue up a
 *   TurnAction (repeats each tick until facing correct direction) and a MoveBump action,
 *   and being attacked before the movebump begins can interrupt the player,
 *   returning control to allow for new input instead of finishing the sequence
 *   ** Will require user feedback to communicate what happened
*/

namespace Roguelike.Karma.Actions
{
    internal abstract class ActionUnit
    {
        public Actor Actor { get; set; }
        /// <summary>
        /// Whether this action can be interrupted by being attacked or other status effects
        /// </summary>
        public bool Interruptable { get; set; }
        /// <summary>
        /// Must be accurate after Perform(); (Either getter property or set in Perform)
        /// </summary>
        public bool IsComplete { get; set; }

        /// <summary>
        /// This is executed when this action is the 'real time' current action, and can be treated as 'live'
        /// </summary>
        public abstract void Perform();

        /// <summary>
        /// If this action runs, and is not complete after,
        /// how long to delay the schedule of this actor for repeat/next stage
        /// </summary>
        /// <returns>Karma Schedule time offset</returns>
        public abstract long GetDelay();

        public ActionUnit(Actor actor)
        {
            Actor = actor;
        }
    }
}
