using GoRogue;
using Microsoft.Xna.Framework.Input;
using Roguelike.Consoles;
using SadConsole;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Helpers
{
    enum UIManagerState
    {
        MainScreen,
        MainGame,
        CharacterCreation,
    }

    class UIManager : ContainerConsole
    {
        /// <summary>
        /// The width of the entire screen/window
        /// </summary>
        public static readonly int GameWidth = 120;
        /// <summary>
        /// The height of the entire screen/window
        /// </summary>
        public static readonly int GameHeight = 60;
        /// <summary>
        /// The width of the 'Game' screen sub-area
        /// </summary>
        public static readonly int MapScreenWidth = 60;
        /// <summary>
        /// The height of the 'Game' screen sub-area
        /// </summary>
        public static readonly int MapScreenHeight = 40;
        /// <summary>
        /// The width of the in-game generated Dungeon
        /// </summary>
        public static readonly int MapWidth = 100;
        /// <summary>
        /// The height of the in-game generated Dungeon
        /// </summary>
        public static readonly int MapHeight = 100;

        private static readonly Dictionary<Keys, Direction> KEYS_DIRECTIONS = new Dictionary<Keys, Direction>
        {
            { Keys.NumPad7, Direction.UP_LEFT }, { Keys.NumPad8, Direction.UP }, { Keys.NumPad9, Direction.UP_RIGHT },
            { Keys.NumPad4, Direction.LEFT }, { Keys.NumPad6, Direction.RIGHT },
            { Keys.NumPad1, Direction.DOWN_LEFT }, { Keys.NumPad2, Direction.DOWN }, { Keys.NumPad3, Direction.DOWN_RIGHT },
            { Keys.Up, Direction.UP }, { Keys.Down, Direction.DOWN }, { Keys.Left, Direction.LEFT }, { Keys.Right, Direction.RIGHT },
            { Keys.Q, Direction.UP_LEFT }, { Keys.W, Direction.UP }, { Keys.E, Direction.UP_RIGHT },
            { Keys.A, Direction.LEFT }, { Keys.D, Direction.RIGHT },
            { Keys.Z, Direction.DOWN_LEFT }, { Keys.X, Direction.DOWN }, { Keys.C, Direction.DOWN_RIGHT }
        };

        public UIManagerState CurrentState { get; private set; }
        public Console CurrentScreen { get; private set; }

        private GameConsole GameScreen;


        public UIManager()
        {
            GameScreen = new GameConsole(GameWidth, GameHeight, SadConsole.Global.FontDefault);
            Children.Add(GameScreen);

            CurrentState = UIManagerState.MainGame;
            CurrentScreen = GameScreen;

            Parent = SadConsole.Global.CurrentScreen;

            IsFocused = true;
            IsVisible = true;
        }
        
        public void SetGameMap(GameMap map)
        {
            GameScreen.SetMap(map);
        }

        public override bool ProcessKeyboard(SadConsole.Input.Keyboard info)
        {
            // GLOBAL!!!! keybinds
            if (info.IsKeyReleased(Keys.Escape))
            {
                SadConsole.Game.Instance.Exit();
            }

            switch (CurrentState)
            {
                case UIManagerState.MainScreen:
                case UIManagerState.CharacterCreation:
                case UIManagerState.MainGame:
                default:
                    // PLAYER MOVEMENT START
                    Direction moveDirection = Direction.NONE;

                    // Simplified way to check if any key we care about is pressed and set movement direction.
                    foreach (Keys key in KEYS_DIRECTIONS.Keys)
                    {
                        if (info.IsKeyPressed(key))
                        {
                            moveDirection = KEYS_DIRECTIONS[key];
                            break;
                        }
                    }

                    if (moveDirection != Direction.NONE)
                    {
                        MyGame.CommandManager.MovePlayer(moveDirection);
                        return true;
                    }
                    // PLAYER MOVEMENT END

                    break;
            }

            // Continue processing other hotkeys

            return base.ProcessKeyboard(info);
        }
    }
}
