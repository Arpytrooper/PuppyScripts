using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FistVR;
using BepInEx;
using BepInEx.Configuration;
using Sodalite.ModPanel;
using Valve.VR;
//using Popcron;
namespace PuppyScripts
{
    //public class plugins : BaseUnityPlugin
    // {


    public class DoubleJumpQB : FVRPhysicalObject
    {
        /*[Header("DEBUG")]
        public bool TurnOnGroundDelay = false;
        public Transform NewNeckPos;
        public Transform NewHeadPose;
        public bool ReplacesNeckPos = false;
        public bool replacesNeckLastPos = false;
        public bool ReplacesHeadPos = false;
        public float NewArmswingerStepHeight = 0.4f;
        public bool multpositions = false;
        public float MultNeckPose;
        public float MultHeadPose;
        public float MultNecklastPose;
        public bool DrawRed = false;
        public bool DrawYellow = false;*/



        public Transform[] BoostEffectPoints;
        //private Transform oldNeckPositionJoinThing;
       // private bool replacesTransform = false;
     //   public Transform NewNeckPosition;
        public Transform BlastDirection;
        public float JumpForce = 18f;
        public bool hasDoubleJumped = false;
        public int jumpAmount = 1;
        public int timesJumped = 0;
        private bool Grounded = false;
        public AudioEvent jumpsound;
        private LayerMask mask;
        private LayerMask resets;
        public float velocityForWallRun = 1.0f;
        public float velocityForSlide = 1.0f;
        private Vector3 lastVelocity;
        private Vector3 Velocity;
        public bool isWallrunning = false;
        public float MultVelBy = 0.99f;
        private bool jumpButtonPressed = false;
        private bool jumpButtonDown = false;
        private bool slidingHeld = false;
        private bool SlidePrepped = false;
        private bool slidingCast = false;
        //public float slideHeight = .75f;
        private double JumpTimeDelay = 0;
        private bool didAllowAirControl;
        public float slopeMultiplier = 1;
        private float SlopeMultiplier;
        private SimulationOptions.GravityMode StoredGravityMode;
        // Use GM.CurrentMovementManager for initialization

        // Update is called once per frame
        public override void Start()
        {
            base.Start();
/*              DELETE THIS BEFORE BUILDING FOR EVERYONE
            jumpAmount = JumpPackConfig.instance.CJumpAmount.Value;
            JumpForce = JumpPackConfig.instance.CJumpPower.Value;
            velocityForWallRun = JumpPackConfig.instance.VelocityForWallRun.Value;
            velocityForSlide = JumpPackConfig.instance.CVelocityForSlide.Value;
            MultVelBy = JumpPackConfig.instance.CSlideSlowRate.Value;
            slopeMultiplier = JumpPackConfig.instance.CSlopeVelocity.Value;
*/
            mask = LayerMask.GetMask("Environment");
            resets = LayerMask.GetMask("NavBlock", "Environment", "TeleValid");
            StoredGravityMode = GM.Options.SimulationOptions.PlayerGravityMode;
          //  oldNeckPositionJoinThing = GM.CurrentPlayerBody.NeckJointTransform;
          //  NewNeckPosition.SetParent(GM.CurrentPlayerBody.NeckJointTransform);
          //  NewNeckPosition.localPosition = new Vector3(0, .9f, 0);
           // NewNeckPosition.localRotation = Quaternion.identity;
          //  NewNeckPosition.localScale = Vector3.zero;
            didAllowAirControl = GM.CurrentSceneSettings.DoesAllowAirControl;

        }
        public void Update()
        {
            if (slidingCast)
                GM.CurrentMovementManager.correctionDir.y = 0;
            /*if (DrawRed)
            {
                Vector3 vector3_3 = GM.CurrentMovementManager.LastNeckPos;
                Vector3 b1 = vector3_3;
                vector3_3.y = Mathf.Max(vector3_3.y, GM.CurrentMovementManager.transform.position.y + GM.CurrentMovementManager.m_armSwingerStepHeight);
                b1.y = GM.CurrentMovementManager.transform.position.y;
                float maxDistance1 = Vector3.Distance(vector3_3, b1);
                Debug.DrawRay(vector3_3, -Vector3.up * maxDistance1, Color.red);
                //Popcron.Gizmos.Line(vector3_3, -Vector3.up * maxDistance1, Color.red);
            }*/




            if (JumpTimeDelay > 0)
            {
                JumpTimeDelay -= Time.deltaTime;
            }
            //Debug.Log("U " + GM.CurrentMovementManager.lastHeadPos.y);
            //Debug.Log("U " + GM.CurrentMovementManager.LastNeckPos.y);

            //Debug.Log(GM.CurrentMovementManager.m_delayGroundCheck);
            //Velocity = VelocitySet(GM.CurrentMovementManager);

            base.FVRUpdate();
            //if (GM.CurrentMovementManager.Mode == FVRMovementManager.MovementMode.Armswinger)
            //{
            //    GM.CurrentMovementManager.m_armSwingerVelocity = new Vector3(Velocity.x, 0, Velocity.z);

            //}
            //else if (GM.CurrentMovementManager.Mode == FVRMovementManager.MovementMode.TwinStick)
            // {
            //    GM.CurrentMovementManager.m_twoAxisVelocity = new Vector3(Velocity.x, 0, Velocity.z);

            //}
            //Movement stuff
            if (m_quickbeltSlot != null)
            {
                GM.CurrentSceneSettings.DoesAllowAirControl = true;
                //storing pressed bool
                if (GM.Options.MovementOptions.AXButtonSnapTurnState == MovementOptions.AXButtonSnapTurnMode.Jump && (GM.CurrentMovementManager.Hands[0].Input.AXButtonPressed || GM.CurrentMovementManager.Hands[1].Input.AXButtonPressed))
                {
                    jumpButtonPressed = true;
                   // Debug.Log("AXButtonpressed");
                }
                else if (GM.Options.MovementOptions.CurrentMovementMode == FVRMovementManager.MovementMode.TwinStick && GM.Options.MovementOptions.TwinStickJumpState == MovementOptions.TwinStickJumpMode.Enabled)
                {
                    if (GM.CurrentMovementManager.Hands[0].CMode == ControlMode.Oculus || GM.CurrentMovementManager.Hands[1].CMode == ControlMode.Oculus)
                    {
                        if (GM.CurrentMovementManager.Hands[0].Input.TouchpadSouthPressed || GM.CurrentMovementManager.Hands[1].Input.TouchpadSouthPressed)
                        {
                            jumpButtonPressed = true;
                            Debug.Log("touchpad south pressed 1");
                        }
                    }
                    else if (GM.CurrentMovementManager.Hands[0].CMode == ControlMode.Vive || GM.CurrentMovementManager.Hands[1].CMode == ControlMode.Vive)
                    {
                        if (GM.Options.MovementOptions.Touchpad_Confirm == FVRMovementManager.TwoAxisMovementConfirm.OnClick)
                        {
                            if (GM.CurrentMovementManager.Hands[0].Input.TouchpadSouthPressed || GM.CurrentMovementManager.Hands[1].Input.TouchpadSouthPressed)
                            {
                                jumpButtonPressed = true;
                                Debug.Log("touchpad south pressed 2");
                            }
                        }
                    }
                    else if (GM.CurrentMovementManager.Hands[0].Input.Secondary2AxisSouthPressed || GM.CurrentMovementManager.Hands[1].Input.Secondary2AxisSouthPressed)
                    { 
                        jumpButtonPressed = true;
                        Debug.Log("secondary 2axis south pressed");
                    }
                }
                else
                {
                    jumpButtonPressed = false;
                }

                //Storing down bool
                if (GM.Options.MovementOptions.AXButtonSnapTurnState == MovementOptions.AXButtonSnapTurnMode.Jump && (GM.CurrentMovementManager.Hands[0].Input.AXButtonDown || GM.CurrentMovementManager.Hands[1].Input.AXButtonDown))
                {
                    jumpButtonDown = true;
                    //Debug.Log("AXButtonDown");
                }
                else if (GM.Options.MovementOptions.CurrentMovementMode == FVRMovementManager.MovementMode.TwinStick && GM.Options.MovementOptions.TwinStickJumpState == MovementOptions.TwinStickJumpMode.Enabled)
                {
                    if (GM.CurrentMovementManager.Hands[0].CMode == ControlMode.Oculus || GM.CurrentMovementManager.Hands[1].CMode == ControlMode.Oculus)
                    {
                        if (GM.CurrentMovementManager.Hands[0].Input.TouchpadSouthDown || GM.CurrentMovementManager.Hands[1].Input.TouchpadSouthDown)
                            jumpButtonDown = true;
                        Debug.Log("touchpad south down 1");
                    }
                    else if (GM.CurrentMovementManager.Hands[0].CMode == ControlMode.Vive || GM.CurrentMovementManager.Hands[1].CMode == ControlMode.Vive)
                    {
                        if (GM.Options.MovementOptions.Touchpad_Confirm == FVRMovementManager.TwoAxisMovementConfirm.OnClick)
                        {
                            if (GM.CurrentMovementManager.Hands[0].Input.TouchpadDown && GM.CurrentMovementManager.Hands[0].Input.TouchpadSouthPressed || GM.CurrentMovementManager.Hands[1].Input.TouchpadDown && GM.CurrentMovementManager.Hands[1].Input.TouchpadSouthPressed)
                                jumpButtonDown = true;
                            Debug.Log("touchpad south down and clicked 1");
                        }
                        else if (GM.CurrentMovementManager.Hands[0].Input.TouchpadSouthDown || GM.CurrentMovementManager.Hands[1].Input.TouchpadSouthDown)
                            jumpButtonDown = true;
                        Debug.Log("touchpad south down 2");
                    }
                    else if (GM.CurrentMovementManager.Hands[0].Input.Secondary2AxisSouthDown || GM.CurrentMovementManager.Hands[1].Input.Secondary2AxisSouthDown)
                       jumpButtonDown = true;
                     Debug.Log("secondary 2axis south down");
                }
                else
                {
                    jumpButtonDown = false;
                }




                //WAll Running code
                if (!Grounded && jumpButtonPressed && ((Mathf.Abs(GM.CurrentMovementManager.m_armSwingerVelocity.x) + Mathf.Abs(GM.CurrentMovementManager.m_armSwingerVelocity.z)) > velocityForWallRun || (Mathf.Abs(GM.CurrentMovementManager.m_twoAxisVelocity.x) + (Mathf.Abs(GM.CurrentMovementManager.m_twoAxisVelocity.z)) > velocityForWallRun)))
                {
                    Vector3 CastPoint = new Vector3(GM.CurrentPlayerBody.Torso.position.x, GM.CurrentPlayerBody.Torso.position.y - .35f, GM.CurrentPlayerBody.Torso.position.z);
                    Transform BodyMid = GM.CurrentPlayerBody.Torso;

                    if (Physics.Raycast(CastPoint, BodyMid.TransformDirection(Vector3.right), .25f, mask) || Physics.Raycast(CastPoint, BodyMid.TransformDirection(Vector3.left), .25f, mask) || Physics.Raycast(CastPoint, BodyMid.TransformDirection(Vector3.forward), .25f, mask) || Physics.Raycast(CastPoint, BodyMid.TransformDirection(Vector3.back), .25f, mask))
                    {
                        if (!isWallrunning) //sets isWallrunning to true and adds a jump if you have none left
                        {

                            isWallrunning = true;
                            GM.Options.SimulationOptions.PlayerGravityMode = SimulationOptions.GravityMode.None;
                        }
                        if (timesJumped == jumpAmount)
                        {
                            --timesJumped;
                        }
                        //Debug.Log("CastHit");
                        if (GM.CurrentMovementManager.m_armSwingerVelocity.y < 1)
                        {
                            GM.CurrentMovementManager.m_armSwingerVelocity.y = 0;
                        }
                        if (GM.CurrentMovementManager.m_twoAxisVelocity.y < 1)
                        {
                            GM.CurrentMovementManager.m_twoAxisVelocity.y = 0;
                        }
                    }
                    else if (isWallrunning)
                    {
                        isWallrunning = false;
                        GM.Options.SimulationOptions.PlayerGravityMode = StoredGravityMode;
                    }
                    //else Debug.Log("Cast Didn't Hit");
                }
                //Extra Jump code
                if (!Grounded && !hasDoubleJumped && jumpButtonDown)
                {

                    GM.CurrentMovementManager.Blast(-BlastDirection.transform.forward, JumpForce);
                    //JumpTimeDelay = .4;
                    if (slidingCast)
                    {
                        JumpSlidingCull();
                    }
                    GM.CurrentMovementManager.DelayGround(.3f);
                    SM.PlayCoreSound(FVRPooledAudioType.Generic, jumpsound, transform.position);
                    ++timesJumped;
                    if (timesJumped >= jumpAmount)
                    {
                        hasDoubleJumped = true; //stops you from jumping if you've used all your jumps
                    }
                }
                if (//!SlidePrepped &&
                    (GM.Options.MovementOptions.CurrentMovementMode == FVRMovementManager.MovementMode.Armswinger && GM.CurrentMovementManager.m_armSwingerGrounded
                    || GM.Options.MovementOptions.CurrentMovementMode == FVRMovementManager.MovementMode.TwinStick && GM.CurrentMovementManager.m_twoAxisGrounded))
                {
                    //Debug.Log("GROUNDED");
                    hasDoubleJumped = false;
                    timesJumped = 0;
                    Grounded = true;
                    //Debug.Log("returning cause Ground");
                    return;
                }
                Grounded = false;
            }
            else if (m_quickbeltSlot == null)
            {
                Grounded = true;
                SlidePrepped = false;
                slidingHeld = false;
                jumpButtonDown = false;
                jumpButtonPressed = false;
                GM.CurrentSceneSettings.DoesAllowAirControl = didAllowAirControl;
            }


        }
        public Vector3 VelocitySet(FVRMovementManager body)
        {
            Vector3 retVel = new Vector3();
            if (body.Mode == FVRMovementManager.MovementMode.Armswinger)
            {
                retVel = body.m_armSwingerVelocity;


            }
            else if (body.Mode == FVRMovementManager.MovementMode.TwinStick)
            {
                retVel = body.m_twoAxisVelocity;

            }
            return retVel;
        }
        public void VelocityDrag(FVRMovementManager body, Vector3 Vel)
        {
            //while (Vel.x > .5 || Vel.x < -.5 || Vel.z > .5 || Vel.z < -.5)
            //{
            Vel *= MultVelBy * SlopeMultiplier;
            Vector3 newVel = new Vector3(Vel.x, 0, Vel.z);
            if (body.Mode == FVRMovementManager.MovementMode.Armswinger)
            {
                body.m_armSwingerVelocity = newVel;

            }
            else if (body.Mode == FVRMovementManager.MovementMode.TwinStick)
            {
                body.m_twoAxisVelocity = newVel;

            }

            //}

        }
        public void LateUpdate()
        {

            /*if (replacesTransform)
            {
                GM.CurrentPlayerBody.NeckJointTransform = NewNeckPosition;
            }
            else
            {
                GM.CurrentPlayerBody.NeckJointTransform = oldNeckPositionJoinThing;
            }
            if (TurnOnGroundDelay)
            {
                GM.CurrentMovementManager.DelayGround(.2f);
            }
            if (ReplacesNeckPos && !replacesNeckLastPos)
            {
                GM.CurrentMovementManager.CurNeckPos = NewNeckPos.position;
            }
            else if (replacesNeckLastPos && !ReplacesNeckPos)
            {
                GM.CurrentMovementManager.LastNeckPos = NewNeckPos.position;
            }
            if (ReplacesHeadPos)
            {
                GM.CurrentMovementManager.lastHeadPos = NewHeadPose.position;
            }
            //GM.CurrentMovementManager.m_armSwingerStepHeight = NewArmswingerStepHeight;

            if (multpositions)
            {
                GM.CurrentMovementManager.lastHeadPos.y *= MultHeadPose;
                GM.CurrentMovementManager.CurNeckPos.y *= MultNeckPose;
                GM.CurrentMovementManager.LastNeckPos.y *= MultNecklastPose;
            }

            if (DrawYellow)
            {
                Vector3 vector3_3 = GM.CurrentMovementManager.LastNeckPos;
                Vector3 b1 = vector3_3;
                vector3_3.y = Mathf.Max(vector3_3.y, GM.CurrentMovementManager.transform.position.y + GM.CurrentMovementManager.m_armSwingerStepHeight);
                b1.y = GM.CurrentMovementManager.transform.position.y;
                float maxDistance1 = Vector3.Distance(vector3_3, b1);
                Debug.DrawRay(vector3_3, -Vector3.up * maxDistance1, Color.yellow);
                //Popcron.Gizmos.Line(vector3_3, -Vector3.up * maxDistance1, Color.yellow);
            }*/



            if (!Grounded && jumpButtonPressed && !jumpButtonDown && !isWallrunning && (Velocity.y <= 0) )//&& JumpTimeDelay <= 0)// && GM.CurrentMovementManager.m_delayGroundCheck > 0)
            {

                SlidePrepped = true;

            }
            else if (SlidePrepped || slidingCast || GM.Options.SimulationOptions.PlayerGravityMode != StoredGravityMode)
            {
                SlidePrepped = false;
                slidingCast = false;
                //GM.CurrentMovementManager.m_delayGroundCheck = 0;
                //GM.CurrentMovementManager.m_armSwingerStepHeight = .4f;
                //GM.CurrentMovementManager.m_twoAxisStepHeight = .4f;
                GM.Options.SimulationOptions.PlayerGravityMode = StoredGravityMode;
                GM.CurrentMovementManager.DelayGround(0f);
                //replacesTransform = false;//DELETE THIS IS DEBUG
            }


            if (SlidePrepped)
            {
                GM.CurrentMovementManager.m_armSwingerGrounded = false;
                GM.CurrentMovementManager.m_twoAxisGrounded = false;
                GM.CurrentMovementManager.DelayGround(0.2f);
                //Debug.Log("Sliding is true now");
                //GM.CurrentMovementManager.DelayGround(.1f);
                //GM.CurrentMovementManager.LM_TeleCast = LayerMask.GetMask();
                if (Mathf.Abs(VelocitySet(GM.CurrentMovementManager).x) + Mathf.Abs(VelocitySet(GM.CurrentMovementManager).z) > velocityForSlide || slidingCast)
                {

                    GM.CurrentMovementManager.DelayGround(0.2f);
                   /* if (Physics.Raycast(GM.CurrentPlayerBody.EyeCam.transform.position, Vector3.down, 2.25f, mask))
                    {
                        
                        //replacesTransform = true;//DELETE THIS IS DEBUG
                        //GM.CurrentMovementManager.m_armSwingerStepHeight = 3f;
                        //GM.CurrentMovementManager.m_twoAxisStepHeight = 3f;
                    }*/
                    //while (Velocity.x > .5 || Velocity.x < -.5 || Velocity.z > .5 || Velocity.z < -.5)
                    if (Physics.Raycast(GM.CurrentPlayerBody.EyeCam.transform.position, Vector3.down, GM.CurrentPlayerBody.EyeCam.transform.localPosition.y + 0.05f, mask))
                    {
                        
                        GM.Options.SimulationOptions.PlayerGravityMode = SimulationOptions.GravityMode.None;
                        slidingCast = true;
                        // Debug.Log("SlidingCastRayCastWorks");
                    }
                    else if (!Physics.Raycast(GM.CurrentPlayerBody.EyeCam.transform.position, Vector3.down, GM.CurrentPlayerBody.EyeCam.transform.localPosition.y + .25f, mask))
                    {
                        slidingCast = false;
                        GM.Options.SimulationOptions.PlayerGravityMode = StoredGravityMode;
                        GM.CurrentMovementManager.DelayGround(.2f);

                    }

                    if (slidingCast)
                    {
                        float HeightCorrection;
                        RaycastHit heightHit;                        
                        if (Physics.Raycast(GM.CurrentPlayerBody.EyeCam.transform.position, Vector3.down, out heightHit, GM.CurrentPlayerBody.EyeCam.transform.position.y + .1f, mask))
                        {
                            HeightCorrection = heightHit.distance - GM.CurrentPlayerBody.EyeCam.transform.localPosition.y;
                            GM.CurrentMovementManager.correctionDir.y -= HeightCorrection;
                            SlopeMultiplier = 1 + ( HeightCorrection * slopeMultiplier);
                        }
                        //RaycastHit rayhit = new RaycastHit();
                        //Physics.Raycast(GM.CurrentPlayerBody.EyeCam.transform.position, Vector3.down, out rayhit, slideHeight, mask);
                        //if (rayhit.distance != slideHeight)
                        //{
                        //    GM.CurrentMovementManager.correctionDir.y = slideHeight - rayhit.distance;
                        //}
                        timesJumped = 0;

                        //GM.CurrentMovementManager.lastHeadPos.y *= -1;
                        //GM.CurrentMovementManager.LastNeckPos.y *= -1;                     

                        //                  NewNeckPosition.position.y =

                        GM.CurrentMovementManager.m_armSwingerVelocity.y = 0;
                        GM.CurrentMovementManager.m_twoAxisVelocity.y = 0;
                        //GM.CurrentMovementManager.m_armSwingerStepHeight = 2f;
                        //GM.CurrentMovementManager.m_twoAxisStepHeight = 2f;
                        Velocity.y = 0;
                        VelocityDrag(GM.CurrentMovementManager, new Vector3(Velocity.x, 0, Velocity.z));
                        /*  {
                          Velocity *= MultVelBy;
                          Vector3 newVel = new Vector3(Velocity.x, 0, Velocity.z);
                          if (GM.CurrentMovementManager.Mode == FVRMovementManager.MovementMode.Armswinger)
                          {
                              GM.CurrentMovementManager.m_armSwingerVelocity = newVel;

                          }
                          else if (GM.CurrentMovementManager.Mode == FVRMovementManager.MovementMode.TwinStick)
                          {
                              GM.CurrentMovementManager.m_twoAxisVelocity = newVel;

                          }

                      }*/
                    }
                }
            }
            /*if (SlidePrepped && !jumpButtonPressed || SlidePrepped && Grounded || SlidePrepped && jumpButtonDown)
            {
                //Debug.Log("Sliding is false now");
                SlidePrepped = false;
                slidingCast = false;
                GM.CurrentMovementManager.m_delayGroundCheck = 0;
                GM.CurrentMovementManager.m_armSwingerStepHeight = .4f;
                GM.CurrentMovementManager.m_twoAxisStepHeight = .4f;
                GM.Options.SimulationOptions.PlayerGravityMode = StoredGravityMode;
                //GM.CurrentMovementManager.LM_TeleCast = resets;
            }*/
            Velocity = VelocitySet(GM.CurrentMovementManager);

        }
        public void SlidingCull()
        {
            SlidePrepped = false;
            slidingCast = false;

            //GM.CurrentMovementManager.m_delayGroundCheck = 0;
            //GM.CurrentMovementManager.lastHeadPos.y = Mathf.Abs(GM.CurrentMovementManager.lastHeadPos.y);
          //  GM.CurrentMovementManager.LastNeckPos.y = Mathf.Abs(GM.CurrentMovementManager.LastNeckPos.y);
            //GM.CurrentMovementManager.m_armSwingerStepHeight = .4f;
            //GM.CurrentMovementManager.m_twoAxisStepHeight = .4f;
            GM.Options.SimulationOptions.PlayerGravityMode = StoredGravityMode;
            //replacesTransform = false;//DELETE THIS IS DEBUG
        }
        public void JumpSlidingCull()
        {
            SlidePrepped = false;
            slidingCast = false;

            //GM.CurrentMovementManager.m_delayGroundCheck = 0;            
            //GM.CurrentMovementManager.LastNeckPos.y = Mathf.Abs(GM.CurrentMovementManager.lastHeadPos.y);
            //GM.CurrentMovementManager.m_armSwingerStepHeight = .4f;
            //GM.CurrentMovementManager.m_twoAxisStepHeight = .4f;
            GM.Options.SimulationOptions.PlayerGravityMode = StoredGravityMode;
        }

    }
    
    }


