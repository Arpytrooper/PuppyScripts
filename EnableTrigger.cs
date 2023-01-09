using UnityEngine;
using FistVR;

namespace PuppyScripts
{

  

    public class EnableTrigger : FVRInteractiveObject
    {

        public GameObject ObjectToEnable;
        public AudioEvent TriggerSound;
        public Transform SoundLocation;
        private Transform PlaceToPlaySound; 
        public override void Start()
        {
            base.Start();
            if (SoundLocation != null)
            {
                PlaceToPlaySound = SoundLocation;
            } else if (SoundLocation == null)
            {
                PlaceToPlaySound = gameObject.transform;
            }
        }
        public override void SimpleInteraction(FVRViveHand hand)
        {
            base.SimpleInteraction(hand);
            ObjectToEnable.SetActive(true);
            //hand.EndInteractionIfHeld(this);
            //hand.TouchSphere.material = hand.TouchSphereMat_NoInteractable;
            //hand.HandTriggerExit();
            hand.CurrentInteractable = null;
            hand.ClosestPossibleInteractable = null;
            SM.PlayGenericSound(TriggerSound, PlaceToPlaySound.position);
            gameObject.SetActive(false);
        }
        
    }

}
