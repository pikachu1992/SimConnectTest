using SimLib;
using System;

namespace PilotClient
{
    public partial class connectedExampleFrm : SimConnectForm
    {
        // Response number 
        int response = 1;

        // Output text - display a maximum of 10 lines 
        string output = "\n\n\n\n\n\n\n\n\n\n";

        public connectedExampleFrm()
        {
            InitializeComponent();
        }

        void displayText(string s)
        {
            // remove first string from output 
            output = output.Substring(output.IndexOf("\n") + 1);

            // add the new string 
            output += "\n" + response++ + ": " + s;

            // display it 
            txtLog.Text = output;
        }

        private void connectedExampleFrm_SimConnectOpen(object sender, EventArgs e)
        {
            displayText("Connected to simulator");
        }

        private void connectedExampleFrm_SimConnectClosed(object sender, EventArgs e)
        {
            displayText("Disconnected from simulator");
        }

        private void connectedExampleFrm_SimConnectTransponderChanged(object sender, EventArgs e)
        {
            TransponderChangedEventArgs args = (TransponderChangedEventArgs)e;

            displayText(String.Format("Transponder: {0}", args.Transponder.ToString("X").PadLeft(4, '0')));
        }
    }
}
