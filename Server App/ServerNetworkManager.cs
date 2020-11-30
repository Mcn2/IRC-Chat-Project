using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace Server_App
{
    public class ServerNetworkManager
    {
        public ServerNetworkManager()
        {
            Listener = new TcpListener(IPAddress.Any, Port);
            Listener.Start();
            Chat = new ServerChatManager();
        }

        ~ServerNetworkManager()
        {
            Listener.Stop();
        }

        private TcpListener Listener;
        private readonly Int32 Port = 6667;
        private ServerChatManager Chat;

        public int Run()
        {
            Byte[] bytes;
            string data;
            try
            {
                while (true)
                {
                    Console.Write("Waiting for a connection...");

                    TcpClient Client = Listener.AcceptTcpClient();
                    Console.WriteLine("Connected!");

                    data = "";
                    bytes = new byte[1024];

                    NetworkStream stream = Client.GetStream();

                    stream.Read(bytes, 0, bytes.Length);
                    data = Encoding.ASCII.GetString(bytes, 0, bytes.Length).TrimEnd(new char[] { (char)0 });
                    

                    Console.WriteLine("Message Recived: " + data);
                    
                    data = Chat.ParseMessage(data);
                    
                    bytes = Encoding.ASCII.GetBytes(data);
                    
                    stream.Write(bytes, 0, bytes.Length);
                    Console.WriteLine("Sent: " + data);
                    

                    Client.Close();

                    Chat.RemoveEmpty();
                }
            }
            catch(SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                Listener.Stop();
            }

            Console.WriteLine("\nHit enter to continue...");
            Console.Read();
            

            return 0;
        }


    }
}
