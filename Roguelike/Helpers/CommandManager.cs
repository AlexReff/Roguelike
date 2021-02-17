using GoRogue;
using Microsoft.Xna.Framework;
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

        //public bool MoveActorBy(Actor actor, Point position)
        //{
        //    //DebugManager.Instance.AddMessage(new DebugMessage("User Attempted Move: " + moveDirection.ToString(), DebugSource.User));
        //    //MapScreen.Map.ControlledGameObject.Position += moveDirection;
        //}

        public void MovePlayerDirection(Player player, Direction moveDirection)
        {
            DebugManager.Instance.AddMessage(new DebugMessage("User Attempted Move: " + moveDirection.ToString(), DebugSource.User));
            player.Position += moveDirection;
        }
    }
}
