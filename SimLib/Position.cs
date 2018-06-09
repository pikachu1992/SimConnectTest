using Microsoft.FlightSimulator.SimConnect;
using System;
using System.Runtime.InteropServices;

namespace SimLib
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class Position
    {
        // this is how you declare a fixed size string 
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public String title;
        public double latitude;
        public double longitude;
        public double altitude;
        public double pitch;
        public double bank;
        public double heading;
        public uint airspeed;

        public static void Register(SimConnect sim, Enum defineID)
        {
            try
            {
                sim.AddToDataDefinition(defineID, "Title", null, SIMCONNECT_DATATYPE.STRING256, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                sim.AddToDataDefinition(defineID, "PLANE LATITUDE", "degrees", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                sim.AddToDataDefinition(defineID, "PLANE LONGITUDE", "degrees", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                sim.AddToDataDefinition(defineID, "PLANE ALTITUDE", "feet", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                sim.AddToDataDefinition(defineID, "PLANE PITCH DEGREES", "degrees", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                sim.AddToDataDefinition(defineID, "PLANE BANK DEGREES", "degrees", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                sim.AddToDataDefinition(defineID, "PLANE HEADING DEGREES TRUE", "degrees", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                sim.AddToDataDefinition(defineID, "AIRSPEED TRUE", "knots", SIMCONNECT_DATATYPE.INT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);

                sim.RegisterDataDefineStruct<Position>(defineID);
            }
            catch (COMException ex)
            {
                throw ex;
            }
        }
    }
}
