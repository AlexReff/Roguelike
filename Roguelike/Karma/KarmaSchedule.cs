using Roguelike.Entities;
using Roguelike.Interfaces;
using Roguelike.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roguelike.Karma
{
    internal class KarmaSchedule
    {
        private long _time;
        private readonly SortedDictionary<long, List<Actor>> _scheduleables;

        public long CurrentTime { get { return _time; } }

        public long NextSchedulableTime
        {
            get
            {
                return _scheduleables.Keys.First();
            }
        }

        public KarmaSchedule()
        {
            _time = 0;
            _scheduleables = new SortedDictionary<long, List<Actor>>();
        }

        //// Add a new object to the schedule 
        //// Place it at the current time plus the object's Time property.
        //public void Add(Actor scheduleable)
        //{
        //    //MyBasicEntity baseEntity = scheduleable as MyBasicEntity;
        //    int key = _time + scheduleable.Time;
        //    if (!_scheduleables.ContainsKey(key))
        //    {
        //        _scheduleables.Add(key, new List<Actor>());
        //    }
        //    _scheduleables[key].Add(scheduleable);
        //}

        // Add a new object to the schedule 
        // Place it at the current time plus the object's Time property.
        public void Add(long timeOffset, Actor scheduleable)
        {
            //MyBasicEntity baseEntity = scheduleable as MyBasicEntity;
            long key = _time + timeOffset;
            
            //if (key > long.MaxValue / 5 * 4)
            //{
            //    DebugManager.Instance.AddMessage("Time about to overflow... {key}");
            //}
            //else if (key > long.MaxValue / 2)
            //{
            //    DebugManager.Instance.AddMessage("Time getting absurdly high... {key}");
            //}
            //else if (key > long.MaxValue / 2)
            //{
            //    DebugManager.Instance.AddMessage("Time getting absurdly high... {key}");
            //}
            //else if (key > long.MaxValue / 3)
            //{
            //    DebugManager.Instance.AddMessage("Time getting concerningly high... {key}");
            //}
            //else if (key > long.MaxValue / 4)
            //{
            //    DebugManager.Instance.AddMessage("Time getting high... {key}");
            //}

            if (!_scheduleables.ContainsKey(key))
            {
                _scheduleables.Add(key, new List<Actor>());
            }
            _scheduleables[key].Add(scheduleable);
            //DebugManager.Instance.AddMessage($"Scheduled {scheduleable.Name}#{scheduleable.ID}");
        }

        public long GetLastInstance(Actor scheduleable)
        {
            var keys = _scheduleables.Keys.ToList();
            keys.Reverse();
            long baseOffset = -1;
            foreach (var idx in keys)
            {
                if (_scheduleables[idx] != null && _scheduleables[idx].Contains(scheduleable))
                {
                    baseOffset = idx;
                    break;
                }
            }

            return baseOffset;
        }

        /// <summary>
        /// This schedules an action for timeOffset ticks after the actor's last scheduled action in the system (for sequential actions)
        /// </summary>
        public void AddAfterLast(long timeOffset, Actor scheduleable)
        {
            long baseOffset = GetLastInstance(scheduleable);
            Add(baseOffset + timeOffset, scheduleable);
        }
        
        /// <summary>
        /// Remove all instances of an actor from the schedule
        /// </summary>
        /// <param name="scheduleable"></param>
        public void RemoveAll(Actor scheduleable)
        {
            while (GetLastInstance(scheduleable) != -1)
            {
                Remove(scheduleable);
            }
        }

        // Remove a specific object from the schedule.
        // Useful for when an monster is killed to remove it before it's action comes up again.
        public void Remove(Actor scheduleable)
        {
            KeyValuePair<long, List<Actor>> scheduleableListFound
                = new KeyValuePair<long, List<Actor>>();

            foreach (var scheduleablesList in _scheduleables)
            {
                if (scheduleablesList.Value.Contains(scheduleable))
                {
                    scheduleableListFound = scheduleablesList;
                    break;
                }
            }
            if (scheduleableListFound.Value != null)
            {
                scheduleableListFound.Value.Remove(scheduleable);
                if (scheduleableListFound.Value.Count <= 0)
                {
                    _scheduleables.Remove(scheduleableListFound.Key);
                }
            }
        }

        public void Stop()
        {
            _scheduleables.Clear();
        }

        public bool IsActorScheduled(Actor actor)
        {
            return _scheduleables.Values.Any(m => m.Contains(actor));
        }

        // Get the next object whose turn it is from the schedule. Advance time if necessary
        public Actor Get()
        {
            var firstScheduleableGroup = _scheduleables.First();
            var firstScheduleable = firstScheduleableGroup.Value.First();
            Remove(firstScheduleable);
            _time = firstScheduleableGroup.Key;
            return firstScheduleable;
        }

        // Get the current time (turn) for the schedule
        public long GetTime()
        {
            return _time;
        }

        // Reset the time and clear out the schedule
        public void Clear()
        {
            _time = 0;
            _scheduleables.Clear();
        }
    }
}
