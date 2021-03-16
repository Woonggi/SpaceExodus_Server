using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace SpaceExodus_Server
{
    class ServerHandle
    {
        public static void WelcomeReceived (int fromClient, CustomPacket packet)
        {
            int clientIdCheck = packet.ReadInt();
            string username = packet.ReadString();
            Console.WriteLine($"{Server.clients[fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully and is now player {fromClient}");
            if (fromClient != clientIdCheck)
            {
                Console.WriteLine($"Player \"{username}\" (ID : {fromClient}) has assumed the wrong client ID ({clientIdCheck})!");
            }
            // TODO : send player in the game.
            Server.clients[fromClient].SendIntoGame(username);
        }

        public static void PlayerMovement(int fromClient, CustomPacket packet)
        {
            bool[] inputs = new bool[packet.ReadInt()];
            for (int i = 0; i < inputs.Length; i++)
            {
                inputs[i] = packet.ReadBool();
            }
            // TEST!
            //Quaternion rotation = packet.ReadQuaternion();
            float angle = packet.ReadFloat();

            if (Server.clients[fromClient].player != null) 
            { 
                Server.clients[fromClient].player.SetInputs(inputs, angle);
            }
        }

        public static void PlayerShooting(int fromClient, CustomPacket packet)
        {
            Server.clients[fromClient].player.Shooting();
        }
    }
}
