using Microsoft.FlightSimulator.SimConnect;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SimLib
{
    public partial class SimConnectForm : Form
    {
        public void RegisterDataDefinitions()
        {
            // register data structures
            simconnect.AddToDataDefinition(
                DEFINITIONS.Radios,
                "TRANSPONDER CODE:1",
                "BCO16",
                SIMCONNECT_DATATYPE.INT32,
                0.0f,
                SimConnect.SIMCONNECT_UNUSED);
            simconnect.RegisterDataDefineStruct<Radios>(DEFINITIONS.Radios);

            simconnect.RequestDataOnSimObjectType(DATA_REQUESTS.Radios, DEFINITIONS.Radios, 0, SIMCONNECT_SIMOBJECT_TYPE.USER);
        }

        private void OnRecvRadios(object sender, Radios radios)
        {
            if (LastRadios.Transponder != radios.Transponder)
            {
                LastRadios.Transponder = radios.Transponder;
                SimConnectTransponderChanged(sender, new TransponderChangedEventArgs() { Transponder = radios.Transponder });
            }
            simconnect.RequestDataOnSimObjectType(DATA_REQUESTS.Radios, DEFINITIONS.Radios, 0, SIMCONNECT_SIMOBJECT_TYPE.USER);
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct Radios
        {
            public int Transponder;
        }

        public Radios LastRadios;

        public class TransponderChangedEventArgs : EventArgs
        {
            public int Transponder
            { get; internal set; }
        }
        public event EventHandler SimConnectTransponderChanged;
    }
}
