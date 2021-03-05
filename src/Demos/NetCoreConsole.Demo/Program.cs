using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using WatsonWebsocket;

namespace NetCoreConsole.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            WatsonWsClient client = new WatsonWsClient("localhost", 4999, false);
            client.ServerConnected += ServerConnected;
            client.ServerDisconnected += ServerDisconnected;
            client.MessageReceived += MessageReceived;
            client.Start();

            static void MessageReceived(object sender, MessageReceivedEventArgs args)
            {
                Console.WriteLine("Message from server: " + Encoding.UTF8.GetString(args.Data));
            }

            static void ServerConnected(object sender, EventArgs args)
            {
                Console.WriteLine("Server connected");
            }

            static void ServerDisconnected(object sender, EventArgs args)
            {
                Console.WriteLine("Server disconnected");
            }

            Console.ReadKey();
        }
    }
}