using Microsoft.FlightSimulator.SimConnect;
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

        public async Task<AITraffic> AddAITrafficAsync()
        {
            AITraffic = new TaskCompletionSource<AITraffic>();
            simconnect.RequestDataOnSimObjectType(DATA_REQUESTS.AiTraffic, DEFINITIONS.AiTraffic, 0, SIMCONNECT_SIMOBJECT_TYPE.USER);

            return await AITraffic.Task;
        }

        private void OnRecvAITraffic(object sender, AITraffic traffic)
        {
            if (AITraffic != null)
                AITraffic.SetResult(traffic);
        }
    }
}
