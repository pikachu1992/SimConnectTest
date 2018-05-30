using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.FlightSimulator.SimConnect;

namespace SimLib
{
    public partial class SimConnectForm : Form
    {
        public void RegisterPosition()
        {
            try
            {
                simconnect.AddToDataDefinition(DEFINITIONS.Position, "Title", null, SIMCONNECT_DATATYPE.STRING256, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                simconnect.AddToDataDefinition(DEFINITIONS.Position, "Plane Latitude", "degrees", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                simconnect.AddToDataDefinition(DEFINITIONS.Position, "Plane Longitude", "degrees", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                simconnect.AddToDataDefinition(DEFINITIONS.Position, "Plane Altitude", "feet", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);

                simconnect.RegisterDataDefineStruct<Position>(DEFINITIONS.Position);
                simconnect.RequestDataOnSimObjectType(DATA_REQUESTS.Position, DEFINITIONS.Position, 0, SIMCONNECT_SIMOBJECT_TYPE.USER);
            }
            catch (COMException ex)
            {
                throw ex;
            }
        }

        private void OnRecvPosition(object sender, Position position)
        {
            if (PositionChanged(position))
                SimConnectPositionChanged(sender, new PositionChangedEventArgs() { position = position });

            simconnect.RequestDataOnSimObjectType(DATA_REQUESTS.Position, DEFINITIONS.Position, 0, SIMCONNECT_SIMOBJECT_TYPE.USER);
        }

        private bool PositionChanged(Position position)
        {
            // if (position != LastPosition)
                return true;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct Position
        {
            // this is how you declare a fixed size string 
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public String title;
            public double latitude;
            public double longitude;
            public double altitude;
        }

        public Position LastPosition;

        public class PositionChangedEventArgs : EventArgs
        {
            public Position position
            { get; internal set; }
        }
        public event EventHandler SimConnectPositionChanged;
    }
}
