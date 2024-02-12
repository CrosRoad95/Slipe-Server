﻿using SlipeServer.Packets;
using SlipeServer.Packets.Enums;
using SlipeServer.Server.Elements;
using SlipeServer.Server.Enums;
using System.Net;

namespace SlipeServer.Server.Clients;

/// <summary>
/// A stub implementation for a client, you can use this to implement fake players for testing purposes
/// There is no point in using this in production, since players do not yield any additional values over using peds
/// And the MTA master server list will not include fake clients in the player count
/// </summary>
public class FakeClient : IClient
{
    public Player Player { get; set; }

    public string? Serial { get; set; }

    public string? Extra { get; set; }

    public string? Version { get; set; }

    public IPAddress? IPAddress { get; set; }
    public bool IsConnected { get; set; }

    public ClientConnectionState ConnectionState { get; set; }

    public uint Ping { get; set; }

    public FakeClient(Player player)
    {
        this.Player = player;
    }

    public void FetchSerial() { }
    public void FetchIp() { }
    public void ResendModPackets() { }
    public void ResendPlayerACInfo() { }
    public void SendPacket(Packet packet) { }
    public void SendPacket(PacketId packetId, byte[] data, PacketPriority priority = PacketPriority.Medium, PacketReliability reliability = PacketReliability.Unreliable) { }
    public void SetVersion(ushort version) { }
    public void SetDisconnected()
    {
        this.ConnectionState = ClientConnectionState.Quit;
    }

    public void ResetConnectionState() { }
}
