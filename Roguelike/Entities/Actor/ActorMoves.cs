using GoRogue;
using GoRogue.Pathing;
using Roguelike.Entities.Items;
using Roguelike.Karma.Actions;
using Roguelike.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roguelike.Entities
{
    internal enum ActorMoveRate
    {
        Crouching,
        Walking,
        Jogging,
        Sprinting,
    }

    internal partial class Actor
    {
        public ActorMoveRate MoveRate { get; set; }

        private void Actor_Moved(object sender, ItemMovedEventArgs<GoRogue.GameFramework.IGameObject> e)
        {
            CalculateFOV();
        }

        public Direction GetNextStepInPath(Coord pathCenter)
        {
            Coord? targetPos = null;

            if (Distance.EUCLIDEAN.Calculate(Position, pathCenter) < 1.5)
            {
                targetPos = pathCenter;
            }
            else
            {
                AStar gps = new AStar(CurrentMap.WalkabilityView, MyGame.GameSettings.FOVRadiusType);
                Path path = gps.ShortestPath(Position, pathCenter);
                if (path != null)
                {
                    targetPos = path.Steps.FirstOrDefault();
                }
            }

            if (targetPos.HasValue)
            {
                Direction targetDir = Direction.GetDirection(Position, targetPos.Value);

                if (targetDir != null && targetDir != Direction.NONE)
                {
                    return targetDir;
                }
            }

            return null;
        }

        ///// <summary>
        ///// Will calculate a path to the target position, and attempt to move one unit in the correct direction for the path
        ///// </summary>
        ///// <param name="pos"></param>
        ///// <returns></returns>
        //public bool NavigateToStep(Coord pos)
        //{
        //    Direction targetDir = GetNextStepInPath(pos);
        //    if (targetDir != null)
        //    {

        //    }
        //    //AStar gps = new AStar(CurrentMap.WalkabilityView, MyGame.GameSettings.FOVRadiusType);
        //    //Path path = gps.ShortestPath(Position, pos);
        //    //if (path == null)
        //    //{
        //    //    //No path found - wander around instead?
        //    //    //DebugManager.Instance.AddMessage($"{actor.Name} unable to path to {player.Name}");
        //    //    return false;
        //    //}
        //    //else
        //    //{
        //    //    var targetPos = path.Steps.FirstOrDefault();
        //    //    if (Distance.EUCLIDEAN.Calculate(Position, pos) < 2)
        //    //    {
        //    //        targetPos = pos;
        //    //    }

        //    //    Direction targetDir = Direction.GetDirection(Position, targetPos);
        //    //    if (targetDir != null && targetDir != Direction.NONE)
        //    //    {
        //    //        return MoveBump(targetDir);
        //    //    }
        //    //    else
        //    //    {
        //    //        //DebugManager.Instance.AddMessage($"Unable to parse Direction from {actor.Position} to {path.Start} ({targetDir})");
        //    //        return false;
        //    //    }
        //    //}
        //}


        //private int GetTurnDirectionTickDelay(Direction start, Direction end)
        //{
        //    //1 tick per 45 degrees
        //    var diff = Math.Abs((int)end.Type - (int)start.Type);
        //    return diff;
        //}

        /////// <summary>
        /////// Moves the actor in the given direction. No validity checks
        /////// </summary>
        //public void StartMove(Direction direction)
        //{
        //    // QueuedActions.Enqueue(new ResolveMoveAction(this, direction));
        //    State = ActorState.Moving;
        //    ActionQueue.Enqueue(new MoveDirectionAction(this, direction));
        //    //MyGame.Karma.AddAfterLast(KarmaMoveSpeed, this);
        //}

        //public void StartMove(Direction direction)
        //{
        //    //if (RequestTurnToDirection(direction))
        //    //{
        //    //    // we are facing this direction already!
        //    //    Position += direction;
        //    //    State = ActorState.Moving;
        //    //    MyGame.Karma.Add(this.KarmaMoveSpeed, this);
        //    //}
        //    //else
        //    //{
        //    //    // we now have a queue of actions set up to turn to this direction
        //    //    // add a move step to the end of it
        //    //    QueuedActions.Enqueue(new ResolveMoveAction(this, direction));
        //    //    MyGame.Karma.AddAfterLast(KarmaMoveSpeed, this);
        //    //}

        //    //TurnToDirection(direction);
        //    State = ActorState.Moving;
        //    QueuedActions.Enqueue(new ResolveMoveAction(this, direction));
        //    MyGame.Karma.AddAfterLast(KarmaMoveSpeed, this);
        //}

        public bool CanMove(Direction direction)
        {
            var pos = Position + direction;
            if (CurrentMap.WalkabilityView[pos])
            {
                var actor = CurrentMap.GetEntityAt<Actor>(pos);
                if (actor == null || !IsHostileTo(actor))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Moves the actor in the direction. No validity checking.
        /// </summary>
        public void DoMove(Direction direction)
        {
            Position += direction;
            //State = ActorState.Idle;
            CalculateFOV();
        }

        ///// <summary>
        ///// Will move in a direction, or attack a hostile if one is on the given location. Will also turn if needed.
        ///// Must also schedule the actor
        ///// </summary>
        ///// <returns>True if an action occurs</returns>
        //public bool MoveBump(Direction direction)
        //{
        //    Coord target = Position + direction;
        //    //FacingDirection = direction;

        //    Actor otherActor = MyGame.World.CurrentMap.GetEntityAt<Actor>(target);

        //    if (otherActor != null)
        //    {
        //        // if an enemy is in the way, kill it!
        //        // could be refactored to move around or through the enemy if there is a 'higher priority' move or attack order (in future versions)
        //        if (this.IsHostileTo(otherActor))
        //        {
        //            BumpAttack(otherActor);
        //            return true;
        //        }
        //    }

        //    if (MyGame.World.CurrentMap.IsTileWalkable(target))
        //    {
        //        Item item = MyGame.World.CurrentMap.GetEntityAt<Item>(target);
        //        if (item != null)
        //        {
        //            //an item is in the tile we are about to walk to

        //            //potentially...
        //            //monsters and npc's might equip/use/throw items as needed

        //            if (this is Player)
        //            {
        //                if (item is Currency && MyGame.GameSettings.GoldAutoPickup)
        //                {
        //                    PickupItem(item);
        //                }
        //                else
        //                {
        //                    PlayerMessageManager.Instance.AddMessage(new PlayerMessage($"Encountered a {item.Name}", MessageCategory.Notification));
        //                }
        //            }
        //        }

        //        MoveDirection(direction);
        //        return true;
        //    }
        //    else
        //    {
        //        //DebugManager.Instance.AddMessage(new DebugMessage($"Actor {Glyph}, Unable to walk to non-walkable: {target}", DebugSource.System));
        //        //ImmediateFeedbackManager.Instance.AddMessage($"Cannot walk {direction.ToString().Replace("_", "").ToProperCase()}");
        //        //feedbackMsg = $"Cannot walk {direction.ToString().Replace("_", "").ToProperCase()}";
        //        //return false;

        //        //// Check for the presence of a door
        //        //TileDoor door = GameLoop.World.CurrentMap.GetTileAt<TileDoor>(Position + positionChange);
        //        //// if there's a door here,
        //        //// try to use it
        //        //if (door != null)
        //        //{
        //        //    GameLoop.CommandManager.UseDoor(this, door);
        //        //    return true;
        //        //}
        //    }

        //    //ImmediateFeedbackManager.Instance.AddMessage(feedbackMsg);
        //    //MyGame.UIManager.GameScreen.MapScreen.UpdateFOV();
        //    MyGame.Karma.Add(this);
        //    return false;
        //}
    }
}
