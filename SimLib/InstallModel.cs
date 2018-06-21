using System;
using System.Collections.Generic;
using System.IO;

namespace SimLib
{
    static class InstallModel
    {
        private static List<string> foldersToInstall = new List<string>();

        private static List<string> filesToInstall = new List<string>();

        private static string FolderName { get; set; }

        public static void GetFolderAndFilesPath(string simrootpath, string simObjectsFolder, Dictionary<string, Model> modelToInstall)
        {
            
            /// <summary>
            /// Returns a model title to install
            /// </summary>
            /// <param name="AircraftCFGTitle">relative Aircraft.cfg Title
            /// </param>
            /// <returns></returns>
            string modelTitle = "Piper Pa-28-180 Cherokee 2";

            FolderName = modelToInstall[modelTitle].Name;

            //Gets Model and Texture folders path
            foreach (var folder in modelToInstall[modelTitle].Folders)
            {
                foldersToInstall.Add(folder.ToString());             
            }

            //Gets texture.(model) folder path and (sim).air file path
            foreach (var section in modelToInstall[modelTitle].ConfigSections)
            {
                if(section.Keys["title"] != null && section.Keys["title"] == modelTitle)
                    foldersToInstall.Add("texture." + section.Keys["texture"].ToString());

                if (section.Keys["title"] != null && section.Keys["title"] == modelTitle)
                    filesToInstall.Add(section.Keys["sim"].ToString() + ".air");
            }

            // list all model files in %simrootpath%/SimObjects/simObjectsFolder/simModelName
            string[] modelFiles =
                Directory.GetFiles(
                    Path.Combine(simrootpath, "SimObjects", simObjectsFolder, modelToInstall[modelTitle].Name));

            // Gets aircraft.cfg file path
            foreach (string modelFile in modelFiles)
            {           
                if (modelFile != Path.Combine(simrootpath, "SimObjects", simObjectsFolder, modelToInstall[modelTitle].Name, "aircraft.cfg"))
                    continue;

                filesToInstall.Add("aircraft.cfg");
            }

            GetAndCopyFilesToNetworkFolder(simrootpath, simObjectsFolder, filesToInstall, foldersToInstall);

        }

        private static void GetAndCopyFilesToNetworkFolder(string simrootpath, string simObjectsFolder, List<string> filesToInstall, List<string> foldersToInstall)
        {
            //Create folder with AircraftType
            if(!File.Exists(Path.Combine(simrootpath, "SimObjects", "NETWORK", FolderName)))
                Directory.CreateDirectory(Path.Combine(simrootpath, "SimObjects", "NETWORK", FolderName));

            //Copy main files to Aircraft Type folder
            foreach (var file in filesToInstall)
            {
                if (!File.Exists(Path.Combine(simrootpath, "SimObjects", "NETWORK", FolderName, file.ToString())))
                    File.Copy(Path.Combine(simrootpath, "SimObjects", simObjectsFolder, FolderName, file.ToString()), Path.Combine(simrootpath, "SimObjects", "NETWORK", FolderName, file.ToString()));
            }

            // Copy all texture folders and respective files
            foreach (var folder in foldersToInstall)
            {
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(Path.Combine(simrootpath, "SimObjects", "NETWORK", FolderName, folder));

                foreach(var file in Directory.GetFiles(Path.Combine(simrootpath, "SimObjects", simObjectsFolder, FolderName, folder.ToString())))
                {
                    if (!File.Exists(Path.Combine(simrootpath, "SimObjects", "NETWORK", FolderName, folder.ToString(), Path.GetFileName(file))))
                        File.Copy(Path.Combine(simrootpath, "SimObjects", simObjectsFolder, FolderName, folder.ToString(), Path.GetFileName(file)), Path.Combine(simrootpath, "SimObjects", "NETWORK", FolderName, folder.ToString(), Path.GetFileName(file)));
                }
            }
        }
    }
}
