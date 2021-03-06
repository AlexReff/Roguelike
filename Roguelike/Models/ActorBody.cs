using GoRogue;
using Microsoft.Xna.Framework;
using Roguelike.Entities.Items;
using SadConsole;
using System.Collections.Generic;
using System.Linq;

namespace Roguelike.Models
{
    public class ActorBody
    {
        /// <summary>
        /// List of all 'limbs' on this body, are connected to other limbs, can be dismembered/removed from body
        /// </summary>
        public List<Limb> Limbs { get; set; }
        
        /// <summary>
        /// Parts that can grapple and hold things/weapons
        /// </summary>
        public List<Hand> Hands { get; set; }

        /// <summary>
        /// Parts that can walk and balance
        /// </summary>
        public List<Foot> Feet { get; set; }

        public ActorBody()
        {
            Limbs = new List<Limb>();
            Hands = new List<Hand>();
            Feet = new List<Foot>();
        }

        /// <summary>
        /// 'Severs' the body part, as well as any sub-connected body parts
        /// </summary>
        /// <returns>The removed Limb</returns>
        public Limb RemoveBodyPart(string name)
        {
            Limb part = Limbs.FirstOrDefault((limb) => limb.Name == name);

            if (part != null)
            {
                RemoveChildBodyParts(part);
                part.LimbParent = null;
            }

            return part;
        }

        private void RemoveChildBodyParts(Limb part)
        {
            this.Limbs.Remove(part);

            if (part is Hand)
            {
                this.Hands.Remove((Hand)part);
            }
            if (part is Foot)
            {
                this.Feet.Remove((Foot)part);
            }

            if (part.LimbChildren.Count > 0)
            {
                foreach (var childPart in part.LimbChildren)
                {
                    RemoveChildBodyParts(childPart);
                }
            }
        }

        public static ActorBody HumanoidBody(Color foregroundColor)
        {
            ActorBody body = new ActorBody();

            Limb Torso = new Limb("Torso", 't', LimbSize.Torso, foregroundColor);
            body.Limbs.Add(Torso);

            Limb Neck = new Limb("Neck", 'n', LimbSize.Neck, foregroundColor);
            body.Limbs.Add(Neck);
            Neck.LimbParent = Torso;
            Torso.LimbChildren.Add(Neck);

            Limb Head = new Limb("Head", 'h', LimbSize.Head, foregroundColor);
            body.Limbs.Add(Head);
            Head.LimbParent = Neck;
            Neck.LimbChildren.Add(Head);

            Limb LeftUpperArm = new Limb("Left Upper Arm", 'a', LimbSize.ArmLeg, foregroundColor);
            body.Limbs.Add(LeftUpperArm);
            LeftUpperArm.LimbParent = Torso;
            Torso.LimbChildren.Add(LeftUpperArm);

            Limb LeftLowerArm = new Limb("Left Lower Arm", 'a', LimbSize.ArmLeg, foregroundColor);
            body.Limbs.Add(LeftLowerArm);
            LeftLowerArm.LimbParent = LeftUpperArm;
            LeftUpperArm.LimbChildren.Add(LeftLowerArm);

            Hand LeftHand = new Hand("Left Hand", foregroundColor);
            body.Limbs.Add(LeftHand);
            body.Hands.Add(LeftHand);
            LeftHand.LimbParent = LeftLowerArm;
            LeftLowerArm.LimbChildren.Add(LeftHand);

            Limb LeftUpperLeg = new Limb("Left Upper Leg", 'l', LimbSize.ArmLeg, foregroundColor);
            body.Limbs.Add(LeftUpperLeg);
            LeftUpperLeg.LimbParent = Torso;
            Torso.LimbChildren.Add(LeftUpperLeg);

            Limb LeftLowerLeg = new Limb("Left Lower Leg", 'l', LimbSize.ArmLeg, foregroundColor);
            body.Limbs.Add(LeftLowerLeg);
            LeftLowerLeg.LimbParent = LeftUpperLeg;
            LeftUpperLeg.LimbChildren.Add(LeftLowerLeg);

            Foot LeftFoot = new Foot("Left Foot", foregroundColor);
            body.Limbs.Add(LeftFoot);
            body.Feet.Add(LeftFoot);
            LeftFoot.LimbParent = LeftLowerLeg;
            LeftLowerLeg.LimbChildren.Add(LeftFoot);

            Limb RightUpperArm = new Limb("Right Upper Arm", 'a', LimbSize.ArmLeg, foregroundColor);
            body.Limbs.Add(RightUpperArm);
            RightUpperArm.LimbParent = Torso;
            Torso.LimbChildren.Add(RightUpperArm);

            Limb RightLowerArm = new Limb("Right Lower Arm", 'a', LimbSize.ArmLeg, foregroundColor);
            body.Limbs.Add(RightLowerArm);
            RightLowerArm.LimbParent = RightUpperArm;
            RightUpperArm.LimbChildren.Add(RightLowerArm);

            Hand RightHand = new Hand("Right Hand", foregroundColor);
            body.Limbs.Add(RightHand);
            body.Hands.Add(RightHand);
            RightHand.LimbParent = RightLowerArm;
            RightLowerArm.LimbChildren.Add(RightHand);

            Limb RightUpperLeg = new Limb("Right Upper Leg", 'l', LimbSize.ArmLeg, foregroundColor);
            body.Limbs.Add(RightUpperLeg);
            RightUpperLeg.LimbParent = Torso;
            Torso.LimbChildren.Add(RightUpperLeg);

            Limb RightLowerLeg = new Limb("Right Lower Leg", 'l', LimbSize.ArmLeg, foregroundColor);
            body.Limbs.Add(RightLowerLeg);
            RightLowerLeg.LimbParent = RightUpperLeg;
            RightUpperLeg.LimbChildren.Add(RightLowerLeg);

            Foot RightFoot = new Foot("Right Foot", foregroundColor);
            body.Limbs.Add(RightFoot);
            body.Feet.Add(RightFoot);
            RightFoot.LimbParent = RightLowerLeg;
            RightLowerLeg.LimbChildren.Add(RightFoot);

            return body;
        }
    }


    public class ActorBodyPart : Item
    {
        public ActorBodyPart(string name, Color foreground, Color background, char glyph) : base(name, foreground, background, glyph)
        {
            Name = name;
        }
    }

    public enum LimbSize
    {
        Eye,
        Finger,
        HandFoot,
        ArmLeg,
        Neck,
        Head,
        Torso,
    }

    public class Limb : ActorBodyPart
    {
        /// <summary>
        /// NULL if THIS is TORSO, else the body part this this part connects to
        /// </summary>
        public Limb LimbParent { get; set; }

        /// <summary>
        /// IMMEDIATELY-connected parts
        /// </summary>
        public List<Limb> LimbChildren { get; set; }

        public LimbSize LimbSize { get; set; }

        public Limb(string name, char glyph, LimbSize size, Color foregroundColor) : base(name, foregroundColor, Color.Transparent, glyph)
        {
            Name = name;
            LimbParent = null;
            LimbChildren = new List<Limb>();
            LimbSize = size;
        }
    }

    public class Hand : Limb
    {
        /// <summary>
        /// Set to false when the hand loses the ability to grip
        /// </summary>
        public bool IsAbleToGrip { get; set; }
        
        public bool IsHoldingItem { get { return HeldItem != null; } }

        public Item HeldItem { get; set; }

        public Hand(string name, Color foregroundColor) : base(name, 'h', LimbSize.HandFoot, foregroundColor)
        {
            Name = name;

            IsAbleToGrip = true;
            //IsHoldingItem = false;
        }

        public bool HoldItem(Item item)
        {
            if (HeldItem == null && IsAbleToGrip)
            {
                HeldItem = item;
                return true;
            }

            return false;
        }

        public void ReleaseItem()
        {
            HeldItem = null;
        }
    }

    public class Foot : Limb
    {
        /// <summary>
        /// Whether this foot is connected to 'ground'
        /// </summary>
        public bool IsOnGround { get; set; }

        /// <summary>
        /// Whether this foot is 'stable' on the ground, providing balance
        /// </summary>
        public bool IsStable { get; set; }

        public Foot(string name, Color foregroundColor) : base(name, 'f', LimbSize.HandFoot, foregroundColor)
        {
            Name = name;

            IsOnGround = true;
            IsStable = true;
        }
    }

    //public class ActorBody : DirectionalTree<ActorBodyPart>
    //{
    //    //public DirectionalTree<ActorBodyPart> BodyParts { get; set; }

    //    public ActorBody(ActorBodyPart primaryTorso) : base(primaryTorso)
    //    {
    //        //BodyParts = new DirectionalTree<ActorBodyPart>(primaryTorso);
    //    }

    //    //public void AddBodyBodyPart(XYZRelativeDirection dir, ActorBodyPart part)
    //    //{
    //    //    base.AddPart(dir, part);
    //    //}

    //    public static ActorBody HumanoidBody(double baseHp)
    //    {
    //        ActorBodyPart Torso = new ActorBodyPart("Torso", baseHp * 2.0);
    //        ActorBody body = new ActorBody(Torso);

    //        ActorBodyPart Neck = new ActorBodyPart("Neck", baseHp * .2);
    //        body.AddPart(XYZRelativeDirection.Up, Neck);

    //        var NeckTree = body.GetTrees("Neck")[0];

    //        ActorBodyPart Head = new ActorBodyPart("Head", baseHp * .5);
    //        NeckTree.AddPart(XYZRelativeDirection.Up, Head);

    //        var HeadTree = NeckTree.GetTrees("Head")[0];

    //        var LeftUpperArm = new ActorBodyPart("LeftUpperArm", baseHp * .7);
    //        body.AddPart(XYZRelativeDirection.Left, LeftUpperArm);

    //        var LeftUpperArmTree = body.GetTrees("LeftUpperArm")[0];

    //        var LeftLowerArm = new ActorBodyPart("LeftLowerArm", baseHp * .7);
    //        LeftUpperArmTree.AddPart(XYZRelativeDirection.Left, LeftLowerArm);

    //        var LeftLowerArmTree = LeftUpperArmTree.GetTrees("LeftLowerArm")[0];

    //        var LeftHand = new ActorBodyPart("LeftHand", baseHp * .4);
    //        LeftLowerArmTree.AddPart(XYZRelativeDirection.Left, LeftHand);

    //        return body;
    //    }
    //}

    //public class ActorBodyPart : DTObj
    //{
    //    /// <summary>
    //    /// Represents the 'layers' of the body part, where the first item is the outermost layer, eg skin
    //    /// </summary>
    //    public LinkedList<BodyPartLayer> Layers { get; set; }
        
    //    public string Name { get; set; }
    //    public double Health { get; set; }
    //    /// <summary>
    //    /// Size, from 0-? of the connected body part. A regular torso would be 100. A nose or ear might be 5.
    //    /// </summary>
    //    public double Size { get; set; }
    //    public bool CanBleed { get; set; }
    //    public bool CanBeInfected { get; set; }

    //    public ActorBodyPart(string name, double hp)
    //    {
    //        Name = name;
    //        Health = hp;
    //        Layers = new LinkedList<BodyPartLayer>();
    //        CanBleed = true;
    //        CanBeInfected = true;
    //    }

    //    public ActorBodyPart(string name, double hp, bool canBleed) : this(name, hp)
    //    {
    //        CanBleed = canBleed;
    //    }
    //}

    //public class BodyPartLayer
    //{
    //    public string Name { get; set; }
    //    public double Health { get; set; }
    //    public bool CanBeCut { get; set; }
    //    public bool CanBeFractured { get; set; }
    //    public bool CanBeBroken { get; set; }

    //    public BodyPartLayer(string name, double hp)
    //    {
    //        Name = name;
    //        Health = hp;
    //    }
    //}

    ////Represents potentially 'connected' body parts
    //public enum BodyPart
    //{
    //    Head,
    //    Neck,
    //    Chest,
    //    Stomach,
    //    UpperArm,
    //    LowerArm,
    //    UpperLeg,
    //    LowerLeg,
    //    //Pelvis, //?
    //    Foot,
    //}
}
