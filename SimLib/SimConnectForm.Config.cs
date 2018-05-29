using System.Windows.Forms;

namespace SimLib
{
    /// <summary>
    /// SimConnectForm configuration settings
    /// </summary>
    public partial class SimConnectForm : Form
    {
        public SimConnectForm()
        {
            SimConnectPoolCooldown = 75;
        }

        /// <summary>
        /// Time between SimConnect pools for position reports, in milliseconds
        /// </summary>
        public int SimConnectPoolCooldown
        { get; set; }
    }
}
