using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceExodus_Server
{
    class ServerSend
    {
        private static void SendTCPData(int toClient, CustomPacket packet)
        {
            packet.WriteLength();
            Server.clients[toClient].tcp.SendData(packet);
        }

        private static void SendUDPData(int toClient, CustomPacket packet)
        {
            packet.WriteLength();
            Server.clients[toClient].udp.SendData(packet);
        }

        private static void SendTCPDataToAll (CustomPacket packet)
        {
            packet.WriteLength();
            for (int i = 1; i <= Server.maxPlayers; i++)
            {
                Server.clients[i].tcp.SendData(packet);
            }
        }

        private static void SendTCPDataToAll (CustomPacket packet, int except)
        {
            packet.WriteLength();
            for (int i = 1; i <= Server.maxPlayers; i++)
            {
                if (i != except)
                {
                    Server.clients[i].tcp.SendData(packet);
                }
            }
        }

        private static void SendUDPDataToAll(CustomPacket packet)
        {
            packet.WriteLength();
            for (int i = 1; i <= Server.maxPlayers; i++)
            {
                Server.clients[i].udp.SendData(packet);
            }
        }

        private static void SendUDPDataToAll(CustomPacket packet, int except)
        {
            packet.WriteLength();
            for (int i = 1; i <= Server.maxPlayers; i++)
            {
                if (i != except)
                {
                    Server.clients[i].udp.SendData(packet);
                }
            }
        }

        public static void Welcome (int toClient, string msg)
        {
            using (CustomPacket packet = new CustomPacket((int)ServerPackets.SP_WELCOME))
            {
                packet.Write(msg);
                packet.Write(toClient);
                SendTCPData(toClient, packet);
            }
        }

        public static void SpawnPlayer(int toClient, Player player)
        {
            using (CustomPacket packet = new CustomPacket((int)ServerPackets.SP_SPAWN_PLAYER))
            {
                packet.Write(player.id);
                packet.Write(player.username);
                packet.Write(player.position);
                packet.Write(player.rotation);

                SendTCPData(toClient, packet);
            }
        }

        public static void PlayerPosition(Player player)
        {
            using (CustomPacket packet = new CustomPacket((int)ServerPackets.SP_PLAYER_POSITION))
            {
                packet.Write(player.id);
                packet.Write(player.position);
                //Console.WriteLine(player.position);
                SendUDPDataToAll(packet);
            }
        }
        public static void PlayerRotation(Player player)
        {
            using (CustomPacket packet = new CustomPacket((int)ServerPackets.SP_PLAYER_ROTATION))
            {
                // TEST!
                packet.Write(player.id);
                packet.Write(player.angle);
                SendUDPDataToAll(packet);
            }
        }
    }
}
