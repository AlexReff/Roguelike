using GoRogue;
using Microsoft.Xna.Framework;
using Roguelike.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Helpers
{
    class CommandManager
    {
        public CommandManager()
        {
            //
        }

        /// <summary>
        /// Resolves a move request in a specified direction
        /// </summary>
        /// <param name="direction"></param>
        public bool MovePlayer(Direction direction)
        {
            DebugManager.Instance.AddMessage(new DebugMessage($"Command MovePlayer: {direction}", DebugSource.System));
            return MoveActorBy(MyGame.World.Player, direction);
        }

        public bool MoveActorBy(Actor actor, Direction direction)
        {
            DebugManager.Instance.AddMessage(new DebugMessage($"Command MoveActorBy: {actor.Name}, {direction}", DebugSource.System));
            return actor.MoveBy(direction);
        }

        public void CenterOnActor(Actor actor)
        {
            DebugManager.Instance.AddMessage(new DebugMessage($"Command CenterOnActor: {actor.Name}", DebugSource.System));
            MyGame.World.CurrentMap.CenterOnActor(actor);
        }
    }
}
