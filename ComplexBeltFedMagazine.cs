using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FistVR;
using UnityEngine;
namespace PuppyScripts
{
    public class ComplexBeltFedMagazine : FVRFireArmMagazine
    {
        public bool isAutoLoadBelt = false;
        private bool hasloadedBelt = false;
        public void Update()
        {
            if (!hasloadedBelt && FireArm != null)
            {
                AutoLoadBelt();
            }
        }
        public void AutoLoadBelt()
        {
            if (isAutoLoadBelt && IsBeltBox)
            {
                hasloadedBelt = true;
                FireArm.BeltDD.StripBeltSegment(FireArm.)

            }
        }
    }
}
