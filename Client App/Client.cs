using System;
using System.Net;

namespace Client_App
{
    class Client
    {
        static void Main(string[] args)
        {
            ClientChatManager Chat = new ClientChatManager();
            while (!Chat.Network.ReadServerIP()) ;
            string TestMessage;
            
            while (true)
            {
                TestMessage = Console.ReadLine();
                Console.WriteLine("Returned: \n" + Chat.Network.Send(TestMessage));
            }
            
        }

        
    }
}
