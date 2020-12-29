using System;
using System.IO;
using System.Numerics;
using System.Text;

namespace TVR.Service.Core.Network
{
    internal class Buffer
    {
        private readonly MemoryStream stream;

        public Buffer()
        {
            stream = new MemoryStream();
        }

        public Buffer(byte[] data)
        {
            stream = new MemoryStream(data);
        }

        public void Write(byte[] data)
        {
            stream.Write(data, 0, data.Length);
        }

        public void Write(int i)
        {
            Write(BitConverter.GetBytes(i));
        }

        public void Write(ushort s)
        {
            Write(BitConverter.GetBytes(s));
        }

        public void Write(byte b)
        {
            Write(new byte[] { b });
        }

        public void Write(float f)
        {
            Write(BitConverter.GetBytes(f));
        }

        public void Write(Vector3 v)
        {
            Write(v.X);
            Write(v.Y);
            Write(v.Z);
        }

        public void Write(Quaternion q)
        {
            Write(q.X);
            Write(q.Y);
            Write(q.Z);
            Write(q.W);
        }

        public void Write(string str)
        {
            Write(Encoding.ASCII.GetBytes(str));
            Write((byte)0);
        }

        public byte[] ReadBytes(int len)
        {
            var buf = new byte[len];
            stream.Read(buf, 0, len);
            return buf;
        }

        public int ReadInt()
        {
            return BitConverter.ToInt32(ReadBytes(sizeof(int)), 0);
        }

        public ushort ReadUshort()
        {
            return BitConverter.ToUInt16(ReadBytes(sizeof(ushort)), 0);
        }

        public byte ReadByte()
        {
            return (byte)stream.ReadByte();
        }

        public float ReadFloat()
        {
            return BitConverter.ToSingle(ReadBytes(sizeof(float)), 0);
        }

        public Vector3 ReadVector3()
        {
            return new Vector3(ReadFloat(), ReadFloat(), ReadFloat());
        }

        public Quaternion ReadQuaternion()
        {
            return new Quaternion(ReadFloat(), ReadFloat(), ReadFloat(), ReadFloat());
        }

        public string ReadString()
        {
            // TODO There's got to be a more elegant way :D
            var tmpStream = new MemoryStream();
            while (stream.Position < stream.Length)
            {
                var b = ReadByte();
                if (b == 0x00) break;
                else tmpStream.WriteByte(b);
            }
            return Encoding.ASCII.GetString(tmpStream.ToArray());
        }

        public byte[] ToArray()
        {
            return stream.ToArray();
        }
    }
}