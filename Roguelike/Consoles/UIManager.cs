using Microsoft.Xna.Framework.Input;
using Roguelike.Consoles;
using SadConsole;
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
        private UIManagerState CurrentState;
        private GameConsole GameScreen;

        public const int GameWidth = 120;
        public const int GameHeight = 60;

        public UIManager()
        {
            GameScreen = new GameConsole(GameWidth, GameHeight, SadConsole.Global.FontDefault);
            Children.Add(GameScreen);

            CurrentState = UIManagerState.MainGame;

            Parent = SadConsole.Global.CurrentScreen;
        }

        public override void Update(System.TimeSpan timeElapsed)
        {
            CheckKeyboard();
            base.Update(timeElapsed);
        }

        private void CheckKeyboard()
        {
            // GLOBAL!!!! keybinds
            if (SadConsole.Global.KeyboardState.IsKeyReleased(Keys.Escape))
            {
                SadConsole.Game.Instance.Exit();
            }
        }
    }
}
