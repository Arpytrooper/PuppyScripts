using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FistVR;
using UnityEngine;

namespace PuppyScripts.MusicPlayer
{
    public class PhysicalSong : FVRPhysicalObject
    {
        public List<AudioClip> Songs;
        public MusicPlayer CurPlayer;
        public Rigidbody RB;


        public override void BeginInteraction(FVRViveHand hand)
        {
            base.BeginInteraction(hand);
            if (CurPlayer != null)
            {
                CurPlayer.DiscExit();
                transform.parent = null;
                RB.isKinematic = false;
                CurPlayer = null;
            }
        }
    }
}
