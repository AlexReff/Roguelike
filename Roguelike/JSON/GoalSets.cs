using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Roguelike.JSON
{
    internal struct GoalSets
    {
        public string ID { get; set; }
        public List<GoalItem> Goals { get; set; }

        public GoalSets(JsonElement m)
        {
            ID = m.GetProperty("ID").GetString();

            Goals = new List<GoalItem>();
            var actions = m.GetProperty("Goals").EnumerateArray();
            while (actions.MoveNext())
            {
                GoalItem item = new GoalItem(actions.Current);
                Goals.Add(item);
            }
        }
    }

    internal struct GoalItem
    {
        public string Goal { get; set; }
        public object Value { get; set; }

        public GoalItem(JsonElement node)
        {
            Goal = node.GetProperty("Goal").GetString();
            Value = node.GetProperty("Value");
        }
    }
}
