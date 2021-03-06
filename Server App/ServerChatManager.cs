﻿using System;
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
        //LVCH | Leave a channel, channel name in message, if the user is no longer in the channel or if the channel dosn't exist
        //CHCH | Request channel chat history, channel name in message, returns channel chat history
        //MEMB | Requests a list of members in a channel, channel name in message, returns comma seperated list of members
        //PING | Checks if server is responding, no message required, returns "Hello Client!"
        //MECH | Messages a channel, channel name,text in message, returns chat history for channel
        //GBYE | Signals ending of connection, no message required, returns "Bye!"


        private Dictionary<int, UserData> UserDict = new Dictionary<int, UserData>();
        private Dictionary<String, ChannelData> ChannelDict = new Dictionary<string, ChannelData>();

        public void RemoveEmpty()
        {
            foreach (String Channel in ChannelDict.Keys)
            {
                if(ChannelDict[Channel].Members.Count == 0)
                {
                    ChannelDict.Remove(Channel);
                    Console.WriteLine("Removed empty channel: " + Channel);
                }
            }
            return;
        }
        

        public string ParseMessage(string Message)
        {
            int UserID;
            String Type;
            String Contents;
            try
            {
                Type = Message.Substring(0, 4);
                Console.WriteLine("Message type " + Type);
                if (Message.Substring(4, 4) == "0000")
                {
                    UserID = 0;
                }
                else
                {
                    UserID = int.Parse((Message.Substring(4, 4)).TrimStart('0'));
                }
                Contents = Message.Substring(8);
                Console.WriteLine("User Id: " + UserID.ToString());
                Console.WriteLine("Message: " + Contents);
            }
            catch
            {
                return "Message Not Accepted";
            }

            if(Type != "PING" && UserID != 0 && !UserDict.ContainsKey(UserID))
            {
                return "User ID not accepted, message not processed";
            }

            switch (Type){
                case "NWID":
                    Console.WriteLine("New ID");
                    if(UserID != 0 && UserDict.ContainsKey(UserID))
                    {
                        return UserID.ToString();
                    }else
                    {
                        for(int i = 1; i < 1000; i++)
                        {
                            if (!UserDict.ContainsKey(i))
                            {
                                UserDict.Add(i, new UserData(i, Contents, new List<String>()));
                                return i.ToString();
                            }
                        }
                    }
                    break;
                case "CHLS":
                    Console.WriteLine("List Channels");
                    string output = "";
                    foreach (String Name in ChannelDict.Keys)
                    {
                        output = output + Name + ",";
                    }
                    output.Trim(',');
                    return output;
                case "CRCH":
                    Console.WriteLine("Create Channel");
                    if (ChannelDict.ContainsKey(Contents.Trim()))
                    {
                        return "Already Exists";
                    }
                    else
                    {
                        ChannelDict.Add(Contents.Trim(),
                            new ChannelData(Contents.Trim(),
                            "Channel " + Contents.Trim() + " Started by " + UserDict[UserID].Alias + "\n",
                            new List<int>()));
                        ChannelDict[Contents.Trim()].Members.Add(UserID);
                        return "Channel Created";
                    }
                case "JNCH":
                    Console.WriteLine("Join Channel");
                    if (ChannelDict.ContainsKey(Contents.Trim()))
                    {
                        if (!ChannelDict[Contents.Trim()].Members.Contains(UserID))
                        {
                            ChannelDict[Contents.Trim()].Members.Add(UserID);
                        }
                        if (!UserDict[UserID].Channels.Contains(Contents.Trim()))
                        {
                            UserDict[UserID].Channels.Add(Contents.Trim());
                        }
                        
                        return ChannelDict[Contents.Trim()].Messages;
                    }
                    else
                    {
                        return "Channel Does Not Exist";
                    }
                case "LVCH":
                    Console.WriteLine("Leave Channel");
                    if (!ChannelDict.ContainsKey(Contents.Trim()))
                    {
                        return "Channel Does Not Exist";
                    }
                    if (UserDict[UserID].Channels.Contains(Contents.Trim()))
                    {
                        UserDict[UserID].Channels.Remove(Contents.Trim());
                    }
                    if(ChannelDict[Contents.Trim()].Members.Contains(UserID))
                    {
                        ChannelDict[Contents.Trim()].Members.Remove(UserID);
                    }
                    return "User is no longer in channel " + Contents.Trim();
                case "CHCH":
                    Console.WriteLine("Channel History");
                    if (ChannelDict.ContainsKey(Contents.Trim()))
                    {
                        if (ChannelDict[Contents.Trim()].Members.Contains(UserID))
                        {
                            return ChannelDict[Contents.Trim()].Messages;
                        }
                        else
                        {
                            return "User is not a member of this channel";
                        }
                    }
                    else
                    {
                        return "Channel Does not exist";
                    }
                case "MEMB":
                    Console.WriteLine("Member List");
                    if (ChannelDict.ContainsKey(Contents.Trim()))
                    {
                        if (ChannelDict[Contents.Trim()].Members.Contains(UserID))
                        {
                            string members = "";
                            foreach(int UserInt in ChannelDict[Contents.Trim()].Members)
                            {
                                members = members + UserDict[UserInt].Alias + ", ";
                            }
                            members.Trim();
                            members.Trim(',');
                            return members;
                        }
                        else
                        {
                            return "User is not a member of this channel";
                        }
                    }
                    else
                    {
                        return "Channel Does not exist";
                    }
                case "PING":
                    Console.WriteLine("Ping");
                    return "Hello Client!";
                case "MECH":
                    Console.WriteLine("Message Channel");
                    String ChatMessage;
                    String ChannelName;
                    ChannelName = Contents.Substring(0, Contents.IndexOf(','));
                    ChatMessage = Contents.Substring(Contents.IndexOf(',') + 1);
                    if (!ChannelDict.ContainsKey(ChannelName))
                    {
                        return "Channel dosen't exist";
                    }
                    ChannelDict[ChannelName].Messages = ChannelDict[ChannelName].Messages + UserDict[UserID].Alias + ": " + ChatMessage + "\n";
                    return ChannelDict[ChannelName.Trim()].Messages;
                case "GBYE":
                    Console.WriteLine("Disconnect");
                    foreach (ChannelData Channel in ChannelDict.Values)
                    {
                        if (Channel.Members.Contains(UserID))
                        {
                            Channel.Members.Remove(UserID);
                        }
                    }
                    UserDict.Remove(UserID);
                    return "Bye!";
                default:
                    Console.WriteLine("No Command");
                    return "Message Type not Recognised";
            }
            return "";
        }


        private class UserData
        {
            public int UserID;
            public string Alias;
            public List<String> Channels;

            public UserData(int userID, string alias, List<String> channels)
            {
                UserID = userID;
                Alias = alias;
                Channels = channels;
            }
        }

        private class ChannelData
        {
            public String Name;
            public String Messages;
            public List<int> Members;

            public ChannelData(string name, string messages, List<int> members)
            {
                Name = name;
                Messages = messages;
                Members = members;
            }
        }
    }
}
