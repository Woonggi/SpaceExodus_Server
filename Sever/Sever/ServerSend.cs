using System;
using System.Collections.Generic;
using System.Text;

namespace Sever
{
    class ServerSend
    {
        private static void SendTCPData(int client, CustomPacket packet)
        {
            packet.WriteLength();
            Server.clients[client].tcp.SendData(packet);
        }

        private static void SendUDPData(int client, CustomPacket packet)
        {
            packet.WriteLength();
            Server.clients[client].udp.SendData(packet);
        }

        private static void SendTCPDataToAll(CustomPacket packet)
        {
            packet.WriteLength();
            for (int i = 0; i < Server.maxPlayers; ++i)
            {
                Server.clients[i].tcp.SendData(packet);
            }
        }

        private static void SendTCPDataToAll(CustomPacket packet, int except)
        {
            for (int i = 0; i < Server.maxPlayers; ++i)
            {
                if (i == except)
                {
                    continue;
                }
                Server.clients[i].tcp.SendData(packet);
            }
        }

        private static void SendUDPDataToAll(CustomPacket packet)
        {
            packet.WriteLength();
            for (int i = 0; i < Server.maxPlayers; ++i)
            {
                Server.clients[i].udp.SendData(packet);
            }
        }

        private static void SendUDPDataToAll(CustomPacket packet, int except)
        {
            for (int i = 0; i < Server.maxPlayers; ++i)
            {
                if (i == except)
                {
                    continue;
                }
                Server.clients[i].udp.SendData(packet);
            }
        }

        public static void Welcome(int client, string msg)
        {
            // using is the right way to use IDisposable object.
            using (CustomPacket packet = new CustomPacket((int)ServerPackets.SP_WELCOME))
            {
                packet.Write(msg);
                packet.Write(client);
                SendTCPData(client, packet);
            }
        }

        public static void UDPTest(int client)
        {
            using (CustomPacket packet = new CustomPacket((int)ServerPackets.UDP_TEST))
            {
                packet.Write("A test packet for UDP.");
                SendUDPData(client, packet);
            }
        }
        
        public static void SpawnPlayer(int toClient, Player player)
        {
            using (CustomPacket packet = new CustomPacket((int)ServerPackets.SP_SPAWNPLAYER))
            {
                packet.Write(player.id);
                packet.Write(player.username);
                packet.Write(player.position);
                packet.Write(player.rotation);

                SendTCPData(toClient, packet);
            }
        }
    }
}
