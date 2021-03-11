using AirCC.Client;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using WatsonWebsocket;

namespace NetCoreConsole.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            //WatsonWsClient client = new WatsonWsClient("localhost", 4999, false);
            WatsonWsClient client = new WatsonWsClient(new Uri("ws://localhost:4999?token=test&appId=AirCC"));
            client.ServerConnected += ServerConnected;
            client.ServerDisconnected += ServerDisconnected;
            client.MessageReceived += MessageReceived;
            client.Start();

            static void MessageReceived(object sender, MessageReceivedEventArgs args)
            {
                using var ms = new MemoryStream(args.Data);
                BinaryFormatter bf = new BinaryFormatter();
                var settings = bf.Deserialize(ms) as AirCCSettingCollection;
                foreach (var item in settings.AirCCSettings)
                {
                    Console.WriteLine($"{item.Key} : {item.Value}");
                }
                //Console.WriteLine("Message from server: " + Encoding.UTF8.GetString(args.Data));
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