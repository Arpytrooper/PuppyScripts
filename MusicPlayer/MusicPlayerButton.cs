using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FistVR;
using UnityEngine;
namespace PuppyScripts.MusicPlayer
{
    internal class MusicPlayerButton : FVRInteractiveObject
    {
        public MusicPlayer MP;
        public MPButtonTypes BT;
        public GameObject ThingPositiveObject;
        public GameObject ThingNegativeObject;
        public MeshRenderer ButtonGeo;
        public void Update()
        {
            if (BT == MPButtonTypes.PausePlay)
            {
                if (MP.isMusicPaused && ThingPositiveObject.activeSelf)
                {
                    swapActiveObjects();
                }
                else if (!MP.isMusicPaused && ThingNegativeObject.activeSelf)
                {
                    swapActiveObjects();
                }
            }
            else if (BT == MPButtonTypes.Restart)
            {

            }
            else if (BT == MPButtonTypes.Eject)
            {

            }
            else if (BT == MPButtonTypes.Repeat)
            {

            }
            else if (BT == MPButtonTypes.Start)
            {

            }
            else if (BT == MPButtonTypes.Mute)
            {

            }
        }
        public override void SimpleInteraction(FVRViveHand hand)
        {
            base.SimpleInteraction(hand);
            if (BT == MPButtonTypes.PausePlay)
            {
                MP.pausePlay();
                swapActiveObjects();
            }
            else if (BT == MPButtonTypes.Restart)
            {
                MP.restartMusic();
            }
            else if (BT == MPButtonTypes.Eject)
            {

            }
            else if (BT == MPButtonTypes.Repeat)
            {
                MP.repeatMusic();
                swapActiveObjects();
            }
            else if (BT == MPButtonTypes.Start)
            {
                MP.startMusic();
            }
            else if (BT == MPButtonTypes.Mute)
            {
                MP.muteMusic();
                swapActiveObjects();
            }
        }
        public void swapActiveObjects()
        {
            if (ThingNegativeObject.activeSelf)
            {
                ThingNegativeObject.SetActive(false);
                ThingPositiveObject.SetActive(true);
            }
            else if (ThingPositiveObject.activeSelf)
            {
                ThingNegativeObject.SetActive(true);
                ThingPositiveObject.SetActive(false);
            }
        }

        public enum MPButtonTypes
        {
            Restart,
            PausePlay,
            Eject,
            Repeat,
            Start,
            Mute,
            Shuffle,
        }
    }
}
