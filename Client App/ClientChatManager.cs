using System;
using System.Collections.Generic;
using System.Text;

namespace Client_App
{
    class ClientChatManager
    {
        public ClientChatManager()
        {
            Network = new ClientNetworkManager();
        }
        public ClientNetworkManager Network;

        public static void Help()
        {
            Console.Write(
                "Command List:\n" +
                "Connect to server         | /Connect\n" +
                "List rooms                | /ListRooms\n" +
                "Connect to room           | /Join\n" +
                "Create new room           | /Create\n" +
                "Leave a room              | /Leave\n" +
                "View joined rooms         | /MyRooms\n" +
                "View a room               | /View\n" +
                "Message Multiple rooms    | /Multi-message\n" +
                "View room members         | /MemberList\n" +
                "Disconnect from a server  | /Disconnect\n" +
                "Show Help (This Menu)     | /Help\n" +
                "Close Program             | /Exit\n" +
                "Lines entered with no command are a message to the currently viewed room\n"
                );
        }

    }
}
