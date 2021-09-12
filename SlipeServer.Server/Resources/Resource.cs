﻿using SlipeServer.Packets.Definitions.Resources;
using SlipeServer.Server.Elements;
using SlipeServer.Server.Extensions;
using SlipeServer.Server.Resources.ResourceServing;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlipeServer.Server.Resources
{
    public class Resource
    {
        private readonly MtaServer server;
        private readonly IResourceServer resourceServer;
        private readonly string path;

        public DummyElement Root { get; }
        public DummyElement DynamicRoot { get; }
        public ushort NetId { get; set; }
        public int PriorityGroup { get; set; }
        public List<string> Exports { get; }
        public string Name { get; }

        public Resource(MtaServer server, RootElement root, IResourceServer resourceServer, string name, string? path = null)
        {
            this.NetId = resourceServer.AllocateNetId();
            this.server = server;
            this.resourceServer = resourceServer;
            this.Name = name;
            this.path = path ?? $"./{name}";

            this.Root = new DummyElement()
            {
                Parent = root,
                ElementTypeName = name,
            }.AssociateWith(server);
            this.DynamicRoot = new DummyElement()
            {
                Parent = this.Root,
                ElementTypeName = name,
            }.AssociateWith(server);

            this.Exports = new List<string>();
        }

        public void Start()
        {
            var files = this.resourceServer.GetResourceFiles(this.path);
            this.server.BroadcastPacket(new ResourceStartPacket(
                this.Name, this.NetId, this.Root.Id, this.DynamicRoot.Id, 0, null, null, false, this.PriorityGroup, files, this.Exports)
            );
        }

        public void Stop()
        {
            this.server.BroadcastPacket(new ResourceStopPacket(this.NetId));
        }

        public void StartFor(Player player)
        {
            var files = this.resourceServer.GetResourceFiles(this.path);
            new ResourceStartPacket(this.Name, this.NetId, this.Root.Id, this.DynamicRoot.Id, 0, null, null, false, this.PriorityGroup, files, this.Exports)
                .SendTo(player);
        }

        public void StopFor(Player player)
        {
            new ResourceStopPacket(this.NetId).SendTo(player);
        }
    }
}
