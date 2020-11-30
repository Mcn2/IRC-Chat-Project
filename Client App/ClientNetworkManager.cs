using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace Client_App
{
    class ClientNetworkManager
    {
        String ServerIP;
        readonly Int32 Port = 6667;

        public bool ReadServerIP()
        {
            string UserInput;
            Console.Write("Enter Server IP (If local, use 127.0.0.1): ");
            UserInput = Console.ReadLine();
            bool ValidateIp = IPAddress.TryParse(UserInput, out _);
            if (ValidateIp)
            {
                //Console.WriteLine("Valid");
                ServerIP = UserInput;
                return true;
            }
            else
            {
                Console.WriteLine("Invalid IP");
                return false;
            }
                
        }

        public String Send(string Message)
        {
            string responceData = "";
          
                var client = new TcpClient(ServerIP, Port);
                NetworkStream Stream = client.GetStream();

                byte[] data = Encoding.ASCII.GetBytes(Message);
                Stream.Write(data, 0, data.Length);
                Console.WriteLine("Send message: " + Message);
                responceData = String.Empty;


                int i;
                data = new byte[1024];

                while ((i = Stream.Read(data, 0, data.Length)) != 0)
                {
                    responceData = Encoding.ASCII.GetString(data, 0, i).TrimEnd(new char[] { (char)0 });
                }

                //int bytes = Stream.Read(data, 0, data.Length);
                //responceData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine("Recived Message: " + responceData);

                Stream.Close();
                client.Close();
                     

            return responceData;
        }
    }
}
