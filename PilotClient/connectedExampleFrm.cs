using Newtonsoft.Json;
using SimLib;
using System;
using System.Diagnostics;
using WebSocketSharp;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;

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

            OAuthToken = null;
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
            // sim opened, send user to login form
            Process.Start("http://37.59.115.154/html/login.html");
        }

        /// <summary>
        /// Validates a given ASSR code on the Auth API token endpoint, populates OAuthToken when a valid squawk code is set
        /// </summary>
        /// <param name="ASSR"></param>
        private async void ValidateASSR(string ASSR)
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri("https://fa-authapi.herokuapp.com");

            HttpResponseMessage response = await client.GetAsync("/token/" + ASSR);


            if ((int)response.StatusCode == 200)
            {
                // logged in

                // this is the secret to send on the next API requests
                OAuthToken = response.Content.ReadAsStringAsync().Result;

                webSocket = new WebSocket(@"wss://fa-live.herokuapp.com/echo");

                webSocket.OnMessage += Receive;

                webSocket.Connect();

                await Send();
                
            }
            else
            {
                OAuthToken = null; // not sure
            }
        }

        private async Task Send()
        {
            while (webSocket.IsAlive)
            {
                Position payload = await GetPositionAsync();

                AITraffic traffic = await AddAITrafficAsync("TSZ001", 38.76697, -9.143276, 500, "Airbus A321");

                webSocket.Send(JsonConvert.SerializeObject(payload));

                webSocket.Send(JsonConvert.SerializeObject(traffic));

                int millisecondDelay = 1500;
                await Task.Delay(millisecondDelay);
            }
        }

        private void Receive(object sender, MessageEventArgs e)
        {
            Position payload = JsonConvert.DeserializeObject<Position>(e.Data);

            AITraffic traffic = JsonConvert.DeserializeObject<AITraffic>(e.Data);

            displayText(JsonConvert.SerializeObject(payload));

            displayText(JsonConvert.SerializeObject(traffic));
        }

        private void connectedExampleFrm_SimConnectClosed(object sender, EventArgs e)
        {
            if (webSocket != null)
                webSocket.Close();
            displayText("Disconnected from simulator");
        }

        private async void connectedExampleFrm_SimConnectTransponderChanged(object sender, EventArgs e)
        {
            TransponderChangedEventArgs args = (TransponderChangedEventArgs)e;
            if (OAuthToken == null)
            {
                // wait for the user to set on a code
                await Task.Delay(2500);

                if (LastRadios.Transponder == args.Transponder)
                    // validate new squawk codes on the API
                    ValidateASSR(args.Transponder.ToString("X").PadLeft(4, '0'));
            }
        }

        private async void btnGetPositionAsync_Click(object sender, EventArgs e)
        {
            Position p = await GetPositionAsync();
            displayText(JsonConvert.SerializeObject(p));
        }
    }
}
