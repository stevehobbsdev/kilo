using System;
using FileServer.Types;
using Kilo.Networking;

namespace FileServer
{
    class Program
    {
        static void Main(string[] args)
        {            
            var listener = new SocketListener();

            listener.MessageReceived += Listener_MessageReceived;

            listener.Listen(4001);

            Console.WriteLine($"Listening on port { listener.EndPoint.Port } (press any key to exit)");
            Console.ReadKey();
        }

        private static void Listener_MessageReceived(object sender, SocketMessageEventArgs args)
        {            
            if (args.Message.MessageTypeId == (int)MessageIds.List)
            {

            }
        }
    }
}
