using Microsoft.Xna.Framework;
using Roguelike.Systems;
using SadConsole;
using SadConsole.Components;
using SadConsole.Controls;
using SadConsole.Input;
using SadConsole.Themes;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Consoles
{
    internal class WorldGenConsole : Console
    {
        private const int ControlsWidth = 40;

        private Color BackgroundColor;
        private Color BorderColor;
        private ControlsConsole ControlConsole;
        private Console ControlContainer;
        private Console MapDisplayConsole;
        private Label SeedLabel;

        private DebugConsole DebugScreen;
        private CharMapOutputConsole CharMapScreen;

        private WorldGen Generator;

        private Button GenerateBtn;
        private Button ToggleMapBtn;

        private int displayMode = 0;

        public WorldGenConsole(int width, int height, Color backgroundColor, Color borderColor) : base(width, height)
        {
            UseKeyboard = true;
            BackgroundColor = backgroundColor;
            BorderColor = borderColor;

            ControlContainer = new Console(width - 2, height - 2);
            ControlContainer.Position = new Point(1, 1);
            //Children.Add(ControlContainer);

            ControlConsole = new ControlsConsole(ControlsWidth * 2, 3, SadConsole.Global.Fonts["CP850"].GetFont(Font.FontSizes.One));
            ControlConsole.Position = new Point(1, 1);
            ControlContainer.Children.Add(ControlConsole);

            ButtonTheme BtnTheme = (ButtonTheme)MyGame.GameSettings.ButtonTheme.Clone();
            BtnTheme.EndCharacterLeft = '1';
            GenerateBtn = new Button(ControlsWidth * 2, 1);
            GenerateBtn.Theme = BtnTheme;
            GenerateBtn.ThemeColors = MyGame.GameSettings.ButtonColors;
            GenerateBtn.Click += Btn_Click;
            GenerateBtn.Text = "Generate World";
            ControlConsole.Add(GenerateBtn);

            ButtonTheme ToggleMapTheme = (ButtonTheme)MyGame.GameSettings.ButtonTheme.Clone();
            ToggleMapTheme.EndCharacterLeft = '2';
            ToggleMapBtn = new Button(ControlsWidth * 2, 1);
            ToggleMapBtn.Position = new Point(0, 1);
            ToggleMapBtn.Theme = ToggleMapTheme;
            ToggleMapBtn.ThemeColors = MyGame.GameSettings.ButtonColors;
            ToggleMapBtn.Click += ToggleMapBtn_Click;
            ToggleMapBtn.Text = "Toggle Display";
            ControlConsole.Add(ToggleMapBtn);


            //ButtonTheme LabelTheme = (ButtonTheme)MyGame.GameSettings.ButtonTheme.Clone();
            //LabelTheme.EndCharacterLeft = '2';
            SeedLabel = new Label(ControlsWidth * 2);
            SeedLabel.Position = new Point(0, 2);
            //SeedLabel.Theme = LabelTheme;
            SeedLabel.ThemeColors = MyGame.GameSettings.ButtonColors;
            ControlConsole.Add(SeedLabel);

            //TextBox floatValBox = new TextBox(ControlsWidth * 2);
            //floatValBox.TextChanged += FloatValBox_TextChanged;
            //floatValBox.Text = ".2f";
            //floatValBox.Position = new Point(1, 3);
            //floatValBox.AllowDecimal = true;
            //floatValBox.FocusOnClick = true;
            //floatValBox.UseKeyboard = true;
            //ControlConsole.Add(floatValBox);

            Generator = new WorldGen();

            if (MyGame.GameSettings.EnableDebugOutput)
            {
                // Debug Console
                Point dbgPos = new Point(0, height - MyGame.GameSettings.DebugHeight);
                DebugScreen = new DebugConsole(MyGame.GameSettings.DebugConsoleWidth, MyGame.GameSettings.DebugHeight, MyGame.GameSettings.DebugScreenBgColor, MyGame.GameSettings.DebugScreenBorderColor);
                DebugScreen.Position = dbgPos;
                //Children.Add(DebugScreen);
            }

            if (MyGame.GameSettings.EnableCharMapOutput)
            {
                // Character map for easy reference
                //Point chrMapPos = new Point(Width - Font.Columns - CharMapOutputConsole.AdditionalWidth, height - Font.Rows - CharMapOutputConsole.AdditionalHeight);
                Point chrMapPos = new Point(DebugScreen.Position.X + DebugScreen.Width + CharMapOutputConsole.AdditionalWidth, DebugScreen.Position.Y);
                // chrMapPos.X *= 2;
                CharMapScreen = new CharMapOutputConsole(Font.Columns, Font.Rows);
                CharMapScreen.Position = chrMapPos;
                //Children.Add(CharMapScreen);
            }

            UseKeyboard = true;

            StartGeneration();
        }

        private void ToggleMapBtn_Click(object sender, System.EventArgs e)
        {
            switch (displayMode++ % 3)
            {
                case 0:
                    Generator.Draw(MapDisplayConsole);
                    break;
                case 1:
                    Generator.DrawTemp(MapDisplayConsole);
                    break;
                case 2:
                default:
                    Generator.DrawDebug(MapDisplayConsole);
                    break;
            }
        }

        private void Btn_Click(object sender, System.EventArgs e)
        {
            StartGeneration();
        }

        private void MyClear()
        {
            Children.Clear();

            this.DrawBorderBgTitle(new Rectangle(0, 0, Width, Height - MyGame.GameSettings.DebugHeight), "World Generation", BackgroundColor, BorderColor);

            //Children.Add(ControlConsole);
            Children.Add(ControlContainer);
            Children.Add(CharMapScreen);

            if (MyGame.GameSettings.EnableDebugOutput)
            {
                Children.Add(DebugScreen);
            }
        }

        private void StartGeneration()
        {
            MyClear();

            Generator.GenerateSeed();

            SeedLabel.DisplayText = Generator.MasterSeed;
            SeedLabel.IsDirty = true;

            int width = 32;
            int height = 48;

            var mapContainer = new Console(width, height);
            //MapDisplayConsole = new Console(width * 4, height * 4, Font.Master.GetFont(Font.FontSizes.Quarter));
            MapDisplayConsole = new Console(width, height);
            //int xPos = (int)System.Math.Floor((Width - mapDisplayConsole.Width) / 2.0);
            int xPos = Width - mapContainer.Width - 1;
            mapContainer.Position = new Point(xPos, 1);

            //mapDisplayConsole.FillWithRandomGarbage();
            //mapDisplayConsole.Fill(null, Color.Aqua, '#');

            Children.Add(mapContainer);
            mapContainer.Children.Add(MapDisplayConsole);

            Generator.ProcessMap(MapDisplayConsole);

            IsDirty = true;
        }

        public override bool ProcessKeyboard(Keyboard info)
        {
            if (info.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.Escape))
            {
                MyGame.CommandManager.Exit();
                return true;
            }
            else if (info.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.NumPad1))
            {
                GenerateBtn.DoClick();
                return true;
            }
            else if (info.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.NumPad2))
            {
                ToggleMapBtn.DoClick();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

