using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BepInEx;

namespace PuppyScripts.BepinexPlugins
{

    [BepInPlugin("h3vr.Arpy.BepinexLoaderizer", "PuppyScripts: Loaderizer", "1.0.0")]
    class AdditionalBarrel_BepInEx : BaseUnityPlugin
    {
        public AdditionalBarrel_BepInEx()
        {
            Logger.LogInfo("PuppyScripts Loaded!");
        }
    }

}