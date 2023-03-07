using System;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;
using FistVR;
namespace PuppyScripts
{
    public class ShotgunMagazineUnloader : FVRInteractiveObject
    {
        public FVRFireArmMagazine Magazine;
        public TubeFedShotgun Shotgun;
        public Collider ShotgunMagazineCollider;
        public Transform ejectionspot;
        public float Speed_Held = 100;
        private bool isHandInTrigger = false;
        public float ShellLerpSpeedMultiplier = 1000;
        public AudioEvent PushElevatorUp;
        public AudioEvent ElevatorFallsDown;
        //private bool isLerpingShell = false;
        private GameObject LerpingShell;
        private FVRLoadedRound SledingShell;
        private Quaternion targetRotation;
        private Collider PhysColliderForLerpingShell;
        public Transform PopOutPoint;
        public Transform FullOutPoint;
        public FVRInteractiveObject ShellSled;
        private bool isSleding = false;
        private float handZOffset = 0;
        private float handHeldTarget = 0;
        private Vector3 DispBullet1OT;
        //private Vector3 DispBullet2OT;
        private GameObject DB1Parent;
        //private GameObject DB2Parent;
        private bool CarrierisAffected = true;
        public GameObject FullInPoint;
        // private MeshFilter SledMesh;
        // private MeshRenderer SledMat;
        [Header("These Go from 0 to 1")]
        public AnimationCurve DispBulletXRotationOverExtraction;
        public AnimationCurve DispBulletYTravelOverExtraction;
        public bool isTesting = false;
        [Tooltip("This Gameobject works in the unity editor. \n Make sure this is parented to the ShellSled and the local position is 0")]
        public GameObject TestingShell;
        public override void Start()
        {
            base.Start();
            targetRotation = Quaternion.Euler(Shotgun.Carrier.localEulerAngles[0] + Shotgun.CarrierRots[1], Shotgun.Carrier.localEulerAngles[1], Shotgun.Carrier.localEulerAngles[2]);
            DispBullet1OT = Magazine.DisplayBullets[0].transform.localPosition;
            //DispBullet2OT = Magazine.DisplayBullets[1].transform.localPosition;
            DB1Parent = Magazine.DisplayBullets[0].transform.parent.gameObject;
           // DB2Parent = Magazine.DisplayBullets[1].transform.parent.gameObject;
            //FullInPoint = new GameObject("FullInPoint");
            //FullInPoint.transform.SetParent(DB1Parent.transform);
            //FullInPoint.transform.localPosition = DispBullet1OT;
        
            //SledMesh = ShellSled.gameObject.AddComponent<MeshFilter>();
            //SledMat = ShellSled.gameObject.AddComponent<MeshRenderer>();

        }
        public void Update()
        {
            if (isTesting)
            {
                float t = Mathf.InverseLerp(Shotgun.transform.InverseTransformPoint(FullInPoint.transform.position).z, Shotgun.transform.InverseTransformPoint(FullOutPoint.transform.position).z, Shotgun.transform.InverseTransformPoint(ShellSled.transform.position).z);
                TestingShell.transform.localPosition = new Vector3(TestingShell.transform.localPosition.x, DispBulletYTravelOverExtraction.Evaluate(t), TestingShell.transform.localPosition.z);
                TestingShell.transform.localEulerAngles = new Vector3(DispBulletXRotationOverExtraction.Evaluate(t), TestingShell.transform.localEulerAngles.y, TestingShell.transform.localEulerAngles.z);
            
            }
           if (isSleding && ShellSled.IsHeld && ShellSled.transform.position != FullInPoint.transform.position && ShellSled.transform.position != FullOutPoint.position)
            {
                handHeldTarget = Shotgun.transform.InverseTransformPoint(ShellSled.GetClosestValidPoint(FullInPoint.transform.position, FullOutPoint.position, ShellSled.m_hand.Input.Pos + -ShellSled.transform.forward * handZOffset*Shotgun.transform.localScale.x)).z;
                float num = Mathf.MoveTowards(ShellSled.transform.localPosition.z, handHeldTarget, Speed_Held * Time.deltaTime);
                float t = Mathf.InverseLerp(Shotgun.transform.InverseTransformPoint(FullInPoint.transform.position).z, Shotgun.transform.InverseTransformPoint(FullOutPoint.transform.position).z, Shotgun.transform.InverseTransformPoint(ShellSled.transform.position).z);
                if (t != 1)
                {
                    Magazine.DisplayBullets[0].transform.localPosition = new Vector3(Magazine.DisplayBullets[0].transform.localPosition.x, DispBulletYTravelOverExtraction.Evaluate(t), Magazine.DisplayBullets[0].transform.localPosition.z);
                    Magazine.DisplayBullets[0].transform.localEulerAngles = new Vector3(DispBulletXRotationOverExtraction.Evaluate(t), Magazine.DisplayBullets[0].transform.localEulerAngles.y, Magazine.DisplayBullets[0].transform.localEulerAngles.z);
                }
                //this is where we actually move the sled
                ShellSled.transform.localPosition = new Vector3(ShellSled.transform.localPosition.x, ShellSled.transform.localPosition.y, num);
            }else if(isSleding && ShellSled.IsHeld && ShellSled.transform.position == FullInPoint.transform.position)
            {
                ShellSled.EndInteraction(ShellSled.m_hand);
                ResetSled();
            }
            else if (isSleding && ShellSled.IsHeld && ShellSled.transform.position == FullOutPoint.position)
            {
                FVRViveHand hand = ShellSled.m_hand;
                GameObject Jeff = Instantiate(Magazine.RemoveRound(false), Magazine.DisplayBullets[0].transform.position, Shotgun.transform.rotation);
                hand.ForceSetInteractable(Jeff.GetComponent<FVRInteractiveObject>());
                LerpingShell = Jeff;
                ShellSled.EndInteraction(hand);
                ResetSled();
            }


        }
        public void MoveCarrierInResponseToHand()
        {

            //Shotgun.Carrier.localRotation = Quaternion.RotateTowards(transform.localRotation, targetRotation, ShellLerpSpeedMultiplier * Time.deltaTime);
            //Debug.Log("MoveCarrierCalled");
              Shotgun.m_tarCarrierRot = Shotgun.CarrierRots.y;
              Shotgun.m_curCarrierRot = Mathf.MoveTowards(Shotgun.m_curCarrierRot, Shotgun.m_tarCarrierRot, 270f * Time.deltaTime);
              Shotgun.Carrier.localEulerAngles = new Vector3(Shotgun.m_curCarrierRot, 0.0f, 0.0f);
            //if this doesn't then have it set target rotations here
        }
       
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == 9)//layer 9 = handtrigger
            {
                FVRViveHand Temp = other.gameObject.GetComponent<FVRViveHand>();
                if (Temp != null && Temp.CurrentInteractable == null)
                {
                    isHandInTrigger = true;
                    SM.PlayGenericSound(PushElevatorUp, Shotgun.Carrier.transform.position);
                    //Shotgun.m_tarCarrierRot = Shotgun.CarrierRots.y;
                    Shotgun.UsesAnimatedCarrier = false;
                    CarrierisAffected = true;
                }

            }
           /* if (other.gameObject.tag == "FVRFirearmRound")
            {
                if (LerpingShell == null && other.gameObject == LerpingShell) 
                {
                   Collider ammoTrigger = LerpingShell.GetComponent<Collider>();
                    if (ammoTrigger!= null && ammoTrigger.isTrigger)
                    {
                        Physics.IgnoreCollision(ammoTrigger, ShotgunMagazineCollider, true);
                    }
                        foreach (Collider col in Shotgun.m_colliders)
                        {
                            Physics.IgnoreCollision(PhysColliderForLerpingShell, col, true);
                        }
                    
                }
            }*/
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == 9)//layer 9 = handtrigger
            {
                isHandInTrigger = false;
                SM.PlayGenericSound(ElevatorFallsDown, Shotgun.Carrier.transform.position);
                //Shotgun.UsesAnimatedCarrier = true;
            }
            if(other.gameObject.tag == "FVRFireArmRound")
            {
                /*if (LerpingShell != null && other.gameObject == LerpingShell)
                {
                   
                    foreach (Collider col in Shotgun.m_colliders)
                    {
                        Physics.IgnoreCollision(PhysColliderForLerpingShell, col, false);
                    }
                    
                    LerpingShell = null;
                    PhysColliderForLerpingShell = null;
                }*/

                ShotgunMagazineCollider.enabled = true;
                
            }
        }
       
        private void ResetSled() 
        {
            Magazine.DisplayBullets[0].transform.SetParent(DB1Parent.transform);

            Magazine.DisplayBullets[0].transform.localPosition = DispBullet1OT;
            Magazine.DisplayBullets[0].transform.localEulerAngles = Vector3.zero;
            ShellSled.transform.position = PopOutPoint.transform.position;
            isSleding = false;
        }
        public override void SimpleInteraction(FVRViveHand hand)
        {
           if(Magazine.m_numRounds > 0)
            {
                //dont forget to add sounds back in
                //also your colliders are now not gonna be ignored
                //copied this from anton
                if (!isSleding)
                {
                    SM.PlayGenericSound(Shotgun.AudioClipSet.MagazineEjectRound, hand.transform.position);
                    Magazine.DisplayBullets[0].transform.SetParent(ShellSled.transform, true);
                    Magazine.DisplayBullets[0].transform.localPosition = Vector3.zero;
                    //Magazine.DisplayBullets[1].transform.SetParent(ShellSled.transform);
                    ShellSled.transform.position = PopOutPoint.transform.position;
                    handZOffset = ShellSled.transform.InverseTransformPoint(hand.Input.Pos).z;
                    hand.ForceSetInteractable(ShellSled);
                    ShotgunMagazineCollider.enabled = false;                    
                    isSleding = true;
                    //Might need this Magazine.State = FVRFireArmMagazine.MagazineState.Locked;
                } else if (isSleding)
                {
                    handZOffset = ShellSled.transform.InverseTransformPoint(hand.Input.Pos).z;
                    hand.ForceSetInteractable(ShellSled);
                }

                /*
                ShotgunMagazineCollider.enabled = false;

                GameObject JefferyTheShotShell = Instantiate(Magazine.RemoveRound(false), Magazine.DisplayBullets[0].transform.position, Shotgun.transform.rotation);
                //Debug.Log("instantiated the shell");                
                //LerpingShell = JefferyTheShotShell;
                
                hand.ForceSetInteractable(JefferyTheShotShell.GetComponent<FVRInteractiveObject>());
                PhysColliderForLerpingShell = GetPhysCollider(LerpingShell);
                if (LerpingShell == null)
                {
                  
                    foreach (Collider col in Shotgun.m_colliders)
                    {
                        Physics.IgnoreCollision(PhysColliderForLerpingShell, col, true);
                    }

                }
                
                SM.PlayGenericSound(Shotgun.AudioClipSet.MagazineEjectRound, hand.transform.position);
                */
            }

        }
        
        public void LateUpdate()
        {

            if (isHandInTrigger || isSleding)
            {
                MoveCarrierInResponseToHand();
            } if (CarrierisAffected && !isHandInTrigger && !isSleding)
            {
                Shotgun.UsesAnimatedCarrier = true;
                CarrierisAffected = false;
            }
        }
        public Collider GetPhysCollider(GameObject shell)
        {
            Collider[] colArray = LerpingShell.GetComponentsInChildren<Collider>();
            Collider temp = null;
            foreach (Collider col in colArray)
            {
                if (col.gameObject.layer == 0)
                {
                    temp = col;
                }
            }
            return temp;
        }
    }
}
