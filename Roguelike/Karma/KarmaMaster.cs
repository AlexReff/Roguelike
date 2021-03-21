using Roguelike.Entities;
using Roguelike.Interfaces;
using Roguelike.Karma.Actions;
using System;
using System.Collections.Generic;
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
                bool needsPlan = true;
                if (npc.CurrentAction == null || !npc.CurrentAction.IsValid(npc))
                {
                    npc.CurrentAction?.Invalidate(npc);
                    npc.CurrentAction = null;
                }
                else
                {
                    npc.CurrentAction.Perform(npc);
                    needsPlan = false;
                }

                if (needsPlan)
                {
                    Queue<KarmaAction> plan = GetPlan(npc);
                }
                
                //(scheduleable as NPC).PerformAction();
                //_schedule.Add(scheduleable);

                DoTime();
            }
        }

        public void Add(int time, Actor actor)
        {
            _schedule.Add(time, actor);
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

        public Queue<KarmaAction> GetPlan(NPC actor)
        {
            Queue<KarmaAction> plan = null;

            if (_plans.ContainsKey(actor.ID))
            {
                return _plans[actor.ID];
            }

            //do logic to get a new plan here
            plan = KarmaPlanner.GetPlan(actor);

            if (plan != null && plan.Count > 0)
            {
                _plans.Add(actor.ID, plan);
            }

            return plan;
        }

        //public IKarmaGoalState GetState<T>(T entity) where T : MyBasicEntity
        //{
        //    return State;
        //}

        //public void SetState(IKarmaGoalState newState)
        //{
        //    State = newState;
        //}
    }
}
