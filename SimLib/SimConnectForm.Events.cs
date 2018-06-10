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
    }
}
