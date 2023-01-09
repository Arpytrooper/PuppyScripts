using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FistVR;
using UnityEngine;


namespace PuppyScripts.MusicPlayer
{
    public class MusicPlayer : FVRInteractiveObject
    {
		
			public AudioClip RecordStartSound;
			protected PhysicalSong CurPS;
			public AudioSource Speaker;
			public AudioSource StartSoundSource;
			public Transform PhysicalSongPosition;
			public bool isMusicPaused = false;
			public bool isPlaying;			
			// Use this for initialization
			public override void Start()
			{
			base.Start();
				StartSoundSource.clip = RecordStartSound;
			}
			public virtual void OnTriggerEnter(Collider other)
			{
				if (CurPS == null)
				{
					PhysicalSong TD = other.GetComponentInParent<PhysicalSong>();
					if (TD != null)
					{
						DiscInsert(TD);
					}
				}
			}

			
			protected virtual void DiscInsert(PhysicalSong D)
			{
				D.m_hand.EndInteractionIfHeld(D);
				D.EndInteraction(D.m_hand);
				D.CurPlayer = this;
				CurPS = D;
				Speaker.clip = D.Songs[0];
				D.Transform.parent = PhysicalSongPosition;
				D.Transform.localPosition = new Vector3(0, 0, 0);
				D.Transform.localRotation = Quaternion.identity;
				D.RB.isKinematic = true;


			}
			public virtual void DiscExit()
			{
				stopMusic();
				Speaker.clip = null;
				CurPS = null;

			}
			public virtual void startMusic()
			{
				if (Speaker.clip != null && !Speaker.isPlaying && !StartSoundSource.isPlaying)
				{
					isPlaying = true;
					StartSoundSource.Play();
					Speaker.PlayDelayed(RecordStartSound.length);
					isMusicPaused = false;
				}
			}
			public virtual void stopMusic()
			{
				if (Speaker.isPlaying)
				{
					StartSoundSource.Stop();
					Speaker.Stop();
					isPlaying = false;
					isMusicPaused = false;
				}
			}
			public virtual void pausePlay()
			{
				if (!isMusicPaused)
				{
					Speaker.Pause();
					isMusicPaused = true;
					isPlaying = false;

				}
				else if (isMusicPaused)
				{
					Speaker.UnPause();
					isMusicPaused = false;
					isPlaying = true;
				}
			}
			public virtual void restartMusic()
			{
				isPlaying = true;
				StartSoundSource.Stop();
				Speaker.Stop();
				StartSoundSource.Play();
				Speaker.PlayDelayed(RecordStartSound.length);
				isMusicPaused = false;
			}

			public virtual void repeatMusic()
			{
				if (Speaker.loop)
				{
					Speaker.loop = false;


				}
				else if (!Speaker.loop)
				{
					Speaker.loop = true;

				}
			}
			public virtual void muteMusic()
			{
				if (!Speaker.mute)
				{
					Speaker.mute = true;
				}
				else if (Speaker.mute)
				{
					Speaker.mute = false;
				}
			}

		
	}

}

