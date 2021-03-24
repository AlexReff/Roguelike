using GoRogue;
using Microsoft.Xna.Framework;
using Roguelike.Behaviors;
using Roguelike.Interfaces;
using Roguelike.Models;
using Roguelike.Karma;
using Roguelike.Karma.Actions;
using Roguelike.Karma.Goals;
using System;
using System.Collections.Generic;
using System.Text;
using Roguelike.JSON;
using System.Reflection;
using Roguelike.Systems;
using SadConsole;
using Roguelike.Maps;

namespace Roguelike.Entities
{
    internal partial class NPC : Actor
    {
        //public IBehavior Behavior { get; set; }
        public string NPCID { get; }

        public Dictionary<long, int> TurnsAlerted { get; set; }



        private Dictionary<string, object> _goals;
        public Dictionary<string, object> Goals 
        {
            get
            {
                return new Dictionary<string, object>(_goals);
            }
        }

        public List<KarmaAction> AvailableActions { get; set; }

        public List<KarmaAction> ValidActions
        {
            get
            {
                List<KarmaAction> valid = new List<KarmaAction>();
                foreach (var action in AvailableActions)
                {
                    if (action.IsValid())
                    {
                        valid.Add(action);
                    }
                }
                return valid;
            }
        }


        public NPC(
            string npcId,
            string name,
            double maxHealth,
            double maxMana,
            int strength,
            int agility,
            int stamina,
            int willpower,
            int intelligence,
            int vitae,
            double actionSpeed,
            double moveSpeed,
            double awareness,
            double innerFovAwareness,
            bool hasVision,
            double fovViewAngle,
            string faction,
            XYZRelativeDirection visionDirection,
            //ActorBody body,
            string bodyType,
            Color foreground,
            Color background,
            char glyph,
            Coord position,
            bool isWalkable,
            bool isTransparent
            ) : base(name, maxHealth, maxMana, strength, agility, stamina, willpower, intelligence, vitae, actionSpeed, moveSpeed, awareness, innerFovAwareness, hasVision, fovViewAngle, faction, visionDirection, bodyType,
                foreground, background, glyph, position, (int)MapLayer.MONSTERS, isWalkable, isTransparent)
        {
            NPCID = npcId;
            TurnsAlerted = new Dictionary<long, int>();

            AvailableActions = new List<KarmaAction>();
            _goals = new Dictionary<string, object>();
        }

        public NPC(
            string npcId,
            string name,
            double maxHealth,
            double maxMana,
            int strength,
            int agility,
            int stamina,
            int willpower,
            int intelligence,
            int vitae,
            double actionSpeed,
            double moveSpeed,
            double awareness,
            double innerFovAwareness,
            bool hasVision,
            double fovViewAngle,
            string faction,
            XYZRelativeDirection visionDirection,
            string bodyType,
            string actionSet,
            string goalSet,
            Color foreground,
            Color background,
            char glyph,
            Coord position,
            bool isWalkable,
            bool isTransparent
            ) : this(npcId, name, maxHealth, maxMana, strength, agility, stamina, willpower, intelligence, vitae, actionSpeed, moveSpeed, awareness, innerFovAwareness, hasVision, fovViewAngle, faction, visionDirection, bodyType,
                foreground, background, glyph, position, isWalkable, isTransparent)
        {
            if (Data.ActionSets.ContainsKey(actionSet))
            {
                ActionSets actions = Data.ActionSets[actionSet];
                foreach (ActionListItem el in actions.Actions)
                {
                    var args = new object[] { this, el.Cost };
                    var actionType = Type.GetType($"Roguelike.Karma.Actions.{el.Action}Action");
                    var result = Activator.CreateInstance(actionType, args);

                    if (result != null && result is KarmaAction)
                    {
                        this.AvailableActions.Add(result as KarmaAction);
                    }
                    else
                    {
                        DebugManager.Instance.AddMessage($"Unable to find action type for '{el.Action}'");
                    }
                }
            }
            if (Data.GoalSets.ContainsKey(goalSet))
            {
                GoalSets goals = Data.GoalSets[goalSet];
                foreach (GoalItem el in goals.Goals)
                {
                    var val = el.Value;
                    var valStr = val.ToString();
                    if (valStr == "True")
                    {
                        val = true;
                    }
                    else if (valStr == "False")
                    {
                        val = false;
                    }
                    _goals.Add(el.Goal, val);
                }
            }
        }

        public NPC(string npcId, NPCStats npc, Coord pos) : this(npcId, npc.Name, npc.MaxHealth, npc.MaxMana, npc.Strength, npc.Agility, npc.Stamina, npc.Willpower, npc.Intelligence, npc.Vitae, npc.ActionSpeed, npc.MoveSpeed, npc.Awareness, npc.InnerFovAwareness, npc.HasVision, npc.FovViewAngle, npc.Faction,
            (XYZRelativeDirection)Enum.Parse(typeof(XYZRelativeDirection), npc.VisionDirection), npc.BodyType, npc.ActionSet, npc.GoalSet, new Color(new Color(npc.GlyphColor), 1.0f), Color.Transparent, npc.Glyph[0], pos, true, true)
        {
            //
        }

        public NPC(string npcId, Coord pos) : this(npcId, Data.NPCStats[npcId], pos)
        {
            //
        }
    }
}
