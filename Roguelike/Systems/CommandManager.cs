using GoRogue;
using Microsoft.Xna.Framework;
using Roguelike.Entities;
using Roguelike.Entities.Items;
using Roguelike.Interfaces;
using Roguelike.Karma.Actions;
using Roguelike.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Roguelike.Systems
{
    class CommandManager
    {
        public bool IsGameOver { get; set; }

        public CommandManager()
        {
            IsGameOver = false;
        }

        public void GameOver()
        {
            IsGameOver = true;
        }

        public void PlayerTurnStarted()
        {
            //
        }

        public void EndPlayerTurn()
        {
            MyGame.Karma.EndPlayerTurn();
        }

        public void KillPlayer()
        {
            MyGame.World.Player.Die();
        }

        public void KillActor(Actor actor)
        {
            actor.Die();
        }

        public void DestroyEquippedWeapon(Hand hand, Weapon weapon)
        {
            hand.ReleaseItem();
            weapon.Destroy();
        }

        public void CenterOnActor(Actor actor)
        {
            //DebugManager.Instance.AddMessage(new DebugMessage($"Command CenterOnActor: {actor.Name}", DebugSource.Command));
            MyGame.World.CurrentMap.CenterOnActor(actor);
        }

        ///// <summary>
        ///// Resolves a move request in a specified direction
        ///// </summary>
        //public bool MovePlayer(Direction direction)
        //{
        //    //DebugManager.Instance.AddMessage(new DebugMessage($"Command MovePlayer: {direction}", DebugSource.Command));
        //    if (MoveActorBy(MyGame.World.Player, direction))
        //    {
        //        EndPlayerTurn();
        //        return true;
        //    }

        //    return false;
        //}

        #region Player Inputs
        
        /// <summary>
        /// Resolves a move request in a specified direction
        /// </summary>
        public void Input_PlayerMoveBump(Direction direction)
        {
            MyGame.World.Player.QueueTurn(direction);
            MyGame.World.Player.QueueBumpAttack(direction);
            MyGame.Karma.AddImmediate(MyGame.World.Player);
            EndPlayerTurn();
        }

        #endregion
    }
}
