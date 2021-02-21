using System;
using System.Collections.Generic;
using System.Linq;
using GoRogue;
using GoRogue.MapGeneration;
using GoRogue.MapViews;
using Microsoft.Xna.Framework;
using Roguelike.Entities;
using SadConsole;
using SadConsole.Entities;

namespace Roguelike
{
    internal enum MapLayer
    {
        TERRAIN,
        ITEMS,
        MONSTERS,
        PLAYER
    }

    internal class GameMap : BasicMap
    {
        // Handles the changing of tile/entity visiblity as appropriate based on Map.FOV.
        public FOVVisibilityHandler FovVisibilityHandler { get; }

        // Since we'll want to access the player as our Player type, create a property to do the cast for us.  The cast must succeed thanks to the ControlledGameObjectTypeCheck
        // implemented in the constructor.
        public new Player ControlledGameObject
        {
            get => (Player)base.ControlledGameObject;
            set => base.ControlledGameObject = value;
        }

        public GameMap(int width, int height)
            // Allow multiple items on the same location only on the items layer.  This example uses 8-way movement, so Chebyshev distance is selected.
            : base(width, height, Enum.GetNames(typeof(MapLayer)).Length - 1, Distance.CHEBYSHEV, entityLayersSupportingMultipleItems: LayerMasker.DEFAULT.Mask(new[] { (int)MapLayer.ITEMS, (int)MapLayer.MONSTERS }))
        {
            ControlledGameObjectChanged += ControlledGameObjectTypeCheck<Player>; // Make sure we don't accidentally assign anything that isn't a Player type to ControlledGameObject
            FovVisibilityHandler = new DefaultFOVVisibilityHandler(this, ColorAnsi.BlackBright);
        }

        // IsTileWalkable checks
        // to see if the actor has tried
        // to walk off the map or into a non-walkable tile
        // Returns true if the tile location is walkable
        // false if tile location is not walkable or is off-map
        public bool IsTileWalkable(Point point)
        {
            if (point.X < 0 || point.Y < 0 || point.X >= Width || point.Y >= Height)
                return false;

            return WalkabilityView[point.Y * Width + point.X];
        }

        // Checking whether a certain type of
        // entity is at a specified location the manager's list of entities
        // and if it exists, return that Entity
        public T GetEntityAt<T>(Point location) where T : Entity
        {
            return GetEntitiesAt<T>(location).FirstOrDefault();
        }

        public IEnumerable<T> GetEntitiesAt<T>(Point location) where T : Entity
        {
            return Entities.GetItems(location).OfType<T>();
        }
    }
}
