using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Helpers
{
    enum MessageCategory
    {
        Movement,
        Combat,
        Status,
    }

    class PlayerMessage
    {
        public string Message { get; set; }
        public MessageCategory Category { get; set; }

        public PlayerMessage(string msg, MessageCategory cat)
        {
            Message = msg;
            Category = cat;
        }
    }

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

        public void AddMessage(PlayerMessage msg)
        {
            //TODO: Add any coloring/formatting per msg category
            AddMessage(msg.Message);
        }

        public void AddMessage(string msg)
        {
            msg = msg.Replace("\r", "").Replace("\n", "").Replace(Environment.NewLine, "");
            Messages.Enqueue(msg);
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
