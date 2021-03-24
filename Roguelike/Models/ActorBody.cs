using GoRogue;
using Microsoft.Xna.Framework;
using Roguelike.Entities;
using Roguelike.Entities.Items;
using SadConsole;
using System.Collections.Generic;
using System.Linq;

namespace Roguelike.Models
{
    internal class ActorBody
    {
        /// <summary>
        /// The actor this body is connected to
        /// </summary>
        public Actor Parent { get; set; }

        /// <summary>
        /// List of all 'limbs' on this body, are connected to other limbs, can be dismembered/removed from body
        /// Limbs[0] must represent the Torso or 'primary' limb
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


        private Dictionary<string, Limb> _limbs;


        public ActorBody()
        {
            Limbs = new List<Limb>();
            _limbs = new Dictionary<string, Limb>();
            Hands = new List<Hand>();
            Feet = new List<Foot>();
        }

        /// <summary>
        /// 'Severs' the body part, as well as any sub-connected body parts
        /// </summary>
        /// <returns>The removed Limb</returns>
        public Limb RemoveBodyPart(string name)
        {
            Limb part = GetLimb(name);

            if (part != null)
            {
                var fatal = RemoveChildBodyParts(part);
                part.LimbParent = null;

                if (fatal && Parent != null)
                {
                    Parent.Die();
                }
            }

            return part;
        }

        public bool AddLimb(Limb limb)
        {
            if (_limbs.TryAdd(limb.Name, limb))
            {
                Limbs.Add(limb);

                if (limb is Hand)
                {
                    Hands.Add(limb as Hand);
                }
                if (limb is Foot)
                {
                    Feet.Add(limb as Foot);
                }

                return true;
            }

            return false;
        }

        public Limb GetLimb(string name)
        {
            //return Limbs.FirstOrDefault((limb) => limb.Name == name);
            return _limbs[name];
        }

        /// <summary>
        /// Removes child body parts
        /// </summary>
        /// <returns>TRUE if any part is marked IsLifeSupporting</returns>
        private bool RemoveChildBodyParts(Limb part)
        {
            var isLifeSupporting = part.IsLifeSupporting;

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
                    if (RemoveChildBodyParts(childPart))
                    {
                        isLifeSupporting = true;
                    }
                }
            }

            return isLifeSupporting;
        }

        public static ActorBody HumanoidBody(Color foregroundColor)
        {
            ActorBody body = new ActorBody();

            Limb Torso = new Limb("Torso", 't', true, LimbSize.Torso, foregroundColor);
            body.AddLimb(Torso);

            Limb Neck = new Limb("Neck", 'n', true, LimbSize.Neck, foregroundColor);
            body.AddLimb(Neck);
            Neck.LimbParent = Torso;
            Torso.LimbChildren.Add(Neck);

            Limb Head = new Limb("Head", 'h', true, LimbSize.Head, foregroundColor);
            body.AddLimb(Head);
            Head.LimbParent = Neck;
            Neck.LimbChildren.Add(Head);

            Limb LeftUpperArm = new Limb("Left Upper Arm", 'a', false, LimbSize.ArmLeg, foregroundColor);
            body.AddLimb(LeftUpperArm);
            LeftUpperArm.LimbParent = Torso;
            Torso.LimbChildren.Add(LeftUpperArm);

            Limb LeftLowerArm = new Limb("Left Lower Arm", 'a', false, LimbSize.ArmLeg, foregroundColor);
            body.AddLimb(LeftLowerArm);
            LeftLowerArm.LimbParent = LeftUpperArm;
            LeftUpperArm.LimbChildren.Add(LeftLowerArm);

            Hand LeftHand = new Hand("Left Hand", foregroundColor);
            body.AddLimb(LeftHand);
            //body.Hands.Add(LeftHand);
            LeftHand.LimbParent = LeftLowerArm;
            LeftLowerArm.LimbChildren.Add(LeftHand);

            Limb LeftUpperLeg = new Limb("Left Upper Leg", 'l', false, LimbSize.ArmLeg, foregroundColor);
            body.AddLimb(LeftUpperLeg);
            LeftUpperLeg.LimbParent = Torso;
            Torso.LimbChildren.Add(LeftUpperLeg);

            Limb LeftLowerLeg = new Limb("Left Lower Leg", 'l', false, LimbSize.ArmLeg, foregroundColor);
            body.AddLimb(LeftLowerLeg);
            LeftLowerLeg.LimbParent = LeftUpperLeg;
            LeftUpperLeg.LimbChildren.Add(LeftLowerLeg);

            Foot LeftFoot = new Foot("Left Foot", foregroundColor);
            body.AddLimb(LeftFoot);
            //body.Feet.Add(LeftFoot);
            LeftFoot.LimbParent = LeftLowerLeg;
            LeftLowerLeg.LimbChildren.Add(LeftFoot);

            Limb RightUpperArm = new Limb("Right Upper Arm", 'a', false, LimbSize.ArmLeg, foregroundColor);
            body.AddLimb(RightUpperArm);
            RightUpperArm.LimbParent = Torso;
            Torso.LimbChildren.Add(RightUpperArm);

            Limb RightLowerArm = new Limb("Right Lower Arm", 'a', false, LimbSize.ArmLeg, foregroundColor);
            body.AddLimb(RightLowerArm);
            RightLowerArm.LimbParent = RightUpperArm;
            RightUpperArm.LimbChildren.Add(RightLowerArm);

            Hand RightHand = new Hand("Right Hand", foregroundColor);
            body.AddLimb(RightHand);
            //body.Hands.Add(RightHand);
            RightHand.LimbParent = RightLowerArm;
            RightLowerArm.LimbChildren.Add(RightHand);

            Limb RightUpperLeg = new Limb("Right Upper Leg", 'l', false, LimbSize.ArmLeg, foregroundColor);
            body.AddLimb(RightUpperLeg);
            RightUpperLeg.LimbParent = Torso;
            Torso.LimbChildren.Add(RightUpperLeg);

            Limb RightLowerLeg = new Limb("Right Lower Leg", 'l', false, LimbSize.ArmLeg, foregroundColor);
            body.AddLimb(RightLowerLeg);
            RightLowerLeg.LimbParent = RightUpperLeg;
            RightUpperLeg.LimbChildren.Add(RightLowerLeg);

            Foot RightFoot = new Foot("Right Foot", foregroundColor);
            body.AddLimb(RightFoot);
            //body.Feet.Add(RightFoot);
            RightFoot.LimbParent = RightLowerLeg;
            RightLowerLeg.LimbChildren.Add(RightFoot);

            return body;
        }
    }

    //internal class ActorBodyPart : Item
    //{
    //    public double Health { get; set; }
    //    public double MaxHealth { get; set; }

    //    public ActorBodyPart(string name, Color foreground, Color background, char glyph) : base(name, foreground, background, glyph)
    //    {
    //        //
    //    }
    //}

    internal class ActorBodyPart
    {
        public double Health { get; set; }
        public double MaxHealth { get; set; }

        // item properties
        public string Name { get; set; }
        public Color Foreground { get; set; }
        public Color Background { get; set; }
        public char Glyph { get; set; }

        public ActorBodyPart(string name, Color foreground, Color background, char glyph)
        {
            Name = name;
            Foreground = foreground;
            Background = background;
            Glyph = glyph;
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

    internal class Limb : ActorBodyPart
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
        /// <summary>
        /// Whether this limb is required for the body to continue living
        /// </summary>
        public bool IsLifeSupporting { get; set; }

        public Limb(string name, char glyph, bool isLifeSupporting, LimbSize size, Color foregroundColor) : base(name, foregroundColor, Color.Transparent, glyph)
        {
            Name = name;
            LimbParent = null;
            LimbChildren = new List<Limb>();
            LimbSize = size;
            IsLifeSupporting = isLifeSupporting;
        }

        public Item CreateItemInstance(Coord pos)
        {
            return new Item(Name, Foreground, Background, Glyph, pos);
        }

        public Item CreateItemInstance()
        {
            return new Item(Name, Foreground, Background, Glyph);
        }
    }

    internal class Hand : Limb
    {
        /// <summary>
        /// Set to false when the hand loses the ability to grip
        /// </summary>
        public bool IsAbleToGrip { get; set; }
        
        public bool IsHoldingItem { get { return HeldItem != null; } }

        public Item HeldItem { get; set; }

        public Hand(string name, Color foregroundColor) : base(name, 'h', false, LimbSize.HandFoot, foregroundColor)
        {
            Name = name;

            IsAbleToGrip = true;
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

    internal class Foot : Limb
    {
        /// <summary>
        /// Whether this foot is connected to 'ground'
        /// </summary>
        public bool IsOnGround { get; set; }

        /// <summary>
        /// Whether this foot is 'stable' on the ground, providing balance
        /// </summary>
        public bool IsStable { get; set; }

        public Foot(string name, Color foregroundColor) : base(name, 'f', false, LimbSize.HandFoot, foregroundColor)
        {
            Name = name;

            IsOnGround = true;
            IsStable = true;
        }
    }

    //internal class ActorBody : DirectionalTree<ActorBodyPart>
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

    //internal class ActorBodyPart : DTObj
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

    //internal class BodyPartLayer
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
