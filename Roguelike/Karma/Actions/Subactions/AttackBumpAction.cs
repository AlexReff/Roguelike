using GoRogue;
using Roguelike.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Karma.Actions
{
    internal class AttackBumpAction : MultiStageAction
    {
        private Actor _target;

        public AttackBumpAction(Actor attacker, Actor target/*AttackData, target*/) : base(attacker)
        {
            //TODO: store attack data
            _target = target;
        }

        public override long GetDelay()
        {
            return Actor.KarmaActionSpeed;
        }

        public override void Perform()
        {
            Actor.ResolveAttack(_target);
            MyGame.Karma.AddAfterLast(_target.KarmaReactionSpeed, _target);
            IsComplete = true;
        }
    }
}
