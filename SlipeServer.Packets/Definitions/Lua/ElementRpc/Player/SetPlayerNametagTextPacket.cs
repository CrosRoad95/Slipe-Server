﻿using SlipeServer.Packets.Builder;
using SlipeServer.Packets.Enums;
using SlipeServer.Packets.Structs;
using System;

namespace SlipeServer.Packets.Definitions.Lua.ElementRpc.Player;

public class SetPlayerNametagTextPacket : Packet
{
    public override PacketId PacketId => PacketId.PACKET_ID_LUA_ELEMENT_RPC;
    public override PacketReliability Reliability => PacketReliability.ReliableSequenced;
    public override PacketPriority Priority => PacketPriority.High;

    public ElementId ElementId { get; set; }
    public string NametagText { get; set; }

    public SetPlayerNametagTextPacket(ElementId elementId, string nametagText)
    {
        this.ElementId = elementId;
        this.NametagText = nametagText;
    }

    public override void Read(byte[] bytes)
    {
        throw new NotSupportedException();
    }

    public override byte[] Write()
    {
        var builder = new PacketBuilder();
        builder.Write((byte)ElementRpcFunction.SET_PLAYER_NAMETAG_TEXT);
        builder.Write(this.ElementId);
        builder.Write(this.NametagText);
        return builder.Build();
    }
}
