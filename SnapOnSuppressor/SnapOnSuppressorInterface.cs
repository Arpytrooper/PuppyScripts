using FistVR;

namespace PuppyScripts.SnapOnSuppressor
{

    public class SnapOnSuppressorInterface : MuzzleDeviceInterface
    {

        private MuzzleBrake tempBrake;

        public override void Awake()
        {
            base.Awake();
            tempBrake = Attachment as MuzzleBrake;
        }

        public override void OnAttach()
        {
            tempBrake = Attachment as MuzzleBrake;
            if (Attachment.curMount.GetRootMount().Parent is FVRFireArm Firearm)
            {
                Firearm.RegisterMuzzleBrake(tempBrake);
                Firearm.m_isSuppressed = true;
            }
            base.OnAttach();
        }

        public override void OnDetach()
        {
            if (this.Attachment.curMount.GetRootMount().Parent is FVRFireArm Firearm)
            {
                Firearm.RegisterMuzzleBrake((MuzzleBrake)null);

                Firearm.m_isSuppressed = false;

            }

            base.OnDetach();
        }
    }
}
