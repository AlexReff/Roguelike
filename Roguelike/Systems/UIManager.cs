using GoRogue;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Roguelike.Consoles;
using Roguelike.Entities;
using Roguelike.Settings;
using SadConsole;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Systems
{
    enum UIManagerState
    {
        MainScreen,
        MainGame,
        CharacterCreation,
        WorldGen,
    }

    class UIManager : ContainerConsole
    {
        private static readonly Dictionary<Keys, Direction> KEYS_DIRECTIONS = new Dictionary<Keys, Direction>
        {
            //{ Keys.NumPad7, Direction.UP_LEFT }, { Keys.NumPad8, Direction.UP }, { Keys.NumPad9, Direction.UP_RIGHT },
            //{ Keys.NumPad4, Direction.LEFT }, { Keys.NumPad6, Direction.RIGHT },
            //{ Keys.NumPad1, Direction.DOWN_LEFT }, { Keys.NumPad2, Direction.DOWN }, { Keys.NumPad3, Direction.DOWN_RIGHT },
            //{ Keys.Up, Direction.UP }, { Keys.Down, Direction.DOWN }, { Keys.Left, Direction.LEFT }, { Keys.Right, Direction.RIGHT },
            { Keys.Q, Direction.UP_LEFT }, { Keys.W, Direction.UP }, { Keys.E, Direction.UP_RIGHT },
            { Keys.A, Direction.LEFT }, { Keys.D, Direction.RIGHT },
            { Keys.Z, Direction.DOWN_LEFT }, { Keys.S, Direction.DOWN }, { Keys.C, Direction.DOWN_RIGHT }
        };

        public UIManagerState CurrentState { get; private set; }
        public Console CurrentScreen { get; private set; }

        public GameConsole GameScreen { get; private set; }
        public WorldGenConsole WorldGenScreen { get; private set; }

        //public Stack<Console> Screens { get; private set; }

        public UIManager()
        {
            var fullWidth = MyGame.GameSettings.GameWidth;
            var fullHeight = MyGame.GameSettings.GameHeight + (MyGame.GameSettings.EnableDebugOutput ? MyGame.GameSettings.DebugHeight : 0);
            
            GameScreen = new GameConsole(fullWidth, fullHeight, SadConsole.Global.FontDefault);

            WorldGenScreen = new WorldGenConsole(fullWidth, fullHeight, Color.Black, Color.White);

            SetCurrentUIScreen(UIManagerState.WorldGen);

            // set this as the current screen
            Parent = SadConsole.Global.CurrentScreen;
            IsFocused = true;
        }

        public void SetCurrentUIScreen(UIManagerState target)
        {
            Children.Clear();

            switch (target)
            {
                case UIManagerState.WorldGen:
                    Children.Add(WorldGenScreen);
                    CurrentScreen = WorldGenScreen;
                    break;
                case UIManagerState.MainGame:
                case UIManagerState.CharacterCreation:
                case UIManagerState.MainScreen:
                default:
                    Children.Add(GameScreen);
                    CurrentScreen = GameScreen;
                    break;
            }

            CurrentState = target;
        }
        
        public void SetGameMap(GameMap map)
        {
            GameScreen.SetMap(map);
        }

        public void CenterOnActor(Actor actor)
        {
            GameScreen.CenterOnActor(actor);
        }

        public override bool ProcessKeyboard(SadConsole.Input.Keyboard info)
        {
            //DebugManager.Instance.AddMessage("UIManager::ProcessKeyboard");

            //// GLOBAL!!!! keybinds
            //if (info.IsKeyReleased(Keys.Escape))
            //{
            //    SadConsole.Game.Instance.Exit();
            //    return true;
            //}

            switch (CurrentState)
            {
                case UIManagerState.MainScreen:
                case UIManagerState.CharacterCreation:
                case UIManagerState.WorldGen:
                    if (WorldGenScreen.ProcessKeyboard(info))
                    {
                        return true;
                    }
                    break;
                case UIManagerState.MainGame:
                default:
                    if (MyGame.CommandManager.IsGameOver)
                    {
                        return false;
                    }
                    if (MyGame.Karma.IsPlayerTurn)
                    {
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
                            //if (MyGame.CommandManager.MovePlayer(moveDirection))
                            //{
                            //    //player successfully performed movement or action
                            //    MyGame.CommandManager.EndPlayerTurn();
                            //}
                            MyGame.CommandManager.Input_PlayerMoveBump(moveDirection);

                            // tell processKeyboard that the input was processed
                            return true;
                        }
                        // PLAYER MOVEMENT END

                        // Continue processing other hotkeys
                    }
                    else
                    {
                        //MyGame.Karma.DoTime();
                    }

                    break;
            }

            if (GameScreen.ControlsConsole.ProcessKeyboard(info))
            {
                return true;
            }

            if (info.IsKeyReleased(Keys.Escape))
            {
                SadConsole.Game.Instance.Exit();
                return true;
            }

            return base.ProcessKeyboard(info);
        }
    }
}
