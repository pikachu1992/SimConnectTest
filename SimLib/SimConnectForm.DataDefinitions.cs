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

            // register the main data event handler
            simconnect.OnRecvSimobjectDataBytype += Simconnect_OnRecvSimobjectDataBytype;
        }

        private void Simconnect_OnRecvSimobjectDataBytype(SimConnect sender, SIMCONNECT_RECV_SIMOBJECT_DATA_BYTYPE data)
        {
            try
            {
                switch ((DATA_REQUESTS)data.dwRequestID)
                {
                    case DATA_REQUESTS.Radios:
                        Radios CurrentRadios = (Radios)data.dwData[0];
                        if (LastRadios.Transponder != CurrentRadios.Transponder)
                            LastRadios.Transponder = CurrentRadios.Transponder;
                        SimConnectTransponderChanged(sender, new TransponderChangedEventArgs() { Transponder = CurrentRadios.Transponder });
                        break;
                    default:
                        break;
                }
            }
            catch (COMException ex)
            {
                throw ex;
            }
        }

        enum DEFINITIONS
        {
            Radios
        }

        enum DATA_REQUESTS
        {
            Radios
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
