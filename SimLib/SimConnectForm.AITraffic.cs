using Microsoft.FlightSimulator.SimConnect;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimLib
{
    public partial class SimConnectForm : Form
    {
        private TaskCompletionSource<uint> addTrafficTask;

        public async Task<uint> AddAITrafficAsync(dynamic position)
        {
            addTrafficTask = new TaskCompletionSource<uint>();

            FSX.Sim.AICreateNonATCAircraft("Mooney Bravo Retro", position.Callsign, new SIMCONNECT_DATA_INITPOSITION(){
                Latitude = position.State.latitude,
                Longitude = position.State.longitude,
                Altitude = position.State.altitude,
                Pitch = position.State.pitch,
                Bank = position.State.bank,
                Heading = position.State.heading,
                OnGround = 0,
                Airspeed = position.State.airspeed
            }, (DEFINITIONS)2);

            return await addTrafficTask.Task;
        }

        private void OnRecvAITraffic(object sender, uint trafficId)
        {
            if (addTrafficTask != null)
                addTrafficTask.SetResult(trafficId);
        }
    }
}
