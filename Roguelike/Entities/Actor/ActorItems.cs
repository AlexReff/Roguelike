﻿using Roguelike.Entities.Items;
using Roguelike.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roguelike.Entities
{
    internal partial class Actor
    {
        public List<Item> Inventory { get; set; }
        public double MaxCarryWeight { get; set; }
        public double CurrentCarryWeight { get; set; }

        private int _currency;
        public int Currency
        {
            get { return _currency; }
            set
            {
                var prevValue = _currency;
                _currency = Math.Max(0, value);
                var difference = _currency - prevValue;
                if (this is Player && this.Health > 0)
                {
                    if (difference < 0)
                    {
                        PlayerMessageManager.Instance.AddMessage(new PlayerMessage($"Lost {Math.Abs(difference)} gold", MessageCategory.Notification));
                    }
                    else if (difference > 0)
                    {
                        PlayerMessageManager.Instance.AddMessage(new PlayerMessage($"Acquired {difference} gold", MessageCategory.Notification));
                    }
                }
            }
        }

        public void DropAllItems()
        {
            foreach (var item in Inventory)
            {
                DropItem(item);
            }
        }

        public void DropItem(long id)
        {
            var item = Inventory.FirstOrDefault((inv) => inv.ID == id);
            if (item != null && item.ID == id)
            {
                DropItem(item);
            }
        }

        public void DropItem(Item item)
        {
            if (!item.IsDroppable)
            {
                DestroyItem(item);
                return;
            }

            item.Position = Position;
            this.CurrentMap.AddEntity(item);
            Inventory.Remove(item);
            CalculateCurrentCarryWeight();
        }

        public void DestroyItem(Item item)
        {
            Inventory.Remove(item);
            item.Destroy();
            CalculateCurrentCarryWeight();
        }

        public void CalculateCurrentCarryWeight()
        {
            this.CurrentCarryWeight = this.Inventory.Sum(m => m.Weight);
        }

        public void AddItem(Item item)
        {
            if (item is Currency)
            {
                AddCurrency((item as Currency).Amount);
            }
            else
            {
                this.Inventory.Add(item);
                CalculateCurrentCarryWeight();
            }
        }

        public void PickupItem(Item item)
        {
            if (item.CurrentMap != null)
            {
                item.CurrentMap.RemoveEntity(item);
            }
            AddItem(item);
        }

        public void AddCurrency(int amount)
        {
            if (amount > 0)
            {
                Currency += amount;
            }
        }

        public void DropCurrency(int amount)
        {
            if (amount <= 0)
            {
                return;
            }
            if (Currency >= amount)
            {
                Currency -= amount;
                this.CurrentMap.AddEntity(new Currency(amount, Position));
            }
            else if (Currency > 0)
            {
                this.CurrentMap.AddEntity(new Currency(Currency, Position));
                Currency = 0;
            }
        }
    }
}
