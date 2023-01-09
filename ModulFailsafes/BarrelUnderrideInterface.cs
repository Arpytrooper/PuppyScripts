using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FistVR;

public class BarrelUnderrideInterface : FVRFireArmAttachmentInterface
{
    private bool didRegister = false;
    public override void OnAttach()
    {
        Attachment.ClearQuickbeltState();
        SM.PlayCoreSoundOverrides(FVRPooledAudioType.GenericClose, Attachment.AudClipAttach, transform.position, new Vector2(0.2f, 0.2f), new Vector2(1f, 1.1f));
        for (int index1 = 0; index1 < SubMounts.Length; ++index1)
        {
            SubMounts[index1].Parent = Attachment.curMount.Parent;
            if (!Attachment.curMount.SubMounts.Contains(SubMounts[index1]))
            {
                Attachment.curMount.SubMounts.Add(SubMounts[index1]);
                for (int index2 = 0; index2 < SubMounts[index1].AttachmentsList.Count; ++index2)
                    Attachment.curMount.Parent.RegisterAttachment(SubMounts[index1].AttachmentsList[index2]);
            }
        }
        if ((Attachment.curMount.GetRootMount().Parent as FVRFireArm).MuzzleDevices.Count <= 0)
        {
            if (Attachment.curMount.GetRootMount().Parent is FVRFireArm && Attachment is MuzzleDevice)
                (Attachment.curMount.GetRootMount().Parent as FVRFireArm).RegisterMuzzleDevice(Attachment as MuzzleDevice);
            didRegister = true;
        }
        else
        {
            didRegister = false;
        }
        Attachment.Sensor.SetTriggerState(false);
    }
    public override void OnDetach()
    {
        if (Attachment.curMount.GetRootMount().Parent is FVRFireArm && Attachment is MuzzleDevice)
            (Attachment.curMount.GetRootMount().Parent as FVRFireArm).DeRegisterMuzzleDevice(Attachment as MuzzleDevice);
        SM.PlayCoreSoundOverrides(FVRPooledAudioType.GenericClose, Attachment.AudClipDettach, transform.position, new Vector2(0.2f, 0.2f), new Vector2(1f, 1.1f));
        if ((Object)Attachment.curMount != (Object)null)
        {
            for (int index1 = 0; index1 < SubMounts.Length; ++index1)
            {
                if ((Object)SubMounts[index1] != (Object)null)
                {
                    SubMounts[index1].Parent = (FVRPhysicalObject)null;
                    if (didRegister)
                    {
                        if (Attachment.curMount.SubMounts.Contains(SubMounts[index1]))
                        {
                            Attachment.curMount.SubMounts.Remove(SubMounts[index1]);
                            for (int index2 = 0; index2 < SubMounts[index1].AttachmentsList.Count; ++index2)
                            {
                                if ((Object)SubMounts[index1].AttachmentsList[index2] != (Object)null)
                                    Attachment.curMount.Parent.DeRegisterAttachment(SubMounts[index1].AttachmentsList[index2]);
                            }
                        }
                        didRegister = false;
                    }
                }
            }
        }
        Attachment.Sensor.SetTriggerState(true);
    }

}
