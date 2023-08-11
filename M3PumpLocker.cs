using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using FistVR;
namespace PuppyScripts
{
    public class M3PumpLocker : MonoBehaviour
    {
        public TubeFedShotgunHandle handle;
        private bool isHandleLocked = false;
        
        public void FVRUpdate()
        {
            if (!isHandleLocked && handle.Shotgun.Mode == TubeFedShotgun.ShotgunMode.PumpMode && handle.CurPos == TubeFedShotgunHandle.BoltPos.Rear && handle.Shotgun.Magazine.m_numRounds == 0)
            {
                isHandleLocked = true;
                handle.LockHandle();
            } else if (isHandleLocked && handle.Shotgun.Mode == TubeFedShotgun.ShotgunMode.PumpMode && handle.CurPos == TubeFedShotgunHandle.BoltPos.Forward)
            {
                handle.UnlockHandle();
                isHandleLocked=false;
            }
        }
    }
}
