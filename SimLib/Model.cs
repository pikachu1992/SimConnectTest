using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using IniParser;
using IniParser.Model;

namespace SimLib
{
    public class Model
    {
        public Model(string model, string simObjectsFolder)
        {
            // generate model absolute path
            string path = Path.Combine(
                simrootpath,
                "SimObjects",
                simObjectsFolder,
                model);

            // load aircraft.cfg
            IniData config = GetConfigData(path);

            // lift all needed info from cfg file
            List<string> folders = new List<string>() { "model", "texture" };
            List<SectionData> configSections = new List<SectionData>();
            List<Texture> textures = new List<Texture>();
            foreach (SectionData section in config.Sections)
            {
                // isolate texture section into Texture class
                if (section.SectionName.StartsWith("fltsim."))
                    textures.Add(new Texture(section));

                if (section.SectionName == "General")
                    Type = section.Keys["atc_model"];

                // all other sections are added on the texture level
                configSections.Add(section);
            }

            Name = model;
            Folders = folders.ToArray();
            ConfigSections = configSections.ToArray();
            Textures = textures.ToArray();
        }

        /// <summary>
        /// Hide constructor
        /// </summary>
        private Model() { }

        /// <summary>
        /// TODO: look this up, somehow
        /// </summary>
        private const string simrootpath =
            @"C:\Microsoft Flight Simulator X";

        /// <summary>
        /// The name of the model folder as installed in
        /// %simrootpath%/SimObjects/%SimObjectFolder%
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// ATC aircraft type
        /// </summary>
        public string Type { get; private set; }

        /// <summary>
        /// All required folder names for the current model
        /// </summary>
        public string[] Folders { get; private set; }

        /// <summary>
        /// All the configurations, except texture information, available on the 
        /// aircraft.cfg file for this model.
        /// </summary>
        public SectionData[] ConfigSections { get; private set; }

        /// <summary>
        /// List of available model textures
        /// </summary>
        public Texture[] Textures { get; private set; }

        public class Texture
        {
            /// <summary>
            /// Reads a given Texture entry from aircraft.cfg
            /// </summary>
            /// <param name="section"></param>
            public Texture(SectionData section)
            {
                List<string> folders = new List<string>();

                // get the texture entry name
                Name = section.Keys["title"];
                folders.Add("texture." + section.Keys["texture"]);

                // get any specific model folders
                if (section.Keys["model"] != "")
                    folders.Add(section.Keys["model"]);

                Folders = folders.ToArray();
                ConfigSection = section;
            }

            /// <summary>
            /// Hide constructor
            /// </summary>
            private Texture() { }

            /// <summary>
            /// The texture name, as its used in SimConnect
            /// </summary>
            public string Name { get; private set; }

            /// <summary>
            /// All model required folders, as specified in the ConfigSection keys
            /// </summary>
            public string[] Folders { get; private set; }

            /// <summary>
            /// The aircraft.cfg section
            /// </summary>
            public SectionData ConfigSection { get; internal set; }
        }
        
        /// <summary>
        /// Loads a given cfg file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static IniData GetConfigData(string path)
        {
            // configure INI file parser
            FileIniDataParser cfgFile = new FileIniDataParser();
            cfgFile.Parser.Configuration.CommentRegex =
                new Regex(@"(//.*$)|(;.*$)|(^(-)+$)");
            cfgFile.Parser.Configuration.AllowDuplicateKeys = true;
            cfgFile.Parser.Configuration.SkipInvalidLines = true;
            cfgFile.Parser.Configuration.AllowDuplicateSections = true;

            // read CFG file, it's just an INI file format
            return cfgFile.ReadFile(
                Path.Combine(path, "aircraft.cfg"));
        }

        /// <summary>
        /// Returns a map of available textures and their corresponding model name
        /// </summary>
        /// <param name="simObjectsFolder">relative path to
        /// %simrootpath%/SimObjects/</param>
        /// <returns></returns>
        public static Dictionary<string, Model> MapModels(string simObjectsFolder)
        {
            Dictionary<string, Model> result = new Dictionary<string, Model>();

            // list all model folders in %simrootpath%/SimObjects/simObjectsFolder
            string[] modelFolders =
                Directory.GetDirectories(
                    Path.Combine(simrootpath, "SimObjects", simObjectsFolder));

            // traverse all models looking for their textures
            foreach (string modelPath in modelFolders)
            {
                if (!File.Exists(Path.Combine(modelPath, "aircraft.cfg")))
                    continue;

                Model model = new Model(
                    Path.GetFileName(modelPath),
                    simObjectsFolder);
                foreach (Texture texture in model.Textures)
                    // Path.GetFileName also returns the last directory name
                    result.Add(texture.Name, model);
            }

            return result;
        }

        private void WriteAircraftCFGFile(IniData data)
        {
            FileIniDataParser parser = new FileIniDataParser();

            parser.WriteFile(Path.Combine(simrootpath, "SimObjects", "NETWORK", Name, "aircraft.cfg"), data);
        }

        private void GetSectionsForNewModelCFG(string modelTitle)
        {
            IniData iniData = new IniData();

            foreach (var section in ConfigSections)
            {
                string[] sectionSplit = section.SectionName.Split('.');

                foreach (var sectionKey in section.Keys)
                {
                    if (sectionKey.Value == modelTitle)
                    {
                        iniData.Sections.Add(section);
                    }
                }

                if (sectionSplit[0] != "fltsim")
                {
                    iniData.Sections.Add(section);
                }
            }

            WriteAircraftCFGFile(iniData);
        }

        private void GetSectionsForExistModelCFG(string modelTitle)
        {
            IniData iniData = new IniData();

            foreach (var section in ConfigSections)
            {
                string[] sectionSplit = section.SectionName.Split('.');

                foreach (var sectionKey in section.Keys)
                {
                    if (sectionKey.Value == modelTitle)
                    {
                        iniData.Sections.Add(section);
                    }
                }

            }

            iniData.Merge(GetConfigData(Path.Combine(simrootpath, "SimObjects", "NETWORK", Name)));

            WriteAircraftCFGFile(iniData);
        }

        private void WriteMainModelFolders()
        {
            if (!Directory.Exists(Path.Combine(simrootpath, "SimObjects", "NETWORK", Name)))
                Directory.CreateDirectory(Path.Combine(simrootpath, "SimObjects", "NETWORK", Name));

            foreach (string folder in Folders)
            {
                if (!Directory.Exists(Path.Combine(simrootpath, "SimObjects", "NETWORK", Name, folder)))
                    Directory.CreateDirectory(Path.Combine(simrootpath, "SimObjects", "NETWORK", Name, folder));

                foreach (var file in Directory.GetFiles(Path.Combine(simrootpath, "SimObjects", "Airplanes", Name, folder)))
                {
                    if (!File.Exists(Path.Combine(simrootpath, "SimObjects", "NETWORK", Name, folder, Path.GetFileName(file))))
                        File.Copy(Path.Combine(simrootpath, "SimObjects", "Airplanes", Name, folder, Path.GetFileName(file)), Path.Combine(simrootpath, "SimObjects", "NETWORK", Name, folder, Path.GetFileName(file)));
                }
            }
        }

        private void GetTextureFolder(string modelTitle)
        {
            foreach (var texture in Textures)
            {
                if (texture.Name == modelTitle)
                    InstallTextureFolder(texture.Folders[0]);               
            }

            void InstallTextureFolder(string texture)
            {
                Directory.CreateDirectory(Path.Combine(simrootpath, "SimObjects", "NETWORK", Name, texture));

                foreach (var file in Directory.GetFiles(Path.Combine(simrootpath, "SimObjects", "Airplanes", Name, texture)))
                {
                    if (!File.Exists(Path.Combine(simrootpath, "SimObjects", "NETWORK", Name, texture, Path.GetFileName(file))))
                        File.Copy(Path.Combine(simrootpath, "SimObjects", "Airplanes", Name, texture, Path.GetFileName(file)), Path.Combine(simrootpath, "SimObjects", "NETWORK", Name, texture, Path.GetFileName(file)));
                }

                InstallAirFile();
            }

            void InstallAirFile()
            {
                foreach (var section in ConfigSections)
                {
                    if (section.Keys["title"] == modelTitle)
                        if (!File.Exists(Path.Combine(simrootpath, "SimObjects", "NETWORK", Name, Path.GetFileName(section.Keys["sim"].ToString() + ".air"))))
                            File.Copy(Path.Combine(simrootpath, "SimObjects", "Airplanes", Name, Path.GetFileName(section.Keys["sim"].ToString() + ".air")), Path.Combine(simrootpath, "SimObjects", "NETWORK", Name, Path.GetFileName(section.Keys["sim"].ToString() + ".air")));
                }
            }
        }

        public void Install(string modelTitle)
        {
            
            WriteMainModelFolders();            

            if (!File.Exists(Path.Combine(simrootpath, "SimObjects", "NETWORK", Name, "aircraft.cfg")))
                GetSectionsForNewModelCFG(modelTitle);
            else
                GetSectionsForExistModelCFG(modelTitle);

            GetTextureFolder(modelTitle);

        }
    }
}
