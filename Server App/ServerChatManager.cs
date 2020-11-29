using System;
using System.Collections.Generic;
using System.Text;

namespace Server_App
{
    class ServerChatManager
    {
        //Messages consist of MessageType(see table below),UserID#(4 digits),StringMessage(varries by message type)
        //NWID | request new user id, UserName in message, returns user id number to be used in further communication
        //CHLS | request a list of channels, no message needed, returns comma seperated list of channels
        //CRCH | Create a new channel, channel name in message, returns if the channel was created sucussfully
        //JNCH | Join a channel, channel name in message, returns channel chat history
        //LVCH | Leave a channel, channel name in message, returns channel chat history
        //CHCH | Request channel chat history, channel name in message, returns channel chat history
        //MEMB | Requests a list of members in a channel, channel name in message, returns comma seperated list of members
        //PING | Checks if server is responding, no message required, returns "Hello Client!"
        //MECH | Messages a channel, channel name,text in message, returns chat history for channel
        //GBYE | Signals ending of connection, no message required, returns "Bye!"

        public string ParseMessage(string Message)
        {
            string Type = Message.Substring(0, 4);
            Console.WriteLine("Message type " + Type);
            int UserID = int.Parse((Message.Substring(4, 4)).TrimStart('0'));
            string Contents = Message.Substring(8);
            Console.WriteLine("User Id: " + UserID.ToString());
            Console.WriteLine("Message: " + Contents);

            switch (Type){
                case "NWID":
                    Console.WriteLine("New ID");
                    break;
                case "CHLS":
                    Console.WriteLine("List Channels");
                    break;
                case "CRCH":
                    Console.WriteLine("Create Channel");
                    break;
                case "JNCH":
                    Console.WriteLine("Join Channel");
                    break;
                case "LVCH":
                    Console.WriteLine("Leave Channel");
                    break;
                case "CHCH":
                    Console.WriteLine("Channel History");
                    break;
                case "MEMB":
                    Console.WriteLine("Member List");
                    break;
                case "PING":
                    Console.WriteLine("Ping");
                    return "Hello Client!";
                    break;
                case "MECH":
                    Console.WriteLine("Message Channel");
                    break;
                case "GBYE":
                    Console.WriteLine("Disconnect");
                    break;
                default:
                    Console.WriteLine("No Command");
                    break;
            }
            return "";
        }
    }
}
