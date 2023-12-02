﻿using SlipeServer.Packets;
using SlipeServer.Packets.Enums;
using SlipeServer.Server.Elements;
using SlipeServer.Server.Enums;
using System;
using System.Collections.Generic;
using System.Net;

namespace SlipeServer.Server.Clients;

/// <summary>
/// Class indicating the lack of a client.
/// An instance of this class is only ever used very briefly when a player has been instantiated, but the client has not yet been created.
/// </summary>
public class TemporaryClient : IClient
{
    public Queue<QueuedClientPacket> PacketQueue { get; } = new();

    public TemporaryClient() { }

    public Player Player
    {
        get => throw new NullReferenceException("Attempt to access client of player without client.");
        set => throw new NullReferenceException("Attempt to access client of player without client.");
    }

    public string? Serial => throw new NullReferenceException("Attempt to access client of player without client.");

    public string? Extra => throw new NullReferenceException("Attempt to access client of player without client.");

    public string? Version => throw new NullReferenceException("Attempt to access client of player without client.");

    public IPAddress? IPAddress { get => throw new NullReferenceException("Attempt to access client of player without client."); set => throw new NullReferenceException("Attempt to access client of player without client."); }
    public bool IsConnected { get => throw new NullReferenceException("Attempt to access client of player without client."); set => throw new NullReferenceException("Attempt to access client of player without client."); }

    public ClientConnectionState ConnectionState => throw new NullReferenceException("Attempt to access client of player without client.");

    public uint Ping { get => throw new NullReferenceException("Attempt to access client of player without client."); set => throw new NullReferenceException("Attempt to access client of player without client."); }

    public void FetchSerial() => throw new NullReferenceException("Attempt to access client of player without client.");
    public void FetchIp() => throw new NullReferenceException("Attempt to access client of player without client.");
    public void ResendModPackets() => throw new NullReferenceException("Attempt to access client of player without client.");
    public void ResendPlayerACInfo() => throw new NullReferenceException("Attempt to access client of player without client.");
    public void SetVersion(ushort version) => throw new NullReferenceException("Attempt to access client of player without client.");
    public void SetDisconnected() => throw new NullReferenceException("Attempt to access client of player without client.");
    public void ResetConnectionState() => throw new NullReferenceException("Attempt to access client of player without client.");

    public void SendPacket(Packet packet)
    {
        this.PacketQueue.Enqueue(new QueuedClientPacket()
        {
            PacketId = packet.PacketId,
            Data = packet.Write(),
            priority = packet.Priority,
            reliability = packet.Reliability,
        });
    }

    public void SendPacket(PacketId packetId, byte[] data, PacketPriority priority = PacketPriority.Medium, PacketReliability reliability = PacketReliability.Unreliable)
    {
        this.PacketQueue.Enqueue(new QueuedClientPacket()
        {
            PacketId = packetId,
            Data = data,
            priority = priority,
            reliability = reliability,
        });
    }
}
