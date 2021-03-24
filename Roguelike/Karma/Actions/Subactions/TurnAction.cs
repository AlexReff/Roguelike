using GoRogue;
using Roguelike.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Roguelike.Karma.Actions
{
    internal class TurnAction : MultiStageAction
    {
        private Direction _dir;
        private Queue<Direction> _steps;

        public TurnAction(Actor actor, Direction dir) : base(actor)
        {
            _dir = dir;
            _steps = new Queue<Direction>();

            if (actor.FacingDirection == dir)
            {
                // already facing this direction
                actor.State = ActorState.Idle;
                IsComplete = true;
                return;
            }

            var clockwise = new List<Direction>() { actor.FacingDirection + 1 };
            var counterCw = new List<Direction>() { actor.FacingDirection - 1 };

            List<Direction> turnSteps;

            while (true)
            {
                if (clockwise[clockwise.Count - 1] == dir)
                {
                    turnSteps = clockwise;
                    break;
                }
                else if (counterCw[counterCw.Count - 1] == dir)
                {
                    turnSteps = counterCw;
                    break;
                }
                clockwise.Add(clockwise[clockwise.Count - 1] + 1);
                counterCw.Add(counterCw[counterCw.Count - 1] - 1);
            }

            _steps = new Queue<Direction>(turnSteps);
        }

        public override long GetDelay()
        {
            return 1;
        }

        public override void Perform()
        {
            if (Actor.FacingDirection == _dir || _steps.Count == 0)
            {
                MyGame.Karma.AddAfterLast(1, Actor);
                IsComplete = true;
                return;
            }

            Actor.State = ActorState.Turning;
            var thisStep = _steps.Dequeue();
            Actor.FacingDirection = thisStep;

            if (Actor.FacingDirection == _dir || _steps.Count == 0)
            {
                MyGame.Karma.AddAfterLast(1, Actor);
                IsComplete = true;
                return;
            }
            //Actor.QueuedActions.Enqueue(this);
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
