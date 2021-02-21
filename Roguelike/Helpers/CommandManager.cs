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
        public void MovePlayer(Direction direction)
        {
            DebugManager.Instance.AddMessage(new DebugMessage($"Command MovePlayer: {direction}", DebugSource.System));
            //MyGame.World.Player.MoveBy(moveDirection);
            MoveActorBy(MyGame.World.Player, direction);
        }

        public bool MoveActorBy(Actor actor, Direction direction)
        {
            DebugManager.Instance.AddMessage(new DebugMessage($"Command MoveActorBy: {actor.Name}, {direction}", DebugSource.System));
            return actor.MoveBy(direction);
        }

        //public void CenterOnActor(Actor actor)
        //{
        //    //MapScreen.MapRenderer.CenterViewPortOnPoint(actor.Position);
        //}
    }
}
