﻿using Neon.Communication.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Neon.Communication.Packets.Outgoing
{
    public class ServerPacket : IServerPacket
    {
        private readonly Encoding Encoding = Encoding.Default;

        private readonly List<byte> Body = new List<byte>();

        public ServerPacket(int id)
        {
            Id = id;
            WriteShort(id);
        }

        public int Id { get; private set; }

        public byte[] GetBytes()
        {
            List<byte> Final = new List<byte>();
            Final.AddRange(BitConverter.GetBytes(Body.Count)); // packet len
            Final.Reverse();
            Final.AddRange(Body); // Add Packet
            return Final.ToArray();
        }

        public void WriteByte(byte b)
        {
            Body.Add(b);
        }

        public void WriteByte(int b)
        {
            Body.Add((byte)b);
        }

        public void WriteBytes(byte[] b, bool IsInt) // d
        {
            if (IsInt)
            {
                for (int i = (b.Length - 1); i > -1; i--)
                {
                    Body.Add(b[i]);
                }
            }
            else
            {
                Body.AddRange(b);
            }
        }

        internal void WriteString(object p)
        {
            throw new NotImplementedException();
        }

        public void WriteDouble(double d) // d
        {
            string Raw = Math.Round(d, 1).ToString();

            if (Raw.Length == 1)
            {
                Raw += ".0";
            }

            WriteString(Raw.Replace(',', '.'));
        }

        public void WriteString(string s) // d
        {
            WriteShort(s.Length);
            WriteBytes(Encoding.GetBytes(s), false);
        }

        public void WriteShort(int s) // d
        {
            short i = (short)s;
            WriteBytes(BitConverter.GetBytes(i), true);
        }

        public void WriteInteger(int i) // d
        {
            WriteBytes(BitConverter.GetBytes(i), true);
        }

        public void WriteBoolean(bool b) // d
        {
            WriteBytes(new[] { (byte)(b ? 1 : 0) }, false);
        }

        public void writeList(List<string> list)
        {
            WriteInteger(list.Count);
            foreach (string obj in list)
            {
                WriteString(obj);
            }
        }
    }
}