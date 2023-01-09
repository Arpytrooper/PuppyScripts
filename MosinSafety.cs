using UnityEngine;
using System.Collections;
using FistVR;
using Popcron;
using Gizmos = Popcron.Gizmos;
namespace PuppyScripts

{
    public class MosinSafety : FVRInteractiveObject
    {
        public Transform RotatingGeo;
        public BoltActionRifle Rifle;
        private float SafetyRotDelta;
        public float returnSpeed;
        [Header("Z Distances for the Safety")]
        public float OffSafeZForward;
        public float offSafeHammerCocked;
        public float OnSafeZForward;
        public float ZRearward;
        private GameObject OnSafeforwardVector;
        private GameObject OffSafeforwardVector;
        private GameObject OffSafeHammerCockedVector;
        private GameObject rearwardVector;
        [Header("Rotation for the Safety, assumes that 0 degrees is off safe")]
        public float SafeRotationAngle;
        public bool StartsSafe;
        private bool isSafeAngle;
        [Header("Multiplies rotation by the number")]
        public float RotationDamping = 1;
        private Vector3 lastHandForward = Vector3.zero;
        private Vector3 lastMountForward = Vector3.zero;
        private bool isBack = false;
        private float SafetyRotation;
        private Plane SP;
        //private Collider BoltCollider;
        /*[Header("Debug")]
        public bool isDebugMode = false;
        //public GameObject PlaneVisual;
        public Transform ControllerUp;
        public Transform PointOnPlane;
        public Transform SafetyUp;
        public Transform HandUpAlwaysOn;
        public FVRViveHand hand;
        public Transform LHS1;
        public Transform RHS1;
        public Transform LHS2;
        public Transform RHS2;*/
      

        public override void Start()
        {
            //BoltCollider = Rifle.BoltHandle.GetComponent<Collider>();
            base.Start();
            
            isSafeAngle = StartsSafe;
            OnSafeforwardVector = new GameObject("OnSafeforwardVector");
            OnSafeforwardVector.transform.SetParent(transform.parent, true);
            OnSafeforwardVector.transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, OnSafeZForward);
            OnSafeforwardVector.transform.localEulerAngles = transform.localEulerAngles;
            OffSafeforwardVector = new GameObject("Off Safe Forward Vector");
            OffSafeforwardVector.transform.SetParent(transform.parent, true);
            OffSafeforwardVector.transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, OffSafeZForward);
            OffSafeforwardVector.transform.localEulerAngles = transform.localEulerAngles;
            rearwardVector = new GameObject("Rearward position");
            rearwardVector.transform.SetParent(transform.parent, true);
            rearwardVector.transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, ZRearward);
            rearwardVector.transform.localEulerAngles = transform.localEulerAngles;
            OffSafeHammerCockedVector = new GameObject("Off Safe Hammer Cocked Vector");
            OffSafeHammerCockedVector.transform.SetParent(transform.parent, true);
            OffSafeHammerCockedVector.transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, offSafeHammerCocked);
            OffSafeHammerCockedVector.transform.localEulerAngles = transform.localEulerAngles;

            if (StartsSafe)
            {
                Rifle.m_fireSelectorMode = (int)BoltActionRifle.FireSelectorModeType.Safe;
            } else if (!StartsSafe)
            {
                Rifle.m_fireSelectorMode = (int)BoltActionRifle.FireSelectorModeType.Single;
            }

        }
        public void Update()
        {
            //HandUpAlwaysOn.localPosition = hand.Input.Up;
            if (!Rifle.m_isHammerCocked && transform.localPosition.z >= ((ZRearward-OffSafeZForward)*.75))
            {
                Rifle.m_isHammerCocked = true;
            }
            if (!IsHeld)
            {
                if (isSafeAngle)
                {
                    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, (Mathf.Lerp(rearwardVector.transform.localPosition.z, OnSafeforwardVector.transform.localPosition.z, Time.deltaTime * returnSpeed)));
                }
                else if (!isSafeAngle && Rifle.m_isHammerCocked)
                {
                    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, (Mathf.Lerp(rearwardVector.transform.localPosition.z, OffSafeHammerCockedVector.transform.localPosition.z, Time.deltaTime * returnSpeed)));
                }
                else if (!isSafeAngle && !Rifle.m_isHammerCocked)
                {
                    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, (Mathf.Lerp(rearwardVector.transform.localPosition.z, OffSafeforwardVector.transform.localPosition.z, Time.deltaTime * returnSpeed)));
                }
            }
            
            //if (isSafeAngle)
            //{
              //  Rifle.BoltHandle.DriveBolt(0);
            //}
        }
        
      
        public override void BeginInteraction(FVRViveHand hand)
        {
            
            base.BeginInteraction(hand);
           Rifle.m_fireSelectorMode = (int)BoltActionRifle.FireSelectorModeType.Safe;
            //BoltCollider.enabled = false;
            //lastHandForward = m_hand.Input.Up;
            //lastMountForward = Rifle.transform.forward;
        }
        public override void EndInteraction(FVRViveHand hand)
        {
            if(SafetyRotation > SafeRotationAngle * .75f)
            {
                //transform.localEulerAngles = new Vector3(0.0f, 0.0f, SafeRotationAngle);
                RotatingGeo.transform.localEulerAngles = new Vector3(0.0f, 0.0f, SafeRotationAngle);
                Rifle.m_fireSelectorMode = (int)BoltActionRifle.FireSelectorModeType.Safe;
                isSafeAngle = true;
                Rifle.PlayAudioEvent(FirearmAudioEventType.FireSelector);
            } else if (SafetyRotation <= SafeRotationAngle * .75f)
            {
                //transform.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
                RotatingGeo.transform.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
                Rifle.m_fireSelectorMode = (int)BoltActionRifle.FireSelectorModeType.Single;
                isSafeAngle = false;
                Rifle.PlayAudioEvent(FirearmAudioEventType.FireSelector);
                // BoltCollider.enabled = true;
            }
            isBack = false;
            base.EndInteraction(hand);
        }
        public override void UpdateInteraction(FVRViveHand hand)
        {
            
            base.UpdateInteraction(hand);
            SP = new Plane(transform.up, transform.position);
            if (Rifle.BoltHandle.HandleRot == BoltActionRifle_Handle.BoltActionHandleRot.Down)
            {
                if (SafetyRotation > SafeRotationAngle * .75f)
                {
                    isSafeAngle = true;
                } else if (SafetyRotation <= SafeRotationAngle * .75f)
                {
                    isSafeAngle=false;
                }
                if (isSafeAngle)
                {
                    transform.position = GetClosestValidPoint(OnSafeforwardVector.transform.position, rearwardVector.transform.position, m_handPos);
                    //Mathf.InverseLerp(OnSafeforwardVector.z, rearwardVector.z, transform.position.z);
                }
                else if (!isSafeAngle && !Rifle.m_isHammerCocked)
                {
                    transform.position = GetClosestValidPoint(OffSafeforwardVector.transform.position, rearwardVector.transform.position, m_handPos);
                    //Mathf.InverseLerp(OffSafeforwardVector.z, rearwardVector.z, transform.position.z);
                }
                else if (!isSafeAngle && Rifle.m_isHammerCocked)
                {
                    transform.position = GetClosestValidPoint(OffSafeHammerCockedVector.transform.position, rearwardVector.transform.position, m_handPos);
                    //Mathf.InverseLerp(OffSafeforwardVector.z, rearwardVector.z, transform.position.z);
                }
                if (!isBack && transform.position.z == rearwardVector.transform.position.z)
                {
                    isBack = true;
                   lastHandForward = m_hand.Input.Up;
                   lastMountForward = Rifle.transform.forward;
                    //lastMountForward = testingTransform.up;

                }
                else if (isBack && transform.position.z != rearwardVector.transform.position.z)
                {
                    isBack = false;
                }
                
            }
            



        }
        public void SafetyRotDeltaAdd(float f) => SafetyRotDelta += Mathf.Abs(f);

        public override void FVRFixedUpdate()
        {

            if (isBack && IsHeld)
            {  

                float curRot = SafetyRotation;
                //Vector3 lhs = Vector3.ProjectOnPlane(m_hand.Input.Forward, -transform.forward);
                // Vector3 rhs = Vector3.ProjectOnPlane(lastHandForward, -transform.forward);
                //float num = 0;// = Mathf.Atan2(Vector3.Dot(-transform.forward, Vector3.Cross(lhs, rhs)), Vector3.Dot(lhs, rhs)) * 57.29578f;
                //Vector3 lhs1 = Vector3.ProjectOnPlane(m_hand.Input.Up, -transform.forward);
                Vector3 lhs1 = Vector3.ProjectOnPlane(m_hand.Input.Up, -transform.forward);
                //LHS1.position = lhs1;
                Vector3 rhs1 = Vector3.ProjectOnPlane(lastHandForward, -transform.forward);
               // RHS1.position = rhs1;
                SafetyRotation += Mathf.Atan2(Vector3.Dot(-transform.forward, Vector3.Cross(lhs1, rhs1)), Vector3.Dot(lhs1, rhs1)) * 57.29578f * RotationDamping;
                Vector3 lhs2 = Vector3.ProjectOnPlane(Rifle.transform.up, -transform.forward);
               // LHS2.position = lhs2;
                Vector3 rhs2 = Vector3.ProjectOnPlane(lastMountForward, -transform.forward);               
                //RHS2.position = rhs2;
                SafetyRotation -= Mathf.Atan2(Vector3.Dot(-transform.forward, Vector3.Cross(lhs2, rhs2)), Vector3.Dot(lhs2, rhs2)) * 57.29578f * RotationDamping;
                //if (num > 0.0)
                //  SafetyRotation += num;
               // lastMountForward = testingTransform.up;
                SafetyRotation = Mathf.Clamp(SafetyRotation, 0.0f, SafeRotationAngle);

                //transform.localEulerAngles = new Vector3(0.0f, 0.0f, SafetyRotation);
                RotatingGeo.transform.localEulerAngles = new Vector3(0.0f, 0.0f, SafetyRotation);
                //lastHandForward = m_hand.Input.FilteredUp;
                lastHandForward = m_hand.Input.Up;
                lastMountForward = Rifle.transform.up;

            }
            SP = new Plane(transform.forward, transform.localPosition); //creates the plane that all rotation calculations will occur on
            
            /*if (isBack && IsHeld)
            {

                float PlaneDistance = SP.GetDistanceToPoint(m_hand.Input.Up);
                Vector3 PlanePoint = m_hand.Input.Up;
                PlanePoint = PlanePoint - (SP.normal * PlaneDistance);
                //ControllerUp.position = m_hand.Input.Up;
                //PointOnPlane.position = Vector3.ProjectOnPlane(m_hand.Input.Up, -transform.forward) + Vector3.Dot(transform.localPosition, transform.forward) * -transform.forward;
                //SafetyUp.position = transform.up;
                

                //SafetyRotation = Mathf.Clamp(SafetyRotation, 0.0f, SafeRotationAngle);
                //RotatingGeo.transform.localEulerAngles = new Vector3(0.0f, 0.0f, SafetyRotation);
            }*/
            
            
            base.FVRFixedUpdate();
        }
        static public Vector3 ClosestPointOnPlane(Vector3 planeOffset, Vector3 planeNormal, Vector3 point)
      => point + DistanceFromPlane(planeOffset, planeNormal, point) * planeNormal;
        static public float DistanceFromPlane(Vector3 planeOffset, Vector3 planeNormal, Vector3 point)
      => Vector3.Dot(planeOffset - point, planeNormal);


    }
}
