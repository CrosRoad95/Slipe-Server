﻿using SlipeServer.Packets;
using SlipeServer.Server.Elements;
using SlipeServer.Server.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace SlipeServer.Server.PacketHandling.QueueHandlers.SyncMiddleware
{
    public class RangeSyncHandlerMiddleware<TData> : ISyncHandlerMiddleware<TData>
    {
        private readonly IElementRepository elementRepository;
        private readonly float range;
        private readonly bool excludesSource;

        public RangeSyncHandlerMiddleware(IElementRepository elementRepository, float range, bool excludesSource = true)
        {
            this.elementRepository = elementRepository;
            this.range = range;
            this.excludesSource = excludesSource;
        }

        public IEnumerable<Player> GetPlayersToSyncTo(Player player, TData packet)
        {
            var elements = this.elementRepository
                .GetWithinRange<Player>(player.Position, this.range, ElementType.Player);

            if (this.excludesSource)
                return elements.Where(x => x != player);
            return elements;
        }
    }
}
