using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Karma.Goals
{
    internal class KarmaGoal
    {
        public string Name { get; set; }
        public Dictionary<string, object> Goals { get; }

        public KarmaGoal(string name)
        {
            Name = name;
            Goals = new Dictionary<string, object>();
        }

        //static

        public static List<KarmaGoal> AllGoals { get; }
        static KarmaGoal()
        {
            AllGoals = new List<KarmaGoal>();
        }
    }
}
