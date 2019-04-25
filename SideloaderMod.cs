using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using BepInEx.Logging;
using Harmony;
using UnityEngine;
using System.Reflection;

namespace Sideloader
{
    [BepInPlugin(packageURL, "Outward Sideloader", "0.0.1")]
    public class SideloaderMod : BaseUnityPlugin
    {
        public const string packageURL = "com.elec0.outward.sideloader";
        internal new static ManualLogSource Logger;
        public static LoadFiles fileLoader;

        public SideloaderMod()
        {
            Logger = base.Logger;
            
            // Start the file loading
            // This *should not* be async
            fileLoader = new LoadFiles();

            // Now that the file data has been loaded, convert the texture data to actual textures
            StartLoadBundle.ConvertToTexture(fileLoader);
        }

        public void Awake()
        {
            var harmony = HarmonyInstance.Create(packageURL);
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
