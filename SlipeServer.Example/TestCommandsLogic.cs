using SlipeServer.Server;
using SlipeServer.Server.Elements;
using SlipeServer.Server.Events;
using SlipeServer.Server.Services;

namespace SlipeServer.Example;

public class TestCommandsLogic
{
    private readonly CommandService commandService;
    private readonly ChatBox chatBox;
    private readonly MtaServer mtaServer;

    public TestCommandsLogic(CommandService commandService, ChatBox chatBox, MtaServer mtaServer)
    {
        this.commandService = commandService;
        this.chatBox = chatBox;
        this.mtaServer = mtaServer;
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
