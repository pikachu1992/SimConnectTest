using Newtonsoft.Json;
using SimLib;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PilotClient
{

    public partial class connectedExampleFrm : SimConnectForm
    {
        private ClientWebSocket WebSocket;

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

                Uri wsUri = new Uri("wss://echo.websocket.org");

                WebSocket = new ClientWebSocket();
                await WebSocket.ConnectAsync(wsUri, CancellationToken.None);
                await Task.WhenAll(Receive(WebSocket), Send(WebSocket));

                //WebSocket = IO.Socket("http://37.59.115.154:8000/");
                //WebSocket = IO.Socket("http://localhost:8000/");
                //WebSocket = IO.Socket("https://fa-live.herokuapp.com/");

                
            }
            else
            {
                OAuthToken = null; // not sure
            }
        }

        private async Task Send(ClientWebSocket ws)
        {
            while (ws.State == WebSocketState.Open)
            {
                Position payload = await GetPositionAsync();

                byte[] buff = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(payload));

                await ws.SendAsync(new ArraySegment<byte>(buff), WebSocketMessageType.Text, false, CancellationToken.None);

                int millisecondDelay = 1500;
                await Task.Delay(millisecondDelay);
            }
        }

        private async Task Receive(ClientWebSocket ws)
        {
            byte[] buff = new byte[2048];
            while (ws.State == WebSocketState.Open)
            {
                var result = await ws.ReceiveAsync(new ArraySegment<byte>(buff), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                }
                else
                {
                    Position payload = JsonConvert.DeserializeObject<Position>(Encoding.UTF8.GetString(buff).TrimEnd('\0'));

                    displayText(JsonConvert.SerializeObject(payload));
                }
            }
        }

        private async void connectedExampleFrm_SimConnectClosed(object sender, EventArgs e)
        {
            if (WebSocket != null)
                await WebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
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
