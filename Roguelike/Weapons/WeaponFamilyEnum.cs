using GoRogue;
using Microsoft.Xna.Framework;
using Roguelike.Entities.Items;
using Roguelike.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Weapons
{
    public enum WeaponFamilyEnum
    {
        Punching,
        Kicking,
        Wrestling,
        Hatchet, //small, short, fast, throwable, could be backup or alt weapon
        FellingAxe, //axe goes CHOP, 2h or 1h+shield
        Tomahawk, //primary big boy throwing axe, more throwing dmg than hatchet, but slower in melee, slightly heavier
        BattleAxe, //war axe, 1h
        GreatAxe, //2h-only biggest axe type
        ShortSword,
        Kodachi,
        LongSword,
        GreatSword,
        Rapier,
        Katana,
        Cutlass,
        Falchion,
        Shamshir,
        Dagger, //+throwing
        Shield,
        Spear, //'standard' 1h/2h
        Pike, //2h, longer
        Lance, //horseback-only use mostly (unless throwing with exceptional strength)
        Javelin, //for throwing
        Archery, //bows
        Crossbow,
    }

    public enum WeaponQuality
    {
        Broken,
        Shattered,
        Fractured,
        Poor,
        Novice,
        Average,
        High,
        Superior,
        Flawless,
        Perfect,
    }

    //internal abstract class WeaponFamily
    //{
    //    private static Dictionary<WeaponFamilyEnum, WeaponFamily> _weaponFamilies;
    //    public static Dictionary<WeaponFamilyEnum, WeaponFamily> WeaponFamilies { get { return _weaponFamilies; } }
    //    static WeaponFamily()
    //    {
    //        _weaponFamilies = new Dictionary<WeaponFamilyEnum, WeaponFamily>();
    //    }

    //    public WeaponFamilyEnum Family { get; set; }
    //    public string Name { get; set; }

    //    public abstract void Test();

    //    public WeaponFamily(WeaponFamilyEnum weaponFamily, string name)
    //    {
    //        if (_weaponFamilies.ContainsKey(weaponFamily))
    //        {
    //            throw new Exception();
    //        }

    //        Family = weaponFamily;
    //        Name = name;

    //        _weaponFamilies.Add(weaponFamily, this);
    //    }
    //}

    //internal class Punching : WeaponFamily
    //{
    //    public Punching() : base(WeaponFamilyEnum.Punching, "Hand-to-Hand")
    //    {

    //    }
    //    public override void Test()
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
