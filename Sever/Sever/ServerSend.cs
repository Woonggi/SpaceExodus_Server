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
            Console.WriteLine($"id : {client}");
            Server.clients[client].tcp.SendData(packet);
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
    }
}
