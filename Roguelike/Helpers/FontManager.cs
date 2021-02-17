using Roguelike.Models;
using SadConsole;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Roguelike.Helpers
{
    public class FontManager
    {
        private static readonly FontManager instance = new FontManager();

        // Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
        static FontManager() { }

        public static FontManager Instance
        {
            get
            {
                return instance;
            }
        }

        private const string FONT_FOLDER = "Content\\fonts";
        private bool FontsLoaded = false;

        public Dictionary<string, FontMaster> Fonts { get; private set; }

        private FontManager()
        {
            Fonts = new Dictionary<string, FontMaster>();
        }

        public void LoadFonts()
        {
            if (FontsLoaded)
            {
                return;
            }

            var currentDirectory = Directory.GetCurrentDirectory();

            var files = Directory.EnumerateFiles(currentDirectory + "\\" + FONT_FOLDER);
            foreach (var file in files)
            {
                if (file.ToLower().EndsWith(".font"))
                {
                    var jsonDat = File.ReadAllText(file);
                    FontFileJson parsedModel = JsonSerializer.Deserialize<FontFileJson>(jsonDat);
                    if (parsedModel != null && !string.IsNullOrWhiteSpace(parsedModel.Name) && !string.IsNullOrWhiteSpace(parsedModel.FilePath))
                    {
                        var localPath = file.Replace(currentDirectory, "").Replace("\\", "/").Substring(1);
                        if (!Fonts.ContainsKey(localPath))
                        {
                            try
                            {
                                var fontMaster = SadConsole.Global.LoadFont(localPath);
                                Fonts.Add(localPath, fontMaster);
                                var fontParts = localPath.Split("/");
                                var fontNameParts = fontParts[fontParts.Length - 1].Split(".");
                                DebugManager.Instance.AddMessage(new DebugMessage("Added font: " + fontNameParts[0], DebugSource.Backend));
                            }
                            catch (Exception ex)
                            {
                                DebugManager.Instance.AddMessage(new DebugMessage("Failed to add font: " + localPath, DebugSource.Backend));
                            }
                        }
                    }
                }
            }

            FontsLoaded = true;
        }
    }
}
