using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

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
            Byte[] bytes = new byte[256];
            string data = null;
            try
            {
                while (true)
                {
                    Console.Write("Waiting for a connection...");

                    TcpClient Client = Listener.AcceptTcpClient();
                    Console.WriteLine("Connected!");

                    data = "";

                    NetworkStream stream = Client.GetStream();

                    int i;

                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        data = data + Encoding.ASCII.GetString(bytes, 0, i);
                    }
                    Console.WriteLine("Message Recived: " + data);
                    
                    data = Chat.ParseMessage(data);
                    
                    byte[] msg = Encoding.ASCII.GetBytes(data);
                    
                    stream.Write(msg, 0, msg.Length);
                    Console.WriteLine("Sent: " + data);
                    

                    Client.Close();
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
