using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Client_App
{
    class ClientNetworkManager
    {
        String ServerIP;
        readonly Int32 Port = 6667;

        public bool ReadServerIP()
        {
            string UserInput;
            Console.WriteLine("Enter Server IP: ");
            UserInput = Console.ReadLine();
            bool ValidateIp = IPAddress.TryParse(UserInput, out _);
            if (ValidateIp)
            {
                Console.WriteLine("Valid");
                ServerIP = UserInput;
                return true;
            }
            else
            {
                Console.WriteLine("Invalid");
                return false;
            }
                
        }

        public int Send(string Message)
        {
            try
            {
                var client = new TcpClient(ServerIP, Port);
                NetworkStream Stream = client.GetStream();

                byte[] data = System.Text.Encoding.ASCII.GetBytes(Message);
                Stream.Write(data, 0, data.Length);
                Console.WriteLine("Send message: " + Message);

                data = new byte[256];
                string responceData = String.Empty;

                Int32 bytes = Stream.Read(data, 0, data.Length);
                responceData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine("Recived Message: " + responceData);

                Stream.Close();
                client.Close();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }

            Console.WriteLine("\nPress Enter to continue...");
            Console.Read();
            return 0;
        }
    }
}
