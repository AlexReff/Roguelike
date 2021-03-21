//using Microsoft.Xna.Framework;
//using Roguelike.Systems;
//using SadConsole;
//using SadConsole.Controls;
//using SadConsole.Themes;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Roguelike.Menus
//{
//    internal class PlayerSpellMenu : ControlsConsole
//    {
//        private List<Spell> PlayerSpells { get; set; }

//        /// <summary>
//        /// Displays a list of available attack options the player has (eg melee attack, ranged attack, spell)
//        /// </summary>
//        /// <param name="width"></param>
//        /// <param name="height"></param>
//        public PlayerSpellMenu(int width, int height) : base(width, height)
//        {
//            UseKeyboard = true;
//            PlayerSpells = MyGame.World.Player.Spells;

//            for (int i = 0; i < PlayerSpells.Count; i++)
//            {
//                Spell spell = PlayerSpells[i];

//                ButtonTheme thisSpellTheme = (ButtonTheme)MyGame.GameSettings.ButtonTheme.Clone();
//                thisSpellTheme.EndCharacterLeft = (i + 1).ToString().ToCharArray()[0];

//                SelectionButton spellBtn = new SelectionButton(MyGame.GameSettings.GameMenuWidth, 1)
//                {
//                    Name = spell.Name,
//                    Text = spell.Name + $" ({spell.BaseManaCost})",
//                    TextAlignment = HorizontalAlignment.Center,
//                    Position = new Point(0, i),
//                    Theme = thisSpellTheme,
//                    ThemeColors = MyGame.GameSettings.ButtonColors,
//                };

//                spellBtn.Click += GetSpellClickFunction(spell);

//                Add(spellBtn);
//            }

//            for (var i = 0; i < ControlsList.Count; i++)
//            {
//                var targetPrev = i > 0 ? i : ControlsList.Count - 1;
//                var targetNext = i < ControlsList.Count - 1 ? i : 0;
//                ((SelectionButton)ControlsList[i]).PreviousSelection = (SelectionButton)ControlsList[targetPrev];
//                ((SelectionButton)ControlsList[i]).NextSelection = (SelectionButton)ControlsList[targetNext];
//            }
//        }

//        public override bool ProcessKeyboard(SadConsole.Input.Keyboard info)
//        {
//            for (int i = 0; i < PlayerSpells.Count; i++)
//            {
//                if (info.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.NumPad1 + i))
//                {
//                    var selectedSpell = PlayerSpells[i];
//                    GetSpellClickFunction(selectedSpell)(this, null);
//                }
//            }

//            return false;
//        }

//        private EventHandler GetSpellClickFunction(Spell spell)
//        {
//            return (object sender, EventArgs e) =>
//            {
//                DebugManager.Instance.AddMessage(new DebugMessage($"Player cast spell: {spell.Name}", DebugSource.Command));
//            };
//        }

//        //private void SpellBtn_Click(object sender, System.EventArgs e)
//        //{
//        //    if (AttackBtnAction != null)
//        //    {
//        //        AttackBtnAction();
//        //        AttackBtn.IsFocused = false;
//        //    }
//        //}
//    }
//}
