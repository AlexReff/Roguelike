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

namespace Roguelike.Entities
{
    public enum ActionStatus
    {
        Idle,
        Goto,
        Animate,
    }

    internal partial class NPC : Actor
    {
        //public IBehavior Behavior { get; set; }
        public Dictionary<uint, int> TurnsAlerted { get; set; }
        public uint? CurrentTarget { get; set; }

        public KarmaAction CurrentAction { get; set; }

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
            string name,
            double maxHealth,
            double maxMana,
            int strength,
            int agility,
            int stamina,
            int willpower,
            int intelligence,
            int vitae,
            int actionSpeed,
            int moveSpeed,
            double awareness,
            double innerFovAwareness,
            bool hasVision,
            double fovViewAngle,
            XYZRelativeDirection visionDirection,
            //ActorBody body,
            string bodyType,
            Color foreground,
            Color background,
            char glyph,
            Coord position,
            bool isWalkable,
            bool isTransparent
            ) : base(name, maxHealth, maxMana, strength, agility, stamina, willpower, intelligence, vitae, actionSpeed, moveSpeed, awareness, innerFovAwareness, hasVision, fovViewAngle, visionDirection, bodyType,
                foreground, background, glyph, position, (int)MapLayer.MONSTERS, isWalkable, isTransparent)
        {
            //Behavior = new StandardMoveAndAttack();
            TurnsAlerted = new Dictionary<uint, int>();

            AvailableActions = new List<KarmaAction>();
            _goals = new Dictionary<string, object>();
        }

        public NPC(
            string name,
            double maxHealth,
            double maxMana,
            int strength,
            int agility,
            int stamina,
            int willpower,
            int intelligence,
            int vitae,
            int actionSpeed,
            int moveSpeed,
            double awareness,
            double innerFovAwareness,
            bool hasVision,
            double fovViewAngle,
            XYZRelativeDirection visionDirection,
            //ActorBody body,
            string bodyType,
            string actionSet,
            string goalSet,
            Color foreground,
            Color background,
            char glyph,
            Coord position,
            bool isWalkable,
            bool isTransparent
            ) : this(name, maxHealth, maxMana, strength, agility, stamina, willpower, intelligence, vitae, actionSpeed, moveSpeed, awareness, innerFovAwareness, hasVision, fovViewAngle, visionDirection, bodyType,
                foreground, background, glyph, position, isWalkable, isTransparent)
        {
            if (Data.ActionSets.ContainsKey(actionSet))
            {
                ActionSets actions = Data.ActionSets[actionSet];
                foreach (ActionListItem el in actions.Actions)
                {
                    switch(el.Action)
                    {
                        case "IdleInPlace":
                            this.AvailableActions.Add(new IdleInPlaceAction(this, el.Cost));
                            break;
                        case "GoToAction":
                            this.AvailableActions.Add(new GoToAction(this, el.Cost));
                            break;
                        default:
                            break;
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

        public NPC(NPCStats npc, Coord pos) : this(npc.Name, npc.MaxHealth, npc.MaxMana, npc.Strength, npc.Agility, npc.Stamina, npc.Willpower, npc.Intelligence, npc.Vitae, npc.ActionSpeed, npc.MoveSpeed, npc.Awareness, npc.InnerFovAwareness, npc.HasVision, npc.FovViewAngle, 
            (XYZRelativeDirection)Enum.Parse(typeof(XYZRelativeDirection), npc.VisionDirection), npc.BodyType, npc.ActionSet, npc.GoalSet, new Color(npc.GlyphColor), Color.Transparent, npc.Glyph[0], pos, true, true)
        {
            //
        }
    }
}
