using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FistVR;

namespace PuppyScripts
{
    public class TempRecoilProfile : FVRFireArmRecoilProfile
    {
        public float StoredVerticalRotPerShot;
        public float StoredHorizontalRotPerShot;
        public float StoredMaxVerticalRot;
        public float StoredMaxHorizontalRot;
        public float StoredVerticalRotRecovery;
        public float StoredHorizontalRotRecovery;
        public float StoredZLinearPerShot;
        public float StoredZLinearMax;
        public float StoredZLinearRecovery;
        public float StoredXYLinearPerShot;
        public float StoredXYLinearMax;
        public float StoredXYLinearRecovery;
        public bool StoredIsConstantRecoil;

        public float StoredVerticalRotPerShot_Bipodded;
        public float StoredHorizontalRotPerShot_Bipodded;
        public float StoredMaxVerticalRot_Bipodded;
        public float StoredMaxHorizontalRot_Bipodded;

        public Vector4 StoredRecoveryStabilizationFactors_Foregrip = new Vector4(1f, 1f, 0.8f, 0.8f);
        public Vector4 StoredRecoveryStabilizationFactors_Twohand = new Vector4(0.5f, 0.8f, 0.3f, 0.75f);
        public Vector4 StoredRecoveryStabilizationFactors_None = new Vector4(0.25f, 0.5f, 0.25f, 0.25f);

        public float StoredMassDriftIntensity = 0.1f;
        public Vector4 StoredMassDriftFactors = new Vector4(0.25f, 0.6f, 1f, 0.1f);
        public float StoredMaxMassDriftMagnitude = 5f;
        public float StoredMaxMassMaxRotation = 20f;
        public float StoredMassDriftRecoveryFactor = 5f;
    }
}
