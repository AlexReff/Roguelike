using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Entities.Items
{
    public class WeaponBase
    {
        private static readonly int BaseRangeFactor = 6;

        public static WeaponBase LongBlade = new WeaponBase(1, 2, false, BaseRangeFactor * 2);
        public static WeaponBase ShortBlade = new WeaponBase(1, 1, false, BaseRangeFactor);
        public static WeaponBase Dagger = new WeaponBase(1, 1, true, BaseRangeFactor / 2);
        public static WeaponBase HandAxe = new WeaponBase(1, 1, false, BaseRangeFactor);
        public static WeaponBase BattleAxe = new WeaponBase(2, 4, false, BaseRangeFactor * 2);
        public static WeaponBase DualBlade = new WeaponBase(2, 2, false, BaseRangeFactor * 2);
        public static WeaponBase Pike = new WeaponBase(1, 2, false, BaseRangeFactor * 2);
        public static WeaponBase Polearm = new WeaponBase(2, 2, false, (int)(BaseRangeFactor * 2.5));
        public static WeaponBase Scythe = new WeaponBase(1, 2, false, BaseRangeFactor * 2);

        static WeaponBase()
        {
            //
        }

        public bool Concealable { get; protected set; }
        public int MinimumHands { get; protected set; }
        public int MaximumHands { get; protected set; }
        public int WeaponRange { get; protected set; }

        protected WeaponBase(int minHands, int maxHands, bool concealable, int range)
        {
            WeaponRange = range;
            MinimumHands = minHands;
            MaximumHands = maxHands;
            Concealable = concealable;
        }
    }
}
