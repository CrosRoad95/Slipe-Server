﻿using SlipeServer.Packets.Definitions.Resources;
using SlipeServer.Packets.Structs;
using SlipeServer.Server.Elements;
using SlipeServer.Server.Elements.Events;
using SlipeServer.Server.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SlipeServer.Server.Resources;

/// <summary>
/// Represents a client-side Lua resource
/// </summary>
public class Resource
{
    private readonly MtaServer server;

    public DummyElement Root { get; }
    public DummyElement DynamicRoot { get; }
    public ushort NetId { get; set; }
    public int PriorityGroup { get; set; }
    public List<string> Exports { get; init; }
    public List<ResourceFile> Files { get; init; }
    public Dictionary<string, byte[]> NoClientScripts { get; init; }
    private Dictionary<string, byte[]> SanitisedNoClientScripts => this.NoClientScripts.Where(x => x.Value.Length > 0).ToDictionary(x => x.Key, x => x.Value);
    public string Name { get; }
    public string Path { get; }
    public bool IsOopEnabled { get; set; }

    public Resource(
        MtaServer server, 
        RootElement root, 
        string name, 
        string? path = null
    )
    {
        this.server = server;
        this.Name = name;
        this.Path = path ?? $"./{name}";

        this.Files = new();
        this.NoClientScripts = new();

        this.Root = new DummyElement()
        {
            Parent = root,
            ElementTypeName = name,
        }.AssociateWith(server);
        this.DynamicRoot = new DummyElement()
        {
            Parent = this.Root,
            ElementTypeName = "map",
        }.AssociateWith(server);

        this.Exports = new List<string>();
    }

    public void Start()
    {
        this.server.BroadcastPacket(new ResourceStartPacket(
            this.Name, this.NetId, this.Root.Id, this.DynamicRoot.Id, (ushort)this.SanitisedNoClientScripts.Count, null, null, this.IsOopEnabled, this.PriorityGroup, this.Files, this.Exports)
        );

        this.server.BroadcastPacket(new ResourceClientScriptsPacket(
            this.NetId, this.SanitisedNoClientScripts.ToDictionary(x => x.Key, x => CompressFile(x.Value)))
        );
    }

    public void Stop()
    {
        this.server.BroadcastPacket(new ResourceStopPacket(this.NetId));
    }

    public void StartFor(Player player)
    {
        new ResourceStartPacket(this.Name, this.NetId, this.Root.Id, this.DynamicRoot.Id, (ushort)this.SanitisedNoClientScripts.Count, null, null, this.IsOopEnabled, this.PriorityGroup, this.Files, this.Exports)
            .SendTo(player);

        if (this.SanitisedNoClientScripts.Any())
            new ResourceClientScriptsPacket(this.NetId, this.SanitisedNoClientScripts.ToDictionary(x => x.Key, x => CompressFile(x.Value)))
                .SendTo(player);
    }

    public async Task StartForAsync(Player player, CancellationToken cancelationToken = default)
    {
        if (player.IsDestroyed || !player.Client.IsConnected)
            throw new InvalidOperationException("Player is not connected to the server.");

        using var cts = new CancellationTokenSource(300_000);
        using var cts2 = CancellationTokenSource.CreateLinkedTokenSource(cts.Token, cancelationToken);
        var source = new TaskCompletionSource();

        cts2.Token.Register(() =>
        {
            source.SetException(new TaskCanceledException());
        });

        player.ResourceStarted += HandleResourceStart;
        player.Disconnected += HandlePlayerDisconnected;

        void HandleResourceStart(Player sender, PlayerResourceStartedEventArgs e)
        {
            if (e.NetId != this.NetId || cts2.Token.IsCancellationRequested)
                return;

            source.SetResult();
        }

        void HandlePlayerDisconnected(Player disconnectingPlayer, PlayerQuitEventArgs e)
        {
            if(player != disconnectingPlayer || cts2.Token.IsCancellationRequested)
                return;

            source.SetException(new Exception("Player disconnected."));
        }

        cts2.Token.ThrowIfCancellationRequested();

        StartFor(player);

        try
        {
            await source.Task;
        }
        finally
        {
            player.ResourceStarted -= HandleResourceStart;
            player.Disconnected -= HandlePlayerDisconnected;
        }
    }

    public void StopFor(Player player)
    {
        new ResourceStopPacket(this.NetId).SendTo(player);
    }

    private byte[] CompressFile(byte[] input)
    {
        var compressed = Ionic.Zlib.ZlibStream.CompressBuffer(input);

        var result = new byte[] {
                (byte)((input.Length >> 24) & 0xFF),
                (byte)((input.Length >> 8) & 0xFF),
                (byte)((input.Length >> 24) & 0xFF),
                (byte)(input.Length & 0xFF)
            }.Concat(compressed).ToArray();

        return result;
    }
}
