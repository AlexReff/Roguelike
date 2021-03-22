using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Roguelike.JSON
{
    internal struct ActionSets
    {
        public string ID { get; set; }
        public List<ActionListItem> Actions { get; set; }

        public ActionSets(JsonElement m)
        {
            ID = m.GetProperty("ID").GetString();

            Actions = new List<ActionListItem>();
            var actions = m.GetProperty("Actions").EnumerateArray();
            while (actions.MoveNext())
            {
                ActionListItem item = new ActionListItem(actions.Current);
                Actions.Add(item);
            }
        }
    }

    internal struct ActionListItem
    {
        public string Action { get; set; }
        public double Cost { get; set; }

        public ActionListItem(string action, float cost)
        {
            Action = action;
            Cost = cost;
        }

        public ActionListItem(JsonElement node)
        {
            Action = node.GetProperty("Action").GetString();
            Cost = node.GetProperty("Cost").GetDouble();
        }
    }
}
