using Roguelike.Systems;
using Roguelike.Weapons;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Entities
{
    internal partial class Actor
    {
        //attributes
        public int Strength { get; set; }
        public int Agility { get; set; }
        public int Stamina { get; set; }
        public int Willpower { get; set; }
        public int Intelligence { get; set; }
        public int Vitae { get; set; }




        //
        public Dictionary<WeaponFamilyEnum, double> WeaponSkills { get; set; }
        //public double HandAxeSkill { get; set; }
        //public double GreatAxeSkill { get; set; }
        //public double LongSwordSkill { get; set; }
        //public double ShortSwordSkill { get; set; }
        //public double DaggerSkill { get; set; }
        //public double ArcherySkill { get; set; }
        //public double CrossbowSkill { get; set; }
        //public double PikeSkill { get; set; }


        //lesser attributes
        public bool HasVision { get; set; }
        /// <summary>
        /// Map FOV Angle (total)
        /// </summary>
        public double FOVViewAngle { get; set; }
        /// <summary>
        /// Map FOV distance
        /// </summary>
        public double Awareness { get; set; }
        public double InnerFOVAwareness { get; set; }
        public double MoveSpeed { get; set; }
        
        private double _actionSpeed;
        public double ActionSpeed
        {
            get
            {
                if (this.IsDead)
                {
                    return 0;
                }
                return _actionSpeed;
            }
            set { _actionSpeed = value; }
        }

        //other properties
        private int _currentLevel;
        public int CurrentLevel
        {
            get { return _currentLevel; }
            set
            {
                _currentLevel = System.Math.Max(1, value);
            }
        }

        public double MaxMana { get; set; }
        private double _mana;
        public double Mana
        {
            get { return _mana; }
            set
            {
                //cannot have negative mana
                _mana = Math.Max(0, Math.Round(value, 2));
            }
        }
    }
}
