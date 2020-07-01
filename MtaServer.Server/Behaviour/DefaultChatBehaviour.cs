﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using MtaServer.Packets.Definitions.Commands;
using MtaServer.Server.Elements;

namespace MtaServer.Server.Behaviour
{
    public class DefaultChatBehaviour
    {
        public DefaultChatBehaviour(MtaServer server)
        {
            Player.OnJoin += (player) =>
            {
                player.OnCommand += (command, arguments) =>
                {
                    if(command == "say")
                    {
                        var packet = new ChatEchoPacket(server.Root.Id, player.Name + ": " + string.Join(' ', arguments), Color.White);
                        foreach (var _player in server.ElementRepository.GetByType<Player>(ElementType.Player))
                        {
                            _player.Client.SendPacket(packet);
                        }
                    }
                };
            };
        }
    }
}
