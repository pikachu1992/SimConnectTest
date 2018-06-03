using Microsoft.FlightSimulator.SimConnect;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SimLib
{
    public partial class SimConnectForm : Form
    {
        public event EventHandler SimConnectEvent;
        
        /// <summary>
        /// Raised when a connection to a SimConnect server is succefully achieved.
        /// </summary>
        public event EventHandler SimConnectOpen;

        /// <summary>
        /// Raised when a connection to SimConnect server is closed by the server.
        /// </summary>
        public event EventHandler SimConnectClosed;

        // Set up all the SimConnect related event handlers 
        private void RegisterEvents()
        {
            try
            {
                // listen to connect and quit msgs 
                simconnect.OnRecvOpen += new SimConnect.RecvOpenEventHandler(SimConnect_OnRecvOpen);
                simconnect.OnRecvQuit += new SimConnect.RecvQuitEventHandler(SimConnect_OnRecvQuit);

                // listen to exceptions 
                simconnect.OnRecvException += new SimConnect.RecvExceptionEventHandler(SimConnect_OnRecvException);

                // listen to events 
                simconnect.OnRecvEvent += new SimConnect.RecvEventEventHandler(SimConnect_OnRecvEvent);
                simconnect.OnRecvSimobjectDataBytype += new SimConnect.RecvSimobjectDataBytypeEventHandler(SimConnect_OnRecvSimobjectDataBytype);

            }
            catch (COMException ex)
            {
                throw ex;
            }
        }

        private void SimConnect_OnRecvOpen(SimConnect sender, SIMCONNECT_RECV_OPEN data)
        {
            SimConnectOpen(this, new EventArgs());
        }

        private void SimConnect_OnRecvException(SimConnect sender, SIMCONNECT_RECV_EXCEPTION data)
        {
            SIMCONNECT_EXCEPTION exception = (SIMCONNECT_EXCEPTION)data.dwException;

            throw new ApplicationException(exception.ToString());
        }

        private void SimConnect_OnRecvQuit(SimConnect sender, SIMCONNECT_RECV data)
        {
            DisposeSimConnect();
            OpenSimConnect();

            SimConnectClosed(this, new EventArgs());
        }

        private void SimConnect_OnRecvEvent(SimConnect sender, SIMCONNECT_RECV_EVENT data)
        {
            SimConnectEvent(this, new EventArgs());
        }
        
        private void SimConnect_OnRecvSimobjectDataBytype(SimConnect sender, SIMCONNECT_RECV_SIMOBJECT_DATA_BYTYPE data)
        {
            try
            {
                switch ((DATA_REQUESTS)data.dwRequestID)
                {
                    case DATA_REQUESTS.Radios:
                        OnRecvRadios(sender, (Radios)data.dwData[0]);
                        break;
                    case DATA_REQUESTS.Position:
                        OnRecvPosition(sender, (Position)data.dwData[0]);
                        break;
                    case DATA_REQUESTS.AiTraffic:
                        OnRecvAITraffic(sender, (AITraffic)data.dwData[0]);
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
    }
}
