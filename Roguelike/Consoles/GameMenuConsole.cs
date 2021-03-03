using SadConsole;
using GoRogue;
using Roguelike.Helpers;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Text;
using Roguelike.Menus;
using SadConsole.Controls;

namespace Roguelike.Consoles
{
    internal class GameMenuConsole : SadConsole.ControlsConsole
    {
        private Stack<ControlsConsole> MenuStack { get; set; }
        
        //allow for on-demand loading of submenus to prevent attempting to read from game/player before they are initialized
        private PlayerBaseMenu _pbmInstance { get; set; }
        private PlayerBaseMenu PlayerBaseMenu
        {
            get
            {
                if (_pbmInstance == null)
                {
                    _pbmInstance = new PlayerBaseMenu(Width, Height);
                }

                return _pbmInstance;
            }
        }
        
        private PlayerAttackMenu _pamInstance { get; set; }
        private PlayerAttackMenu PlayerAttackMenu
        {
            get
            {
                if (_pamInstance == null)
                {
                    _pamInstance = new PlayerAttackMenu(Width, Height);
                }

                return _pamInstance;
            }
        }
        
        private PlayerSpellMenu _psmInstance { get; set; }
        private PlayerSpellMenu PlayerSpellMenu
        {
            get
            {
                if (_psmInstance == null)
                {
                    _psmInstance = new PlayerSpellMenu(Width, Height);
                }

                return _psmInstance;
            }
        }

        public GameMenuConsole(int width, int height) : base(width, height)
        {
            IsVisible = true;
            UseKeyboard = true;

            MenuStack = new Stack<ControlsConsole>();

            AddMenu(PlayerBaseMenu);

            PlayerBaseMenu.AttackBtnAction = () => AddMenu(PlayerAttackMenu);
            PlayerBaseMenu.SpellBtnAction = () => AddMenu(PlayerSpellMenu);
            //PlayerAttackMenu.AttackBtnAction = () => AddMenu();
        }

        public override bool ProcessKeyboard(SadConsole.Input.Keyboard info)
        {
            if (info.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.Escape) ||
                info.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.Back))
            {
                if (GoToPreviousMenu())
                {
                    return true;
                }
            }
            else if (MenuStack.Peek().ProcessKeyboard(info))
            {
                return true;
            }

            return base.ProcessKeyboard(info);
        }

        public bool GoToPreviousMenu()
        {
            if (MenuStack.Count > 1)
            {
                Children.Remove(MenuStack.Pop());
                return true;
            }

            return false;
        }

        private void AddMenu(ControlsConsole menu)
        {
            MenuStack.Push(menu);
            Children.Add(menu);
        }
    }
}
