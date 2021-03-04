using System.Linq;
using System.Collections.Generic;
using GoRogue;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Roguelike.Entities.Interfaces;
using Roguelike.Helpers;
using Roguelike.Models;
using Roguelike.Spells;
using SadConsole;
using Roguelike.Attacks;

namespace Roguelike.Entities
{
    public class Player : Actor, IHasVision, ICastsSpells, ICanAttack, IHasBody
    {
        private static readonly char CHAR_PLAYER = (char)1;

        public double FOVRadius { get; set; }
        public ActorBody Body { get; set; }
        public List<Attack> Attacks { get; set; }
        public List<Spell> Spells { get; set; }
        /// <summary>
        /// Relative to the Player's FacingDirection
        /// </summary>
        public XYZRelativeDirection VisionDirection { get; set; }

        /// <summary>
        /// 0.0-100.0, overall health 'percentage'
        /// </summary>
        public double OverallHealth { get; set; }

        public Player(Coord position)
            : base(MyGame.GameSettings.PlayerCharacterGlyphColor, Color.Black, CHAR_PLAYER, position, (int)MapLayer.PLAYER, isWalkable: false, isTransparent: true)
        {
            Name = "Player";
            FOVRadius = 14;
            OverallHealth = 100.0;
            Spells = SpellManager.Instance.GetAllSpells();
            VisionDirection = XYZRelativeDirection.Forward;
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

        public List<AttackType> GetAvailableAttacks()
        {
            List<AttackType> attacks = new List<AttackType>();

            return attacks;
        }

        public Attack GetAttack(uint attackId)
        {
            return Attacks.FirstOrDefault(m => m.ID == attackId);
        }

        public Spell GetSpell(uint spellId)
        {
            return Spells.FirstOrDefault(m => m.ID == spellId);
        }

        public bool CanAttack(uint attackId, Actor target)
        {
            Attack targetAttack = GetAttack(attackId);
            if (targetAttack == null || targetAttack.ID != attackId)
            {
                DebugManager.Instance.AddMessage(new DebugMessage($"Player::CanAttack: Invalid ID: {attackId}", DebugSource.System));
                return false;
            }

            if (this.CurrentMap.DistanceMeasurement.Calculate(this.Position, target.Position) < (1 + targetAttack.Range))
            {
                return true;
            }

            return false;
        }

        public bool DoAttack(uint attackId, Actor target)
        {
            Attack targetAttack = GetAttack(attackId);
            if (targetAttack == null || targetAttack.ID != attackId)
            {
                DebugManager.Instance.AddMessage(new DebugMessage($"Player::DoAttack: Invalid ID: {attackId}", DebugSource.System));
                return false;
            }

            DebugManager.Instance.AddMessage(new DebugMessage($"Player::DoAttack: {targetAttack.Name}", DebugSource.System));
            return true;
        }

        public bool CastSpell(uint spellId, Actor target)
        {
            Spell targetSpell = GetSpell(spellId);
            if (targetSpell == null || targetSpell.ID != spellId)
            {
                DebugManager.Instance.AddMessage(new DebugMessage($"Player::CastSpell: Invalid ID: {spellId}", DebugSource.System));
                return false;
            }

            return true;
        }

        public Attack GetBestAttack(Actor target)
        {
            return null;
        }
    }
}
