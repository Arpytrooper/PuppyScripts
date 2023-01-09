
using FistVR;
using UnityEngine;

namespace PuppyScripts
{
    public class GripDiverter : FVRInteractiveObject
    {
       
        public FVRPhysicalObject POBJ;
        public override void BeginInteraction(FVRViveHand hand)
        {
            base.BeginInteraction(hand);
            EndInteraction(hand);
            hand.ForceSetInteractable(POBJ);
            if (POBJ != null)
            {
                POBJ.BeginInteraction(hand);
            }
        }
    }
}
