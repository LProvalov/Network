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
                ConcurrentDictionary<int, Socket> sockets = new ConcurrentDictionary<int, Socket>();

                var t = listenSocket.AcceptAsync();
                t.ContinueWith(socket => {
                    sockets.TryAdd(socket.Id, socket);
                });
                Socket handler = listenSocket.Accept();
                byte[] receivedBuffer = new byte[1024];
                ArraySegment<byte> buffer = new ArraySegment<byte>(receivedBuffer);

                while (true)
                {
                    handler.ReceiveAsync(buffer, SocketFlags.Broadcast).ContinueWith(t => {
                        Console.WriteLine("{0}", buffer);
                    });
                    

                    handler.Receive(receivedBuffer);
                    string receivedMsg = ASCIIEncoding.ASCII.GetString(receivedBuffer);
                    Console.WriteLine("{0}", receivedMsg);
                }
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
