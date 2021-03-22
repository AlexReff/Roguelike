using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Roguelike.JSON
{
    internal static class Data
    {
        public static Dictionary<string, Materials> Materials { get; }
        public static Dictionary<string, Factions> Factions { get; }
        public static Dictionary<string, NPCStats> NPCStats { get; }
        public static Dictionary<string, ActionSets> ActionSets { get; }
        public static Dictionary<string, GoalSets> GoalSets { get; }

        static Data()
        {
            Materials = new Dictionary<string, Materials>();
            Factions = new Dictionary<string, Factions>();
            NPCStats = new Dictionary<string, NPCStats>();
            ActionSets = new Dictionary<string, ActionSets>();
            GoalSets = new Dictionary<string, GoalSets>();
        }

        public static async Task LoadCoreData()
        {
            CDB doc = null;
            using (FileStream openStream = File.OpenRead("Content/data.cdb"))
            {
                doc = await JsonSerializer.DeserializeAsync<CDB>(openStream);
            }
            if (doc == null || doc.sheets == null || doc.sheets.Length == 0)
            {
                throw new Exception("Unable to load data.cdb");
            }

            for (int i = 0; i < doc.sheets.Length; i++)
            {
                var thisSheet = doc.sheets[i];
                switch (thisSheet.name)
                {
                    case "Materials":
                        Materials.Clear();
                        foreach (var record in thisSheet.lines)
                        {
                            var mat = new Materials(record);
                            Materials.Add(mat.ID, mat);
                        }
                        break;
                    case "Factions":
                        Factions.Clear();
                        foreach (var record in thisSheet.lines)
                        {
                            var faction = new Factions(record);
                            Factions.Add(faction.ID, faction);
                        }
                        break;
                    case "NPCStats":
                        NPCStats.Clear();
                        foreach (var record in thisSheet.lines)
                        {
                            var npcs = new NPCStats(record);
                            NPCStats.Add(npcs.ID, npcs);
                        }
                        break;
                    case "ActionSets":
                        ActionSets.Clear();
                        foreach (var record in thisSheet.lines)
                        {
                            var actionSets = new ActionSets(record);
                            ActionSets.Add(actionSets.ID, actionSets);
                        }
                        break;
                    case "GoalSets":
                        GoalSets.Clear();
                        foreach (var record in thisSheet.lines)
                        {
                            var goalSet = new GoalSets(record);
                            GoalSets.Add(goalSet.ID, goalSet);
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }

    internal class CDB
    {
        public CDBSheet[] sheets { get; set; }
        public object[] customTypes { get; set; }
        public bool compress { get; set; }
    }

    internal class CDBSheet
    {
        public string name { get; set; }
        public object[] columns { get; set; }
        public JsonElement[] lines { get; set; }
        public object[] separators { get; set; }
        public object props { get; set; }
    }
}
