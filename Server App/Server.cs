using System;
using System.Net;

namespace IRC_Server_App
{
    class Server
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Server");
            string HostName = Dns.GetHostName();
            string myIP = Dns.GetHostByName(HostName).AddressList[0].ToString();
            Console.WriteLine("The IP for the server hosted at " + HostName + " is: " + myIP);
        }
    }
}
