using GoRogue;
using GoRogue.Pathing;
using Roguelike.Entities;
using Roguelike.Interfaces;
using Roguelike.Systems;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Roguelike.Behaviors
{
    internal class StandardMoveAndAttack
    {
        public bool Act(NPC actor)
        {
            Player player = MyGame.World.Player;
            GameMap map = MyGame.World.CurrentMap;

            FOV fov = new FOV(map.TransparencyView);

            //see if there is a player or other enemy in FOV
            if (actor.CurrentTarget != null)
            {
                var deg = Helpers.GetFOVDegree(actor);
                fov.Calculate(actor.Position, actor.Awareness, MyGame.GameSettings.FOVRadiusType, deg, actor.FOVViewAngle);
                if (fov.BooleanFOV[player.Position.X, player.Position.Y])
                {
                    //Delay showing this until the player can see the monster
                    //fov.Calculate(player.Position.X, player.Position.Y, player.Awareness, MyGame.GameSettings.FOVRadiusType, deg, player.FOVViewAngle, player.InnerFOVAwareness);
                    //if (fov.BooleanFOV[actor.Position.X, actor.Position.Y])
                    //{
                    //    PlayerMessageManager.Instance.AddMessage(new PlayerMessage($"{actor.Name} turns towards {player.Name}", MessageCategory.Combat));
                    //}

                    //actor.IsAlertedToPlayer = true;
                    //actor.TurnsAlerted = 1;
                }
            }

            //if (actor.TurnsAlerted.HasValue)
            //{
            //    if (actor.HasVision)
            //    {
            //        AStar gps = new AStar(map.WalkabilityView, MyGame.GameSettings.FOVRadiusType);
            //        Path path = gps.ShortestPath(actor.Position, player.Position);
            //        if (path == null)
            //        {
            //            //No path found - wander around instead?
            //            DebugManager.Instance.AddMessage($"{actor.Name} unable to path to {player.Name}");
            //        }
            //        else
            //        {
            //            var targetPos = path.Steps.FirstOrDefault();
            //            if (Distance.EUCLIDEAN.Calculate(actor.Position, player.Position) < 2)
            //            {
            //                targetPos = player.Position;
            //            }

            //            Direction targetDir = Direction.GetDirection(actor.Position, targetPos);
            //            if (targetDir != null && targetDir != Direction.NONE)
            //            {
            //                actor.MoveBump(targetDir);
            //            }
            //            else
            //            {
            //                DebugManager.Instance.AddMessage($"Unable to parse Direction from {actor.Position} to {path.Start} ({targetDir})");
            //            }
            //        }
            //    }

            //    actor.TurnsAlerted++;

            //    if (actor.TurnsAlerted > 15)
            //    {
            //        actor.TurnsAlerted = null;
            //    }
            //}

            return true;
        }
    }
}
