
using FistVR;
using UnityEngine;

namespace PuppyScripts
{
    public class PistolSlideDiverter : FVRInteractiveObject
    {

        public FVRInteractiveObject POBJ;
        public bool isHoldingObject = false;
        private FVRViveHand holdingHand;
        public override void BeginInteraction(FVRViveHand hand)
        {
            holdingHand = hand;
            isHoldingObject = true;
            base.BeginInteraction(hand);
            EndInteraction(hand);
            hand.ForceSetInteractable(POBJ);
            if (POBJ != null)
            {
                POBJ.BeginInteraction(hand);
            }
        }
        public void Update()
        {
            if (isHoldingObject && POBJ.m_hand != null && POBJ.m_hand != holdingHand)
            {
                holdingHand = null;
                isHoldingObject = false;
                Debug.Log("stopped holding the object");
            }
            if (isHoldingObject && POBJ.m_hand == null)
            {
                holdingHand = null;
                isHoldingObject = false;
                Debug.Log("stopped holding the object");
            }
            else if (isHoldingObject && POBJ.m_hand != null && POBJ.m_hand == holdingHand)
            {
                Debug.Log("holding the object");

            }
            /*else if (POBJ.m_hand == holdingHand)
            {
                isHoldingObject = true;
            }*/
        }
    }
}
