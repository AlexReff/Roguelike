using Roguelike.Models;
using Roguelike.Systems;
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
        public const string ContentFolder = "Content";

        private const string CDBFilePath = ContentFolder + "\\data.cdb";
        private const string JsonWordsFolderPath = ContentFolder + "\\json";

        public static Dictionary<string, Materials> Materials { get; }
        public static Dictionary<string, Factions> Factions { get; }
        public static Dictionary<string, NPCStats> NPCStats { get; }
        public static Dictionary<string, ActionSets> ActionSets { get; }
        public static Dictionary<string, GoalSets> GoalSets { get; }

        public static List<SaveGameSummary> SaveGames { get; }
        
        private static HashSet<string> _words { get; }
        public static List<string> Words { get { return _words.ToList(); } }

        static Data()
        {
            Materials = new Dictionary<string, Materials>();
            Factions = new Dictionary<string, Factions>();
            NPCStats = new Dictionary<string, NPCStats>();
            ActionSets = new Dictionary<string, ActionSets>();
            GoalSets = new Dictionary<string, GoalSets>();

            SaveGames = new List<SaveGameSummary>();

            _words = new HashSet<string>();
        }

        public static async Task LoadCoreData()
        {
            var cdbLoad = LoadCDB();
            var wordsLoad = LoadWords();
            var saveLoad = LoadSaveSummaries();

            await cdbLoad;
            await wordsLoad;
            await saveLoad;
        }

        private static async Task LoadCDB()
        {
            CDB doc = null;
            using (FileStream openStream = File.OpenRead(CDBFilePath))
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

        private static async Task LoadWords()
        {
            var jsonWordFiles = Directory.GetFiles(JsonWordsFolderPath, "*.json");
            var wordsCleared = false;
            foreach (var file in jsonWordFiles)
            {
                var fileNameParts = file.Split("\\");
                try
                {
                    using (FileStream stream = File.OpenRead(file))
                    {
                        var theseWords = await JsonSerializer.DeserializeAsync<WordsJson[]>(stream);
                        foreach (var word in theseWords)
                        {
                            // wait until we actually have some succesful
                            // word(s) to add before clearing out the words
                            if (!wordsCleared)
                            {
                                wordsCleared = true;
                                _words.Clear();
                            }
                            if (!string.IsNullOrEmpty(word.word))
                            {
                                var parts = word.word.Split(" ");
                                foreach (var part in parts)
                                {
                                    if (part.Length > 5)
                                    {
                                        _words.Add(part.ToLower());
                                    }
                                }
                            }
                        }

                        DebugManager.Instance.AddMessage($"Added {fileNameParts[fileNameParts.Length - 1]}");
                    }
                }
                catch (Exception ex)
                {
                    DebugManager.Instance.AddMessage($"Failed to add {fileNameParts[fileNameParts.Length - 1]}");
                    continue;
                }
            }
        }

        private static async Task LoadSaveSummaries()
        {
            SaveGames.Clear();

            var saveFolders = Directory.EnumerateDirectories(SaveManager.SaveFolder);
            if (!saveFolders.Any())
            {
                DebugManager.Instance.AddMessage($"No saves found in {Directory.GetCurrentDirectory()}\\{SaveManager.SaveFolder}");
                return;
            }

            foreach (var folder in saveFolders)
            {
                try
                {
                    using (FileStream stream = File.OpenRead(folder + "\\" + SaveManager.SummaryFileName))
                    {
                        var saveSummary = await JsonSerializer.DeserializeAsync<SaveGameSummary>(stream);
                        SaveGames.Add(saveSummary);
                    }
                }
                catch (Exception ex)
                {
                    DebugManager.Instance.AddMessage($"Unable to parse {SaveManager.SummaryFileName} in {folder}: {ex.InnerException?.Message ?? ex.Message}");
                    continue;
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

    internal class WordsJson
    {
        public string word { get; set; }
    }
}
