using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Controls;
using SadConsole.Themes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Menus
{
    internal class PlayerMeleeAttackMenu : ControlsConsole
    {
        public SelectionButton AttackBtn { get; private set; }
        //public Action AttackBtnAction { get; set; }
        //public SelectionButton WrestleBtn { get; private set; }
        //public Action WrestleBtnAction { get; set; }
        //public SelectionButton RangedAttackBtn { get; private set; }
        //public Action RangedAttackBtnAction { get; set; }

        /// <summary>
        /// Displays a list of available attack options the player has (eg melee attack, ranged attack, spell)
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public PlayerMeleeAttackMenu(int width, int height) : base(width, height)
        {
            //var equippedWeapons = MyGame.World.Player.EquippedWeapons;

            ButtonTheme AttackBtnTheme = (ButtonTheme)MyGame.GameSettings.ButtonTheme.Clone();
            AttackBtnTheme.EndCharacterLeft = '1';

            AttackBtn = new SelectionButton(MyGame.GameSettings.GameMenuWidth, 1)
            {
                Name = "Melee",
                Text = "Melee Attack",
                TextAlignment = HorizontalAlignment.Center,
                Position = new Point(0, 0),
                Theme = AttackBtnTheme,
                ThemeColors = MyGame.GameSettings.ButtonColors,
            };

            //AttackBtn.MouseEnter += AttackBtn_MouseEnter;
            AttackBtn.Click += AttackBtn_Click;

            Add(AttackBtn);
        }

        private void AttackBtn_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }
    }
}
