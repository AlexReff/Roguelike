using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Controls;
using SadConsole.Themes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Menus
{
    internal class PlayerAttackMenu : ControlsConsole
    {
        public SelectionButton AttackBtn { get; private set; }
        public Action AttackBtnAction { get; set; }
        public SelectionButton WrestleBtn { get; private set; }
        public Action WrestleBtnAction { get; set; }
        public SelectionButton RangedAttackBtn { get; private set; }
        public Action RangedAttackBtnAction { get; set; }

        /// <summary>
        /// Displays a list of available attack options the player has (eg melee attack, ranged attack, spell)
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public PlayerAttackMenu(int width, int height) : base(width, height)
        {
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

            ButtonTheme WrestleBtnTheme = (ButtonTheme)MyGame.GameSettings.ButtonTheme.Clone();
            WrestleBtnTheme.EndCharacterLeft = '2';

            WrestleBtn = new SelectionButton(MyGame.GameSettings.GameMenuWidth, 1)
            {
                Name = "Wrestle",
                Text = "Wrestle",
                TextAlignment = HorizontalAlignment.Center,
                Position = new Point(0, 1),
                Theme = WrestleBtnTheme,
                ThemeColors = MyGame.GameSettings.ButtonColors,
            };

            //SpellBtn.MouseEnter += SpellBtn_MouseEnter;
            WrestleBtn.Click += WrestleBtn_Click;

            Add(WrestleBtn);

            ButtonTheme RangedAttackBtnTheme = (ButtonTheme)MyGame.GameSettings.ButtonTheme.Clone();
            RangedAttackBtnTheme.EndCharacterLeft = '3';

            RangedAttackBtn = new SelectionButton(MyGame.GameSettings.GameMenuWidth, 1)
            {
                Name = "Ranged",
                Text = "Ranged Attack",
                TextAlignment = HorizontalAlignment.Center,
                Position = new Point(0, 2),
                Theme = RangedAttackBtnTheme,
                ThemeColors = MyGame.GameSettings.ButtonColors,
            };

            RangedAttackBtn.Click += RangedAttackBtn_Click;

            Add(RangedAttackBtn);

            for (var i = 0; i < ControlsList.Count; i++)
            {
                var targetPrev = i > 0 ? i : ControlsList.Count - 1;
                var targetNext = i < ControlsList.Count - 1 ? i : 0;
                ((SelectionButton)ControlsList[i]).PreviousSelection = (SelectionButton)ControlsList[targetPrev];
                ((SelectionButton)ControlsList[i]).NextSelection = (SelectionButton)ControlsList[targetNext];
            }
        }

        private void RangedAttackBtn_Click(object sender, System.EventArgs e)
        {
            if (RangedAttackBtnAction != null)
            {
                RangedAttackBtnAction();
                RangedAttackBtn.IsFocused = false;
            }
        }

        private void WrestleBtn_Click(object sender, System.EventArgs e)
        {
            if (WrestleBtnAction != null)
            {
                WrestleBtnAction();
                WrestleBtn.IsFocused = false;
            }
        }

        private void AttackBtn_Click(object sender, System.EventArgs e)
        {
            if (AttackBtnAction != null)
            {
                AttackBtnAction();
                AttackBtn.IsFocused = false;
            }
        }
    }
}
