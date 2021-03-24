using Roguelike.Entities;
using Roguelike.Interfaces;
using Roguelike.Karma;
using Roguelike.Karma.Actions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

/*
 * goals will be added in priority order
 * 
 * we will start with 12 ticks / second
 * we will need to be able to schedule actions
 * 
 * 
 * when an actor schedules an attack, there is an immediate flag of 'attempting_attack' and when the scheduled action comes up, it should PERFORM the 'attack', then set the new value
 * 
 * so the goal gets chosen
 * scheduled ACTIONS and scheduled GOAL-choosing-behavior-initiating (which could schedule future actions)
 * 
 * 
 * when an npc is added, it delays it's first 'tick of time' by it's action speed(?)
 * then when the npc is pulled, it checks if the actor has a current goal (actor property)
 * if the actor does have a goal, it checks if the goal conditions have been met:
 * - if so advances, the plan
 * - if not, get the goal's current action and perform it
 * advancing the plan sets the new goal and schedules the first action in the goal
 * if actor does not have a goal, get next goal from plan
 * if plan is over, or has not been set, request new plan based on list of goals & find first valid plan
 */

namespace Roguelike.Karma
{
    /// <summary>
    /// GOAP-ish AI manager
    /// 
    /// </summary>
    internal class KarmaMaster
    {
        private KarmaSchedule _schedule;
        private Dictionary<long, Queue<KarmaAction>> _plans;
        private IKarmaWorldState _worldState;

        public bool IsPlayerTurn { get; set; }

        public KarmaMaster()
        {
            IsPlayerTurn = true;
            _schedule = new KarmaSchedule();
            _plans = new Dictionary<long, Queue<KarmaAction>>();
        }

        public void DoTime()
        {
            while (!IsPlayerTurn)
            {
                Actor scheduleable = _schedule.Get();

                // handle recovery
                if (scheduleable.State == ActorState.Recovering)
                {
                    scheduleable.State = ActorState.Idle;
                    Add(scheduleable);
                    continue;
                }

                // continue with any queued actions
                if (scheduleable.QueuedActions.Count > 0)
                {
                    var action = scheduleable.QueuedActions.Peek();
                    if (scheduleable.InterruptQueuedActions && action.Interruptable)
                    {
                        // unless something happened...
                        scheduleable.QueuedActions.Clear();
                        scheduleable.InterruptQueuedActions = false;
                        RemoveAll(scheduleable);
                    }
                    else
                    {
                        action.Perform();
                        if (action.IsComplete)
                        {
                            scheduleable.QueuedActions.Dequeue();
                        }
                        else
                        {
                            AddAfterLast(scheduleable.KarmaReactionSpeed, scheduleable);
                        }
                        continue;
                    }
                }

                if (scheduleable is Player)
                {
                    IsPlayerTurn = true;
                    break;
                }
                
                if (scheduleable is NPC)
                {
                    /* If the actor has a current action, check if it is completed, and still valid, then perform the action
                     * If the actor completes an action successfully, the next action is chosen from the plan (plan advances), and the actor is scheduled
                     * If the actor's action becomes invalid (precondition check fail) invalidate the goal and request a new goal
                     */

                    NPC npc = scheduleable as NPC;
                    if (npc.CurrentAction != null)
                    {
                        if (npc.CurrentAction.IsCompleted())
                        {
                            // action is complete!
                            npc.CurrentAction.Reset();
                            npc.CurrentAction = null;
                        }
                        else if (!npc.CurrentAction.IsValid())
                        {
                            // invalidate/remove the invalid action
                            npc.CurrentAction.Invalidate();
                            npc.CurrentAction.Reset();
                            npc.CurrentAction = null;
                        }
                    }

                    if (npc.CurrentAction == null)
                    {
                        // check to see if we already have a plan for this npc
                        if (_plans.ContainsKey(npc.ID) && _plans[npc.ID].Count > 0)
                        {
                            // plan already exists and has another action available
                            npc.CurrentAction = _plans[npc.ID].Dequeue();
                            if (_plans[npc.ID].Count == 0)
                            {
                                _plans.Remove(npc.ID);
                            }
                        }
                        else
                        {
                            // no action, no stored plan: get a new plan!
                            _plans.Remove(npc.ID);

                            Queue<KarmaAction> plan = KarmaPlanner.GetPlan(npc, GetCombinedState(npc));

                            if (plan != null && plan.Count > 0)
                            {
                                npc.CurrentAction = plan.Dequeue();

                                // if the plan has additional steps, save it
                                if (plan.Count > 0)
                                {
                                    _plans.Add(npc.ID, plan);
                                }
                            }
                        }
                    }

                    // check to see if the action is valid
                    if (npc.CurrentAction != null && npc.CurrentAction.IsValid())
                    {
                        // check to see if we are in range
                        if (npc.CurrentAction.IsInValidRange())
                        {
                            // we have a valid, in-range action! do it!
                            npc.CurrentAction.Perform();
                            continue;
                        }
                        else
                        {
                            // we are out of range! we must move towards the target
                            // add the npc's current action back to the front of the plan
                            // set a getInRange action as the current action
                            // TODO: refactor this logic to instead navigate towards the closest spot in range, instead of a generic pathfind to the center
                        
                            // insert the current action to the front of the plan's queue
                            var npcAction = npc.CurrentAction;
                            var newQueue = new Queue<KarmaAction>(new[] { npcAction });
                            npc.TargetPosition = npcAction.GetTargetPosition();

                            if (_plans.ContainsKey(npc.ID))
                            {
                                var existing = _plans[npc.ID];
                            
                                foreach (var e in existing)
                                {
                                    newQueue.Enqueue(e);
                                }
                                _plans[npc.ID] = newQueue;
                            }
                            else
                            {
                                _plans.Add(npc.ID, newQueue);
                            }

                            npc.CurrentAction = new GetInRangeAction(npc, npcAction.GetRange());
                            npc.CurrentAction.Perform();
                            continue;
                        }
                    }
                    else
                    {
                        // we were not able to get a valid action
                        // add the npc back to the schedule, maybe it will work next time
                        MyGame.Karma.Add(npc.KarmaReactionSpeed, npc);
                    }

                    //DoTime();
                }
            }
        }

        public void RemoveAll(Actor actor)
        {
            _schedule.RemoveAll(actor);
        }

        public void Remove(Actor actor)
        {
            _schedule.Remove(actor);
        }

        public void Add(long time, Actor actor)
        {
            _schedule.Add(time, actor);
        }

        public void AddAfterLast(long time, Actor actor)
        {
            _schedule.AddAfterLast(time, actor);
        }

        public bool IsActorScheduled(Actor actor)
        {
            return _schedule.IsActorScheduled(actor);
        }

        public void EndPlayerTurn()
        {
            IsPlayerTurn = false;
            DoTime();
        }

        /// <summary>
        /// Adds the actor with KarmaReactionSpeed delay
        /// </summary>
        public void Add(Actor actor)
        {
            Add(actor.KarmaReactionSpeed, actor);
        }

        public void Initialize(Player player)
        {
            _schedule.Add(0, player);
        }

        public Dictionary<string, object> GetCombinedState(NPC actor)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            var combinedState = new KarmaCombinedState(actor, _worldState);
            foreach (PropertyInfo property in combinedState.GetType().GetProperties())
            {
                result.Add(property.Name, property.GetValue(combinedState));
            }

            return result;
        }
    }
}
