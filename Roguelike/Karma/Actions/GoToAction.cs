using GoRogue;
using GoRogue.Pathing;
using Roguelike.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roguelike.Karma.Actions
{
    internal class GoToAction : KarmaAction
    {
        public GoToAction(NPC actor, double cost) : base("GoToAction", actor, cost)
        {
            Conditions.Add("AtTargetPosition", false);
            Effects.Add("AtTargetPosition", true);
        }

        public override bool IsCompleted()
        {
            return NPC.AtTargetPosition;
        }

        public override bool IsValid()
        {
            return NPC.TargetPosition.HasValue && !IsCompleted();
        }

        public override bool Perform()
        {
            if (!NPC.TargetPosition.HasValue || NPC.TargetPosition.Value == null)
            {
                return false;
            }

            Coord pos = NPC.TargetPosition.Value;
            AStar gps = new AStar(NPC.CurrentMap.WalkabilityView, MyGame.GameSettings.FOVRadiusType);
            Path path = gps.ShortestPath(NPC.Position, pos);
            if (path == null)
            {
                //No path found - wander around instead?
                //DebugManager.Instance.AddMessage($"{NPC.Name} unable to path to {pos}");
                return false;
            }
            else
            {
                var targetPos = path.Steps.FirstOrDefault();

                //if we are next to the target position, just move to it
                if (Distance.EUCLIDEAN.Calculate(NPC.Position, pos) < 2)
                {
                    targetPos = pos;
                }

                Direction targetDir = Direction.GetDirection(NPC.Position, targetPos);
                if (targetDir != null && targetDir != Direction.NONE)
                {
                    return NPC.MoveBump(targetDir);
                }

                return false;
            }
        }
    }
}
