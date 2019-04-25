using System;
using System.Collections.Generic;
using System.IO;

namespace Sideloader
{
    public class LoadFiles
    {
        public string[] directories;

        // List of supported stuff we can sideload
        public string[] SupportedFolders = { ResourceTypes.Texture };
        
        // First dict has category: list of files in category
        public Dictionary<string, List<string>> readData = new Dictionary<string, List<string>>();
        // This has fileName: data of texture files
        public Dictionary<string, byte[]> imageData = new Dictionary<string, byte[]>();

        private string loadDir = Directory.GetCurrentDirectory() + @"\Outward_Data\Resources";

        public LoadFiles()
        {
            Init();
        }

        public LoadFiles(string resourcePath)
        {
            this.loadDir = Directory.GetCurrentDirectory() + @"\" + resourcePath;
            Init();
        }

        private void Init()
        {
            foreach(string curDir in SupportedFolders)
            {
                // Make sure we have the key initialized
                if (!readData.ContainsKey(curDir))
                    readData.Add(curDir, new List<string>());

                string curPath = loadDir + @"\" + curDir;

                if (!Directory.Exists(curPath))
                    continue;

                string[] files = Directory.GetFiles(curPath);

                // Get the names of the files without having to parse stuff
                foreach (string s in files)
                {
                    FileInfo f = new FileInfo(s);
                    Console.WriteLine(f.Name);
                    readData[curDir].Add(f.Name);
                }
            }

            // Now actually read the files
            // We want to do this as early as possible, especially if we switch it to async later
            // becau
            ReadImageData();
        }

        private void ReadImageData()
        {
            var filesToRead = readData[ResourceTypes.Texture];

            foreach(string file in filesToRead)
            {
                // Make sure the file we're trying to read actually exists (it should but who knows)
                string fullPath = loadDir + @"\" + ResourceTypes.Texture + @"\" + file;
                if (!File.Exists(fullPath))
                    continue;

                byte[] data = File.ReadAllBytes(fullPath);
                imageData.Add(file, data);
            }
        }

        // Ignore this for now
        private string[] getDirectoryNames()
        {
            var dirNames = new List<string>();

            foreach (var d in Directory.GetDirectories(loadDir))
            {
                var dir = new DirectoryInfo(d);
                dirNames.Add(dir.Name);
            }
            directories = dirNames.ToArray();
            return directories;
        }
    }

    public static class ResourceTypes
    {
        public const string Texture = "Texture2D";
    }
}
