using BepuPhysics;
using SlipeServer.Console.Elements;
using SlipeServer.Server;
using SlipeServer.Server.Clients;
using SlipeServer.Server.Elements;
using SlipeServer.Server.Services;
using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace SlipeServer.Console.Logic;

public class NametagTestLogic
{
    private Player? nametagPlayer;
    private readonly MtaServer server;
    private readonly ChatBox chatBox;

    public NametagTestLogic(MtaServer server, CommandService commandService, ChatBox chatBox)
    {

        commandService.AddCommand("nametagtestertogglenametag").Triggered += ToggleNametag;
        commandService.AddCommand("nametagtesternametagtext").Triggered += SetNametagText;
        commandService.AddCommand("nametagtesterrandomcolor").Triggered += RandomiseNametagColor;
        commandService.AddCommand("nametagtesterjoinquit").Triggered += JoinQuit;
        this.server = server;
        this.chatBox = chatBox;
        CreateNametagTester();
    }

    private void CreateNametagTester()
    {
        this.nametagPlayer = new CustomPlayer(server.GetRequiredService<ExplosionService>(), server);
        var client = new FakeClient(this.nametagPlayer)
        {
            Serial = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAB",
            IPAddress = System.Net.IPAddress.Parse("127.0.0.1")
        };
        this.nametagPlayer.Client = client;
        this.nametagPlayer.AssociateWith(server);

        this.nametagPlayer.Name = "NametagTester";
        this.nametagPlayer.IsNametagShowing = true;
        this.nametagPlayer.NametagColor = Color.White;

        this.server.HandlePlayerJoin(this.nametagPlayer);
        this.nametagPlayer.Spawn(new(-5.0615234f, 24.59375f, 3.1171875f), 0, 0, 0, 0);

        this.chatBox.Output("NametagTester joined the server");

        this.nametagPlayer.Destroyed += NametagPlayer_Destroyed;
        this.nametagPlayer.Disconnected += NametagPlayer_Disconnected;
    }

    private void NametagPlayer_Disconnected(Player sender, Server.Elements.Events.PlayerQuitEventArgs e)
    {
        this.chatBox.Output("NametagTester triggered disconnected event");
    }
    private void NametagPlayer_Destroyed(Element obj)
    {
        this.chatBox.Output("NametagTester triggered destroyed event");
    }

    private void JoinQuit(object? sender, Server.Events.CommandTriggeredEventArgs e)
    {
        bool flag = nametagPlayer != null;
        Task.Run(async () =>
        {
            while (true)
            {
                if (flag)
                {
                    nametagPlayer?.TriggerDisconnected(Server.Enums.QuitReason.Quit);
                    nametagPlayer?.Destroy();
                    nametagPlayer = null;
                    this.chatBox.Output("NametagTester left the server");
                } else
                {
                    CreateNametagTester();
                }
                flag = !flag;
                await Task.Delay(2000);
            }
        });
    }

    private void ToggleNametag(object? sender, Server.Events.CommandTriggeredEventArgs e)
    {
        this.nametagPlayer.IsNametagShowing = !this.nametagPlayer.IsNametagShowing;
    }

    private void SetNametagText(object? sender, Server.Events.CommandTriggeredEventArgs e)
    {
        if (!e.Arguments.Any())
            return;

        this.nametagPlayer.NametagText = e.Arguments[0];
    }

    private void RandomiseNametagColor(object? sender, Server.Events.CommandTriggeredEventArgs e)
    {
        var random = new Random();
        var bytes = new byte[3];
        random.NextBytes(bytes);
        var color = Color.FromArgb(255, bytes[0], bytes[1], bytes[2]);
        this.nametagPlayer.NametagColor = color;
    }
}
