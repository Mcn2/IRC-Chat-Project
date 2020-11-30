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
            Byte[] bytes = new byte[1024];
            string data;
            try
            {
                while (true)
                {
                    Console.Write("Waiting for a connection...");

                    TcpClient Client = Listener.AcceptTcpClient();
                    Console.WriteLine("Connected!");

                    data = "";

                    NetworkStream stream = Client.GetStream();

                    stream.Read(bytes, 0, bytes.Length);
                    data = Encoding.ASCII.GetString(bytes, 0, bytes.Length);
                    

                    Console.WriteLine("Message Recived: " + data.TrimEnd(new char[] { (char)0 }));
                    
                    data = Chat.ParseMessage(data.TrimEnd(new char[] { (char)0 }));
                    
                    bytes = Encoding.ASCII.GetBytes(data.TrimEnd(new char[] { (char)0 }));
                    
                    stream.Write(bytes, 0, bytes.Length);
                    Console.WriteLine("Sent: " + data.TrimEnd(new char[] { (char)0 }));
                    

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
