using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PilotClient
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            connectedExampleFrm frm = new connectedExampleFrm();

            frm.OpenSimConnect(); // start trying to connect SimConnect

            Application.Run(frm);
        }
    }
}
