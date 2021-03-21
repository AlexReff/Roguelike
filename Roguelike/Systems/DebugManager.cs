using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Systems
{
    public enum DebugSource
    {
        Command,
        System,
        Scheduler,
    }

    public struct DebugMessage
    {
        public string Message { get; set; }
        public DebugSource Source { get; set; }

        public DebugMessage(string msg, DebugSource src)
        {
            Message = msg;
            Source = src;
        }
    }

    internal class DebugManager
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

        public Queue<string> Messages { get; private set; }

        private DebugManager()
        {
            Messages = new Queue<string>();
            _subscribers = new Dictionary<string, Action<string>>();
        }

        public void AddMessage(DebugMessage msg)
        {
            var msgStr = msg.Message;

            switch(msg.Source)
            {
                case DebugSource.Command:
                    msgStr = "[c:r f:darkred]" + msgStr;
                    break;
                case DebugSource.System:
                    msgStr = "[c:r f:yellow]" + msgStr;
                    break;
                case DebugSource.Scheduler:
                    msgStr = "[c:r f:green]" + msgStr;
                    break;
                default:
                    break;
            }

            AddMessage(msgStr);
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
