using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

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
            Server.clients[client].SendIntoGame(username);
        }

        public static void PlayerMovement(int client, CustomPacket packet)
        {
            bool[] inputs = new bool[packet.ReadInt()];
            for (int i = 0; i < inputs.Length; i++)
            {
                inputs[i] = packet.ReadBool();
            }
            float rotation = packet.ReadFloat();
            Server.clients[client].player.SetInputs(inputs, rotation);
        }
    }
}
