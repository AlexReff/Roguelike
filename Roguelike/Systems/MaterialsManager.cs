//using Roguelike.Models;
//using Roguelike.JSON;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Roguelike.Systems
//{
//    class MaterialsManager
//    {
//        public Materials Wood { get; private set; }
//        public Materials Iron { get; private set; }
//        public Materials Steel { get; private set; }
//        public Materials Silver { get; private set; }
//        public Materials Titanium { get; private set; }
//        public Materials Adamantine { get; private set; }

//        private Materials[] _allMaterials;
//        public Materials[] AllMaterials
//        {
//            get
//            {
//                return _allMaterials;
//            }
//        }

//        private MaterialsManager()
//        {
//            Wood = new Materials(
//                name: "Wood",
//                canBeUsedForArmor: false,
//                canBeUsedForWeapons: true,
//                density: 1,
//                heatRateOfChange: 1.76,
//                hardness: 1,
//                meltingPoint: -1,
//                ignitePoint: 573,
//                boilingPoint: -1
//            );
//            Iron = new Materials(
//                name: "Iron",
//                canBeUsedForArmor: true,
//                canBeUsedForWeapons: true,
//                density: 7.5,
//                hardness: 4,
//                heatRateOfChange: 4.5,
//                meltingPoint: 1811,
//                ignitePoint: -1,
//                boilingPoint: 3134
//            );
//            Steel = new Materials(
//                name: "Steel",
//                canBeUsedForArmor: true,
//                canBeUsedForWeapons: true,
//                density: 9,
//                heatRateOfChange: 4,
//                hardness: 5,
//                meltingPoint: 2011,
//                ignitePoint: -1,
//                boilingPoint: 3434
//            );
//            Silver = new Materials(
//                name: "Silver",
//                canBeUsedForArmor: true,
//                canBeUsedForWeapons: true,
//                density: 12,
//                heatRateOfChange: 2.4,
//                hardness: 6,
//                meltingPoint: 1235,
//                ignitePoint: -1,
//                boilingPoint: 2435
//            );
//            Titanium = new Materials(
//                name: "Titanium",
//                canBeUsedForArmor: true,
//                canBeUsedForWeapons: true,
//                density: 20,
//                heatRateOfChange: 1.6,
//                hardness: 8,
//                meltingPoint: 1941,
//                ignitePoint: -1,
//                boilingPoint: 3560
//            );
//            Adamantine = new Materials(
//                name: "Adamantine",
//                canBeUsedForArmor: true,
//                canBeUsedForWeapons: true,
//                density: 1,
//                heatRateOfChange: 0.4,
//                hardness: 15,
//                meltingPoint: -1,
//                ignitePoint: -1,
//                boilingPoint: -1
//            );

//            _allMaterials = new Materials[]
//            {
//                Wood,
//                Iron,
//                Steel,
//                Silver,
//                Titanium,
//                Adamantine,
//            };
//        }

//        //public Materials[] ToList()
//        //{
//        //    return new Materials[]
//        //    {
//        //        Wood,
//        //        Iron,
//        //        Steel,
//        //        Adamantine,
//        //        Titanium,
//        //        Silver,
//        //    };
//        //}

//        //

//        private static readonly MaterialsManager instance = new MaterialsManager();

//        // Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
//        static MaterialsManager() { }

//        public static MaterialsManager Instance
//        {
//            get
//            {
//                return instance;
//            }
//        }
//    }
//}
