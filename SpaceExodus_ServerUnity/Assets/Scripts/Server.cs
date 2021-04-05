using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class Server
{
    public static int maxPlayers { get; private set; }
    public static int port { get; private set; }
    public static Dictionary<int, Client> clients = new Dictionary<int, Client>();
    public static int frame = 1;

    public delegate void PacketHandler(int fromClient, CustomPacket packet);
    public static Dictionary<int, PacketHandler> packetHandlers;

    private static TcpListener tcpListener;
    private static UdpClient udpListener;

    public static void Start(int _maxPlayers, int _port)
    {
        maxPlayers = _maxPlayers;
        port = _port;

        InitializeServerData();

        Debug.Log("Starting Server...");

        tcpListener = new TcpListener(IPAddress.Any, port);
        tcpListener.Start();
        tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null);

        udpListener = new UdpClient(port);
        udpListener.BeginReceive(UDPReceiveCallback, null);

        Debug.Log($"Server started on {port}");
    }

    private static void TCPConnectCallback(IAsyncResult result)
    {
        TcpClient client = tcpListener.EndAcceptTcpClient(result);
        tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null);
        Debug.Log($"Incoming connection from {client.Client.RemoteEndPoint}...");

        for (int i = 1; i <= maxPlayers; i++)
        {
            if (clients[i].tcp.socket == null)
            {
                clients[i].tcp.Connect(client);
                return;
            }
        }
        Debug.Log($"{client.Client.RemoteEndPoint} failed to connect: Server Full!");
    }

    private static void UDPReceiveCallback(IAsyncResult result)
    {
        try
        {
            IPEndPoint clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
            byte[] data = udpListener.EndReceive(result, ref clientEndPoint);
            udpListener.BeginReceive(UDPReceiveCallback, null);

            if (data.Length < 4)
            {
                return;
            }

            using (CustomPacket packet = new CustomPacket(data))
            {
                int clientId = packet.ReadInt();
                if (clientId == 0)
                {
                    return;
                }

                if (clients[clientId].udp.endPoint == null)
                {
                    clients[clientId].udp.Connect(clientEndPoint);
                    return;
                }

                if (clients[clientId].udp.endPoint.ToString() == clientEndPoint.ToString())
                {
                    clients[clientId].udp.HandleData(packet);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log($"Error receiving UDP data: {ex}");
        }
    }

    public static void SendUDPData(IPEndPoint clientEndPoint, CustomPacket packet)
    {
        try
        {
            if (clientEndPoint != null)
            {
                udpListener.BeginSend(packet.ToArray(), packet.Length(), clientEndPoint, null, null);
            }
        }
        catch (Exception ex)
        {
            Debug.Log($"Error sending data to {clientEndPoint} via UDP: {ex}");
        }
    }

    private static void InitializeServerData()
    {
        for (int i = 1; i <= maxPlayers; i++)
        {
            clients.Add(i, new Client(i));
        }

        packetHandlers = new Dictionary<int, PacketHandler>()
            {
                { (int)ClientPackets.CP_WELCOME_RECEIVED, ServerHandle.WelcomeReceived },
                { (int)ClientPackets.CP_PLAYER_MOVEMENT, ServerHandle.PlayerMovement },
                { (int)ClientPackets.CP_PLAYER_SHOOTING, ServerHandle.PlayerShooting }
            };
    }
   
    public static void Stop()
    {
        tcpListener.Stop();
        udpListener.Close();
    }
}


