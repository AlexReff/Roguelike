using Roguelike.Systems;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Entities
{
    internal enum ActorState
    {
        Idle,
        Turning,
        Moving,
        Attacking,
        Recovering, //from an attack or action
    }

    internal partial class Actor
    {
        private ActorState _state { get; set; }
        public ActorState State
        {
            get
            {
                return _state;
            }
            set
            {
                //DebugManager.Instance.AddMessage($"{Name} {_state}::{value}");
                _state = value;
            }
        }

        //public void Wait(long ticks = 1)
        //{
        //    MyGame.Karma.Add(ticks, this);
        //}

        //public void WaitHalfSeconds(long halfSeconds = 1)
        //{
        //    Wait(6 * halfSeconds);
        //}

        //public void WaitSeconds(long seconds = 1)
        //{
        //    WaitHalfSeconds(seconds * 2);
        //}
    }
}
