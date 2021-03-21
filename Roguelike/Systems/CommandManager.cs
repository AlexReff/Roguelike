using GoRogue;
using Microsoft.Xna.Framework;
using Roguelike.Entities;
using Roguelike.Entities.Items;
using Roguelike.Interfaces;
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
        public bool IsPlayerTurn { get; set; }
        public bool IsGameOver { get; set; }

        private Stopwatch Timer;
        private double ms;

        public CommandManager()
        {
            IsPlayerTurn = true;
            IsGameOver = false;

            Timer = new Stopwatch();
            ms = 1000 / MyGame.GameSettings.FPSLimit;
        }

        //public void ScheduleTime()
        //{
        //    if (!Timer.IsRunning)
        //    {
        //        Timer.Start();
        //        DoTime();
        //    }
        //    else if (Timer.ElapsedMilliseconds >= ms)
        //    {
        //        Timer.Restart();
        //        DoTime();
        //    }
        //    else
        //    {
        //        var future = ms - Timer.ElapsedMilliseconds;
        //        Task.Delay(TimeSpan.FromMilliseconds(future)).ContinueWith((e) =>
        //        {
        //            ScheduleTime();
        //        });
        //    }
        //}

        public void DoTime()
        {
            //IScheduleable scheduleable = MyGame.Scheduler.Get();
            //if (scheduleable is Player)
            //{
            //    IsPlayerTurn = true;
            //    //?
            //    MyGame.Scheduler.Add(scheduleable);
            //}
            //else
            //{
            //    if (scheduleable != null)
            //    {
            //        (scheduleable as INonPlayerSchedulable).PerformAction();
            //        MyGame.Scheduler.Add(scheduleable);
            //    }

            //    //ScheduleTime();


            //    //DoTime();
            //    //Task.Delay(TimeSpan.FromMilliseconds(ms)).Wait();//.ContinueWith(DoTime);
            //    DoTime();
            //}
        }

        public void GameOver()
        {
            //MyGame.Scheduler.Stop();
            //MyGame.Scheduler.Add(MyGame.World.Player);
            IsGameOver = true;
        }

        public void EndPlayerTurn()
        {
            IsPlayerTurn = false;
        }

        public void KillPlayer()
        {
            MyGame.World.Player.Die();
            IsPlayerTurn = true;
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
            return MoveActorBy(MyGame.World.Player, direction);
        }

        /// <summary>
        /// Resolves a move request in a specified direction
        /// </summary>
        public bool MoveActorBy(Actor actor, Direction direction)
        {
            //DebugManager.Instance.AddMessage(new DebugMessage($"Command MoveActorBy: {actor.Name}, {direction}", DebugSource.Command));
            return actor.MoveBump(direction);
        }

        public void CenterOnActor(Actor actor)
        {
            //DebugManager.Instance.AddMessage(new DebugMessage($"Command CenterOnActor: {actor.Name}", DebugSource.Command));
            MyGame.World.CurrentMap.CenterOnActor(actor);
        }
    }
}
