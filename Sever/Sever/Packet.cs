using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace Sever
{
    public enum ServerPackets
    {
        SP_WELCOME = 0,
        SP_PLAYER_POS,
        SP_PLAYER_ROT,
        SP_BULLET_POS
    };

    public enum ClientPackets
    {
        CP_WELCOME_RECEIVED = 0
    };

    public class CustomPacket : IDisposable
{
    // Vector3 - player positions, rotation..., int, string
    // int - kill scores, health...
    // string - usernames....


    private List<byte> buffer;
    private byte[] readableBuffer;
    private int readPos;

    public CustomPacket()
    {
        buffer = new List<byte>(); // Intialize buffer.
        readPos = 0; // Read position on buffer.
    }

    public CustomPacket(int id)
    {
        buffer = new List<byte>();
        readPos = 0;
        Write(id);
    }

    public CustomPacket(byte[] data)
    {
        buffer = new List<byte>();
        readPos = 0;
        SetBytes(data);
    }

    // Sets the packet's content and prepares it to be read.
    public void SetBytes(byte[] data)
    {
        Write(data);
        readableBuffer = buffer.ToArray();
    }

    public void Write(byte value)
    {
        buffer.Add(value);
    }
    public void Write(byte[] value)
    {
        buffer.AddRange(value);
    }
    public void Write(int value)
    {
        buffer.AddRange(BitConverter.GetBytes(value));
    }

    public void Write(float value)
    {
        buffer.AddRange(BitConverter.GetBytes(value));
    }

    public void Write(bool value)
    {
        buffer.AddRange(BitConverter.GetBytes(value));
    }

    public void Write(string value)
    {
        Write(value.Length); // Store length of the string into buffer.
        buffer.AddRange(Encoding.ASCII.GetBytes(value));
    }

    public void Write(Vector3 value)
    {
        Write(value.X);
        Write(value.Y);
        Write(value.Z);
    }

    // Insert length at the begining of the buffer.
    public void WriteLength()
    {
        buffer.InsertRange(0, BitConverter.GetBytes(buffer.Count));
    }


    public byte ReadByte(bool moveReadPos = true)
    {
        if (buffer.Count > readPos)
        {
            byte value = readableBuffer[readPos];
            if (moveReadPos == true)
            {
                readPos += 1;
            }
            return value;
        }
        else
        {
            throw new Exception("Could not read value of 'byte'");
        }
    }
    public byte[] ReadBytes(int length, bool moveReadPos = true)
    {
        if (buffer.Count > readPos)
        {
            byte[] value = buffer.GetRange(readPos, length).ToArray();
            if (moveReadPos == true)
            {
                readPos += length;
            }
            return value;
        }
        else
        {
            throw new Exception("Could not read value of 'bytes'");
        }
    }
    public int ReadInt(bool moveReadPos = true)
    {
        if (buffer.Count > readPos)
        {
            int value = BitConverter.ToInt32(readableBuffer, readPos); // Convert bytes to int.
            if(moveReadPos == true)
            {
                readPos += sizeof(UInt32);
            }
            return value;
        }
        else
        {
            throw new Exception("Could not read value of 'int'");
        }
    }

    public float ReadFloat(bool moveReadPos = true)
    {
        if (buffer.Count > readPos)
        {
            float value = BitConverter.ToSingle(readableBuffer, readPos);
            if (moveReadPos == true)
            {
                readPos += sizeof(float);
            }
            return value;
        }
        else
        {
            throw new Exception("Could not read value of 'float'");
        }
    }

    public string ReadString(bool moveReadPos = true)
    {
        try
        {
            // We store length of a string at the start of the buffer.
            int length = ReadInt();
            string value = Encoding.ASCII.GetString(readableBuffer, readPos, length);
            if (moveReadPos == true && value.Length > 0)
            {
                readPos += length;
            }
            return value;
        }
        catch
        {
            throw new Exception("Could not read value of 'string'");
        }
    }

    public Vector3 ReadVector(bool moveReadPos = true)
    {
        return new Vector3(ReadFloat(moveReadPos), ReadFloat(moveReadPos), ReadFloat(moveReadPos));
    }
    public int Length()
    {
        return buffer.Count;
    }

    public int UnreadLength()
    {
        return Length() - readPos;
    }

    public void Reset(bool shouldReset = true)
    {
        if (shouldReset == true)
        {
            buffer.Clear();
            readableBuffer = null;
            readPos = 0;
        }
        else
        {
            // Unread the last read int.
            readPos -= 4;  
        }
    }
    public byte[] ToArray()
    {
        readableBuffer = buffer.ToArray();
        return readableBuffer;
    }

    private bool disposed = false;
    protected virtual void Dispose(bool disposing)
    {
        if (disposed == true)
        {
            return;
        }
        if (disposing)
        {
            buffer = null;
            readableBuffer = null;
            readPos = 0;
        }
        disposed = true;

    }
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}


}
