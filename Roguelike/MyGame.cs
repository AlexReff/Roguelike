using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Roguelike.Consoles;
using Roguelike.Helpers;
using Roguelike.Models;
using System.IO;
using System.Text.Json;

namespace Roguelike
{
    public class MyGame : Game
    {
        private MainConsole MainScreen;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private const int GameWidth = 100;
        private const int GameHeight = 60;

        private SpriteFont Roboto;

        public MyGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // Setup the engine and create the main window.
            //SadConsole.Game.Create("Content\\Fonts\\Anno.font", GameWidth, GameHeight, InitGame);
            SadConsole.Game.Create(GameWidth, GameHeight, InitGame);

            // Hook the start event so we can add consoles to the system.
            SadConsole.Game.OnInitialize = Init;
            //SadConsole.Game.OnDraw = Draw;

            SadConsole.Game.OnDestroy = Destroyed;

            base.Initialize();

            // Start the game.
            SadConsole.Game.Instance.Run();
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

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            //_spriteBatch.Begin();
            //_spriteBatch.DrawString(Roboto, "TESTING", new Vector2(10, 10), Color.White);

            //_spriteBatch.End();

            base.Draw(gameTime);
        }

        private void Init()
        {
            FontManager.Instance.LoadFonts();
            SadConsole.Global.FontDefault = SadConsole.Global.Fonts["Anno"].GetFont(SadConsole.Font.FontSizes.One);
            //RootDynamicConsole.Resize(Global.WindowWidth / RootDynamicConsole.Font.Size.X, Global.WindowHeight / RootDynamicConsole.Font.Size.Y, false);
            //SadConsole.Game.Instance.
            //MainScreen.SetFont(SadConsole.Global.Fonts["Tigrex3d"].GetFont(SadConsole.Font.FontSizes.One));
            // Here we pass the viewport and map size as the same, but the map could be larger and the camera would center on the player.
            MainScreen = new MainConsole(GameWidth, GameHeight, SadConsole.Global.FontDefault);
            //MapScreen = new GameMapConsole(MapWidth, MapHeight, MapScreenWidth, MapScreenHeight, MapScreenPosition);
            
            //SadConsole.Global.CurrentScreen = MapScreen;
            // SadConsole.Global.CurrentScreen
            
            //SadConsole.Global.CurrentScreen = MainScreen;
            //MainScreen.Children.Add(MapScreen);

        }

        private void InitGame(Game game)
        {
            //FontManager.Instance.LoadFonts();
            //SadConsole.Global.FontDefault = SadConsole.Global.Fonts["Anno"].GetFont(SadConsole.Font.FontSizes.One);
        }

        private void Destroyed()
        {
            this.Exit();
        }
    }
}
