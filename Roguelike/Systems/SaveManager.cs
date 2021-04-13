using Roguelike.JSON;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Systems
{
    internal class SaveManager
    {
        public const string SaveFolder = Data.ContentFolder + "\\save";
        public const string SummaryFileName = "save.dat";

        public SaveManager()
        {
            //
        }

        public void LoadSave()
        {
            //var jsonWordFiles = Directory.GetFiles(JsonWordsFolderPath, "*.json");
            //var wordsCleared = false;
            //foreach (var file in jsonWordFiles)
            //{
            //    var fileNameParts = file.Split("\\");
            //    try
            //    {
            //        using (FileStream stream = File.OpenRead(file))
            //        {
            //            var theseWords = await JsonSerializer.DeserializeAsync<WordsJson[]>(stream);
            //            foreach (var word in theseWords)
            //            {
            //                // wait until we actually have some succesful
            //                // word(s) to add before clearing out the words
            //                if (!wordsCleared)
            //                {
            //                    wordsCleared = true;
            //                    _words.Clear();
            //                }
            //                if (!string.IsNullOrEmpty(word.word))
            //                {
            //                    var parts = word.word.Split(" ");
            //                    foreach (var part in parts)
            //                    {
            //                        if (part.Length > 5)
            //                        {
            //                            _words.Add(part.ToLower());
            //                        }
            //                    }
            //                }
            //            }

            //            DebugManager.Instance.AddMessage($"Added {fileNameParts[fileNameParts.Length - 1]}");
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        DebugManager.Instance.AddMessage($"Failed to add {fileNameParts[fileNameParts.Length - 1]}");
            //        continue;
            //    }
            //}
        }
    }
}
