using Microsoft.FlightSimulator.SimConnect;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimLib
{
    public partial class SimConnectForm : Form
    {
        private TaskCompletionSource<AITraffic> AITraffic;

        public void RegisterAircraft()
        {
            try
            {
                simconnect.RequestDataOnSimObjectType(DATA_REQUESTS.AiTraffic, DEFINITIONS.AiTraffic, 0, SIMCONNECT_SIMOBJECT_TYPE.USER);

            }
            catch (COMException ex)
            {
                throw ex;
            }
        }

        public async Task<AITraffic> AddAITrafficAsync(string callsign, double latitude, double longitude, double altitude, string type)
        {
            AITraffic = new TaskCompletionSource<AITraffic>();

            simconnect.AICreateNonATCAircraft(type, callsign, new SIMCONNECT_DATA_INITPOSITION(){
                Latitude = latitude,
                Longitude = longitude,
                Altitude = altitude,
                Pitch = -0,
                Bank = -0,
                Heading = 270,
                OnGround = 1,
                Airspeed = 0
            }, DEFINITIONS.AiTraffic);

            return await AITraffic.Task;
        }

        private void OnRecvAITraffic(object sender, AITraffic traffic)
        {
            if (AITraffic != null)
                AITraffic.SetResult(traffic);
        }
    }
}
