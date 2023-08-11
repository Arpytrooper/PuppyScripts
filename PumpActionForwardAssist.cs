using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using FistVR;
namespace PuppyScripts
{
    public class PumpActionForwardAssist : TubeFedShotgunBoltReleaseTrigger
    {
        public bool tempAutoMode = false;

        private bool hasLockedHandle = false;

        //the FVRUpdate is me trying to condense two scripts
        public override void FVRUpdate()
        {
            base.FVRUpdate();
            if (!hasLockedHandle && Shotgun.Mode == TubeFedShotgun.ShotgunMode.PumpMode && Shotgun.Handle.CurPos == TubeFedShotgunHandle.BoltPos.Rear && !Shotgun.m_proxy.IsFull)
            {
                hasLockedHandle = true;
                Shotgun.Handle.LockHandle();
            } else if (hasLockedHandle && Shotgun.Mode == TubeFedShotgun.ShotgunMode.PumpMode && Shotgun.Handle.CurPos == TubeFedShotgunHandle.BoltPos.Forward)
            {
                hasLockedHandle = false;
            }
            
            
        }
        public override void Poke(FVRViveHand hand)
        {
            if (!Shotgun.Handle.m_isHeld && !Shotgun.IsAltHeld && Shotgun.Bolt.CurPos == TubeFedShotgunBolt.BoltPos.Rear && Shotgun.Mode == TubeFedShotgun.ShotgunMode.PumpMode)
            {
                Shotgun.Mode = TubeFedShotgun.ShotgunMode.Automatic;
                tempAutoMode = true;
                Shotgun.Handle.CurPos = TubeFedShotgunHandle.BoltPos.ForwardToMid;
               // Shotgun.Handle.LockHandle();
            }
            base.Poke(hand);
        }
        public void LateUpdate()
        {
            if (tempAutoMode&& Shotgun.Bolt.CurPos!= TubeFedShotgunBolt.BoltPos.Forward)
            {
                float TargetPoint = (Shotgun.Bolt.GetBoltLerpBetweenRearAndFore()*(Shotgun.Handle.m_boltZ_forward-Shotgun.Handle.m_boltZ_rear))+Shotgun.Handle.m_boltZ_rear;
                float MoveTowardsPoint = Mathf.MoveTowards(Shotgun.Handle.m_boltZ_current, TargetPoint, Shotgun.Handle.Speed_Held * Time.deltaTime);
                Shotgun.Handle.m_boltZ_current = MoveTowardsPoint;
                Shotgun.Handle.transform.localPosition = new Vector3(Shotgun.Handle.transform.localPosition.x, Shotgun.Handle.transform.localPosition.y, MoveTowardsPoint);
                
                
            }
            else if (tempAutoMode && Shotgun.Bolt.CurPos == TubeFedShotgunBolt.BoltPos.Forward)
            {
                float TargetPoint = (Shotgun.Bolt.GetBoltLerpBetweenRearAndFore() * (Shotgun.Handle.m_boltZ_forward - Shotgun.Handle.m_boltZ_rear)) + Shotgun.Handle.m_boltZ_rear;
                float MoveTowardsPoint = Mathf.MoveTowards(Shotgun.Handle.m_boltZ_current, TargetPoint, Shotgun.Handle.Speed_Held * Time.deltaTime);
                Shotgun.Handle.m_boltZ_current = MoveTowardsPoint;
                Shotgun.Handle.transform.localPosition = new Vector3(Shotgun.Handle.transform.localPosition.x, Shotgun.Handle.transform.localPosition.y, MoveTowardsPoint);
                Shotgun.Mode = TubeFedShotgun.ShotgunMode.PumpMode;
                tempAutoMode = false;                
                Shotgun.Handle.CurPos = TubeFedShotgunHandle.BoltPos.Forward;
                Shotgun.Handle.LastPos = TubeFedShotgunHandle.BoltPos.ForwardToMid;
                    hasLockedHandle = false;    
                /*if (Shotgun.Mode == TubeFedShotgun.ShotgunMode.PumpMode)
                {
                    Shotgun.Handle.UnlockHandle();
                }*/
            }
            /*if (hasLockedHandle)
            {
                Shotgun.Handle.LockHandle();
            }*/
        }

    }
}
