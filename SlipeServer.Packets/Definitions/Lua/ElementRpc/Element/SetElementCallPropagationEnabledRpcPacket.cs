﻿using SlipeServer.Packets.Builder;
using SlipeServer.Packets.Enums;
using SlipeServer.Packets.Structs;
using System;

namespace SlipeServer.Packets.Definitions.Lua.ElementRpc.Element;

public class SetElementCallPropagationEnabledRpcPacket : Packet
{
    public override PacketId PacketId => PacketId.PACKET_ID_LUA_ELEMENT_RPC;
    public override PacketReliability Reliability => PacketReliability.ReliableSequenced;
    public override PacketPriority Priority => PacketPriority.High;

    public ElementId ElementId { get; set; }
    public bool Enabled { get; set; }

    public SetElementCallPropagationEnabledRpcPacket()
    {

    }

    public SetElementCallPropagationEnabledRpcPacket(ElementId elementId, bool enabled)
    {
        this.ElementId = elementId;
        this.Enabled = enabled;
    }

    public override void Read(byte[] bytes)
    {
        throw new NotSupportedException();
    }

    public override byte[] Write()
    {
        var builder = new PacketBuilder();

        builder.Write((byte)ElementRpcFunction.SET_PROPAGATE_CALLS_ENABLED);
        builder.Write(this.ElementId);

        builder.Write(this.Enabled);

        return builder.Build();
    }
}
