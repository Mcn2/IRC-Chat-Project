using System;
using System.Net;

namespace Client_App
{
    class Client
    {
        static void Main(string[] args)
        {
            ClientChatManager Chat = new ClientChatManager();
            Chat.Run();
            
        }

        
    }
}
