using Roguelike.Attacks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Entities.Interfaces
{
    interface ICanAttack
    {
        List<Attack> Attacks { get; set; }

        Attack GetAttack(uint attackId);
        Attack GetBestAttack(Actor target);

        bool CanAttack(uint attackId, Actor target);

        bool DoAttack(uint attackId, Actor target);
    }
}
