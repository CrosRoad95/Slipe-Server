﻿using SlipeServer.Packets.Definitions.Lua.ElementRpc.Ped;
using SlipeServer.Server.Elements;
using SlipeServer.Server.Elements.Events;

namespace SlipeServer.Server.Behaviour;

/// <summary>
/// Behaviours handling warping players into / out of vehicles
/// </summary>
public class VehicleWarpBehaviour
{
    private readonly MtaServer server;

    public VehicleWarpBehaviour(MtaServer server)
    {
        this.server = server;

        server.ElementCreated += OnElementCreate;
    }

    private void OnElementCreate(Element element)
    {
        if (element is Vehicle vehicle)
        {
            vehicle.PedEntered += HandleEnter;
            vehicle.PedLeft += HandleLeft;
        }
    }

    private void HandleEnter(object? sender, VehicleEnteredEventsArgs eventArgs)
    {
        if (eventArgs.WarpsIn)
        {
            this.server.BroadcastPacket(new WarpIntoVehicleRpcPacket(
                eventArgs.Ped.Id,
                eventArgs.Vehicle.Id,
                eventArgs.Seat,
                eventArgs.Ped.GetAndIncrementTimeContext()
            ));
        }
    }

    private void HandleLeft(object? sender, VehicleLeftEventArgs eventArgs)
    {
        if (eventArgs.WarpsOut)
        {
            this.server.BroadcastPacket(new RemoveFromVehiclePacket(
                eventArgs.Ped.Id,
                eventArgs.Ped.GetAndIncrementTimeContext()
            ));
        }
    }
}
