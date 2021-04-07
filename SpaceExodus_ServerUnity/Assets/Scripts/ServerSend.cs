using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerSend
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

    private static void SendTCPDataToAll(CustomPacket packet)
    {
        packet.WriteLength();
        for (int i = 1; i <= Server.maxPlayers; i++)
        {
            Server.clients[i].tcp.SendData(packet);
        }
    }

    private static void SendTCPDataToAll(CustomPacket packet, int except)
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

    public static void Welcome(int toClient, string msg)
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
            packet.Write(player.transform.position);
            packet.Write(player.transform.rotation);
            packet.Write(player.maxHealth);

            SendTCPData(toClient, packet);
        }
    }

    public static void PlayerPosition(Player player)
    {
        using (CustomPacket packet = new CustomPacket((int)ServerPackets.SP_PLAYER_POSITION))
        {
            packet.Write(player.id);
            packet.Write(player.transform.position);
            packet.Write(Server.frame);
            SendUDPDataToAll(packet);
        }
    }
    public static void PlayerRotation(Player player)
    {
        using (CustomPacket packet = new CustomPacket((int)ServerPackets.SP_PLAYER_ROTATION))
        {
            // TEST!
            packet.Write(player.id);
            packet.Write(player.transform.rotation);
            // TODO: no need to send frame.
            packet.Write(Server.frame);
            SendUDPDataToAll(packet);
        }
    }
    public static void PlayerShooting(Player player)
    {
        using (CustomPacket packet = new CustomPacket((int)ServerPackets.SP_PLAYER_SHOOTING))
        {
            packet.Write(player.id);
            packet.Write(player.heading);
            SendTCPDataToAll(packet);
        }
    }

    public static void PlayerDisconnected(int playerId)
    {
        using (CustomPacket packet = new CustomPacket((int)ServerPackets.SP_PLAYER_DISCONNECTED))
        {
            packet.Write(playerId);
            SendTCPDataToAll(packet);
        }    
    }

    public static void PlayerHit(Player player)
    {
        using (CustomPacket packet = new CustomPacket((int)ServerPackets.SP_PLAYER_HIT))
        {
            packet.Write(player.id);
            packet.Write(player.health);
            SendTCPDataToAll(packet);
        }
    }

    public static void PlayerDestroy(Player player, int killerId)
    {
        using (CustomPacket packet = new CustomPacket((int)ServerPackets.SP_PLAYER_DESTROY))
        {
            packet.Write(player.id);
            packet.Write(killerId);
            SendTCPDataToAll(packet);
        }
    }
    public static void PlayerRespawn(Player player)
    {
        using (CustomPacket packet = new CustomPacket((int)ServerPackets.SP_PLAYER_RESPAWN))
        {
            packet.Write(player.id);
            packet.Write(player.spawnPosition);
            SendTCPDataToAll(packet);
        }
    }
}
