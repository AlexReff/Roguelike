//using GoRogue;
//using Microsoft.Xna.Framework;
//using Roguelike.Behaviors;
//using Roguelike.Interfaces;
//using Roguelike.Models;
//using Roguelike.Karma;
//using Roguelike.Systems;
//using SadConsole;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Roguelike.Entities.Monsters
//{
//    /// <summary>
//    /// Represents a typically-hostile 'creature' of some kind, not affiliated with intelligence-recognized races (eg not a human/elf)
//    /// Animals typically should be 'Creature's rather than Monsters.
//    /// </summary>
//    internal abstract class Monster : NPC
//    {
//        public Monster(
//            string name,
//            double maxHealth,
//            double maxMana,
//            int strength,
//            int agility,
//            int stamina,
//            int willpower,
//            int intelligence,
//            int vitae,
//            int actionSpeed,
//            double moveSpeed,
//            double awareness,
//            double innerFovAwareness,
//            bool hasVision,
//            double fovViewAngle,
//            XYZRelativeDirection visionDirection,
//            ActorBody body, 
//            Color foreground,
//            Color background,
//            char glyph,
//            Coord position,
//            bool isWalkable,
//            bool isTransparent
//            ) : base(name, maxHealth, maxMana, strength, agility, stamina, willpower, intelligence, vitae, actionSpeed, moveSpeed, awareness, innerFovAwareness, hasVision, fovViewAngle, visionDirection, body,
//                foreground, background, glyph, position, (int)MapLayer.MONSTERS, isWalkable, isTransparent)
//        {
//            //Behavior = new StandardMoveAndAttack();
//        }
//    }
//}
