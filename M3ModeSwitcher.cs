using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using FistVR;
namespace PuppyScripts
{
    public class M3ModeSwitcher : FVRInteractiveObject
    {
        public TubeFedShotgun Shotgun;
        public GameObject TurningSwitch;
        public AudioEvent GrabSwitch;
        public AudioEvent ReleaseSwitch;
        [Header("Make sure the Switch's default rotation is 0,0,0")]
        public float SwitchZRotationWhenHeld;
        public Transform ForwardSemiAutoPosition;
        public AudioEvent PumpForwardNoise;
        public float SpeedHeld = 1;
        public float SwitchSpeedHeld = 350;
        private float handZOffset = 0;
        private float handHeldTarget = 0;
        private bool hasTriggeredForwardSound = false;
        
        private bool hasTriggeredRearwardSound = false;

        public override void Start()
        {
            base.Start();
            
        }
        public void Update()
        {
            if (IsHeld && Shotgun.Handle.CurPos == TubeFedShotgunHandle.BoltPos.Forward)
            {
                
                float TempValue = Mathf.MoveTowards(TurningSwitch.transform.localEulerAngles.z, SwitchZRotationWhenHeld, SwitchSpeedHeld * Time.deltaTime);
                TurningSwitch.transform.localEulerAngles = new Vector3(0, 0, TempValue);
                /*float rotationTarget = SwitchZRotationWhenHeld * Time.deltaTime;
                rotationTarget += TurningSwitch.transform.localEulerAngles.z;
                rotationTarget = Mathf.Clamp(rotationTarget, 0, SwitchZRotationWhenHeld);
                TurningSwitch.transform.Rotate(0,0, rotationTarget, Space.Self);*/

                handHeldTarget = Shotgun.transform.InverseTransformPoint(GetClosestValidPoint(Shotgun.Handle.Point_Bolt_Forward.position, ForwardSemiAutoPosition.position, m_hand.Input.Pos + -Shotgun.Handle.transform.forward * handZOffset * Shotgun.transform.localScale.x)).z;
                float num = Mathf.MoveTowards(Shotgun.Handle.transform.localPosition.z, handHeldTarget, SpeedHeld * Time.deltaTime);
                Shotgun.Handle.transform.localPosition = new Vector3(Shotgun.Handle.transform.localPosition.x, Shotgun.Handle.transform.localPosition.y, num);
            }else if (!IsHeld && TurningSwitch.transform.localEulerAngles != Vector3.zero)
            {
                
                float TempValue = Mathf.MoveTowards(TurningSwitch.transform.localEulerAngles.z, 0, SwitchSpeedHeld * Time.deltaTime);
                TurningSwitch.transform.localEulerAngles = new Vector3(0, 0, TempValue);
                /*float rotationTarget = TurningSwitch.transform.localEulerAngles.z * Time.deltaTime;
                float rot = TurningSwitch.transform.localEulerAngles.z - rotationTarget;
                rot = Mathf.Clamp(Mathf.Abs(rot), 0, SwitchZRotationWhenHeld);
                TurningSwitch.transform.Rotate(0, 0, rot);*/
            }
            if (IsHeld && !hasTriggeredForwardSound && Shotgun.transform.InverseTransformPoint(Shotgun.Handle.transform.position).z == Shotgun.transform.InverseTransformPoint(ForwardSemiAutoPosition.transform.position).z)
            {
                SM.PlayGenericSound(PumpForwardNoise, Shotgun.Handle.transform.position);
                hasTriggeredForwardSound = true;
            } else if (IsHeld && !hasTriggeredRearwardSound && Shotgun.transform.InverseTransformPoint(Shotgun.Handle.transform.position).z == Shotgun.transform.InverseTransformPoint(Shotgun.Handle.Point_Bolt_Forward.transform.position).z)
            {

                hasTriggeredRearwardSound = true;
                SM.PlayGenericSound(PumpForwardNoise, Shotgun.Handle.transform.position);
            }
            if (hasTriggeredRearwardSound && Mathf.InverseLerp(Shotgun.transform.InverseTransformPoint(Shotgun.Handle.Point_Bolt_Forward.position).z, Shotgun.transform.InverseTransformPoint(ForwardSemiAutoPosition.position).z, Shotgun.transform.InverseTransformPoint(Shotgun.Handle.transform.position).z) > .5)
            {
                hasTriggeredRearwardSound = false;
                Debug.Log(Mathf.InverseLerp(Shotgun.transform.InverseTransformPoint(Shotgun.Handle.Point_Bolt_Forward.position).z, Shotgun.transform.InverseTransformPoint(ForwardSemiAutoPosition.position).z, Shotgun.transform.InverseTransformPoint(Shotgun.Handle.transform.position).z));
            }
            if (hasTriggeredForwardSound && Mathf.InverseLerp(Shotgun.transform.InverseTransformPoint(Shotgun.Handle.Point_Bolt_Forward.position).z, Shotgun.transform.InverseTransformPoint(ForwardSemiAutoPosition.position).z, Shotgun.transform.InverseTransformPoint(Shotgun.Handle.transform.position).z) < .5)
            {
                hasTriggeredForwardSound = false;
            }




        }
        
        public override void BeginInteraction(FVRViveHand hand)
        {
            if (Shotgun.Handle.CurPos != TubeFedShotgunHandle.BoltPos.Forward)
            {
                hand.EndInteractionIfHeld(this);
            }
            else
            {
                base.BeginInteraction(hand);
                handZOffset = Shotgun.Handle.transform.InverseTransformPoint(hand.Input.Pos).z;
                SM.PlayGenericSound(GrabSwitch, TurningSwitch.transform.position);
            }
        }
        public override void EndInteraction(FVRViveHand hand)
        {
            //if (Shotgun.Handle.transform.position == ForwardSemiAutoPosition.transform.position)//Mathf.InverseLerp(Shotgun.transform.InverseTransformPoint(Shotgun.Handle.Point_Bolt_Forward.position).z, Shotgun.transform.InverseTransformDirection(ForwardSemiAutoPosition.position).z, Shotgun.transform.InverseTransformDirection(Shotgun.Handle.transform.position).z) > .95)
            if (Shotgun.Handle.CurPos != TubeFedShotgunHandle.BoltPos.Forward)
            {
                return;
            }else if (Mathf.InverseLerp(Shotgun.transform.InverseTransformPoint(Shotgun.Handle.Point_Bolt_Forward.position).z, Shotgun.transform.InverseTransformPoint(ForwardSemiAutoPosition.position).z, Shotgun.transform.InverseTransformPoint(Shotgun.Handle.transform.position).z) > .8)
            {
                Shotgun.Handle.transform.position = ForwardSemiAutoPosition.position;
                SM.PlayGenericSound(ReleaseSwitch, TurningSwitch.transform.position);
                
                Shotgun.Handle.LockHandle();
                Shotgun.Mode = TubeFedShotgun.ShotgunMode.Automatic;
                hasTriggeredForwardSound = true;
            } else
            {
                Shotgun.Handle.transform.position = Shotgun.Handle.Point_Bolt_Forward.position;
                SM.PlayGenericSound(ReleaseSwitch, TurningSwitch.transform.position);
                Shotgun.Handle.UnlockHandle();
                Shotgun.Mode = TubeFedShotgun.ShotgunMode.PumpMode;
            hasTriggeredForwardSound = false;
            }
            base.EndInteraction(hand);
        }
    }
}
