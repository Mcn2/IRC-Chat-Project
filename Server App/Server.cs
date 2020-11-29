using System;
using System.Net;

namespace Server_App
{
    class Server
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Starting Server");
            string HostName = Dns.GetHostName();
            string myIP = Dns.GetHostEntry(HostName).AddressList[0].ToString();
            Console.WriteLine("The IP for the server hosted at " + HostName + " is: " + myIP);

            ServerNetworkManager Network = new ServerNetworkManager();

            Network.Run();   //never return

            
        }
    }
}
