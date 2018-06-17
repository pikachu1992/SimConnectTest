using Microsoft.FlightSimulator.SimConnect;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimLib
{
    public static class FSX
    {
        public static SimConnect Sim;

        internal static Dictionary<int, Type> idMap = new Dictionary<int, Type>();

        internal static Dictionary<Type, int> typeMap = new Dictionary<Type, int>();

        public static class Player
        {
            public static string Callsign
            {
                get { return obj.Callsign; }
                set { obj.Callsign = value; }
            }

            private static Aircraft obj = new Aircraft();

            public static async Task<Aircraft> Get()
            {
                await obj.Read();

                return obj;
            }
        }

        public static class Traffic
        {
            private static Dictionary<string, Aircraft> knownTraffic
                = new Dictionary<string, Aircraft>();

            public static void Set(Aircraft traffic)
            {
                if (knownTraffic.ContainsKey(traffic.Callsign))
                    knownTraffic[traffic.Callsign].Update(traffic);
                else
                {
                    knownTraffic.Add(traffic.Callsign, traffic);
                    traffic.Create();
                }
            }
        }

        public static string SimulatorPath
        { get; set; }

        public static void GetSimList(string simPath)
        {
            SimulatorPath = simPath;

            foreach (var directory in Directory.GetDirectories(simPath + "\\SimObjects\\Airplanes"))
            {
                var dir = new DirectoryInfo(directory);
                MySimModels.Add(new MyModelMatching { ModelTitle = dir.Name });
            }

            foreach (var directory in Directory.GetDirectories(simPath + "\\SimObjects\\NETWORK"))
            {
                var dir = new DirectoryInfo(directory);
                MyNetModels.Add(new MyModelMatching { ModelTitle = dir.Name });
            }
        }

        public static List<MyModelMatching> MySimModels = new List<MyModelMatching>();

        public static List<MyModelMatching> MyNetModels = new List<MyModelMatching>();

        public class MyModelMatching
        {
            public string ModelTitle
            { get; set; }
        }

        public class Aircraft
        {
            public string Callsign
            { get; set; }

            public string ModelName
            { get; set; }

            public int ObjectId
            { get; internal set; }

            public AircraftState State
            { get; set; }

            public async void Create()
            {
                ObjectId = await SimObjectType<AircraftState>.
                    AICreateNonATCAircraft(State.title, Callsign, State);

                await VerifyInstalledModel();
            }

            internal async Task<AircraftState> Read()
            {
                State = await SimObjectType<AircraftState>
                    .RequestDataOnSimObjectType();

                return State;
            }

            public async void Update(Aircraft newTraffic)
            {
                State = newTraffic.State;

                await SimObjectType<AircraftState>.
                    SetDataOnSimObject((uint)ObjectId, State);
            }

            public async Task VerifyInstalledModel()
            {
                int trues = 0;

                foreach (var simModels in MySimModels)
                {
                    string[] allFiles = Directory.GetFiles(String.Format("{0}\\SimObjects\\Airplanes\\{1}", SimulatorPath, simModels.ModelTitle), "*.cfg");

                    foreach (string file in allFiles)
                    {
                        string[] lines = File.ReadAllLines(file);
                        string firstOccurrence = lines.FirstOrDefault(l => l.Contains(State.title));
                        if (firstOccurrence != null)
                        {
                            trues = trues + 1;
                        }
                    }
                }

                foreach (var netModels in MyNetModels)
                {
                    string[] allModelFiles = Directory.GetFiles(String.Format("{0}\\SimObjects\\NETWORK\\{1}", SimulatorPath, netModels.ModelTitle), "*.cfg");

                    foreach (string file in allModelFiles)
                    {
                        string[] lines = File.ReadAllLines(file);
                        string firstOccurrence = lines.FirstOrDefault(l => l.Contains(netModels.ModelTitle));
                        if (firstOccurrence != null)
                        {
                            trues = trues + 1;
                        }
                    }
                }

                if (trues == 1)
                    await GetModelMatch(State.title);
             
            }
        }  
        
        public async static Task GetModelMatch(string model)
        { 

            string extractPath = String.Format("{0}\\SimObjects\\NETWORK\\{1}", SimulatorPath, model);

            string zipPath = String.Format("{0}.zip", extractPath);

            using (WebClient Client = new WebClient())
            {

                Uri uri = new Uri(String.Format("https://flyatlantic-va.com/simModels/{0}.zip", model));

                Client.DownloadFileCompleted += Client_DownloadFileCompleted;

                Client.DownloadFileAsync(uri, String.Format("{0}\\SimObjects\\NETWORK\\{1}.zip", SimulatorPath, model));

                void Client_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
                {
                    if (e.Error == null) {
                        
                        ZipFile.ExtractToDirectory(zipPath, extractPath);

                        File.Delete(zipPath);

                        MessageBox.Show(String.Format("{0} is now installed restart your simulator for view new MTL.", model));
                    }
                    else
                    {
                        MessageBox.Show(String.Format("{0} not exists on server and not appears on simulator", model));
                    }
                }              
            }
        }
    }
}
