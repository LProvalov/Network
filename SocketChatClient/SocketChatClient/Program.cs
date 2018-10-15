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
            Console.Write("Enter client name: ");
            string clientName = Console.ReadLine();
            sendMessage(socket, clientName);

            while (true) {
                string line = Console.ReadLine();
                sendMessage(socket, line);
            }            
        }

        static void sendMessage(Socket socket, string message)
        {
            byte[] messageBuffer = ASCIIEncoding.ASCII.GetBytes(message);
            socket.Send(BitConverter.GetBytes(messageBuffer.Length));
            socket.Send(messageBuffer);
        }
    }
}
