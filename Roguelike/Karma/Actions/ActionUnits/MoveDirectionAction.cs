using GoRogue;
using Roguelike.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Karma.Actions
{
    internal class MoveDirectionAction : ActionUnit
    {
        private bool _started;
        private Coord _pos;
        private Direction _dir;

        public MoveDirectionAction(Actor actor, Direction dir) : base(actor)
        {
            _started = false;
            IsComplete = false;
            _dir = dir;
            _pos = actor.Position + dir;
        }

        public override long GetDelay()
        {
            return Actor.KarmaMoveSpeed;
        }

        public override void Perform()
        {
            if (!_started)
            {
                // begin 'walking'
                Actor.State = ActorState.Moving;
                _started = true;
            }
            else
            {
                if (Actor.CanMove(_dir))
                {
                    Actor.DoMove(_dir);
                }

                IsComplete = true;
            }
        }
    }
}



//using GoRogue;
//using GoRogue.Pathing;
//using Roguelike.Entities;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace Roguelike.Karma.Actions
//{
//    internal class MoveDirectionAction : KarmaAction
//    {
//        private bool _started;

//        public MoveDirectionAction(NPC actor, double cost) : base("MoveDirection", actor, cost)
//        {
//            //Conditions.Add("AtTargetPosition", false);
//            //Effects.Add("AtTargetPosition", true);

//            _started = false;
//        }

//        public override Coord? GetTargetPosition()
//        {
//            return null;
//        }

//        public override bool IsCompleted()
//        {
//            return NPC.AtTargetPosition;
//        }

//        public override bool IsValid()
//        {
//            return NPC.TargetPosition.HasValue && !IsCompleted();
//        }

//        public override bool Perform()
//        {
//            if (!NPC.TargetPosition.HasValue || NPC.TargetPosition.Value == null)
//            {
//                return false;
//            }

//            Coord pos = NPC.TargetPosition.Value;
//            AStar gps = new AStar(NPC.CurrentMap.WalkabilityView, MyGame.GameSettings.FOVRadiusType);
//            Path path = gps.ShortestPath(NPC.Position, pos);
//            if (path == null)
//            {
//                //No path found - wander around instead?
//                //DebugManager.Instance.AddMessage($"{NPC.Name} unable to path to {pos}");
//                return false;
//            }
//            else
//            {
//                var targetPos = path.Steps.FirstOrDefault();

//                //if we are next to the target position, just move to it
//                if (Distance.EUCLIDEAN.Calculate(NPC.Position, pos) < 2)
//                {
//                    targetPos = pos;
//                }

//                Direction targetDir = Direction.GetDirection(NPC.Position, targetPos);
//                if (targetDir != null && targetDir != Direction.NONE)
//                {
//                    return NPC.MoveBump(targetDir);
//                }

//                return false;
//            }
//        }
//    }
//}
