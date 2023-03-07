using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using FistVR;
namespace PuppyScripts.MappingTools
{
    public class PlayerTeleport : MonoBehaviour
    {
        public Transform TeleportPoint;


        public void Teleport()
        {
            GM.CurrentMovementManager.TeleportToPoint(TeleportPoint.position, true, TeleportPoint.forward);
        }
    }
}
