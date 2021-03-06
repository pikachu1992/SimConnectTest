﻿using Newtonsoft.Json;
using SimLib;
using System;
using System.Threading.Tasks;
using WebSocketSharp;

namespace PilotClient
{

    public partial class connectedExampleFrm : SimConnectForm
    {
        private WebSocket webSocket;

        private string OAuthToken
        { get; set; }

        // Response number 
        int response = 1;

        // Output text - display a maximum of 10 lines 
        string output = "\n\n\n\n\n\n\n\n\n\n";

        public connectedExampleFrm()
        {
            InitializeComponent();

            FSX.Player.Callsign = "TSZ212";

            if(Properties.Settings.Default.SimulatorPath == "")
                btnConnect.Enabled = false;
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

        private async Task Send()
        {
            while (webSocket.IsAlive)
            {
                FSX.Aircraft player = await FSX.Player.Get();

                string data = JsonConvert.SerializeObject(player);

                webSocket.Send(data);

                int millisecondDelay = 1500;
                await Task.Delay(millisecondDelay);
            }
        }

        private async void Receive(object sender, MessageEventArgs e)
        {
            FSX.Aircraft traffic = JsonConvert.DeserializeObject<FSX.Aircraft>(
                e.Data);

            FSX.Traffic.Set(traffic);
        }

        private void connectedExampleFrm_SimConnectClosed(object sender, EventArgs e)
        {
            if (webSocket != null)
                webSocket.Close();
            displayText("Disconnected from simulator");
        }

        private async void btnGetPositionAsync_Click(object sender, EventArgs e)
        {
            FSX.Aircraft player = await FSX.Player.Get();
            displayText(JsonConvert.SerializeObject(player));
        }

        private async void btnGeXpndrAsync_Click(object sender, EventArgs e)
        {
            Radios r = await SimObjectType<Radios>.RequestDataOnSimObjectType();
            displayText(JsonConvert.SerializeObject(r.Transponder.ToString("X3")));
        }

        private async void btnConnect_Click(object sender, EventArgs e)
        {
            FSX.GetSimList(Properties.Settings.Default.SimulatorPath);

            webSocket = new WebSocket(@"wss://fa-live.herokuapp.com/chat");

            webSocket.OnMessage += Receive;

            webSocket.Connect();

            await Send();
        }

        private void btnSimPath_Click(object sender, EventArgs e)
        {
            getSimulatorPathDialog.ShowDialog();

            Properties.Settings.Default.SimulatorPath = getSimulatorPathDialog.SelectedPath;

            Properties.Settings.Default.Save();

            btnConnect.Enabled = true;
        }
    }
}
