using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEditor.AnimatedValues;
using FistVR;
namespace PuppyScripts
{
    public class AttributeChangingInterface : FVRFireArmAttachmentInterface
    {       
        
        
                                                                                    //Recoil adjustment stuff
        [Header("RECOIL ADJUSTMENTS")]
        public bool AdjustsRecoil = false;
        [Header("These multiply the recoil base recoil in their respective slots")]
        public float HorizontalAdjustment = 1;
        private float HorizontalAmount;
        public float VerticalAdjustment = 1;
        private float VerticalAmount;
        public float ZLinearPerShotAdjustment = 1;
        private float ZLinearAmount;
        public float XYLinearPerShotAdjustment = 1;
        private float XYLinearAmount;
        [Header("These adjust maximum recoil for their respective parameters")]
        public float MaxHorizontalRotationAdjustment = 1;
        private float HorizontalRotationAmount;
        public float MaxVerticalRotationAdjustment = 1;
        private float VerticalRotationAmount;
        public float MaxZLinearMovementAdjustment = 1;
        private float ZLinearRotationAmount;
        public float MaxXYLinearMovementAdjustment = 1;
        private float XYLinearRotationAmount;
        [Header("These adjust recovery time")]
        public float HorizontalRecoveryAdjustment = 1;
        private float HorizontalRecoveryAmount;
        public float VerticalRecoveryAdjustment = 1;
        private float VerticalRecoveryAmount;
        public float ZLinearRecoveryAdjustment = 1;
        private float ZLinearRecoveryAmount;
        public float XYRecoveryAdjustment = 1;
        private float XYLinearRecoveryAmount;
        private TempRecoilProfile profile;
        private bool hasChangedRecoil = false;


        public override void Awake()
        {
            base.Awake();
            if (ChangesMuzzlePoint)
            {
                tempDevice = Attachment as MuzzleDevice;
                tempDevice.Muzzle = NewMuzzlePoint;
            }
        }
        public override void BeginInteraction(FVRViveHand hand)
        {
            if (doesAddForegrip)
            {
                FVRFireArm fvrFireArm = OverrideFirearm;
                if ((Object)fvrFireArm == (Object)null)
                    fvrFireArm = Attachment.GetRootObject() as FVRFireArm;
                if (!(fvrFireArm != null) || !(fvrFireArm.Foregrip != null))
                    return;
                FVRAlternateGrip component = fvrFireArm.Foregrip.GetComponent<FVRAlternateGrip>();
                hand.ForceSetInteractable(component);
                tempGrip = new AttachableForegrip();
                component.BeginInteractionFromAttachedGrip(tempGrip, hand);
                //BeginInteractionFromMe(component, this, hand);
            }
        }
        public override void FVRUpdate()
        {
            base.FVRUpdate();
            if (Attachment.curMount != null) {
                if (AdjustsRecoil)
                {
                    if (!hasChangedRecoil && Attachment.curMount != null)
                    {
                        SetsRecoilSettings(Attachment.curMount.GetRootMount().Parent as FVRFireArm);
                    }
                }
                if (ChangesPoseOverride)
                {
                    if (!HasChangedPoseOverride && Attachment.curMount != null)
                    {
                        AttachPoseChanger(Attachment.curMount.GetRootMount().Parent as FVRFireArm);
                    }

                }
                if (ChangesRecoilProfile)
                {
                    if (!HasChangedRecoilProfile && Attachment.curMount != null)
                    {
                        AttachRecoilProfileChanger(Attachment.curMount.GetRootMount().Parent as FVRFireArm);
                    }
                }
                // if (changesProjectileType && Attachment.curMount != null)
                // {
                //     FVRFireArm gun = Attachment.curMount.GetRootMount().Parent as FVRFireArm;
                //}
                if (ChangesChamberRoundClass && Attachment.curMount != null)
                {
                    RoundClassUpdateVoid(Attachment.curMount.GetRootMount().Parent as FVRFireArm);
                }
                if (ChangesStockPoint)
                {
                    if (!HasChangedStockPoint && Attachment.curMount != null)
                    {
                        AttachChangeStockPoint(Attachment.curMount.GetRootMount().Parent as FVRFireArm);
                    }
                }
                if (ChangesSoundProfile)
                {
                    if (!HasChangedSoundProfile && Attachment.curMount != null)
                    {
                        AttachSoundProfileChange(Attachment.curMount.GetRootMount().Parent as FVRFireArm);
                    }
                }
                if (ChangesMuzzleVelocity)
                {
                    if (!HasChangedMuzzleVelocity && Attachment.curMount != null)
                    {
                        AttachMuzzleVelocityChanger(Attachment.curMount.GetRootMount().Parent as FVRFireArm);
                    }
                }
                if (doesAddPistolBoltHandle)
                {
                    //Debug.Log("doesAddPistolBoltHandle");
                    if (HasChangedBoltHandle)
                    {
                        //Debug.Log("HasChangedBoltHandle");
                        if (newGrabPoint.isHoldingObject)
                        {
                            heldMesh.SetActive(true);
                            unheldMesh.SetActive(false);
                            //Debug.Log("grabbing");
                        }
                        if (!newGrabPoint.isHoldingObject)
                        {
                            heldMesh.SetActive(false);
                            unheldMesh.SetActive(true);
                            //Debug.Log("grabbint");
                        }
                    }
                }
                if (doesAddForegrip && !hasAddedForegrip)
                {
                    AttachForegripAdd(Attachment.curMount.GetRootMount().Parent as FVRFireArm);
                }
            }
        }
        public override void OnAttach()
        {
            
            base.OnAttach();
            if (AdjustsRecoil) //Checks if it adjusts recoil
            {
                SetsRecoilSettings(Attachment.curMount.GetRootMount().Parent as FVRFireArm);
            }
            if (ChangesPoseOverride)
            {
                Debug.Log("ChangesPoseOverride");
                AttachPoseChanger(Attachment.curMount.GetRootMount().Parent as FVRFireArm);
            }
            if (ChangesRecoilProfile)
            {
                AttachRecoilProfileChanger(Attachment.curMount.GetRootMount().Parent as FVRFireArm);
            }
            if (ChangesChamberRoundClass)
            {
                AttachChangeRoundClass(Attachment.curMount.GetRootMount().Parent as FVRFireArm);
            }
            if (ChangesStockPoint)
            {
                AttachChangeStockPoint(Attachment.curMount.GetRootMount().Parent as FVRFireArm);
            }
            if (ChangesSoundProfile)
            {
                AttachSoundProfileChange(Attachment.curMount.GetRootMount().Parent as FVRFireArm);
            }
            if (ChangesMuzzleVelocity)
            {
                AttachMuzzleVelocityChanger(Attachment.curMount.GetRootMount().Parent as FVRFireArm);
            }
            if (ChangesMuzzlePoint)
            {
                AttachNewMuzzlePoint();
            }
            if (doesAddPistolBoltHandle)
            {
                AttachPistolBoltHandleChanger();
            }
            if (doesAddForegrip)
            {
                AttachForegripAdd(Attachment.curMount.GetRootMount().Parent as FVRFireArm);
            }
        }
        public override void OnDetach()
        {
            if (AdjustsRecoil)
            {
                DetatchRecoilChange();
            }
            if (ChangesPoseOverride)
            {
                DetatchPoseChanger(Attachment.curMount.GetRootMount().Parent as FVRFireArm);
            }
            if (ChangesRecoilProfile)
            {
                DetatchRecoilProfileChanger(Attachment.curMount.GetRootMount().Parent as FVRFireArm);
            }
            if (ChangesChamberRoundClass)
            {
                DetatchChangeRoundClass();
            }
            if (ChangesStockPoint)
            {
                DetatchChangeStockPoint(Attachment.curMount.GetRootMount().Parent as FVRFireArm);
            }
            if (ChangesSoundProfile)
            {
                DetatchSoundProfileChange(Attachment.curMount.GetRootMount().Parent as FVRFireArm);
            }
            if (ChangesMuzzleVelocity) {
                DetatchMuzleVelocityChanger(Attachment.curMount.GetRootMount().Parent as FVRFireArm);
            }
            if (ChangesMuzzlePoint)
            {
                DetatchNewMuzzlePoint(Attachment.curMount.GetRootMount().Parent as FVRFireArm);
            }
            if (doesAddPistolBoltHandle)
            {
                DetatchPistolBoltHandleChanger();
            }
            if (doesAddForegrip)
            {
                DetatchForegripAdd();
            }

            base.OnDetach();
            
        }
        public void SetsRecoilSettings(FVRFireArm gun)
        {
            AttributeChangingInterface topAttachment = null;

            Debug.Log("setRecoilSettings got called");
            // TempRecoilProfile recoilProfile = gun.RecoilProfile as TempRecoilProfile;
            if (!(gun.RecoilProfile is TempRecoilProfile))
            {
                Debug.Log("RecoilProfile is null");


                foreach (FVRFireArmAttachment attch in gun.AttachmentsList)
                {
                    if (attch.AttachmentInterface is AttributeChangingInterface ACI)
                    {
                        if (ACI.AdjustsRecoil)
                        {
                            topAttachment = ACI;
                            break;
                        }
                    }

                    Debug.Log("going through the foreach");

                }

                if (this == topAttachment)
                {
                    Debug.Log("making new profile");
                    TempRecoilProfile NewRecoilProfile = ScriptableObject.CreateInstance<TempRecoilProfile>();
                    NewRecoilProfile.StoredVerticalRotPerShot = NewRecoilProfile.VerticalRotPerShot = gun.RecoilProfile.VerticalRotPerShot;
                    NewRecoilProfile.StoredHorizontalRotPerShot = NewRecoilProfile.HorizontalRotPerShot = gun.RecoilProfile.HorizontalRotPerShot;
                    NewRecoilProfile.StoredMaxVerticalRot = NewRecoilProfile.MaxVerticalRot = gun.RecoilProfile.MaxVerticalRot;
                    NewRecoilProfile.StoredMaxHorizontalRot = NewRecoilProfile.MaxHorizontalRot = gun.RecoilProfile.MaxHorizontalRot;
                    NewRecoilProfile.StoredVerticalRotRecovery = NewRecoilProfile.VerticalRotRecovery = gun.RecoilProfile.VerticalRotRecovery;
                    NewRecoilProfile.StoredHorizontalRotRecovery = NewRecoilProfile.HorizontalRotRecovery = gun.RecoilProfile.HorizontalRotRecovery;
                    NewRecoilProfile.StoredZLinearPerShot = NewRecoilProfile.ZLinearPerShot = gun.RecoilProfile.ZLinearPerShot;
                    NewRecoilProfile.StoredZLinearMax = NewRecoilProfile.ZLinearMax = gun.RecoilProfile.ZLinearMax;
                    NewRecoilProfile.StoredZLinearRecovery = NewRecoilProfile.ZLinearRecovery = gun.RecoilProfile.ZLinearRecovery;
                    NewRecoilProfile.StoredXYLinearPerShot = NewRecoilProfile.XYLinearPerShot = gun.RecoilProfile.XYLinearPerShot;
                    NewRecoilProfile.StoredXYLinearMax = NewRecoilProfile.XYLinearMax = gun.RecoilProfile.XYLinearMax;
                    NewRecoilProfile.StoredXYLinearRecovery = NewRecoilProfile.XYLinearRecovery = gun.RecoilProfile.XYLinearRecovery;
                    NewRecoilProfile.StoredIsConstantRecoil = NewRecoilProfile.IsConstantRecoil = gun.RecoilProfile.IsConstantRecoil;
                    NewRecoilProfile.StoredVerticalRotPerShot_Bipodded = NewRecoilProfile.VerticalRotPerShot_Bipodded = gun.RecoilProfile.VerticalRotPerShot_Bipodded;
                    NewRecoilProfile.StoredHorizontalRotPerShot_Bipodded = NewRecoilProfile.HorizontalRotPerShot_Bipodded = gun.RecoilProfile.HorizontalRotPerShot_Bipodded;
                    NewRecoilProfile.StoredMaxVerticalRot_Bipodded = NewRecoilProfile.MaxVerticalRot_Bipodded = gun.RecoilProfile.MaxVerticalRot_Bipodded;
                    NewRecoilProfile.StoredMaxHorizontalRot_Bipodded = NewRecoilProfile.MaxHorizontalRot_Bipodded = gun.RecoilProfile.MaxHorizontalRot_Bipodded;
                    NewRecoilProfile.StoredRecoveryStabilizationFactors_Foregrip = NewRecoilProfile.RecoveryStabilizationFactors_Foregrip = gun.RecoilProfile.RecoveryStabilizationFactors_Foregrip;
                    NewRecoilProfile.StoredRecoveryStabilizationFactors_Twohand = NewRecoilProfile.RecoveryStabilizationFactors_Twohand = gun.RecoilProfile.RecoveryStabilizationFactors_Twohand;
                    NewRecoilProfile.StoredRecoveryStabilizationFactors_None = NewRecoilProfile.RecoveryStabilizationFactors_None = gun.RecoilProfile.RecoveryStabilizationFactors_None;
                    NewRecoilProfile.StoredMassDriftIntensity = NewRecoilProfile.MassDriftIntensity = gun.RecoilProfile.MassDriftIntensity;
                    NewRecoilProfile.StoredMassDriftFactors = NewRecoilProfile.MassDriftFactors = gun.RecoilProfile.MassDriftFactors;
                    NewRecoilProfile.StoredMaxMassDriftMagnitude = NewRecoilProfile.MaxMassDriftMagnitude = gun.RecoilProfile.MaxMassDriftMagnitude;
                    NewRecoilProfile.StoredMaxMassMaxRotation = NewRecoilProfile.MaxMassMaxRotation = gun.RecoilProfile.MaxMassMaxRotation;
                    NewRecoilProfile.StoredMassDriftRecoveryFactor = NewRecoilProfile.MassDriftRecoveryFactor = gun.RecoilProfile.MassDriftRecoveryFactor;
                    gun.RecoilProfile = NewRecoilProfile;
                    profile = NewRecoilProfile;


                }
            }
            else if (gun.RecoilProfile is TempRecoilProfile tempProfile)
            {
                profile = tempProfile;
                Debug.Log("setting profile");
            }

            AttachRecoilChange();
            Debug.Log("Tried to change recoil");
        }
        public void AttachRecoilChange()
        {
            Debug.Log("called AttachRecoilChange");
            HorizontalAmount = HorizontalAdjustment * profile.StoredHorizontalRotPerShot;
            VerticalAmount = VerticalAdjustment * profile.StoredVerticalRotPerShot;
            ZLinearAmount = ZLinearPerShotAdjustment * profile.StoredZLinearPerShot;
            XYLinearAmount = XYLinearPerShotAdjustment * profile.StoredXYLinearPerShot;
            HorizontalRotationAmount = MaxHorizontalRotationAdjustment * profile.StoredMaxHorizontalRot;
            VerticalRotationAmount = MaxVerticalRotationAdjustment * profile.StoredMaxVerticalRot;
            ZLinearRotationAmount = MaxZLinearMovementAdjustment * profile.StoredZLinearMax;
            XYLinearRotationAmount = MaxXYLinearMovementAdjustment * profile.StoredXYLinearMax;
            HorizontalRecoveryAmount = HorizontalRecoveryAdjustment * profile.StoredHorizontalRotRecovery;
            VerticalRecoveryAmount = VerticalRecoveryAdjustment * profile.StoredHorizontalRotRecovery;
            ZLinearRecoveryAmount = ZLinearRecoveryAdjustment * profile.StoredZLinearRecovery;
            XYLinearRecoveryAmount = XYRecoveryAdjustment * profile.StoredXYLinearRecovery;
            //Changes the recoil on the actual gun 
            //Also checks if it would equal less than 0 and if it does then sets all the adjustments to zero
            if (profile.VerticalRotPerShot - VerticalAmount < 0) { 
                VerticalAmount = 0; 
            } else {
                profile.VerticalRotPerShot = profile.VerticalRotPerShot - VerticalAmount;
            }
            if (profile.HorizontalRotPerShot - HorizontalAmount < 0)
            {
                HorizontalAmount = 0;
            }
            else
            {
                profile.HorizontalRotPerShot = profile.HorizontalRotPerShot - HorizontalAmount;
            }
            if (profile.ZLinearPerShot - ZLinearAmount < 0)
            {
                ZLinearAmount = 0;
            }
            else
            {
                profile.ZLinearPerShot = profile.ZLinearPerShot - ZLinearAmount;
            }
            if (profile.XYLinearPerShot - XYLinearAmount < 0)
            {
                XYLinearAmount = 0;
            }
            else
            {
                profile.XYLinearPerShot = profile.XYLinearPerShot - XYLinearAmount;
            }
            if (profile.MaxVerticalRot - VerticalRotationAmount < 0)
            {
                VerticalRotationAmount = 0;
            }
            else
            {
                profile.MaxVerticalRot = profile.MaxVerticalRot - VerticalRotationAmount;
            }
            if (profile.MaxHorizontalRot - HorizontalRotationAmount < 0)
            {
                HorizontalRotationAmount = 0;
            }
            else
            {
                profile.MaxHorizontalRot = profile.MaxHorizontalRot - HorizontalRotationAmount;
            }
            if (profile.ZLinearMax - ZLinearRotationAmount < 0)
            {
                ZLinearRotationAmount = 0;
            }
            else
            {
                profile.ZLinearMax = profile.ZLinearMax - ZLinearRotationAmount;
            }
            if (profile.XYLinearMax - XYLinearRotationAmount < 0)
            {
                XYLinearRotationAmount = 0;
            }
            else
            {
                profile.XYLinearMax = profile.XYLinearMax - XYLinearRotationAmount;
            }
            if (profile.VerticalRotRecovery - VerticalRecoveryAmount < 0)
            {
                VerticalRecoveryAmount = 0;
            }
            else
            {
                profile.VerticalRotRecovery = profile.VerticalRotRecovery - VerticalRecoveryAmount;
            }
            if (profile.HorizontalRotRecovery - HorizontalRecoveryAmount < 0)
            {
                HorizontalRecoveryAmount = 0;
            }
            else
            {
                profile.HorizontalRotRecovery = profile.HorizontalRotRecovery - HorizontalRecoveryAmount;
            }
            if (profile.ZLinearRecovery - ZLinearRecoveryAmount < 0)
            {
                ZLinearRecoveryAmount = 0;
            }
            else
            {
                profile.ZLinearRecovery = profile.ZLinearRecovery - ZLinearRecoveryAmount;
            }
            if (profile.XYLinearRecovery - XYLinearRecoveryAmount < 0)
            {
                XYLinearRecoveryAmount = 0;
            }
            else
            {
                profile.XYLinearRecovery = profile.XYLinearRecovery - XYLinearRecoveryAmount;
            }
            hasChangedRecoil = true;
        }
        public void DetatchRecoilChange()
        {
            profile.VerticalRotPerShot = profile.VerticalRotPerShot + VerticalAmount;
            profile.HorizontalRotPerShot = profile.HorizontalRotPerShot + HorizontalAmount;
            profile.ZLinearPerShot = profile.ZLinearPerShot + ZLinearAmount;
            profile.XYLinearPerShot = profile.XYLinearPerShot + XYLinearAmount;
            profile.MaxVerticalRot = profile.MaxVerticalRot + VerticalRotationAmount;
            profile.MaxHorizontalRot = profile.MaxHorizontalRot + HorizontalRotationAmount;
            profile.ZLinearMax = profile.ZLinearMax + ZLinearRotationAmount;
            profile.XYLinearMax = profile.XYLinearMax + XYLinearRotationAmount;
            profile.VerticalRotRecovery = profile.VerticalRotRecovery + VerticalRecoveryAmount;
            profile.HorizontalRotRecovery = profile.HorizontalRotRecovery + HorizontalRecoveryAmount;
            profile.ZLinearRecovery = profile.ZLinearRecovery + ZLinearRecoveryAmount;
            profile.XYLinearRecovery = profile.XYLinearRecovery + XYLinearRecoveryAmount;
            hasChangedRecoil = false;
        }
                                                                                                                                        // POSE OVERRIDE STUFF
        public bool ChangesPoseOverride = false;
        public Transform newOverride;
        public Transform newRecoilHolder;
        public Transform newOverrideTouch;
        public GripDiverter GD; 
       
        public Vector3 StoredRecoilHolderPos;
        public Vector3 StoredRecoilHolderRot;
        public Vector3 StoredOriginalRecoilHolderPos;
        public Vector3 StoredOverridePos;
        public Vector3 StoredOverrideRot;
        public Vector3 StoredOverridePost;
        public Vector3 StoredOverrideRott;
        public Quaternion StoredLocalPoseOverrideRot;
        private Vector3 StoredXYRecoilBase;
        private bool HasChangedPoseOverride = false;
        public void AttachPoseChanger(FVRFireArm gun)
        {
            // Debug.Log("StartsAttachPoseChanger");
            // StoredOverride = new GameObject().transform;
            //  Debug.Log("makes override 1");
            StoredXYRecoilBase = gun.m_recoilLinearXYBase;
            StoredRecoilHolderPos = gun.RecoilingPoseHolder.localPosition;
            StoredRecoilHolderRot = gun.RecoilingPoseHolder.localEulerAngles;
            StoredOriginalRecoilHolderPos = gun.m_recoilPoseHolderLocalPosStart;
            StoredOverridePos = gun.PoseOverride.localPosition;
            StoredOverrideRot = gun.PoseOverride.localEulerAngles;
            StoredOverridePost = gun.PoseOverride_Touch.localPosition;
            StoredOverrideRott = gun.PoseOverride_Touch.localEulerAngles;
            StoredLocalPoseOverrideRot = gun.m_storedLocalPoseOverrideRot;
            //Begin making the changes
            // gun.RecoilingPoseHolder.localPosition = newRecoilHolder.localPosition;
            // gun.RecoilingPoseHolder.localEulerAngles = newRecoilHolder.localEulerAngles;
            //  gun.m_recoilPoseHolderLocalPosStart = newRecoilHolder.localPosition;
            // Debug.Log("sets override 1 to override");
            
            GD.gameObject.SetActive(true);
            GD.POBJ = gun;
            newRecoilHolder.SetParent(gun.transform, true);
            gun.m_recoilLinearXYBase = new Vector2(newRecoilHolder.localPosition.x, newRecoilHolder.localPosition.y);
            gun.RecoilingPoseHolder.localPosition = newRecoilHolder.localPosition;
            //gun.RecoilingPoseHolder.rotation = newRecoilHolder.rotation;
            gun.m_recoilPoseHolderLocalPosStart = gun.RecoilingPoseHolder.localPosition;
            gun.m_storedLocalPoseOverrideRot = newOverride.transform.localRotation;
            gun.PoseOverride.position = newOverride.position;
            gun.PoseOverride.rotation = newOverride.rotation;
           // Debug.Log("makes override 2");
            gun.PoseOverride_Touch.position = newOverrideTouch.position;
            gun.PoseOverride_Touch.rotation = newOverrideTouch.rotation;
           // Debug.Log("sets override 2 to override");
            
           // gun.PoseOverride = newOverride;
           // Debug.Log("sets new override");
            //gun.PoseOverride_Touch = newOverrideTouch;
           // Debug.Log("sets new overridet");
            HasChangedPoseOverride = true;
        }
        public void DetatchPoseChanger(FVRFireArm gun)
        {
            //gun.PoseOverride.position = StoredOverride.position;
            //gun.PoseOverride.rotation = StoredOverride.rotation;
            
            newRecoilHolder.SetParent(transform, true);
            gun.RecoilingPoseHolder.localPosition = StoredRecoilHolderPos;
            //gun.RecoilingPoseHolder.localEulerAngles = StoredRecoilHolderRot;
            gun.m_recoilPoseHolderLocalPosStart = StoredOriginalRecoilHolderPos;
            gun.m_storedLocalPoseOverrideRot = StoredLocalPoseOverrideRot;
            gun.PoseOverride.localPosition = StoredOverridePos;
            gun.PoseOverride.localEulerAngles = StoredOverrideRot;
            gun.PoseOverride_Touch.localPosition = StoredOverridePost;
            gun.PoseOverride_Touch.localEulerAngles = StoredOverrideRott;
            //gun.PoseOverride_Touch.position = StoredOverrideTouch.position;
            //gun.PoseOverride_Touch.rotation = StoredOverrideTouch.rotation;
            gun.m_recoilLinearXYBase = StoredXYRecoilBase;
            HasChangedPoseOverride = false;
            GD.POBJ = null;
            GD.gameObject.SetActive(false);
        }
                                                                                                                    //Changing Recoil profile to a different profile entirely
        public bool ChangesRecoilProfile = false;
        public FVRFireArmRecoilProfile NewRecoilProfile;
        public FVRFireArmRecoilProfile NewRecoilProfileStocked;
        private FVRFireArmRecoilProfile StoredRecoilProfile;
        private FVRFireArmRecoilProfile StoredRecoilProfileStocked;
        private bool HasChangedRecoilProfile = false;
        public void AttachRecoilProfileChanger(FVRFireArm gun)
        {
            StoredRecoilProfile = gun.RecoilProfile;
            StoredRecoilProfileStocked = gun.RecoilProfileStocked;
            gun.RecoilProfile = NewRecoilProfile;
            gun.RecoilProfileStocked = NewRecoilProfileStocked;
            HasChangedPoseOverride = true;
        }
        public void DetatchRecoilProfileChanger(FVRFireArm gun)
        {
            gun.RecoilProfile = StoredRecoilProfile;
            gun.RecoilProfileStocked = StoredRecoilProfileStocked;
            HasChangedRecoilProfile = false;
        }
                                                                                                                            //Changing Chambered Round class 


        public bool ChangesChamberRoundClass = false;
        public FireArmRoundClass classToChangeTo;
        public bool HasChangedRoundClass;
        private List<FVRFireArmChamber> roundChangingChambers;
        public void AttachChangeRoundClass(FVRFireArm gun)
        {
            roundChangingChambers = gun.GetChambers();
            HasChangedRoundClass = true;
        }
        public void RoundClassUpdateVoid(FVRFireArm gun)
        {
            foreach (FVRFireArmChamber chm in roundChangingChambers)
            {
                if (chm.m_round != null)
                {
                    chm.m_round.RoundClass = classToChangeTo;
                }
            }
        }
        public void DetatchChangeRoundClass()
        {
            roundChangingChambers = null;
            HasChangedRoundClass = false;
        }
                                                                                                                                //Changing Stock Point

        public Transform NewStockPoint;
        private Transform OldStockPoint;
        public bool ChangesStockPoint = false;
        public bool HasChangedStockPoint = false;
        public bool HadAStock = false;
        public void AttachChangeStockPoint(FVRFireArm gun)
        {
            if (gun.StockPos != null)
            {
                OldStockPoint = gun.StockPos;
                HadAStock = true;
            }
            else
            {
                gun.StockPos = NewStockPoint;
                gun.HasActiveShoulderStock = true;
            }
            HasChangedStockPoint = true;
        }
        public void DetatchChangeStockPoint(FVRFireArm gun)
        {
            if (HadAStock)
            {
                gun.StockPos = OldStockPoint;
                HadAStock = false;
            }else
            {
                gun.StockPos = null;
                gun.HasActiveShoulderStock = false;
            }
            HasChangedStockPoint = false;
        }
                                                                                                                        //Changing Sound Profile

        public bool ChangesSoundProfile;
        public FVRFirearmAudioSet NewSoundProfile;
        private FVRFirearmAudioSet OldSoundProfile;
        public bool HasChangedSoundProfile;
        public void AttachSoundProfileChange(FVRFireArm gun)
        {
            OldSoundProfile = gun.AudioClipSet;
            gun.AudioClipSet = NewSoundProfile;
            HasChangedSoundProfile = true;
        }
        public void DetatchSoundProfileChange(FVRFireArm gun)
        {
            gun.AudioClipSet = OldSoundProfile;
            HasChangedSoundProfile = false;
        }
                                                                                                                         //Muzzle Velocity Changing stuff
        public bool ChangesMuzzleVelocity;
        public float VelocityMultiplier = 1.0f;
        private float changeAmount = 0;
        private bool HasChangedMuzzleVelocity = false;
        public void AttachMuzzleVelocityChanger(FVRFireArm gun)
        {
            List<FVRFireArmChamber> chambers = gun.GetChambers();
            if (chambers[0] != null)
            {
                changeAmount = chambers[0].ChamberVelocityMultiplier * VelocityMultiplier;
            }
            foreach (FVRFireArmChamber chm in chambers)
            {
                chm.ChamberVelocityMultiplier += changeAmount;
            }
        }
        public void DetatchMuzleVelocityChanger(FVRFireArm gun)
        {
            List<FVRFireArmChamber> chambers = gun.GetChambers();
            foreach (FVRFireArmChamber chm in chambers)
            {
                chm.ChamberVelocityMultiplier -= changeAmount;
            }
        }
                                                                                                                 //Muzzle Point stuff

        public bool ChangesMuzzlePoint = false;
        public Transform NewMuzzlePoint;
        public MuzzleEffect[] NewMuzzleEffects;
        private MuzzleDevice tempDevice;
        public bool HasChangedMuzzlePoint = false;
        public void AttachNewMuzzlePoint()
        {
            
           if (Attachment.curMount.GetRootMount().Parent is FVRFireArm theGun)
            {
                theGun.RegisterMuzzleDevice(tempDevice);              
            }
            HasChangedMuzzlePoint = true;
        }
        public void DetatchNewMuzzlePoint(FVRFireArm gun)
        {
            gun.DeRegisterMuzzleDevice(tempDevice);
            HasChangedMuzzlePoint = false;
        }
                                                                                                        //Bolt Speed Stuff

        public float newBoltFoward;
        private float oldBoltForward;
        public float newBoltBack;
        private float oldBoltBack;
        public float newSpringSpeed = 0;
        private float oldSpringSpeed = 0;
        public void AttachBoltSpeedChanger(FVRFireArm gun)
        {
            FVRFireArm fireArm = null;
            fireArm = Attachment.curMount.GetRootMount().MyObject as FVRFireArm;
            switch (fireArm)
            {
                case OpenBoltReceiver w:
                    oldBoltForward = w.Bolt.BoltSpeed_Forward;
                    w.Bolt.BoltSpeed_Forward = newBoltFoward;
                    oldBoltBack = w.Bolt.BoltSpeed_Rearward;
                    w.Bolt.BoltSpeed_Rearward = newBoltBack;
                    oldSpringSpeed = w.Bolt.BoltSpringStiffness;
                    w.Bolt.BoltSpringStiffness = newSpringSpeed;
                    break;
                case ClosedBoltWeapon w:
                    oldBoltForward = w.Bolt.Speed_Forward;
                    w.Bolt.Speed_Forward = newBoltFoward;
                    oldBoltBack = w.Bolt.Speed_Rearward;
                    w.Bolt.Speed_Rearward = newBoltBack;
                    oldSpringSpeed = w.Bolt.SpringStiffness;
                    w.Bolt.SpringStiffness = newSpringSpeed;
                    break;
                case Handgun w:
                    oldBoltBack = w.Slide.Speed_Forward;
                    w.Slide.Speed_Forward = newBoltFoward;
                    oldBoltBack = w.Slide.Speed_Rearward;
                    w.Slide.Speed_Rearward = newBoltBack;
                    oldSpringSpeed = w.Slide.SpringStiffness;
                    w.Slide.SpringStiffness = newSpringSpeed;
                    break;
                case TubeFedShotgun w:
                    oldBoltForward = w.Bolt.Speed_Forward;
                    w.Bolt.Speed_Forward = newBoltFoward;
                    oldBoltBack = w.Bolt.Speed_Rearward;
                    w.Bolt.Speed_Rearward = newBoltBack;
                    oldSpringSpeed = w.Bolt.SpringStiffness;
                    w.Bolt.SpringStiffness = newSpringSpeed;
                    break;

            }

        }
        public void DetatchBoltSpeedChanger(FVRFireArm gun)
        {
            FVRFireArm fireArm = null;
            fireArm = Attachment.curMount.GetRootMount().MyObject as FVRFireArm;
            switch (fireArm)
            {
                case OpenBoltReceiver w:                   
                    w.Bolt.BoltSpeed_Forward = oldBoltForward;                    
                    w.Bolt.BoltSpeed_Rearward = oldBoltBack;                    
                    w.Bolt.BoltSpringStiffness = oldSpringSpeed;
                    break;
                case ClosedBoltWeapon w:
                    
                    w.Bolt.Speed_Forward = oldBoltForward;                    
                    w.Bolt.Speed_Rearward = oldBoltBack;                    
                    w.Bolt.SpringStiffness = oldSpringSpeed;
                    break;
                case Handgun w:
                    
                    w.Slide.Speed_Forward = oldBoltForward;
                    w.Slide.Speed_Rearward = oldBoltBack;                    
                    w.Slide.SpringStiffness = oldSpringSpeed;
                    break;
                case TubeFedShotgun w:
                    w.Bolt.Speed_Forward = oldBoltForward;
                    w.Bolt.Speed_Rearward = oldBoltBack;
                    w.Bolt.SpringStiffness = oldSpringSpeed;
                    break;

            }

        }
                                                                                    // Bolt Handle Stuff
        public bool doesAddPistolBoltHandle = false;
        public PistolSlideDiverter newGrabPoint;
        public GameObject SlideCluster;
        public GameObject unheldMesh;
        public GameObject heldMesh;
        private bool HasChangedBoltHandle = false;
        //public Collider newGripCollider;
        //private Collider[] storedColliderGroup;
        public void AttachPistolBoltHandleChanger()
        {
            Handgun fireArm = null;
            fireArm = Attachment.curMount.GetRootMount().MyObject as Handgun;
            if (fireArm != null)
            {                
                newGrabPoint.POBJ = fireArm.Slide;
                SlideCluster.transform.SetParent(fireArm.Slide.transform);
                HasChangedBoltHandle = true;
            }
        }
        public void DetatchPistolBoltHandleChanger()
        {            
            newGrabPoint.POBJ = null;
            SlideCluster.transform.SetParent(transform);
            HasChangedBoltHandle = false;
        }
                                                                                                                    //ForeGrip Stuff

        public bool doesAddForegrip = false;
        private bool hasAddedForegrip = false;
        public bool doesBracing = true;
        public Transform ForePosePoint;
        public FVRFireArm OverrideFirearm;
        private AttachableForegrip tempGrip;
        public void AttachForegripAdd(FVRFireArm gun)
        {
            tempGrip = new AttachableForegrip();
            tempGrip.DoesBracing = doesBracing;
            OverrideFirearm = gun;
            hasAddedForegrip = true;

        }
        public void DetatchForegripAdd()
        {
            hasAddedForegrip = false;
            OverrideFirearm = null;
            tempGrip = null;
        }
        public void BeginInteractionFromMe(FVRAlternateGrip altGrip, AttributeChangingInterface aGrip, FVRViveHand hand)
        {
            if (aGrip == null)
            {
                altGrip.m_wasGrabbedFromAttachableForegrip = false;
                altGrip.tempFlag = false;
                altGrip.m_lastGrabbedInGrip = (AttachableForegrip)null;
                altGrip.BeginInteraction(hand);
            }
            else
            {
                altGrip.PoseOverride.position = aGrip.ForePosePoint.position;
                altGrip.m_wasGrabbedFromAttachableForegrip = true;
                altGrip.BeginInteraction(hand);
                altGrip.m_lastGrabbedInGrip = tempGrip;
                altGrip.tempFlag = true;
            }
        }
        public virtual void PassHandInput(FVRViveHand hand, FVRInteractiveObject o)
        {
        }
    }



    //#if UNITY_EDITOR
    [CustomEditor(typeof(AttributeChangingInterface))]
    public class AttributeAttachmentEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            //EditorStyles.boldLabel;   
            var script = (AttributeChangingInterface)target;
            script.Attachment = (FVRFireArmAttachment)EditorGUILayout.ObjectField("Attachment", script.Attachment, typeof(FVRFireArmAttachment), true);

            script.AdjustsRecoil = EditorGUILayout.Toggle("Adjusts Recoil", script.AdjustsRecoil);
            script.ChangesPoseOverride = EditorGUILayout.Toggle("Changes Pose Override", script.ChangesPoseOverride);
            script.ChangesRecoilProfile = EditorGUILayout.Toggle("Changes Recoil Profile", script.ChangesRecoilProfile);
            script.ChangesChamberRoundClass = EditorGUILayout.Toggle("Changes Chambered Round Class", script.ChangesChamberRoundClass);
            script.ChangesStockPoint = EditorGUILayout.Toggle("Changes Stock Point", script.ChangesStockPoint);
            script.ChangesSoundProfile = EditorGUILayout.Toggle("Changes Sound Profile", script.ChangesSoundProfile);
            script.ChangesMuzzleVelocity = EditorGUILayout.Toggle("Changes Muzzle Velocity", script.ChangesMuzzleVelocity);
            script.ChangesMuzzlePoint = EditorGUILayout.Toggle("Changes Muzzle Point", script.ChangesMuzzlePoint);
            script.doesAddPistolBoltHandle = EditorGUILayout.Toggle("Adds a pistol Bolt handle", script.doesAddPistolBoltHandle);
            script.doesAddForegrip = EditorGUILayout.Toggle("Adds a Foregrip", script.doesAddForegrip);
            if (script.AdjustsRecoil)
            {            

                EditorGUILayout.PrefixLabel("HorizontalAdjustment");
                script.HorizontalAdjustment = EditorGUILayout.FloatField(script.HorizontalAdjustment);
                EditorGUILayout.PrefixLabel("VerticalAdjustment");
                script.VerticalAdjustment = EditorGUILayout.FloatField(script.VerticalAdjustment);
                EditorGUILayout.PrefixLabel("ZLinearPerShotAdjustment");
                script.ZLinearPerShotAdjustment = EditorGUILayout.FloatField(script.ZLinearPerShotAdjustment);
                EditorGUILayout.PrefixLabel("XYLinearPerShotAdjustment");
                script.XYLinearPerShotAdjustment = EditorGUILayout.FloatField(script.XYLinearPerShotAdjustment);
                EditorGUILayout.PrefixLabel("MaxHorizontalRotationAdjustment");
                script.MaxHorizontalRotationAdjustment = EditorGUILayout.FloatField(script.MaxHorizontalRotationAdjustment);
                EditorGUILayout.PrefixLabel("MaxVerticalRotationAdjustment");
                script.MaxVerticalRotationAdjustment = EditorGUILayout.FloatField(script.MaxVerticalRotationAdjustment);
                EditorGUILayout.PrefixLabel("MaxZLinearMovementAdjustment");
                script.MaxZLinearMovementAdjustment = EditorGUILayout.FloatField(script.MaxZLinearMovementAdjustment);
                EditorGUILayout.PrefixLabel("MaxXYLinearMovementAdjustment");
                script.MaxXYLinearMovementAdjustment = EditorGUILayout.FloatField(script.MaxXYLinearMovementAdjustment);
                EditorGUILayout.PrefixLabel("HorizontalRecoveryAdjustment");
                script.HorizontalRecoveryAdjustment = EditorGUILayout.FloatField(script.HorizontalRecoveryAdjustment);
                EditorGUILayout.PrefixLabel("VerticalRecoveryAdjustment");
                script.VerticalRecoveryAdjustment = EditorGUILayout.FloatField(script.VerticalRecoveryAdjustment);
                EditorGUILayout.PrefixLabel("ZLinearRecoveryAdjustment");
                script.ZLinearRecoveryAdjustment = EditorGUILayout.FloatField(script.ZLinearRecoveryAdjustment);
                EditorGUILayout.PrefixLabel("XYRecoveryAdjustment");
                script.XYRecoveryAdjustment = EditorGUILayout.FloatField(script.XYRecoveryAdjustment);
            }
           
            if (script.ChangesPoseOverride)
            {
                EditorGUILayout.PrefixLabel("Pose Override Changes", EditorStyles.boldLabel, EditorStyles.boldLabel);
                //EditorGUILayout.PrefixLabel("New Pose Override");               
                script.GD = (GripDiverter)EditorGUILayout.ObjectField("New Grab Point", script.GD, typeof(GripDiverter), true);
                script.newRecoilHolder = (Transform)EditorGUILayout.ObjectField("New Recoil Holder LOcation", script.newRecoilHolder, typeof(Transform), true);
                script.newOverride = (Transform)EditorGUILayout.ObjectField("New Pose Override", script.newOverride, typeof(Transform), true);
                script.newOverrideTouch = (Transform)EditorGUILayout.ObjectField("New Pose Override Touch", script.newOverrideTouch, typeof(Transform), true);
                //EditorGUILayout.PrefixLabel("New Pose Override Touch");
            }
            if (script.ChangesRecoilProfile)
            {
                EditorGUILayout.PrefixLabel("Recoil Profile Changes", EditorStyles.boldLabel, EditorStyles.boldLabel);
                script.NewRecoilProfile = (FVRFireArmRecoilProfile)EditorGUILayout.ObjectField("New Recoil Profile", script.NewRecoilProfile, typeof(FVRFireArmRecoilProfile), false);
                script.NewRecoilProfileStocked = (FVRFireArmRecoilProfile)EditorGUILayout.ObjectField("New Recoil Profile Stocked", script.NewRecoilProfileStocked, typeof(FVRFireArmRecoilProfile), false);
            }
            if (script.ChangesChamberRoundClass)
            {
                EditorStyles.textField.wordWrap = true;
                EditorGUILayout.PrefixLabel("Changes Round Class (MAKE SURE THE CALIBER THE GUN USES CAN TAKE THIS HOLY SHIT ITLL BREAK EVERYTHING CURSED WONT WORK DRUMMER BE VERY SCARED)", EditorStyles.boldLabel, EditorStyles.boldLabel);
                script.classToChangeTo = (FireArmRoundClass)EditorGUILayout.EnumPopup(script.classToChangeTo);
            }
            if (script.ChangesStockPoint)
            {                
                EditorGUILayout.PrefixLabel("Stock Point Changes", EditorStyles.boldLabel, EditorStyles.boldLabel);
                script.NewStockPoint = (Transform)EditorGUILayout.ObjectField("new Stock Point", script.NewStockPoint, typeof(Transform), true);
            }
            if (script.ChangesSoundProfile)
            {
                EditorGUILayout.PrefixLabel("Sound Profile Changes", EditorStyles.boldLabel, EditorStyles.boldLabel);
                script.NewSoundProfile = (FVRFirearmAudioSet)EditorGUILayout.ObjectField("New Sound Profile", script.NewSoundProfile, typeof(FVRFirearmAudioSet), false);
            }
            if (script.ChangesMuzzleVelocity)
            {
                EditorGUILayout.PrefixLabel("Muzzle Velocity Multiplier", EditorStyles.boldLabel, EditorStyles.boldLabel);
                script.VelocityMultiplier = EditorGUILayout.FloatField(script.VelocityMultiplier); 
            }
            if (script.ChangesMuzzlePoint)
            {
                EditorGUILayout.PrefixLabel("New Muzzle Point Stuffs", EditorStyles.boldLabel, EditorStyles.boldLabel);
                script.NewMuzzlePoint = (Transform)EditorGUILayout.ObjectField("New Muzzle Point", script.NewMuzzlePoint, typeof(Transform), true);
                //EditorGUILayout.PropertyField(serializedObject.FindProperty("NewMuzzleEffects"));                
            }
            if (script.doesAddPistolBoltHandle)
            {
                EditorGUILayout.PrefixLabel("New Pistol Bolt Handle Stuffs", EditorStyles.boldLabel, EditorStyles.boldLabel);
                //script.newGripCollider = (Collider)EditorGUILayout.ObjectField("New Slide Handle", script.newGripCollider, typeof(Collider), true);
                script.newGrabPoint = (PistolSlideDiverter)EditorGUILayout.ObjectField("New Slide Handle", script.newGrabPoint, typeof(PistolSlideDiverter), true);
                script.SlideCluster = (GameObject)EditorGUILayout.ObjectField("Slide Cluster", script.SlideCluster, typeof(GameObject), true);
                script.heldMesh = (GameObject)EditorGUILayout.ObjectField("Held Mesh", script.heldMesh, typeof(GameObject), true);
                script.unheldMesh = (GameObject)EditorGUILayout.ObjectField("unheld mesh", script.unheldMesh, typeof(GameObject), true);
            }
            if (script.doesAddForegrip)
            {
                EditorGUILayout.PrefixLabel("Foregrip Stuffs", EditorStyles.boldLabel, EditorStyles.boldLabel);
                script.doesBracing = EditorGUILayout.Toggle("Does Foregrip Brace", script.doesBracing);
                script.ForePosePoint = (Transform)EditorGUILayout.ObjectField("Fore Pose Point", script.ForePosePoint, typeof(Transform), true);               

            }

        }

    }
    //#endif
}
