using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Roguelike.JSON
{
    public struct Factions
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public List<string> FriendlyWith { get; set; }
        public List<string> HostileWith { get; set; }
        public List<string> AlliedWith { get; set; }

        public Factions(JsonElement m)
        {
            ID = m.GetProperty("ID").GetString();
            Name = m.GetProperty("Name").GetString();

            FriendlyWith = m.GetProperty("FriendlyWith").GetJsonList("Faction");
            HostileWith = m.GetProperty("HostileWith").GetJsonList("Faction");
            AlliedWith = m.GetProperty("AlliedWith").GetJsonList("Faction");
        }
    }
}
