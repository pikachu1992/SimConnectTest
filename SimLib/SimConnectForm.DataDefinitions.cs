using Microsoft.FlightSimulator.SimConnect;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SimLib
{
    public partial class SimConnectForm : Form
    {
        enum DEFINITIONS
        {
            Radios,
            Position,
            AiTraffic
        }

        public void RegisterDataDefinitions()
        {
            SimObjectType<Position>.Register(new SimObjectType<Position>.Field[]
            {
                new SimObjectType<Position>.Field()
                { DatumName = "Title", UnitsName = null,
                    DatumType = SIMCONNECT_DATATYPE.STRING256 },
                new SimObjectType<Position>.Field()
                { DatumName = "PLANE LATITUDE", UnitsName = "degrees",
                    DatumType = SIMCONNECT_DATATYPE.FLOAT64 },
                new SimObjectType<Position>.Field()
                { DatumName = "PLANE LONGITUDE", UnitsName = "degrees",
                    DatumType = SIMCONNECT_DATATYPE.FLOAT64 },
                new SimObjectType<Position>.Field()
                { DatumName = "PLANE ALTITUDE", UnitsName = "feet",
                    DatumType = SIMCONNECT_DATATYPE.FLOAT64 },
                new SimObjectType<Position>.Field()
                { DatumName = "PLANE PITCH DEGREES", UnitsName = "degrees",
                    DatumType = SIMCONNECT_DATATYPE.FLOAT64 },
                new SimObjectType<Position>.Field()
                { DatumName = "PLANE BANK DEGREES", UnitsName = "degrees",
                    DatumType = SIMCONNECT_DATATYPE.FLOAT64 },
                new SimObjectType<Position>.Field()
                { DatumName = "PLANE HEADING DEGREES TRUE", UnitsName = "degrees",
                    DatumType = SIMCONNECT_DATATYPE.FLOAT64 },
                new SimObjectType<Position>.Field()
                { DatumName = "AIRSPEED TRUE", UnitsName = "knots",
                    DatumType = SIMCONNECT_DATATYPE.FLOAT64 },
            });

            SimObjectType<Radios>.Register(new SimObjectType<Radios>.Field[]
            {
                new SimObjectType<Radios>.Field()
                { DatumName = "TRANSPONDER CODE:1", UnitsName = "BCO16",
                    DatumType = SIMCONNECT_DATATYPE.INT32 },
            });
        }
    }
}
