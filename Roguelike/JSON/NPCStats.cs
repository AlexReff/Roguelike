using Roguelike.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Roguelike.JSON
{
    internal struct NPCStats
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public double MaxHealth { get; set; }
        public double MaxMana { get; set; }
        public int Strength { get; set; }
        public int Agility { get; set; }
        public int Stamina { get; set; }
        public int Willpower { get; set; }
        public int Intelligence { get; set; }
        public int Vitae { get; set; }
        public double ActionSpeed { get; set; }
        public double MoveSpeed { get; set; }
        public int Awareness { get; set; }
        public int InnerFovAwareness { get; set; }
        public bool HasVision { get; set; }
        public int FovViewAngle { get; set; }
        public string BodyType { get; set; }
        public string VisionDirection { get; set; }
        public string Glyph { get; set; }
        public uint GlyphColor { get; set; }

        /// <summary>
        /// Reference to JSON.ActionSets
        /// </summary>
        public string ActionSet { get; set; }
        /// <summary>
        /// Reference to JSON.GoalSets
        /// </summary>
        public string GoalSet { get; set; }

        /// <summary>
        /// Reference to JSON.Factions
        /// </summary>
        public string Faction { get; set; }

        public NPCStats(JsonElement m)
        {
            ID = m.GetProperty("ID").GetString();
            Name = m.GetProperty("Name").GetString();
            MaxHealth = m.GetProperty("MaxHealth").GetDouble();
            MaxMana = m.GetProperty("MaxMana").GetDouble();
            Strength = m.GetProperty("Strength").GetInt32();
            Agility = m.GetProperty("Agility").GetInt32();
            Stamina = m.GetProperty("Stamina").GetInt32();
            Willpower = m.GetProperty("Willpower").GetInt32();
            Intelligence = m.GetProperty("Intelligence").GetInt32();
            Vitae = m.GetProperty("Vitae").GetInt32();
            ActionSpeed = m.GetProperty("ActionSpeed").GetDouble();
            MoveSpeed = m.GetProperty("MoveSpeed").GetDouble();
            Awareness = m.GetProperty("Awareness").GetInt32();
            InnerFovAwareness = m.GetProperty("InnerFovAwareness").GetInt32();
            HasVision = m.GetProperty("HasVision").GetBoolean();
            FovViewAngle = m.GetProperty("FovViewAngle").GetInt32();
            BodyType = m.GetProperty("BodyType").GetString();
            VisionDirection = m.GetProperty("VisionDirection").GetString();
            Glyph = m.GetProperty("Glyph").GetString();
            GlyphColor = m.GetProperty("GlyphColor").GetUInt32();
            ActionSet = m.GetProperty("ActionSet").GetString();
            GoalSet = m.GetProperty("GoalSet").GetString();
            Faction = m.GetProperty("Faction").GetString();
        }
    }
}
