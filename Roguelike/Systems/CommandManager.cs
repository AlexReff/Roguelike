﻿using GoRogue;
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
        //public bool IsPlayerTurn { get; set; }
        public bool IsGameOver { get; set; }

        //private Stopwatch Timer;
        //private double ms;

        public CommandManager()
        {
            //IsPlayerTurn = true;
            IsGameOver = false;

            //Timer = new Stopwatch();
            //ms = 1000 / MyGame.GameSettings.FPSLimit;
        }

        public void GameOver()
        {
            //MyGame.Scheduler.Stop();
            //MyGame.Scheduler.Add(MyGame.World.Player);
            IsGameOver = true;
        }

        public void EndPlayerTurn()
        {
            //IsPlayerTurn = false;
            //MyGame.Karma.EndPlayerTurn();
            var player = MyGame.World.Player;
            //player.QueuedActions.Enqueue(new ResolveEndPlayerTurnAction(player));
            MyGame.Karma.AddAfterLast(0, player);
            //MyGame.Karma.DoTime();
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

        /// <summary>
        /// Resolves a move request in a specified direction
        /// </summary>
        public bool MovePlayer(Direction direction)
        {
            //DebugManager.Instance.AddMessage(new DebugMessage($"Command MovePlayer: {direction}", DebugSource.Command));
            if (MoveActorBy(MyGame.World.Player, direction))
            {
                EndPlayerTurn();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Resolves a move request in a specified direction
        /// </summary>
        public bool MoveActorBy(Actor actor, Direction direction)
        {
            //DebugManager.Instance.AddMessage(new DebugMessage($"Command MoveActorBy: {actor.Name}, {direction}", DebugSource.Command));
            //var result = actor.MoveBump(direction);
            var result = actor.CommandMove(direction);

            return result;
        }

        public void CenterOnActor(Actor actor)
        {
            //DebugManager.Instance.AddMessage(new DebugMessage($"Command CenterOnActor: {actor.Name}", DebugSource.Command));
            MyGame.World.CurrentMap.CenterOnActor(actor);
        }
    }
}