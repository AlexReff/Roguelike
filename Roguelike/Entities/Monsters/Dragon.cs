using System.Collections.Generic;
using GoRogue;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Roguelike.Systems;
using Roguelike.Models;
using SadConsole;

namespace Roguelike.Entities.Monsters
{
    internal class Dragon : NPC
    {
        public Dragon(Coord position) : this(
            "Dragon",
            200, //maxHealth
            100, //maxMana
            20, //str
            7, //agi
            20, //stam
            14, //will
            10, //int
            30, //vitae
            20, //actionSpeed
            10, //moveSpeed
            5, //awareness
            2, //innerFovAwareness
            true, //hasVision
            90, //fovViewAngle
            XYZRelativeDirection.Forward, //visionDirection
            ActorBody.HumanoidBody(MyGame.GameSettings.DragonGlyphColor), //body
            position
            )
        { }

        public Dragon(
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
                MyGame.GameSettings.DragonGlyphColor, Color.Transparent, 'g', position, isWalkable: true, isTransparent: true)
        {
            //
        }
    }
}
