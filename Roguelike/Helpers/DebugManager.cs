using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Helpers
{
    class DebugManager
    {
        private static readonly DebugManager instance = new DebugManager();

        // Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
        static DebugManager() { }

        public static DebugManager Instance
        {
            get
            {
                return instance;
            }
        }

        private Dictionary<string, Action<string>> _subscribers { get; set; }

        public List<string> Messages { get; private set; }

        private DebugManager()
        {
            Messages = new List<string>();
            _subscribers = new Dictionary<string, Action<string>>();
        }

        public void AddMessage(string msg)
        {
            Messages.Add(msg);
            NotifySubscribers(msg);
        }

        public void Subscribe(string id, Action<string> callback)
        {
            if (_subscribers.ContainsKey(id))
            {
                return;
            }

            _subscribers.Add(id, callback);
        }

        public void Unsubscribe(string id)
        {
            if (_subscribers.ContainsKey(id))
            {
                _subscribers.Remove(id);
            }
        }

        private void NotifySubscribers(string msg)
        {
            foreach (var cb in _subscribers.Values)
            {
                cb(msg);
            }
        }
    }
}
