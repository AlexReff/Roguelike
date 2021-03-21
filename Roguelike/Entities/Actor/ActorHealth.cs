using Roguelike.Systems;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Entities
{
    internal partial class Actor
    {
        public bool IsDead { get; set; }
        public double MaxHealth { get; set; }

        private double _health;
        public double Health
        {
            get { return _health; }
            set
            {
                var prevValue = _health;
                _health = Math.Round(Math.Max(0, value), 2);
                EventManager.Instance.InvokeActorHealthChanged(this, prevValue);
                //if (this is Player)
                //{
                //    var diff = Math.Round(value - prevValue, 2);
                //    if (diff < 0)
                //    {
                //        PlayerMessageManager.Instance.AddMessage(new PlayerMessage($"[c:r f:red]Lost[c:u] {Math.Abs(diff)} health!", MessageCategory.Health));
                //    }
                //    else if (diff > 0)
                //    {
                //        PlayerMessageManager.Instance.AddMessage(new PlayerMessage($"[c:r f:green]Gained[c:u] {diff} health.", MessageCategory.Health));
                //    }
                //}
                if (_health <= 0)
                    Die();
            }
        }
    }
}
