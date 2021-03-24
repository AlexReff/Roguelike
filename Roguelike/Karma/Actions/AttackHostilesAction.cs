﻿using GoRogue;
using Roguelike.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roguelike.Karma.Actions
{
    internal class AttackHostilesAction : KarmaAction
    {
        public AttackHostilesAction(NPC npc, double cost) : base("AttackHostiles", npc, cost)
        {
            Conditions.Add("SensesHostiles", true);
            Effects.Add("AttackHostiles", true);
        }

        public override Coord? GetTargetPosition()
        {
            AcquireTarget();
            if (Actor is NPC)
            {
                return (Actor as NPC).CurrentTarget?.Position;
            }
            return null;
        }

        public override bool IsCompleted()
        {
            return !IsValid();
        }

        public override bool IsValid()
        {
            return Actor.SensesHostiles;
        }

        public override double GetRange()
        {
            return 1.5;
            //TODO: determine valid attack range (melee vs ranged)
            //return base.GetRange();
        }

        public override bool Perform()
        {
            AcquireTarget();

            if (Actor.CurrentTarget != null)
            {
                if (IsInValidRange())
                {
                    Direction targetDir = Direction.GetDirection(Actor.Position, Actor.CurrentTarget.Position);
                    if (targetDir != null && targetDir != Direction.NONE)
                    {
                        if (Actor.CommandBumpAttack(targetDir))
                        {
                            return true;
                        }
                    }
                }
            }

            MyGame.Karma.Add(0, Actor);
            return false;
        }

        private void AcquireTarget(bool doTurn = false)
        {
            if (Actor.CurrentTarget != null)
            {
                if (!Actor.IsDead && Actor.VisibleEnemies.Contains(Actor.CurrentTarget))
                {
                    // we still have eyes on our target
                    return;
                }
                else
                {
                    if (doTurn)
                    {
                        var dist = Distance.EUCLIDEAN.Calculate(Actor.Position, Actor.CurrentTarget.Position);
                        if (dist < 2)
                        {
                            Direction targetDir = Direction.GetDirection(Actor.Position, Actor.CurrentTarget.Position);
                            if (targetDir != null && targetDir != Direction.NONE)
                            {
                                Actor.StartTurn(targetDir);
                            }
                        }
                    }
                    // we can no longer see our current target
                    Actor.CurrentTarget = null;
                }
            }

            // find a nearby target
            if (Actor.VisibleEnemies.Count > 0)
            {
                var frontEnemy = Actor.CurrentMap.GetEntityAt<Actor>(Actor.Position + Actor.FacingDirection);
                if (frontEnemy != null && Actor.IsHostileTo(frontEnemy))
                {
                    Actor.CurrentTarget = frontEnemy;
                    return;
                }

                Actor closestActor = null;
                double closestDist = double.MaxValue;
                foreach (var enemy in Actor.VisibleEnemies)
                {
                    var dist = Distance.EUCLIDEAN.Calculate(Actor.Position, enemy.Position);
                    if (dist < closestDist)
                    {
                        closestDist = dist;
                        closestActor = enemy;
                    }
                }

                Actor.CurrentTarget = closestActor;
            }
        }
    }
}
