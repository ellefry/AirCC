using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatsonWebsocket;

namespace AirCC.Portal.Infrastructure
{
    public class AirCCWsServer : WatsonWsServer
    {
        public static readonly ConcurrentDictionary<string, string> aicCcClients =
            new ConcurrentDictionary<string, string>();

        public AirCCWsServer(string host, int port)
        : base(host, port, false)
        {
            base.ClientConnected += ClientConnected;
            base.ClientDisconnected += ClientDisconnected;
            base.MessageReceived += MessageReceived;
        }

        static void ClientConnected(object sender, ClientConnectedEventArgs args)
        {
            Console.WriteLine("Client connected: " + args.IpPort);
            aicCcClients.TryAdd(Guid.NewGuid().ToString(), args.IpPort);
        }

        static void ClientDisconnected(object sender, ClientDisconnectedEventArgs args)
        {
            Console.WriteLine("Client disconnected: " + args.IpPort);
        }

        static void MessageReceived(object sender, MessageReceivedEventArgs args)
        {
            Console.WriteLine("Message received from " + args.IpPort + ": " + Encoding.UTF8.GetString(args.Data));
            aicCcClients.TryAdd(Guid.NewGuid().ToString(), args.IpPort);
        }

    }
}
