﻿using SlipeServer.Packets;
using System.Collections.Concurrent;

namespace SlipeServer.Server.PacketHandling;

/// <summary>
/// Object pool for packets, allowing for packets to be reused instead of continuously be reallocated. 
/// Using this reduces the amount of garbage collection invocations.
/// </summary>
public class PacketPool<T> where T : Packet, new()
{
    private readonly int? maxPacketCount;
    private readonly ConcurrentQueue<T> packets;

    public PacketPool(int? maxPacketCount = -1)
    {
        this.maxPacketCount = maxPacketCount;
        this.packets = new();
    }

    public T GetPacket()
    {
        if (this.packets.TryDequeue(out var packet))
            return packet;

        return new T();
    }

    public void ReturnPacket(T packet)
    {
        if (this.packets.Count < this.maxPacketCount || this.maxPacketCount == -1)
        {
            packet.Reset();
            this.packets.Enqueue(packet);
        }
    }
}
