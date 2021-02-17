using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Helpers
{
    class PlayerMessageManager
    {
        private static readonly PlayerMessageManager instance = new PlayerMessageManager();

        // Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
        static PlayerMessageManager() { }

        public static PlayerMessageManager Instance
        {
            get
            {
                return instance;
            }
        }

        private Dictionary<string, Action<string>> _subscribers { get; set; }

        public Queue<string> Messages { get; private set; }

        private PlayerMessageManager()
        {
            Messages = new Queue<string>();
            _subscribers = new Dictionary<string, Action<string>>();
        }

        public void AddMessage(string msg)
        {
            msg = msg.Replace("\r", "").Replace("\n", "").Replace(Environment.NewLine, "");
            Messages.Enqueue(msg);
            NotifySubscribers(msg);
            DebugManager.Instance.AddMessage(new DebugMessage(msg, DebugSource.Player));
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
