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
                "Connect to server         | /Connect IPAddress\n" +
                "List rooms                | /ListRooms\n" +
                "Connect to room           | /Join RoomName\n" +
                "Create new room           | /Create RoomName\n" +
                "Leave a room              | /Leave RoomName\n" +
                "View joined rooms         | /MyRooms\n" +
                "View a room               | /View Roomname\n" +
                "Message Multiple rooms    | /Multi-message Roomname1, Roomname2, Roomname3; Message\n" +
                "View room members         | /MemberList Roomname\n" +
                "Disconnect from a server  | /Disconnect\n" +
                "Show Help (This Menu)     | /Help\n" +
                "Close Program             | /Exit\n" +
                "Lines entered with no command are a message to the currently viewed room\n"
                );
        }

    }
}
