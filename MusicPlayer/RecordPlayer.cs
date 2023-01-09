using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FistVR;
using UnityEngine;

namespace PuppyScripts.MusicPlayer
{
	public class RecordPlayer : MusicPlayer
	{
		public float DiscRotateSpeed = 1;


		void Update()
		{
			if (isPlaying)
			{
				PhysicalSongPosition.Rotate(0, Time.deltaTime * DiscRotateSpeed, 0);

			}
		}
	}
}
