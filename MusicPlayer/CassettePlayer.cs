
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using FistVR;
namespace PuppyScripts.MusicPlayer
{
    public class CassettePlayer : MusicPlayer
    {
        public List<AudioClip> musicClipList = new List<AudioClip>();        
        private int ClipSelection = 0;
        private bool isShuffled = false;
        public GameObject tapeTray;
        private Transform _trayClosedPosition;
        public Transform trayOpenPosition;        
        public float traySpeed = 1;
        private bool _trayClosed = true; //starts closed
        public override void Start()
        {
            base.Start();
            _trayClosedPosition = tapeTray.transform;
        }
        public void nextMusic()
        {
            if (isShuffled)
            {
                ClipSelection = Random.Range(0, CurPS.Songs.Count);
                Speaker.clip = CurPS.Songs[ClipSelection];
                stopMusic();
                startMusic();
            }else if (ClipSelection + 1 <= CurPS.Songs.Count - 1)
            {
                ++ClipSelection;
                Speaker.clip = CurPS.Songs[ClipSelection];
                stopMusic();
                startMusic();
            }
            else if (ClipSelection + 1 > CurPS.Songs.Count - 1)
            {
                ClipSelection = 0;
                Speaker.clip = CurPS.Songs[0];
                stopMusic();
                startMusic();
            }
        }
        public void lastMusic()
        {
            if (ClipSelection == 0)
            {
                Speaker.clip = CurPS.Songs[0];
                stopMusic();
                startMusic();
            } else if (Speaker.time <= 3)
            {
                Speaker.clip = CurPS.Songs[ClipSelection];
                stopMusic();
                startMusic();
            } else if (Speaker.time > 3)
            {
                --ClipSelection;
                Speaker.clip = CurPS.Songs[ClipSelection];
                stopMusic();
                startMusic();
            }

        }
        public void shuffle()
        {
            if (isShuffled)
            {
                isShuffled = false;
            } if (!isShuffled)
            {
                isShuffled = true;
            }
        }
        public override void startMusic()
        {
            if (Speaker.clip != null && !Speaker.isPlaying)
            {
                isPlaying = true;
                if (isShuffled)
                {
                    ClipSelection = Random.Range(0, CurPS.Songs.Count);
                    Speaker.clip = CurPS.Songs[ClipSelection];
                }
                else
                {
                    Speaker.clip = CurPS.Songs[0];
                }
                Speaker.PlayDelayed(.25f);
                isMusicPaused = false;
            }
            
        }
        protected override void DiscInsert(PhysicalSong D)
        {
            D.m_hand.EndInteractionIfHeld(D);
            D.EndInteraction(D.m_hand);
            D.CurPlayer = this;
            CurPS = D;
            Speaker.clip = D.Songs[ClipSelection];
            D.Transform.parent = PhysicalSongPosition;
            D.Transform.localPosition = new Vector3(0, 0, 0);
            D.Transform.localRotation = Quaternion.identity;
            D.RB.isKinematic = true;


        }
      
        public void openTray()
        {
            _trayClosed = false;
            tapeTray.transform.rotation = Quaternion.RotateTowards(tapeTray.transform.rotation, trayOpenPosition.rotation, Time.deltaTime * traySpeed);
            if (CurPS != null)
                CurPS.IsPickUpLocked = false;
        }
        public void closeTray()
        {
            _trayClosed=true;
            tapeTray.transform.rotation = Quaternion.RotateTowards(tapeTray.transform.rotation, _trayClosedPosition.rotation, Time.deltaTime * traySpeed);
            if (CurPS != null) 
                CurPS.IsPickUpLocked = true;
        }
        /*public override void SimpleInteraction(FVRViveHand hand)
        {
            base.SimpleInteraction(hand);      
            if (_trayClosed == false)
            {
                openTray();
            }
        }*/

    }
}
