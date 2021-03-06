using System;
using System.Collections.Generic;
using System.Text;

namespace Sever
{
    class ServerHandle
    {
        public static void WelcomeReceived (int client, CustomPacket packet)
        {
            int clientId = packet.ReadInt();
            string username = packet.ReadString();
            Console.WriteLine($"{Server.clients[client].tcp.socket.Client.RemoteEndPoint} connected! - ID : {client}, Username : {username}");
            if (client != clientId)
            {
                // Something went wrong. IDs are not matched!
                Console.WriteLine($"Player \"{username}\", {client} are not matched with client ID {clientId}");
            }
            //Server.clients[client].SendIntoGame(username);
        }
        public static void UDPTestReceived (int client, CustomPacket packet)
        {
            string msg = packet.ReadString();
            Console.WriteLine($"Received packet via UDP : {msg}");
        }
    }
}
