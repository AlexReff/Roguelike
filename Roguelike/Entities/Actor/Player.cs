using System.Linq;
using System.Collections.Generic;
using GoRogue;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Roguelike.Systems;
using Roguelike.Models;
using Roguelike.Entities.Items;

namespace Roguelike.Entities
{
    internal class Player : Actor
    {
        private static readonly char CHAR_PLAYER = (char)64;// (char)1;

        public Player(Coord position) : this(
            "Player",
            120, //maxHealth
            60, //maxMana
            10, //str
            10, //agi
            10, //stam
            10, //will
            10, //int
            10, //vitae
            20, //actionSpeed
            20, //moveSpeed
            14, //awareness
            4, //innerFovAwareness
            true, //hasVision
            160, //fovViewAngle
            XYZRelativeDirection.Forward, //visionDirection
            "Humanoid",//ActorBody.HumanoidBody(MyGame.GameSettings.PlayerCharacterGlyphColor), //body
            position
            )
        { }

        public Player(
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
            XYZRelativeDirection visionDirection,
            //ActorBody body,
            string bodyType,
            Coord position
            ) : base(name, maxHealth, maxMana, strength, agility, stamina, willpower, intelligence, vitae, actionSpeed, moveSpeed, awareness, innerFovAwareness, hasVision, fovViewAngle, "player", visionDirection, bodyType,
                MyGame.GameSettings.PlayerCharacterGlyphColor, Color.Transparent, CHAR_PLAYER, position, (int)MapLayer.PLAYER, isWalkable: true, isTransparent: true)
        {
            //Spells = SpellSkillManager.Instance.GetAllSpells();
            Moved += Player_Moved;
            EquipDefaultLoadout();
        }

        private void Player_Moved(object sender, ItemMovedEventArgs<GoRogue.GameFramework.IGameObject> e)
        {
            if (MyGame.GameSettings.GoldAutoPickup)
            {
                var gold = CurrentMap.GetEntitiesAt<Currency>(e.NewPosition);
                if (gold != null && gold.Any())
                {
                    foreach (var piece in gold)
                    {
                        PickupItem(piece);
                    }
                }
            }
        }

        private void EquipDefaultLoadout()
        {
            //
        }

        //public static Player DefaultPlayer(Coord position)
        //{
        //    Player result = new Player(position);
        //    result.Name = "Player";
        //    //result.Spells = SpellSkillManager.Instance.GetAllSpells();
        //    result.FacingDirection = Direction.UP;

        //    result.VisionDirection = XYZRelativeDirection.Forward;
        //    result.HasVision = true;
        //    result.Awareness = 14;
        //    result.FOVViewAngle = 160;
        //    result.InnerFOVAwareness = 4;

        //    result.MaxHealth = 120;
        //    result.Health = 120;
        //    result.Mana = 60;

        //    result.MoveSpeed = 10;
        //    result.ActionSpeed = 10;

        //    result.Strength = 10;
        //    result.Stamina = 10;
        //    result.Agility = 10;
        //    result.Intelligence = 10;
        //    result.Willpower = 10;
        //    result.Vitae = 10;

        //    result.Body = ActorBody.HumanoidBody(MyGame.GameSettings.PlayerCharacterGlyphColor);
        //    result.Body.Parent = result;

        //    return result;
        //}
    }
}
