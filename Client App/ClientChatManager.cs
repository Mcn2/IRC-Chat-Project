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
            running = true;
            UserChannels = new List<string>();
            Connected = false;
        }
        public ClientNetworkManager Network;
        private bool running;
        private string Alias;
        private int UserID;
        List<String> UserChannels;
        private bool Connected;
        private String CurrentChannel;

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
                "Disconnect from server    | /Disconnect\n" +
                "Show Help (This Menu)     | /Help\n" +
                "Close Program             | /Exit\n" +
                "Lines entered with no command are a message to the currently viewed room\n"
                );
        }




        public void Run()
        {
            Console.Write(
                "Welcome to the chat client\n" +
                "Please connect to a server to start\n"
                );
            Help();
            
            String UserInput;

            while (running)
            {
                UserInput = Console.ReadLine();
                
                if(UserInput[0] == '/')
                {
                    switch (UserInput)
                    {
                        case "/Connect":
                            Connect();
                            break;
                        case "/ListRooms":
                            ListRooms();
                            break;
                        case "/Join":
                            Console.Clear();
                            Console.Write("Enter the room you want to join: ");
                            JoinRoom(Console.ReadLine().Trim());
                            break;
                        case "/Create":
                            Console.Clear();
                            Console.Write("Enter the name of the new room: ");
                            CreateRoom(Console.ReadLine().Trim());
                            break;
                        case "/Leave":
                            Console.Clear();
                            Console.Write("Enter the name of the room you want to leave: ");
                            LeaveRoom(Console.ReadLine().Trim());
                            break;
                        case "/MyRooms":
                            ViewMyRooms();
                            break;
                        case "/View":
                            Console.Clear();
                            Console.Write("Enter the name of the channel you want to view: ");
                            ViewRoom(Console.ReadLine().Trim());
                            break;
                        case "/Multi-message":
                            Console.Clear();
                            MultiMessage();
                            break;
                        case "/MemberList":
                            Console.Clear();
                            Console.Write("Enter the name of the room you want to view the members of: ");
                            ViewMembers(Console.ReadLine().Trim());
                            break;
                        case "/Disconnect":
                            Disconnect();
                            break;
                        case "/Help":
                            Help();
                            break;
                        case "/Exit":
                            Exit();
                            break;
                        default:
                            Console.WriteLine("Command not recognized. You can check the list of commands with \"/Help\"");
                            break;
                    }
                }
                else
                {
                    if(CurrentChannel == null || CurrentChannel == "")
                    {
                        Console.WriteLine("Please view a channel before sending a message");
                    }
                    else
                    {
                        MessageChannel(CurrentChannel, UserInput);
                    }
                }
            }
        }

        private String IDString()
        {
            if(UserID == 0)
            {
                return "0000";
            }
            if(UserID < 10)
            {
                return "000" + UserID.ToString();
            }
            if(UserID < 100)
            {
                return "00" + UserID.ToString();
            }
            if(UserID < 1000)
            {
                return "0" + UserID.ToString();
            }
            return UserID.ToString();
        }

        private bool Connect()
        {
            if (Connected)
            {
                Console.WriteLine("You are already connected to a server. Please disconnect from the current server before connecting to a new server");
                return false;
            }
            while (!Network.ReadServerIP()) ;
            Console.Write("\nEnter desired alias: ");
            Alias = Console.ReadLine();
            String message = "NWID" + IDString() + Alias;
            String responce = Network.Send(message);
            try
            {
                UserID = int.Parse((message.TrimEnd(new char[] { (char)0 })).TrimStart('0'));
            }
            catch
            {
                Alias = null;
                Console.WriteLine("Error parsing User ID number, user disconnected from server");
                Disconnect();
                return false;
            }
            Console.WriteLine("User " + Alias + " is connected to the server");
            return true;
        }

        private bool Disconnect()
        {
            String Message;
            String Responce;
            Message = "GBYE" + IDString();
            Responce = Network.Send(Message);
            if(Responce != "Bye!")
            {
                Console.WriteLine("Server Responce Mismatch");
            }
            UserID = 0;
            Alias = null;
            UserChannels = new List<string>();
            Connected = false;
            Console.WriteLine("You are now disconnected");
            return true;
        }

        private bool Exit()
        {
            if (Connected)
            {
                Disconnect();
            }
            running = false;
            return true;
        }

        private void ListRooms()
        {
            if (!Connected)
            {
                Console.WriteLine("Please conect to a server before listing rooms");
                return;
            }
            String Message;
            String Responce;

            Message = "CHLS" + IDString();
            Responce = Network.Send(Message);
            String[] Channels = Responce.Split(',');
            Console.Clear();
            Console.WriteLine("Room List:");
            foreach(string ChannelName in Channels)
            {
                Console.WriteLine(ChannelName);
            }
            return;
        }

        private bool JoinRoom(String RoomName)
        {
            if (!Connected)
            {
                Console.WriteLine("Please Connect to a server before joining a room");
                return false;
            }
            String Message;
            String Responce;
            Message = "JNCH" + IDString() + RoomName;
            Responce = Network.Send(Message);
            if(Responce == "Channel Does Not Exist")
            {
                Console.WriteLine("Room Does not exist");
                return false;
            }
            UserChannels.Add(RoomName);
            Console.Clear();
            Console.Write(Responce);
            return true;
        }

        private bool CreateRoom(String RoomName)
        {
            if (!Connected)
            {
                Console.WriteLine("You are not connected to a server");
                return false;
            }

            String Message;
            String Responce;

            Message = "CRCH" + IDString() + RoomName;
            Responce = Network.Send(Message);
            if(Responce == "Already Exists")
            {
                Console.WriteLine("That room name is already being used!");
                return false;
            }
            Console.WriteLine("Room Named " + RoomName + " created! Please join the channel!");
            return true;
        }

        private bool LeaveRoom(String RoomName)
        {
            if(!Connected)
            {
                Console.WriteLine("Please connect to a server before leaving a room");
                return false;
            }
            if (!UserChannels.Contains(RoomName))
            {
                Console.WriteLine("You are not a member of a channel named " + RoomName);
                return false;
            }
            String Message;
            String Responce;
            Message = "LVCH" + IDString() + RoomName;
            Responce = Network.Send(Message);
            if(Responce == "Channel Does Not Exist")
            {
                Console.WriteLine("Channel Does not Exist");
                return false;
            }
            Console.WriteLine("User is no longer in the room named " + RoomName);
            return true;
        }

        private bool ViewMyRooms()
        {
            if (!Connected)
            {
                Console.WriteLine("Please Connect to a server before viewing connected rooms");
                return false;
            }
            if(UserChannels.Count == 0)
            {
                Console.WriteLine("User is not in any channels");
                return true;
            }
            Console.Write("The user is a member of these channels: ");
            foreach (String Channel in UserChannels)
            {
                Console.Write(Channel + ", ");
            }
            Console.Write("\n");
            return true;
        }

        private bool ViewRoom(String RoomName)
        {
            if (!Connected)
            {
                Console.WriteLine("Please connect to a server before viewing rooms");
                return false;
            }
            if (!UserChannels.Contains(RoomName))
            {
                Console.WriteLine("Please Join a room before viewing it");
                return false;
            }
            String Message;
            String Responce;
            Message = "CHCH" + IDString() + RoomName;
            Responce = Network.Send(Message);
            Console.Clear();
            Console.Write(Responce);
            CurrentChannel = RoomName;
            return true;
        }

        private bool MultiMessage()
        {
            if (!Connected)
            {
                Console.WriteLine("Please connect to a server before messaging channels");
                return false;
            }
            Console.Write("List Channels to message to, seperated by commas: ");
            String[] Channels = Console.ReadLine().Split(',');
            Console.Write("Enter Mesage: ");
            String Message = Console.ReadLine();
            foreach (String Channel in Channels)
            {
                MessageChannel(Channel.Trim(), Message);
            }
            return true;
        }

        private bool ViewMembers(String RoomName)
        {
            if (!Connected)
            {
                Console.WriteLine("Please connect to a server before viewing members of a room");
            }
            if (!UserChannels.Contains(RoomName))
            {
                Console.WriteLine("Please Join a room before viewing members");
            }
            String Message;
            String Responce;
            Message = "MEMB" + IDString() + RoomName;
            Responce = Network.Send(Message);
            String[] Members = Responce.Split(',');
            Console.WriteLine("The Members of room " + RoomName + " are:");
            foreach (String Member in Members)
            {
                Console.WriteLine(Member);
            }
            return true;
        }

        private bool MessageChannel(String Channel, String ChatMessage)
        {
            if (!Connected)
            {
                Console.WriteLine("Please connect to a server before messaging a channel");
                return false;
            }
            String Message;
            String Responce;
            Message = "MECH" + IDString() + Channel.Trim() + "," + ChatMessage;
            Responce = Network.Send(Message);
            Console.Clear();
            Console.Write(Responce);
            return true;
        }
    }
}
