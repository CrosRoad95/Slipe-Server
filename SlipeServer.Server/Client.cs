﻿using SlipeServer.Net;
using SlipeServer.Packets;
using SlipeServer.Packets.Definitions.Player;
using SlipeServer.Packets.Enums;
using SlipeServer.Server.Elements;
using SlipeServer.Server.PacketHandling;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace SlipeServer.Server
{
    public class Client
    {
        private readonly INetWrapper netWrapper;
        private readonly uint binaryAddress;

        public Player Player { get; protected set; }

        public string? Serial { get; private set; }
        public string? Extra { get; private set; }
        public string? Version { get; private set; }
        public IPAddress? IPAddress { get; set; }
        public bool IsConnected { get; internal set; }
        public uint Ping { get; set; }

        public Client(uint binaryAddress, INetWrapper netWrapper)
        {
            this.binaryAddress = binaryAddress;
            this.netWrapper = netWrapper;
            this.Player = new Player(this);
            this.IsConnected = true;
        }

        public void SendPacket(Packet packet)
        {
            if (this.IsConnected && (ClientPacketScope.Current == null || ClientPacketScope.Current.ContainsClient(this)))
            {
                this.netWrapper.SendPacket(this.binaryAddress, packet);
            }
        }

        public void SendPacket(PacketId packetId, byte[] data, PacketPriority priority = PacketPriority.Medium, PacketReliability reliability = PacketReliability.Unreliable)
        {
            if (this.IsConnected && (ClientPacketScope.Current == null || ClientPacketScope.Current.ContainsClient(this)))
            {
                this.netWrapper.SendPacket(this.binaryAddress, packetId, data, priority, reliability);
            }
        }

        public void SetVersion(ushort version)
        {
            if(this.IsConnected)
            {
                this.netWrapper.SetVersion(this.binaryAddress, version);
            }
        }
        
        public void ResendModPackets()
        {
            if(this.IsConnected)
            {
                this.netWrapper.ResendModPackets(this.binaryAddress);
            }
        }
        
        public void ResendPlayerACInfo()
        {
            if(this.IsConnected)
            {
                this.netWrapper.ResendPlayerACInfo(this.binaryAddress);
            }
        }

        public void FetchSerial()
        {
            Tuple<string, string, string> serialExtraAndVersion = this.netWrapper.GetClientSerialExtraAndVersion(this.binaryAddress);
            this.Serial = serialExtraAndVersion.Item1;
            this.Extra = serialExtraAndVersion.Item2;
            this.Version = serialExtraAndVersion.Item3;
        }
    }
}
