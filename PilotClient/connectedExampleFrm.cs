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

                webSocket = new WebSocket(@"wss://fa-live.herokuapp.com/chat");

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
                Position payload = await SimObjectType<Position>.GetAsync();

                webSocket.Send(JsonConvert.SerializeObject(payload));

                int millisecondDelay = 1500;
                await Task.Delay(millisecondDelay);
            }
        }

        private async void Receive(object sender, MessageEventArgs e)
        {
            Position payload = JsonConvert.DeserializeObject<Position>(e.Data);

            uint trafficId = await AddAITrafficAsync(payload);

            displayText(trafficId.ToString());
        }

        private void connectedExampleFrm_SimConnectClosed(object sender, EventArgs e)
        {
            if (webSocket != null)
                webSocket.Close();
            displayText("Disconnected from simulator");
        }

        private async void btnGetPositionAsync_Click(object sender, EventArgs e)
        {
            Position p = await SimObjectType<Position>.GetAsync();
            displayText(JsonConvert.SerializeObject(p));
        }

        private async void btnGeXpndrAsync_Click(object sender, EventArgs e)
        {
            Radios r = await SimObjectType<Radios>.GetAsync();
            displayText(JsonConvert.SerializeObject(r));
        }
    }
}
