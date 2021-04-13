using GoRogue;
using GoRogue.GameFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Entities
{
    internal partial class Actor
    {
        private FOV _fov;
        public HashSet<Actor> VisibleActors { get; }
        public HashSet<Actor> VisibleEnemies { get; }

        /// <summary>
        /// Call this after an entity is added to a map
        /// </summary>
        public void AddedToMap()
        {
            _fov = new FOV(CurrentMap.TransparencyView);
            VisibleActors.Clear();
            CalculateFOV();

            CurrentMap.ObjectAdded += Map_ObjectAdded;
            CurrentMap.ObjectMoved += Map_ObjectMoved;
            CurrentMap.ObjectRemoved += Map_ObjectRemoved;
        }

        public void CalculateFOV()
        {
            var deg = Helpers.GetFOVDegree(this);
            _fov.Calculate(Position, Awareness, MyGame.GameSettings.FOVRadiusType, deg, FOVViewAngle);
            foreach (var pos in _fov.NewlyUnseen)
            {
                var actor = CurrentMap.GetEntity<Actor>(pos);
                if (actor != null)
                {
                    VisibleActors.Remove(actor);
                }
            }
            foreach (var pos in _fov.NewlySeen)
            {
                var actor = CurrentMap.GetEntity<Actor>(pos);
                if (actor != null)
                {
                    VisibleActors.Add(actor);
                }
            }
            CheckHostiles();
        }

        public void CheckHostiles()
        {
            VisibleEnemies.Clear();

            if (UnderAttack)
            {
                SensesHostiles = true;
            }

            foreach (var actor in VisibleActors)
            {
                if (IsHostileTo(actor))
                {
                    SensesHostiles = true;
                    VisibleEnemies.Add(actor);
                }
            }

            if (VisibleEnemies.Count == 0 && !UnderAttack)
            {
                SensesHostiles = false;
            }
        }

        private void Map_ObjectAdded(object sender, ItemEventArgs<IGameObject> e)
        {
            if (e.Item.Layer != (int)MapLayer.TERRAIN
                 && e.Item is Actor
                 && _fov.BooleanFOV[e.Position])
            {
                VisibleActors.Add(e.Item as Actor);
            }
        }

        private void Map_ObjectRemoved(object sender, ItemEventArgs<IGameObject> e)
        {
            if (e.Item is Actor)
            {
                VisibleActors.Remove(e.Item as Actor);
            }
        }

        private void Map_ObjectMoved(object sender, ItemMovedEventArgs<IGameObject> e)
        {
            if (e.Item is Actor)
            {
                if (_fov.BooleanFOV[e.NewPosition])
                {
                    VisibleActors.Add(e.Item as Actor);
                }
                else
                {
                    VisibleActors.Remove(e.Item as Actor);
                }
            }
        }
    }
}
