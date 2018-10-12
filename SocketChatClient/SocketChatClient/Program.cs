using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SocketChatClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPAddress address = IPAddress.Parse("127.0.0.1");
            EndPoint serverEp = new IPEndPoint(address, 8005);
            socket.Connect(serverEp);
            while (true) {
                string line = Console.ReadLine();
                byte[] buffer = ASCIIEncoding.ASCII.GetBytes(line);
                socket.Send(buffer);
            }
            
        }
    }
}
