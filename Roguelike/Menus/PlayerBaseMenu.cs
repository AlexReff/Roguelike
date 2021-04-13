using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Controls;
using SadConsole.Themes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Menus
{
    internal class PlayerBaseMenu : ControlsConsole
    {
        public SelectionButton AttackBtn { get; private set; }
        public Action AttackBtnAction { get; set; }
        public SelectionButton SpellBtn { get; private set; }
        public Action SpellBtnAction { get; set; }
        public SelectionButton TurnBtn { get; private set; }
        public Action TurnBtnAction { get; set; }

        public PlayerBaseMenu(int width, int height) : base(width, height)
        {
            //UseKeyboard = true;

            ButtonTheme AttackBtnTheme = (ButtonTheme)MyGame.GameSettings.ButtonTheme.Clone();
            AttackBtnTheme.EndCharacterLeft = '1';

            AttackBtn = new SelectionButton(MyGame.GameSettings.GameMenuWidth, 1)
            {
                Name = "Attack",
                Text = "Attack",
                TextAlignment = HorizontalAlignment.Center,
                Position = new Point(0, 0),
                Theme = AttackBtnTheme,
                ThemeColors = MyGame.GameSettings.ButtonColors,
            };
            
            AttackBtn.Click += AttackBtn_Click;

            Add(AttackBtn);

            ButtonTheme SpellBtnTheme = (ButtonTheme)MyGame.GameSettings.ButtonTheme.Clone();
            SpellBtnTheme.EndCharacterLeft = '2';

            SpellBtn = new SelectionButton(MyGame.GameSettings.GameMenuWidth, 1)
            {
                Name = "Spell",
                Text = "Cast Spell",
                TextAlignment = HorizontalAlignment.Center,
                Position = new Point(0, 1),
                Theme = SpellBtnTheme,
                ThemeColors = MyGame.GameSettings.ButtonColors,
            };

            SpellBtn.Click += SpellBtn_Click;

            Add(SpellBtn);

            ButtonTheme TurnBtnTheme = (ButtonTheme)MyGame.GameSettings.ButtonTheme.Clone();
            TurnBtnTheme.EndCharacterLeft = '3';

            TurnBtn = new SelectionButton(MyGame.GameSettings.GameMenuWidth, 1)
            {
                Name = "Turn",
                Text = "Turn Direction",
                TextAlignment = HorizontalAlignment.Center,
                Position = new Point(0, 2),
                Theme = TurnBtnTheme,
                ThemeColors = MyGame.GameSettings.ButtonColors,
            };

            TurnBtn.Click += TurnBtn_Click;

            Add(TurnBtn);

            for (var i = 0; i < ControlsList.Count; i++)
            {
                var targetPrev = i > 0 ? i : ControlsList.Count - 1;
                var targetNext = i < ControlsList.Count - 1 ? i : 0;
                ((SelectionButton)ControlsList[i]).PreviousSelection = (SelectionButton)ControlsList[targetPrev];
                ((SelectionButton)ControlsList[i]).NextSelection = (SelectionButton)ControlsList[targetNext];
            }
        }

        private void AttackBtn_Click(object sender, System.EventArgs e)
        {
            if (AttackBtnAction != null)
            {
                AttackBtnAction();
            }
        }

        private void SpellBtn_Click(object sender, System.EventArgs e)
        {
            if (SpellBtnAction != null)
            {
                SpellBtnAction();
            }
        }

        private void TurnBtn_Click(object sender, EventArgs e)
        {
            if (TurnBtnAction != null)
            {
                TurnBtnAction();
            }
        }

        public override bool ProcessKeyboard(SadConsole.Input.Keyboard info)
        {
            if (info.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.NumPad1))
            {
                AttackBtn_Click(this, null);
                return true;
            }
            else if (info.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.NumPad2))
            {
                SpellBtn_Click(this, null);
                return true;
            }
            else if (info.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.NumPad3))
            {
                TurnBtn_Click(this, null);
                return true;
            }

            return false;
        }
    }
}
