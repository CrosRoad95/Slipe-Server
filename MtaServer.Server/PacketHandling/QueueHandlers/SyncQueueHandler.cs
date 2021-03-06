﻿using MtaServer.Packets.Definitions.Sync;
using MtaServer.Packets.Enums;
using MtaServer.Server.Elements;
using System;
using System.Threading.Tasks;

namespace MtaServer.Server.PacketHandling.QueueHandlers
{
    public class SyncQueueHandler : BaseQueueHandler
    {
        private readonly MtaServer server;
        private readonly int sleepInterval;

        public SyncQueueHandler(MtaServer server, int sleepInterval, int workerCount): base()
        {
            this.server = server;
            this.sleepInterval = sleepInterval;

            for (int i = 0; i < workerCount; i++)
            {
                _ = Task.Run(HandlePackets);
            }
        }

        public async void HandlePackets()
        {
            while (true)
            {
                while(this.packetQueue.TryDequeue(out PacketQueueEntry queueEntry))
                {
                    try
                    { 
                        Console.WriteLine(queueEntry.PacketId);
                        switch (queueEntry.PacketId)
                        {
                            case PacketId.PACKET_ID_CAMERA_SYNC:
                                CameraSyncPacket cameraPureSyncPacket = new CameraSyncPacket();
                                cameraPureSyncPacket.Read(queueEntry.Data);
                                HandleCameraSyncPacket(queueEntry.Client, cameraPureSyncPacket);
                                break;
                            case PacketId.PACKET_ID_PLAYER_PURESYNC:
                                PlayerPureSyncPacket playerPureSyncPacket = new PlayerPureSyncPacket();
                                playerPureSyncPacket.Read(queueEntry.Data);
                                HandlePlayerPureSyncPacket(queueEntry.Client, playerPureSyncPacket);
                                break;
                        }
                    } catch (Exception e)
                    {
                        Console.WriteLine("Handling packet failed");
                        Console.WriteLine(string.Join(", ", queueEntry.Data));
                        //Console.WriteLine($"{e.Message}\n{e.StackTrace}");
                    }
                }
                await Task.Delay(this.sleepInterval);
            }
        }

        private void HandleCameraSyncPacket(Client client, CameraSyncPacket packet)
        {
            //Console.WriteLine($"client {client.Id} camera sync: isFixed: {packet.IsFixed}, position: {packet.Position}, lookAt: {packet.LookAt}, target: {packet.TargetId}");
        }

        private void HandlePlayerPureSyncPacket(Client client, PlayerPureSyncPacket packet)
        {
            client.SendPacket(new ReturnSyncPacket(packet.Position));

            packet.PlayerId = client.Id;
            packet.Latency = 0;
            foreach (var player in this.server.ElementRepository.GetByType<Client>(ElementType.Player))
            {
                if (player != client)
                {
                    player.SendPacket(packet);
                }
            }

            client.Position = packet.Position;

            //Console.WriteLine($"client {client.Id} pure sync: ");
            //Console.WriteLine($"\tFlags:"); 

            //Console.WriteLine($"\t\tIsInWater: {packet.SyncFlags.IsInWater}");
            //Console.WriteLine($"\t\tIsOnGround: {packet.SyncFlags.IsOnGround}");
            //Console.WriteLine($"\t\tHasJetpack: {packet.SyncFlags.HasJetpack}");
            //Console.WriteLine($"\t\tIsDucked: {packet.SyncFlags.IsDucked}");
            //Console.WriteLine($"\t\tWearsGoggles: {packet.SyncFlags.WearsGoggles}");
            //Console.WriteLine($"\t\tHasContact: {packet.SyncFlags.HasContact}");
            //Console.WriteLine($"\t\tIsChoking: {packet.SyncFlags.IsChoking}");
            //Console.WriteLine($"\t\tAkimboTargetUp: {packet.SyncFlags.AkimboTargetUp}");
            //Console.WriteLine($"\t\tIsOnFire: {packet.SyncFlags.IsOnFire}");
            //Console.WriteLine($"\t\tHasAWeapon: {packet.SyncFlags.HasAWeapon}");
            //Console.WriteLine($"\t\tIsSyncingVelocity: {packet.SyncFlags.IsSyncingVelocity}");
            //Console.WriteLine($"\t\tIsStealthAiming: {packet.SyncFlags.IsStealthAiming}");

            Console.WriteLine($"\tposition: {packet.Position}, rotation: {packet.Rotation}");
            //Console.WriteLine($"\tvelocity: {packet.Velocity}");
            //Console.WriteLine($"\thealth: {packet.Health}, armour: {packet.Armour}");
            //Console.WriteLine($"\tCamera rotation: {packet.CameraRotation}, position: {packet.CameraOrientation.CameraPosition}, forward: {packet.CameraOrientation.CameraForward}");
        }
    }
}
