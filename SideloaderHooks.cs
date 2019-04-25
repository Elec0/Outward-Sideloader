using System.Collections.Generic;
using Harmony;
using UnityEngine;
using BepInEx.Logging;
using System.IO;

namespace Sideloader
{

    [HarmonyPatch(typeof(ResourcesPrefabManager))]
    [HarmonyPatch("Load")]
    public static class StartLoadBundle
    {
        static ManualLogSource Logger = SideloaderMod.Logger;

        // Populated by ConvertToTexture
        private static Dictionary<string, Texture2D> textureData = new Dictionary<string, Texture2D>();

        public static void ConvertToTexture(LoadFiles loadFiles)
        {
            foreach(var img in loadFiles.imageData)
            {
                Texture2D tex = new Texture2D(0, 0);
                tex.LoadImage(img.Value);
                textureData.Add(Path.GetFileNameWithoutExtension(img.Key), tex);
            }
        }

        static void Prefix(ResourcesPrefabManager __instance)
        {
            foreach (UnityEngine.Object o in ResourcesPrefabManager.AllPrefabs)
            {
                // We want to stop iteration as soon as possible, because likely most of the prefabs aren't going to be changed
                if (!(o is GameObject))
                    continue;

                GameObject go = (GameObject)o;
                SkinnedMeshRenderer renderer = go.GetComponent<SkinnedMeshRenderer>();
                if (!renderer || renderer.sharedMaterial == null || renderer.sharedMaterial.mainTexture == null)
                    continue;

                string matName = renderer.sharedMaterial.name;
                string texName = renderer.sharedMaterial.mainTexture.name;

                // Make sure that we actually have the file requested loaded and converted into a texture
                if (!textureData.ContainsKey(texName))
                    continue;

                Logger.LogInfo(string.Format("Patch {0}.{1}", matName, texName));
                renderer.sharedMaterial.mainTexture = textureData[texName];
            }

        }
    }
}
