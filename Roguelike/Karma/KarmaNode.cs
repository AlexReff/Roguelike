using Roguelike.Karma.Actions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Karma
{
	internal class KarmaNode
	{
		public KarmaNode parent;
		public double runningCost;
		public Dictionary<string, object> state;
		public KarmaAction action;

		public KarmaNode(KarmaNode parent, double runningCost, Dictionary<string, object> state, KarmaAction action)
		{
			this.parent = parent;
			this.runningCost = runningCost;
			this.state = state;
			this.action = action;
		}
	}
}
