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

        public List<KarmaAction> AvailableActions { get; set; }

        public List<KarmaAction> ValidActions
        {
            get
            {
                List<KarmaAction> valid = new List<KarmaAction>();
                foreach (var action in AvailableActions)
                {
                    if (action.IsValid(this))
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
            ActorBody body,
            Color foreground,
            Color background,
            char glyph,
            Coord position,
            bool isWalkable,
            bool isTransparent
            ) : base(name, maxHealth, maxMana, strength, agility, stamina, willpower, intelligence, vitae, actionSpeed, moveSpeed, awareness, innerFovAwareness, hasVision, fovViewAngle, visionDirection, body,
                foreground, background, glyph, position, (int)MapLayer.MONSTERS, isWalkable, isTransparent)
        {
            //Behavior = new StandardMoveAndAttack();
            TurnsAlerted = new Dictionary<uint, int>();

            AvailableActions = new List<KarmaAction>();
        }
    }
}
