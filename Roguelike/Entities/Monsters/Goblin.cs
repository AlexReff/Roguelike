using System.Collections.Generic;
using GoRogue;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Roguelike.Systems;
using Roguelike.Models;
using SadConsole;

namespace Roguelike.Entities.Monsters
{
    internal class Goblin : NPC
    {
        public Goblin(Coord position) : this(
            "Goblin",
            40, //maxHealth
            0, //maxMana
            7, //str
            6, //agi
            7, //stam
            5, //will
            4, //int
            0, //vitae
            20, //actionSpeed
            10, //moveSpeed
            3, //awareness
            3, //innerFovAwareness
            true, //hasVision
            75, //fovViewAngle
            XYZRelativeDirection.Forward, //visionDirection
            ActorBody.HumanoidBody(MyGame.GameSettings.GoblinGlyphColor), //body
            position
            ) { }

        public Goblin(
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
            Coord position
            ) : base(name, maxHealth, maxMana, strength, agility, stamina, willpower, intelligence, vitae, actionSpeed, moveSpeed, awareness, innerFovAwareness, hasVision, fovViewAngle, visionDirection, body,
                MyGame.GameSettings.GoblinGlyphColor, Color.Transparent, 'g', position, isWalkable: true, isTransparent: true)
        {
            //
        }
    }
}
