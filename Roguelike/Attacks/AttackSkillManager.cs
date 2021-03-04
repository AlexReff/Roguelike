using Roguelike.Attacks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roguelike.Attacks
{
    class AttackSkillManager
    {
        private static readonly AttackSkillManager instance = new AttackSkillManager();

        // Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
        static AttackSkillManager() { }

        public static AttackSkillManager Instance
        {
            get
            {
                return instance;
            }
        }

        private List<AttackSkill> Attacks;

        public AttackSkillManager()
        {
            //load and manage the list of available Attacks
            Attacks = new List<AttackSkill>();

            AttackSkill punch = new AttackSkill()
            {
                Name = "Punch",
                TargetTypes = new List<TargetType>() { TargetType.TargetEntity },
                BaseManaCost = 0,
            };

            Attacks.Add(punch);

            AttackSkill kick = new AttackSkill()
            {
                Name = "Kick",
                TargetTypes = new List<TargetType>() { TargetType.TargetEntity },
                BaseManaCost = 0,
            };

            Attacks.Add(kick);

            //AttackSkill terrify = new AttackSkill()
            //{
            //    Name = "Terrify",
            //    TargetTypes = new List<TargetType>() { TargetType.TargetEntity },
            //    BaseManaCost = 5,
            //};

            //Attacks.Add(terrify);
        }

        public List<AttackSkill> GetAllAttacks()
        {
            return Attacks.ToList();
        }
    }
}
