﻿using MtaServer.Packets.Enums;
using MtaServer.Server.Elements;
using System.Collections.Concurrent;

namespace MtaServer.Server.PacketHandling.QueueHandlers
{
    public struct PacketQueueEntry
    {
        public Client Client { get; set; }
        public PacketId PacketId { get; set; }
        public byte[] Data { get; set; }
    }

    public abstract class BaseQueueHandler: IQueueHandler
    {
        protected readonly ConcurrentQueue<PacketQueueEntry> packetQueue;

        public BaseQueueHandler()
        {
            this.packetQueue = new ConcurrentQueue<PacketQueueEntry>();
        }

        public virtual void EnqueuePacket(Client client, PacketId packetId, byte[] data)
        {
            this.packetQueue.Enqueue(new PacketQueueEntry()
            {
                Client = client,
                PacketId = packetId,
                Data = data
            });
        }
    }
}
