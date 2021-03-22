using Roguelike.Entities;
using Roguelike.Interfaces;
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
        private Dictionary<uint, Queue<KarmaAction>> _plans;
        private IKarmaWorldState _worldState;

        public bool IsPlayerTurn { get; set; }

        public KarmaMaster()
        {
            _schedule = new KarmaSchedule();
            _plans = new Dictionary<uint, Queue<KarmaAction>>();
        }

        public void DoTime()
        {
            Actor scheduleable = _schedule.Get();
            if (scheduleable is Player)
            {
                IsPlayerTurn = true;
                //?
                //_schedule.Add(scheduleable);
            }
            else if (scheduleable is NPC)
            {
                /* NPC is added fresh to the system, and is now being freshly plucked from the system
                 * We need to get the first valid goal and save the plan to our system
                 * 
                 * If the actor has a current action, check if it is completed, and still valid, then perform the action
                 * If the actor completes an action successfully, the next action is chosen from the plan (plan advances), and the actor is scheduled
                 * If the actor's action becomes invalid (precondition check fail) invalidate the goal and request a new goal
                 * 
                 * agent action example: attack target
                 * action either initiates a move request, or an attack request
                 */
                NPC npc = scheduleable as NPC;
                if (npc.CurrentAction == null || !npc.CurrentAction.IsValid())
                {
                    // invalidate/remove the invalid action
                    npc.CurrentAction?.Invalidate();
                    npc.CurrentAction = null;

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
                            var first = plan.Dequeue();
                            npc.CurrentAction = first;

                            // if the plan has additional steps, save it
                            if (plan.Count > 0)
                            {
                                _plans.Add(npc.ID, plan);
                            }
                        }
                    }
                }

                if (npc.CurrentAction != null)
                {
                    npc.CurrentAction.Perform();
                }

                //(scheduleable as NPC).PerformAction();
                //_schedule.Add(scheduleable);

                DoTime();
            }
        }

        public void Remove(Actor actor)
        {
            _schedule.Remove(actor);
        }

        public void Add(int time, Actor actor)
        {
            _schedule.Add(time, actor);
        }

        public void EndPlayerTurn()
        {
            IsPlayerTurn = false;
        }

        public void Add(Actor actor)
        {
            //Adds the selected actor to the scheduling system
            //Initially delayed to prevent immediate acting
            Add(actor.ActionSpeed, actor);
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
