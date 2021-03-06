using Microsoft.Xna.Framework;
using Roguelike.Entities;
using Roguelike.Entities.Items;
using Roguelike.Helpers;
using SadConsole;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Roguelike.Maps
{
    class PlayerFOVVisibilityHandler : FOVVisibilityHandler
    {
        public Color ExploredColor { get; set; }
        private HashSet<uint> VisibleEntities { get; set; }

        public PlayerFOVVisibilityHandler(BasicMap map, Color exploredColor) : base(map)
        {
            VisibleEntities = new HashSet<uint>();
            ExploredColor = exploredColor;
        }

        protected override void UpdateEntitySeen(BasicEntity entity)
        {
            if (!(entity is Player))
            {
                if (!VisibleEntities.Contains(entity.ID))
                {
                    VisibleEntities.Add(entity.ID);
                    DebugManager.Instance.AddMessage($"Player::EntitySeen: {entity.Name}");
                    if (entity is Currency)
                    {
                        //suppress messages about seeing currency on the ground
                    }
                    else
                    {
                        var n = Helpers.Helpers.IsVowel(entity.Name[0]) ? "n" : "";
                        PlayerMessageManager.Instance.AddMessage($"Spotted a{n} {entity.Name}");
                    }
                }
                
            }
            entity.IsVisible = true;
        }

        protected override void UpdateEntityUnseen(BasicEntity entity)
        {
            DebugManager.Instance.AddMessage($"Player::EntityUnseen: {entity.Name}");
            VisibleEntities.Remove(entity.ID);
            entity.IsVisible = false;
        }

        protected override void UpdateTerrainSeen(BasicTerrain terrain)
        {
            //DebugManager.Instance.AddMessage($"Player::EntitySeen: {entity.Name}");
            terrain.IsVisible = true;
            terrain.RestoreState();
        }

        protected override void UpdateTerrainUnseen(BasicTerrain terrain)
        {
            //DebugManager.Instance.AddMessage($"Player::EntitySeen: {entity.Name}");
            if (Map.Explored[terrain.Position])
            {
                terrain.SaveState();
                terrain.Foreground = ExploredColor;
            }
            else
            {
                terrain.IsVisible = false;
            }
        }
    }
}
