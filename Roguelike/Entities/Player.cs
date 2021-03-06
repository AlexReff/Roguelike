using System.Linq;
using System.Collections.Generic;
using GoRogue;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Roguelike.Helpers;
using Roguelike.Models;
using Roguelike.Spells;
using SadConsole;
using Roguelike.Attacks;

namespace Roguelike.Entities
{
    public class Player : Actor
    {
        private static readonly char CHAR_PLAYER = (char)1;

        public Player(Coord position)
            : base(MyGame.GameSettings.PlayerCharacterGlyphColor, Color.Black, CHAR_PLAYER, position, (int)MapLayer.PLAYER, isWalkable: false, isTransparent: true)
        {
            Name = "Player";
            FOVRadius = 14;
            Spells = SpellSkillManager.Instance.GetAllSpells();
            FacingDirection = Direction.UP;

            MaxHealth = Health = 120;
            Mana = 60;

            MoveSpeed = 1;

            Strength = 10;
            Stamina = 10;
            Agility = 10;
            Intelligence = 10;
            Willpower = 10;
            Vitae = 10;

            Body = ActorBody.HumanoidBody(MyGame.GameSettings.PlayerCharacterGlyphColor);
        }
    }
}
