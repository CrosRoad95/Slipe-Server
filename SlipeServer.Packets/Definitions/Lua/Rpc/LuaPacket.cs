﻿using SlipeServer.Packets;
using SlipeServer.Packets.Builder;
using SlipeServer.Packets.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlipeServer.Packets.Lua
{
    public class LuaPacket : Packet
    {
        public override PacketId PacketId => PacketId.PACKET_ID_LUA;
        public override PacketReliability Reliability => PacketReliability.ReliableSequenced;
        public override PacketPriority Priority => PacketPriority.High;

        public ElementRPCFunction ElementRPCFunction { get; }
        public byte[] Data { get; protected set; }

        public LuaPacket(ElementRPCFunction elementRPCFunction, byte[]? data = null)
        {
            ElementRPCFunction = elementRPCFunction;
            Data = data ?? new byte[0];
        }

        public override byte[] Write()
        {
            PacketBuilder builder = new PacketBuilder();

            builder.Write((byte)ElementRPCFunction);

            if (Data != null)
            {
                builder.Write(Data);
            }

            return builder.Build();
        }

        public override void Read(byte[] bytes)
        {
            throw new NotImplementedException();
        }
    }
}
