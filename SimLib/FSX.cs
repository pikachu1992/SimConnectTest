using Microsoft.FlightSimulator.SimConnect;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
                    AICreateNonATCAircraft(ModelName, Callsign, State);
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
        }
    }
}
