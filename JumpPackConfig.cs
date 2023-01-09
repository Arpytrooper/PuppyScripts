using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BepInEx;
using BepInEx.Configuration;
namespace PuppyScripts
{
    [BepInPlugin("h3vr.arpy.JumpPack", "JumpPackConfig", "1.0.0")]
    public class JumpPackConfig : BaseUnityPlugin
    {
        public static JumpPackConfig instance;
        public ConfigEntry<int> CJumpAmount;
        public ConfigEntry<float> CJumpPower;
        public ConfigEntry<float> CSlideSlowRate;
        public ConfigEntry<float> CVelocityForSlide;
        public ConfigEntry<float> VelocityForWallRun;
        public ConfigEntry<float> CSlopeVelocity;
        public void Awake()
        {
            CJumpAmount = Config.Bind("General",
                "Amount of jumps",
                3,
                "The multiplier for the Height of the head");
            CJumpPower = Config.Bind("General",
                "Jump Power",
                18.0f,
                "How much power each jump has");
            CSlideSlowRate = Config.Bind("General",
                "Slide Slow Rate",
                0.99f,
                "Velocity Multiplier while sliding on a flat surface");
            CVelocityForSlide = Config.Bind("General",
                "Velocity For Slide",
                1f,
                "Velocity in meters per second for a slide to start");
            VelocityForWallRun = Config.Bind("General",
               "Velocity For Wallrun",
               1f,
               "Velocity in meters per second for a Wallrun to start");
            CSlopeVelocity = Config.Bind("General",
                "SlopeVelocity",
                1f,
                "How much do slopes affect your sliding speed");
            instance = this;
        }
    }
}
