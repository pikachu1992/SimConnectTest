using System;
using System.Collections.Generic;
using System.IO;

namespace SimLib
{
    static class InstallModel
    {
        private static List<string> foldersAndFilesToInstall = new List<string>();

        public static void GetFolderAndFilesPath(string simrootpath, string simObjectsFolder, Dictionary<string, Model> modelToInstall)
        {

            /// <summary>
            /// Returns a model title to install
            /// </summary>
            /// <param name="AircraftCFGTitle">relative Aircraft.cfg Title
            /// </param>
            /// <returns></returns>
            string modelTitle = "Piper Pa-28-180 Cherokee 2";

            //Gets Model and Texture folders path
            foreach (var a in modelToInstall[modelTitle].Folders)
            {
                Console.WriteLine(a.ToString());

                foldersAndFilesToInstall.Add(Path.Combine(simrootpath, "SimObjects", simObjectsFolder, modelToInstall[modelTitle].Name, a.ToString()));
                
            }

            //Gets texture.(model) folder path and (sim).air file path
            foreach (var b in modelToInstall[modelTitle].ConfigSections)
            {
                if(b.Keys["title"] != null && b.Keys["title"] == modelTitle)
                    foldersAndFilesToInstall.Add(Path.Combine(simrootpath, "SimObjects", simObjectsFolder, modelToInstall[modelTitle].Name, "texture." + b.Keys["texture"].ToString()));

                if (b.Keys["title"] != null && b.Keys["title"] == modelTitle)
                    foldersAndFilesToInstall.Add(Path.Combine(simrootpath, "SimObjects", simObjectsFolder, modelToInstall[modelTitle].Name, b.Keys["sim"].ToString() + ".air"));
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

                foldersAndFilesToInstall.Add(modelFile);
            }

        }
    }
}
