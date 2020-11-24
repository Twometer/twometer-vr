using System;
using System.IO;
using System.Text;
using TVR.Service.Core.Math;

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

        public void Write(byte b)
        {
            Write(BitConverter.GetBytes(b));
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
    }
}