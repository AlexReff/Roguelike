
/*
 * Determine 'chance to strike' with 'attack/skill'
 * Determine level of penetration, then damage to each layer
 * If sever,
 */

namespace Roguelike.Models
{
    internal class ActorBody
    {
        //
    }

    internal class ActorBodyPart
    {
        public BodyPart Part;

        public ActorBodyPart(BodyPart part)
        {
            Part = part;
        }
    }

    //Represents potentially 'connected' body parts
    internal enum BodyPart
    {
        Head,
        Neck,
        Chest,
        Stomach,
        UpperArm,
        LowerArm,
        UpperLeg,
        LowerLeg,
        //Pelvis, //?
        Foot,
    }

    //internal struct Organ
    //{
    //    string Name;
    //}
}
