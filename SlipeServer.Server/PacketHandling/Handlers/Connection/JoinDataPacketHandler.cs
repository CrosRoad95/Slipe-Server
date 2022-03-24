﻿using MTAServerWrapper.Packets.Outgoing.Connection;
using SlipeServer.Packets.Definitions.Join;
using SlipeServer.Packets.Definitions.Lua.ElementRpc.Element;
using SlipeServer.Packets.Enums;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace SlipeServer.Server.PacketHandling.Handlers.Connection
{
    public class JoinDataPacketHandler : IPacketHandler<PlayerJoinDataPacket>
    {
        private readonly Configuration configuration;

        public PacketId PacketId => PacketId.PACKET_ID_PLAYER_JOINDATA;

        public JoinDataPacketHandler(Configuration configuration)
        {
            this.configuration = configuration;
        }

        public void HandlePacket(Client client, PlayerJoinDataPacket packet)
        {
            if (this.configuration.Password != null)
            {
                using var md5 = MD5.Create();
                var hash = md5.ComputeHash(Encoding.ASCII.GetBytes(this.configuration.Password));
                if (!hash.SequenceEqual(packet.Password))
                {
                    client.SendPacket(new PlayerDisconnectPacket(PlayerDisconnectType.INVALID_PASSWORD, "Incorrect password"));
                    return;
                }
            }

            string osName =
                RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "Windows" :
                RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? "Mac OS" :
                RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? "Linux" :
                RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD) ? "Free BSD" :
                "Unknown";
            client.SendPacket(new JoinCompletePacket($"Slipe Server 0.1.0 [{osName}]\0", "1.5.7-9.0.0"));

            client.Player.RunAsSync(() =>
            {
                client.Player.Name = packet.Nickname;
            });
            client.SetVersion(packet.BitStreamVersion);
            client.FetchSerial();
        }
    }
}
