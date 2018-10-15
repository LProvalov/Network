using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Concurrent;
using System.Text;

namespace ConsoleApp1
{
    class Program
    {
        static int port = 8005;
        static void Main(string[] args)
        {
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Any, port);

            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                listenSocket.Bind(ipPoint);
                listenSocket.Listen(3);
                ConcurrentDictionary<string, Socket> sockets = new ConcurrentDictionary<string, Socket>();

                for (int i = 0; i < 3; i++)
                {
                    var t = listenSocket.AcceptAsync();
                    t.ContinueWith(tsocket =>
                    {
                        if (tsocket.IsCompletedSuccessfully)
                        {
                            Socket handler = tsocket.Result;
                            byte[] buffer = new byte[1024];
                            try
                            {
                                string socketName = receiveMessage(handler);
                                sockets.TryAdd(socketName, tsocket.Result);

                                do
                                {
                                    try
                                    {
                                        string message = receiveMessage(handler);
                                        Console.WriteLine("{0}: {1}", socketName, message);
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine("Error: {0}", ex.Message);
                                    }
                                } while (handler.Connected);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Error: {0}", ex.Message);
                            }
                        }
                    });
                }
                Console.WriteLine("server started...");
                while (true)
                {

                }
            } catch (SocketException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static string receiveMessage(Socket socket)
        {
            byte[] buffer = new byte[1024];
            socket.Receive(buffer);
            int length = BitConverter.ToInt32(buffer, 0);

            byte[] messageBuffer = new byte[length];
            socket.Receive(messageBuffer);
            string message = ASCIIEncoding.ASCII.GetString(messageBuffer);
            return message;
        }
    }
}
