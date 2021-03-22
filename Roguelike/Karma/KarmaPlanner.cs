using Roguelike.Entities;
using Roguelike.Karma.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roguelike.Karma
{
    internal static class KarmaPlanner
    {
        static KarmaPlanner()
        {
            //
        }

		public static Queue<KarmaAction> GetPlan(NPC npc, Dictionary<string, object> state)
		{
			Queue<KarmaAction> plan = new Queue<KarmaAction>();

			//reset all actions
			npc.AvailableActions.ForEach(m => m.Reset());

			var validActions = npc.ValidActions;

			// build up the tree and record the leaf nodes that provide a solution to the goal.
			List<KarmaNode> leaves = new List<KarmaNode>();

			KarmaNode start = new KarmaNode(null, 0, state, null);
			if (!BuildGraph(start, leaves, validActions.ToHashSet(), npc.Goals))
            {
				return null;
            }

			// find cheapest successful route
			KarmaNode cheapest = null;
			foreach (var leaf in leaves)
            {
				if (cheapest == null)
					cheapest = leaf;
				else
				{
					if (leaf.runningCost < cheapest.runningCost)
						cheapest = leaf;
				}
			}

			List<KarmaAction> result = new List<KarmaAction>();

			KarmaNode n = cheapest;
			while (n != null)
			{
				if (n.action != null)
				{
					result.Insert(0, n.action); // insert the action in the front
				}
				n = n.parent;
			}

			foreach (KarmaAction a in result)
			{
				plan.Enqueue(a);
			}

			return plan;
        }

		public static bool BuildGraph(KarmaNode parent, List<KarmaNode> leaves, HashSet<KarmaAction> usableActions, Dictionary<string, object> goal)
		{
			bool foundOne = false;

			// go through each action available at this node and see if we can use it here
			foreach (KarmaAction action in usableActions)
			{
				// if the parent state has the conditions for this action's preconditions, we can use it here
				if (InState(action.Conditions, parent.state))
				{
					// apply the action's effects to the parent state
					Dictionary<string, object> currentState = PopulateState(parent.state, action.Effects);

					KarmaNode node = new KarmaNode(parent, parent.runningCost + action.Cost, currentState, action);

					if (GoalInState(goal, currentState))
					{
						// we found a solution!
						leaves.Add(node);
						foundOne = true;
					}
					else
					{
						// test all the remaining actions and branch out the tree
						HashSet<KarmaAction> subset = ActionSubset(usableActions, action);
						bool found = BuildGraph(node, leaves, subset, goal);
						if (found)
							foundOne = true;
					}
				}
			}

			return foundOne;
		}

		/// <summary>
		/// Creates a subset of the actions excluding removeMe. Creates a new set.
		/// </summary>
		/// <param name="actions"></param>
		/// <param name="removeMe"></param>
		/// <returns></returns>
		public static HashSet<KarmaAction> ActionSubset(HashSet<KarmaAction> actions, KarmaAction removeMe)
		{
			HashSet<KarmaAction> subset = new HashSet<KarmaAction>();
			foreach (KarmaAction a in actions)
			{
				if (!a.Equals(removeMe))
					subset.Add(a);
			}
			return subset;
		}

		/// <summary>
		/// Checks that every item in 'test' is also in 'state' (non-full-matches fail)
		/// </summary>
		public static bool InState(Dictionary<string, object> test, Dictionary<string, object> state)
		{
			bool allMatch = true;
			foreach (KeyValuePair<string, object> t in test)
			{
				bool match = false;
				foreach (KeyValuePair<string, object> s in state)
				{
					if (s.Equals(t))
					{
						match = true;
						break;
					}
				}
				if (!match)
					allMatch = false;
			}
			return allMatch;
		}

		/// <summary>
		/// Apply stateChange to currentState, without modifying the originals
		/// </summary>
		/// <returns>A new Dictionary</returns>
		public static Dictionary<string, object> PopulateState(Dictionary<string, object> currentState, Dictionary<string, object> stateChange)
		{
			Dictionary<string, object> state = new Dictionary<string, object>();
			// copy the KVPs over as new objects
			foreach (KeyValuePair<string, object> s in currentState)
			{
				state.Add(s.Key, s.Value);
			}

			foreach (KeyValuePair<string, object> change in stateChange)
			{
				// if the key exists in the current state, update the Value
				bool exists = false;

				foreach (KeyValuePair<string, object> s in state)
				{
					if (s.Key.Equals(change.Key))
					{
						exists = true;
						break;
					}
				}

				if (exists)
				{
					state.Remove(change.Key);
					state.Add(change.Key, change.Value);
				}
				// if it does not exist in the current state, add it
				else
				{
					state.Add(change.Key, change.Value);
				}
			}
			return state;
		}

		/// <returns>True if at least one goal is met</returns>
		public static bool GoalInState(Dictionary<string, object> test, Dictionary<string, object> state)
		{
			bool match = false;
			foreach (KeyValuePair<string, object> t in test)
			{
				foreach (KeyValuePair<string, object> s in state)
				{
					if (s.Equals(t))
					{
						match = true;
						break;
					}
				}
			}
			return match;
		}

	}
}
