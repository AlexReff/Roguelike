using GoRogue;
using Microsoft.Xna.Framework;
using Roguelike.Entities;
using Roguelike.Systems;
using SadConsole;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Consoles
{
    internal class EffectsScreenConsole : SadConsole.Console
    {
        private double PlayerMaxHealth;
        private double PlayerHealth;

        public EffectsScreenConsole(int width, int height) : base(width, height)
        {

            SubscribeToEvents();

            //AnimatedConsole animation = new AnimatedConsole("one", 1, 1);
            //animation.CreateFrame().SetGlyph(0, 0, '1');
            //animation.CreateFrame().SetGlyph(0, 0, '2');
            //animation.AnimationDuration = 0.5f;
            //animation.Repeat = true;
            //animation.Start();
            //var entity = new SadConsole.Entities.Entity(animation);
            //entity.Position = new Microsoft.Xna.Framework.Point(-1, -1);
            //Children.Add(entity);

            //health: 219
            //half height: 220
        }

        private void SubscribeToEvents()
        {
            EventManager.Instance.ActorDiedEvent += Instance_ActorDiedEvent;
            EventManager.Instance.ActorHealthChangedEvent += Instance_ActorHealthChangedEvent;
            EventManager.Instance.ActorMovedEvent += Instance_ActorMovedEvent;
            EventManager.Instance.PlayerSpawnedEvent += Instance_PlayerSpawnedEvent;
        }

        private void Redraw()
        {
            //draw all animations/overlays
        }

        private void Instance_PlayerSpawnedEvent(Player obj)
        {
            PlayerHealth = obj.Health;
            PlayerMaxHealth = obj.MaxHealth;

            Redraw();
        }

        private void Instance_ActorMovedEvent(Actor actor, Coord oldPos, Coord newPos)
        {
            Redraw();
        }

        private void Instance_ActorHealthChangedEvent(Actor actor, double prevHealth)
        {
            bool dirty = false;

            if (actor is Player)
            {
                //Update the health display
                PlayerHealth = actor.Health;
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
                dirty = true;
            }

            if (dirty)
            {
                Redraw();
            }
        }
    }
}
