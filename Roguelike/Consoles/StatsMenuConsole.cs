using GoRogue;
using Microsoft.Xna.Framework;
using Roguelike.Entities;
using Roguelike.Models;
using Roguelike.Systems;
using SadConsole;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace Roguelike.Consoles
{
    internal class StatsMenuConsole : AnimatedConsole
    {
        private const int RightTextPadding = 8;
        private static readonly Color BoxColor = new Color(45, 45, 45);

        private AnimatedConsole OutputConsole;
        private AnimatedConsole HealthBar;
        private OrderedDictionary LimbBars;

        private Player Player;
        private Coord HealthBarPosition;

        public StatsMenuConsole(int width, int height) : base("Stats", width, height)
        {
            OutputConsole = new AnimatedConsole("StatOutput", width * 2 - 1, height - 2, SadConsole.Global.Fonts["CP850"].GetFont(Font.FontSizes.One));
            OutputConsole.Position = new Point(1, 1);
            Children.Add(OutputConsole);

            DefaultForeground = Color.White;
            DefaultBackground = Color.Black;

            HealthBarPosition = new Coord(1, 1);

            LimbBars = new OrderedDictionary();

            EventManager.Instance.ActorDiedEvent += Instance_ActorDiedEvent;
            EventManager.Instance.ActorHealthChangedEvent += Instance_ActorHealthChangedEvent;
            EventManager.Instance.ActorMovedEvent += Instance_ActorMovedEvent;
            EventManager.Instance.PlayerSpawnedEvent += Instance_PlayerSpawnedEvent;
        }
        private void Redraw()
        {
            this.Clear();
            Helpers.DrawBorderBgTitle(this, new Microsoft.Xna.Framework.Rectangle(0, 0, Width, Height), "Stats", Color.Black, new Color(0, 142, 0));

            OutputConsole.Clear();
            //draw all animations/overlays
            //DrawHealthBar();
            DrawBodyPartHealth();
        }

        private void Instance_PlayerSpawnedEvent(Player player)
        {
            //PlayerHealth = player.Health;
            //PlayerMaxHealth = player.MaxHealth;
            Player = player;

            Redraw();
        }

        private void Instance_ActorMovedEvent(Actor actor, Coord prevPos, Coord newPos)
        {
            //
        }

        private void Instance_ActorHealthChangedEvent(Actor actor, double prevHealth)
        {
            bool dirty = false;

            if (actor is Player)
            {
                //Update the health display
                //PlayerHealth = actor.Health;
                //PlayerMaxHealth = actor.MaxHealth;
                Player = actor as Player;
                dirty = true;
            }

            if (dirty)
            {
                Redraw();
            }
        }

        private void Instance_ActorDiedEvent(Actor actor)
        {
            bool dirty = false;

            if (actor is Player)
            {
                Player = actor as Player;
                dirty = true;
            }

            if (dirty)
            {
                Redraw();
            }
        }

        private void DrawBodyPartHealth()
        {
            if (Player?.Body?.Limbs == null)
            {
                return;
            }
            //if (LimbBar != null)
            //{
            //    OutputConsole.Children.Remove(LimbBar);
            //}

            //LimbBar = new AnimatedConsole("LimbBar", Width - 2, 1, SadConsole.Global.Fonts["CP850"].GetFont(Font.FontSizes.One));
            //the longest string will be the body part with the most number of digits of health and maxhealth + 1
            //int healthBarWidth = OutputConsole.Width - hpStr.Length - 4;

            //string hpStr = $"{Player.Health}/{Player.MaxHealth}";

            var limbs = Player.Body.Limbs;
            //foreach (Limb limb in limbs)
            for(int i = 0; i < limbs.Count; i++)
            {
                Limb limb = limbs[i];
                string hpStr = $"{limb.Health}/{limb.MaxHealth}";
                //int healthBarWidth = OutputConsole.Width - hpStr.Length - 4;
                OutputConsole.Print(OutputConsole.Width - RightTextPadding - 1, i, hpStr, new Color(102, 0, 0), Color.Black);
                var totalWidth = OutputConsole.Width - RightTextPadding - 4;
                OutputConsole.DrawBox(new Microsoft.Xna.Framework.Rectangle(1, i, totalWidth, 1), new Cell(Color.Transparent, BoxColor));
                if (limb.MaxHealth > 0 && limb.Health > 0)
                {
                    int hpWidth = (int)Math.Floor(limb.Health / limb.MaxHealth * totalWidth);
                    OutputConsole.DrawBox(new Microsoft.Xna.Framework.Rectangle(1, i, hpWidth, 1), new Cell(new Color(102, 0, 0), new Color(102, 0, 0)));
                }
            }
        }

        private void DrawHealthBar()
        {
            if (HealthBar != null)
            {
                OutputConsole.Children.Remove(HealthBar);
            }

            string hpStr = $"{Player.Health}/{Player.MaxHealth}";
            int healthBarWidth = OutputConsole.Width - RightTextPadding - 4;
            HealthBar = new AnimatedConsole("HealthBar", healthBarWidth, 1, SadConsole.Global.Fonts["CP850"].GetFont(Font.FontSizes.One));
            List<CellSurface> Frames = new List<CellSurface>();

            for (int i = 0; i < 16; i++)
            {
                Frames.Add(HealthBar.CreateFrame());
            }

            //trying to create a 'pulsing' blood effect on the health bar

            double hpPerCell = Player.MaxHealth / healthBarWidth;
            for (int i = 0; i < healthBarWidth; i++)
            {
                if (Player.Health - .05 >= hpPerCell * i)
                {
                    for (int x = 0; x < Frames.Count; x++)
                    {
                        Color targetColor = new Color(102, 0, 0);
                        if (x >= Frames.Count - 5)
                        {
                            int pulseOffset = Frames.Count - 5 - x;
                            if (pulseOffset == 0 || x == Frames.Count - 1)
                            {
                                targetColor = new Color(92, 0, 0);
                            }
                            else
                            {
                                targetColor = new Color(82, 0, 0);
                            }
                        }

                        Frames[x].SetCellAppearance(i, 0, new Cell(Color.Black, targetColor));
                    }
                }
            }

            HealthBar.AnimationDuration = 1.5f;
            HealthBar.Repeat = true;
            HealthBar.Position = HealthBarPosition;

            OutputConsole.Children.Add(HealthBar);

            OutputConsole.Print(OutputConsole.Width - RightTextPadding - 1, HealthBar.Position.Y, hpStr, Color.White, Color.Black);
            OutputConsole.Print(HealthBar.Position.X, 0, "Health", Color.White, Color.Black);

            OutputConsole.DrawBox(new Microsoft.Xna.Framework.Rectangle(HealthBar.Position.X, HealthBar.Position.Y, HealthBar.Width, HealthBar.Height), new Cell(Color.Transparent, new Color(45, 45, 45)));

            HealthBar.Start();
        }
    }
}
