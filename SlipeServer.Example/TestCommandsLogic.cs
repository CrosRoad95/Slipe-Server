using SlipeServer.Server;
using SlipeServer.Server.ElementCollections;
using SlipeServer.Server.Elements;
using SlipeServer.Server.Events;
using SlipeServer.Server.Services;

namespace SlipeServer.Example;

public class TestCommandsLogic
{
    private readonly CommandService commandService;
    private readonly ChatBox chatBox;
    private readonly MtaServer mtaServer;
    private readonly IElementCollection elementCollection;

    public TestCommandsLogic(CommandService commandService, ChatBox chatBox, MtaServer mtaServer, IElementCollection elementCollection)
    {
        this.commandService = commandService;
        this.chatBox = chatBox;
        this.mtaServer = mtaServer;
        this.elementCollection = elementCollection;
        AddGeneralCommands();
        AddVehiclesCommands();
    }

    private void AddGeneralCommands()
    {
        AddCommand("hello", () => this.chatBox.Output("Hello"));
    }
    
    private void AddVehiclesCommands()
    {
        AddCommand("spawnVehicle", (sender, e) =>
        {
            new Vehicle(404, e.Player.Position).AssociateWith(this.mtaServer);
            this.chatBox.Output("Test vehicle spawned");
        });

        AddCommand("playerVehiclesInfo", (sender, e) =>
        {
            foreach (var player in this.elementCollection.GetByType<Player>())
            {
                this.chatBox.Output($"Player: {player.Name}");
                this.chatBox.Output($"- Vehicle: {(player.Vehicle != null ? player.Vehicle.Model : "<none>")}");
                this.chatBox.Output($"- Vehicle action: {player.VehicleAction}");
                this.chatBox.Output($"- Vehicle seat: {player.Seat}");
            }
        });
    }

    private void AddCommand(string command, Action<object?, CommandTriggeredEventArgs> callback)
    {
        this.commandService.AddCommand(command).Triggered += (sender, e) =>
        {
            callback(sender, e);
        };
    }

    private void AddCommand(string command, Action callback)
    {
        this.commandService.AddCommand(command).Triggered += (sender, e) =>
        {
            callback();
        };
    }
}
