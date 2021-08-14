﻿using SlipeServer.Packets;
using SlipeServer.Packets.Builder;
using SlipeServer.Packets.Enums;
using System;

namespace MTAServerWrapper.Packets.Outgoing.Connection
{
    public class PlayerQuitPacket : Packet
    {
        public override PacketId PacketId => PacketId.PACKET_ID_PLAYER_QUIT;
        public override PacketReliability Reliability => PacketReliability.ReliableSequenced;
        public override PacketPriority Priority => PacketPriority.High;

        public uint PlayerId { get; }
        public byte QuitReason { get; }

        public PlayerQuitPacket(uint playerId, byte quitReason)
        {
            this.PlayerId = playerId;
            this.QuitReason = quitReason;
        }

        public override byte[] Write()
        {
            var builder = new PacketBuilder();

            builder.Write(new byte[] { 0, 0 }); // 2 bytes of padding is required for some reason
            builder.WriteElementId(this.PlayerId);
            builder.WriteCapped(this.QuitReason, 3);

            return builder.Build();
        }

        public override void Read(byte[] bytes)
        {

        }
    }
}
