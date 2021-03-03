using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Sever
{
    public class Client
    {
        public static int dataBufferSize = 4096; // 4mb
        public int id;
        public TCP tcp;

        // Client class
        public Client(int _id)
        {
            id = _id;
            tcp = new TCP(id);
        }
        // To manage socket
        public class TCP 
        {
            public TcpClient socket;
            private readonly int id;
            private NetworkStream stream;
            private CustomPacket receivedData;
            private byte[] receivedBuffer;
        
            public TCP(int _id)
            {
                id = _id;
            }
            public void Connect(TcpClient _socket)
            {
                socket = _socket;
                socket.ReceiveBufferSize = dataBufferSize;
                socket.SendBufferSize = dataBufferSize;
                stream = socket.GetStream();

                receivedData = new CustomPacket();
                receivedBuffer = new byte[dataBufferSize];

                stream.BeginRead(receivedBuffer, 0, dataBufferSize, ReceiveCallback, null);

                // send welcome packet
                ServerSend.Welcome(id, "Connected to server :)");
            }
            public void SendData(CustomPacket packet)
            {
                try
                {
                    if (socket != null)
                    {
                        stream.BeginWrite(packet.ToArray(), 0, packet.Length(), null, null);
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine($"Error sending data to player {id} : {e}");
                }
            }

            private void ReceiveCallback(IAsyncResult _result)
            {
                try
                {
                    int byteLength = stream.EndRead(_result);
                    if(byteLength <= 0)
                    {
                        // disconnect
                        return;
                    }
                    byte[] data = new byte[byteLength];
                    Array.Copy(receivedBuffer, data, byteLength);

                    receivedData.Reset(HandleData(data));
                    stream.BeginRead(receivedBuffer, 0, dataBufferSize, ReceiveCallback, null);
                }
                catch(Exception _ex)
                {
                    Console.WriteLine($"Error receiving TCP data: {_ex}");
                    // disconnect properly.
                }
            }
            private bool HandleData(byte[] data)
            {
                int packetLength = 0;
                receivedData.SetBytes(data);
                // When the bytes that contain length data, 
                if (receivedData.UnreadLength() >= 4)
                {
                    // Read length.
                    packetLength = receivedData.ReadInt();
                    if (packetLength <= 0)
                    {
                        return true; // Reset the data. We don't have to read.
                    }
                }

                while (packetLength > 0 && packetLength <= receivedData.UnreadLength())
                {
                    byte[] packetBytes = receivedData.ReadBytes(packetLength);
                    // Action
                    ThreadManager.ExecuteOnMainThread(() =>
                    {
                        using (CustomPacket packet = new CustomPacket(packetBytes))
                        {
                            int packetId = packet.ReadInt();
                            Server.packetHandlers[packetId](id, packet);
                        }
                    });
                    packetLength = 0;
                    if (receivedData.UnreadLength() >= 4)
                    {
                        // Read length.
                        packetLength = receivedData.ReadInt();
                        if (packetLength <= 0)
                        {
                            return true; // Reset the data. We don't have to read.
                        }
                    }
                }
                if (packetLength <= 1)
                {
                    return true;
                }

                // Still left partial packets to read.
                return false;
            }
        }

    }
}
