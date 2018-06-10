using Microsoft.FlightSimulator.SimConnect;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimLib
{
    public partial class SimConnectForm : Form
    {
        private TaskCompletionSource<Position> PositionTask;
        
        public async Task<Position> GetPositionAsync()
        {
            PositionTask = new TaskCompletionSource<Position>();
            SimConnectWrapper.Sim.RequestDataOnSimObjectType(DATA_REQUESTS.Position, DEFINITIONS.Position, 0, SIMCONNECT_SIMOBJECT_TYPE.USER);

            return await PositionTask.Task;
        }

        private void OnRecvPosition(object sender, Position position)
        {
            if (PositionTask != null)
                PositionTask.SetResult(position);
        }
    }
}
