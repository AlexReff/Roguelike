using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Roguelike.Consoles;
using Roguelike.Helpers;
using Roguelike.Models;
using Roguelike.Settings;
using Roguelike.Spells;
using System.IO;
using System.Text.Json;

namespace Roguelike
{
    internal class MyGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public static GameSettings GameSettings { get; private set; }
        public static UIManager UIManager { get; private set; }
        public static CommandManager CommandManager { get; private set; }
        public static World World { get; private set; }
        public static SpellSkillManager SpellManager { get; private set; }

        public MyGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            GameSettings = new GameSettings();
        }

        protected override void Initialize()
        {
            // Setup the engine and create the main window.
            SadConsole.Game.Create($"Content\\Fonts\\{GameSettings.GameFont}.font", GameSettings.GameWidth, GameSettings.GameHeight);
            //SadConsole.Game.Create(GameWidth, GameHeight, InitGame);

            // Hook the start event so we can add consoles to the system.
            SadConsole.Game.OnInitialize = Init;

            SadConsole.Game.OnDestroy = Destroyed;

            base.Initialize();

            // Start the game.
            SadConsole.Game.Instance.Run();
            
            // Code here will not run until the game window closes.
            
            SadConsole.Game.Instance.Dispose();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            //Roboto = this.Content.Load<SpriteFont>("fonts/Roboto");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        //protected override void Draw(GameTime gameTime)
        //{
        //    GraphicsDevice.Clear(Color.CornflowerBlue);

        //    // TODO: Add your drawing code here
        //    //_spriteBatch.Begin();
        //    //_spriteBatch.DrawString(Roboto, "TESTING", new Vector2(10, 10), Color.White);

        //    //_spriteBatch.End();

        //    base.Draw(gameTime);
        //}

        private void Init()
        {
            FontManager.Instance.LoadFonts();

            CommandManager = new CommandManager();
            UIManager = new UIManager();
            World = new World();
            SpellManager = new SpellSkillManager();
        }

        private void Destroyed()
        {
            this.Exit();
        }
    }
}
