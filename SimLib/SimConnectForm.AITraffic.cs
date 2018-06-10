using Microsoft.FlightSimulator.SimConnect;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimLib
{
    public partial class SimConnectForm : Form
    {
        private TaskCompletionSource<uint> addTrafficTask;

        public async Task<uint> AddAITrafficAsync(Position position)
        {
            addTrafficTask = new TaskCompletionSource<uint>();

            FSX.Sim.AICreateNonATCAircraft("", position.title, new SIMCONNECT_DATA_INITPOSITION(){
                Latitude = position.latitude,
                Longitude = position.longitude,
                Altitude = position.altitude,
                Pitch = position.pitch,
                Bank = position.bank,
                Heading = position.heading,
                OnGround = 0,
                Airspeed = position.airspeed
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
