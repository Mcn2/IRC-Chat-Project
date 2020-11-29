using System;
using System.Net;

namespace Client_App
{
    class Client
    {
        static void Main(string[] args)
        {
            ClientNetworkManager Network = new ClientNetworkManager();
            while (!Network.ReadServerIP());

            Network.Send("Test Message");
        }
    }
}
