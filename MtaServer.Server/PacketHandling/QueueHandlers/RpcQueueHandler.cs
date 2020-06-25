﻿using MtaServer.Packets.Definitions.Join;
using MtaServer.Packets.Enums;
using MtaServer.Packets.Rpc;
using MtaServer.Server.Elements;
using MtaServer.Server.PacketHandling.Factories;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MtaServer.Server.PacketHandling.QueueHandlers
{
    public class RpcQueueHandler : BaseQueueHandler
    {
        private readonly MtaServer server;
        private readonly int sleepInterval;

        public RpcQueueHandler(MtaServer server, int sleepInterval, int workerCount): base()
        {
            this.server = server;
            this.sleepInterval = sleepInterval;
            for (int i = 0; i < workerCount; i++)
            {
                Task.Run(HandlePackets);
            }
        }

        public async void HandlePackets()
        {
            while (true)
            {
                while(this.packetQueue.TryDequeue(out PacketQueueEntry queueEntry))
                {
                    switch (queueEntry.PacketId)
                    {
                        case PacketId.PACKET_ID_RPC:
                            RpcPacket packet = new RpcPacket();
                            packet.Read(queueEntry.Data);
                            HandleRpc(queueEntry.Client, packet);
                            break;
                    }
                }
                await Task.Delay(this.sleepInterval);
            }
        }

        private void HandleRpc(Client client, RpcPacket packet)
        {
            Debug.WriteLine($"Received RPC of type {packet.FunctionId}");
            switch (packet.FunctionId)
            {
                case RpcFunctions.PLAYER_INGAME_NOTICE:
                    client.SendPacket(new JoinedGamePacket(
                        client.Id, 
                        server.ElementRepository.Count + 1, 
                        this.server.Root.Id, 
                        HttpDownloadType.HTTP_DOWNLOAD_ENABLED_PORT, 
                        80, 
                        "", 
                        5, 
                        1
                    ));

                    var existingPlayersListPacket = PlayerPacketFactory.CreatePlayerListPacket(
                        this.server.ElementRepository.GetByType<Client>(ElementType.Player).ToArray(), 
                        true
                    );
                    client.SendPacket(existingPlayersListPacket);

                    var newPlayerListPacket = PlayerPacketFactory.CreatePlayerListPacket(new Client[] { client }, false);
                    foreach (var player in this.server.ElementRepository.GetByType<Client>(ElementType.Player))
                    {
                        player.SendPacket(newPlayerListPacket);
                    }

                    this.server.ElementRepository.Add(client);
                    client.HandleJoin();

                    break;
            }
        }
    }
}
