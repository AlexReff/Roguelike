﻿using Roguelike.Attacks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roguelike.Spells
{
    class SpellManager
    {
        private static readonly SpellManager instance = new SpellManager();

        // Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
        static SpellManager() { }

        public static SpellManager Instance
        {
            get
            {
                return instance;
            }
        }

        private List<Spell> Spells;

        public SpellManager()
        {
            //load and manage the list of available spells
            Spells = new List<Spell>();

            Spell drainMind = new Spell()
            {
                Name = "Drain Mind",
                TargetTypes = new List<TargetType>() { TargetType.TargetEntity },
                BaseManaCost = 10,
            };

            Spells.Add(drainMind);

            Spell terrify = new Spell()
            {
                Name = "Terrify",
                TargetTypes = new List<TargetType>() { TargetType.TargetEntity },
                BaseManaCost = 5,
            };

            Spells.Add(terrify);
        }

        public List<Spell> GetAllSpells()
        {
            return Spells.ToList();
        }
    }
}
