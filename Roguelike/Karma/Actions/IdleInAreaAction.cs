using GoRogue;
using GoRogue.GameFramework;
using Roguelike.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roguelike.Karma.Actions
{
    /// <summary>
    /// Chill around
    /// </summary>
    internal class IdleInAreaAction : KarmaAction
    {
        public IdleInAreaAction(NPC actor, double cost) : base("IdleInArea", actor, cost)
        {
            Effects.Add("IdleInArea", true);
        }

        public override Coord? GetTargetPosition()
        {
            return Actor.SpawnPoint;
        }

        public override bool IsCompleted()
        {
            return true;
        }

        public override bool IsValid()
        {
            return true; //NPC.TargetPosition != null;
        }

        public override bool Perform()
        {
            //every so often, take a random step around an area
            if (Helpers.RandomGenerator.NextDouble() >= .75)
            {
                var validNeighbors = new List<Coord>();
                var allSpots = Actor.Position.GetNeighbors();
                double range = GetRange();
                foreach (Coord spot in allSpots)
                {
                    if (Actor.CurrentMap.WalkabilityView[spot] && Distance.EUCLIDEAN.Calculate(Actor.SpawnPoint, spot) <= range)
                    {
                        validNeighbors.Add(spot);
                    }
                }

                while (validNeighbors.Count > 0)
                {
                    int randIdx = (int)Math.Floor(Helpers.RandomGenerator.NextDouble() * validNeighbors.Count);
                    Direction targetDir = Direction.GetDirection(Actor.Position, validNeighbors[randIdx]);

                    if (targetDir != null && targetDir != Direction.NONE)
                    {
                        return Actor.CommandMove(targetDir);
                    }

                    validNeighbors.RemoveAt(randIdx);
                }
            }

            MyGame.Karma.Add(Actor);
            return false;
        }

        public override double GetRange()
        {
            return 6;
        }
    }
}
