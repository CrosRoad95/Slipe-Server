﻿using Microsoft.Extensions.Logging;
using SlipeServer.Packets.Definitions.Vehicles;
using SlipeServer.Packets.Enums;
using SlipeServer.Server.Constants;
using SlipeServer.Server.Elements;
using SlipeServer.Server.Enums;
using SlipeServer.Server.Extensions;
using SlipeServer.Server.ElementCollections;
using System.Numerics;
using SlipeServer.Server.Clients;
using System;
using System.Reflection;

namespace SlipeServer.Server.PacketHandling.Handlers.Vehicle;

public class VehicleInOutPacketHandler : IPacketHandler<VehicleInOutPacket>
{
    private readonly IElementCollection elementCollection;
    private readonly MtaServer server;
    private readonly ILogger logger;

    public PacketId PacketId => PacketId.PACKET_ID_VEHICLE_INOUT;

    public VehicleInOutPacketHandler(
        IElementCollection elementCollection,
        MtaServer server,
        ILogger logger
    )
    {
        this.elementCollection = elementCollection;
        this.server = server;
        this.logger = logger;
    }

    public void HandlePacket(IClient client, VehicleInOutPacket packet)
    {
        var element = this.elementCollection.Get(packet.VehicleId);
        if (element == null)
        {
            this.logger.LogTrace("Attempt to enter non-existant vehicle by {player}", client.Player.Name);
            return;
        }

        if (element is Elements.Vehicle vehicle)
        {
            switch (packet.ActionId)
            {
                case VehicleInOutAction.RequestIn:
                    HandleRequestIn(client, vehicle, packet);
                    break;
                case VehicleInOutAction.NotifyIn:
                    HandleNotifyIn(client, vehicle);
                    break;
                case VehicleInOutAction.NotifyAbortIn:
                    HandleNotifyInAbort(client, vehicle, packet);
                    break;
                case VehicleInOutAction.RequestOut:
                    HandleRequestOut(client, vehicle, packet);
                    break;
                case VehicleInOutAction.NotifyOut:
                    HandleNotifyOut(client, vehicle, packet);
                    break;
                case VehicleInOutAction.NotifyOutAbort:
                    HandleNotifyOutAbort(client, vehicle, packet);
                    break;
                case VehicleInOutAction.NotifyFellOff:
                    HandleNotifyFellOff(client, vehicle, packet);
                    break;
                case VehicleInOutAction.NotifyJack:
                    HandleNotifyJack(client, vehicle, packet);
                    break;
                case VehicleInOutAction.NotifyJackAbort:
                    HandleNotifyJackAbort(client, vehicle, packet);
                    break;
            }
        }
    }

    private void HandleRequestIn(IClient client, Elements.Vehicle vehicle, VehicleInOutPacket packet)
    {
        VehicleEnterFailReason failReason = VehicleEnterFailReason.Invalid;
        if (vehicle.IsTrailer)
        {
            SendInRequestFailResponse(client, vehicle, VehicleEnterFailReason.Trailer);
            return;
        }
        if (client.Player.VehicleAction != VehicleAction.None)
        {
            SendInRequestFailResponse(client, vehicle, VehicleEnterFailReason.Action);
            return;
        }
        if (client.Player.Vehicle != null)
        {
            SendInRequestFailResponse(client, vehicle, VehicleEnterFailReason.InVehicle);
            return;
        }

        float cutoffDistance = 50f;
        bool warpIn = false;

        if (
            (client.Player.IsInWater || packet.IsOnWater) && VehicleConstants.WaterEntryVehicles.Contains((VehicleModel)vehicle.Model) ||
            vehicle.Model == 464
        )
        {
            cutoffDistance = 10;
            warpIn = true;
        }

        if (vehicle.Driver != null)
            cutoffDistance = 10;

        if (Vector3.Distance(client.Player.Position, vehicle.Position) > cutoffDistance)
        {
            SendInRequestFailResponse(client, vehicle, VehicleEnterFailReason.Distance);
            return;
        }

        if (failReason != VehicleEnterFailReason.Invalid)
        {
            SendInRequestFailResponse(client, vehicle, failReason);
            return;
        }

        if (packet.Seat == 0)
        {
            if (vehicle.Driver == null)
            {
                if (vehicle.CanEnter != null && !vehicle.CanEnter(client.Player, vehicle, packet.Seat))
                {
                    SendInRequestFailResponse(client, vehicle, VehicleEnterFailReason.Script);
                    return;
                }

                client.Player.Seat = packet.Seat;
                if (warpIn)
                {
                    vehicle.AddPassenger(packet.Seat, client.Player, true);
                } else
                {
                    client.Player.EnteringVehicle = vehicle;
                    client.Player.VehicleAction = VehicleAction.Entering;

                    var replyPacket = new VehicleInOutPacket()
                    {
                        PedId = client.Player.Id,
                        VehicleId = vehicle.Id,
                        Door = packet.Door,
                        Seat = 0,
                        OutActionId = VehicleInOutActionReturns.RequestInConfirmed,
                    };
                    this.server.BroadcastPacket(replyPacket);
                }
            } else
            {

                if (vehicle.CanEnter != null && !vehicle.CanEnter(client.Player, vehicle, packet.Seat))
                {
                    SendInRequestFailResponse(client, vehicle, VehicleEnterFailReason.Script);
                    return;
                }

                client.Player.Seat = packet.Seat;
                if (vehicle.Driver is Elements.Player player)
                {
                    client.Player.VehicleAction = VehicleAction.Jacking;
                    player.JackingVehicle = vehicle;
                    player.VehicleAction = VehicleAction.Jacked;
                    vehicle.JackingPed = client.Player;
                    Console.WriteLine("Subscribe jacked player");
                    player.Disconnected += HandleDisconnected;

                    var replyPacket = new VehicleInOutPacket()
                    {
                        PedId = client.Player.Id,
                        VehicleId = vehicle.Id,
                        Door = packet.Door,
                        OutActionId = VehicleInOutActionReturns.RequestJackConfirmed,
                    };
                    this.server.BroadcastPacket(replyPacket);
                }

            }
        } else
        {
            byte? seat = packet.Seat;
            if (vehicle.GetOccupantInSeat(seat.Value) != null || seat > vehicle.GetMaxPassengers())
            {
                seat = vehicle.GetFreePassengerSeat();
            }
            if (seat == null || seat > 8)
            {
                SendInRequestFailResponse(client, vehicle, VehicleEnterFailReason.Seat);
                return;
            }
            if (vehicle.CanEnter != null && !vehicle.CanEnter(client.Player, vehicle, packet.Seat))
            {
                SendInRequestFailResponse(client, vehicle, VehicleEnterFailReason.Script);
                return;
            }

            client.Player.Seat = packet.Seat;
            client.Player.EnteringVehicle = vehicle;
            client.Player.VehicleAction = VehicleAction.Entering;

            if (warpIn)
            {
                vehicle.AddPassenger(seat.Value, client.Player, true);
            } else
            {
                client.Player.Seat = seat;
                var replyPacket = new VehicleInOutPacket()
                {
                    PedId = client.Player.Id,
                    VehicleId = vehicle.Id,
                    Seat = seat.Value,
                    Door = packet.Door,
                    OutActionId = VehicleInOutActionReturns.RequestInConfirmed,
                };
                this.server.BroadcastPacket(replyPacket);
            }
        }
    }

    private void HandleDisconnected(Elements.Player sender, Elements.Events.PlayerQuitEventArgs e)
    {
        Console.WriteLine("HandleDisconnected jacked player");
        if (sender.JackingVehicle?.JackingPed != null)
        {
            sender.JackingVehicle.JackingPed.VehicleAction = VehicleAction.None;
        }
        sender.JackingVehicle = null;
        sender.Disconnected -= HandleDisconnected;
    }

    private void SendInRequestFailResponse(IClient client, Elements.Vehicle vehicle, VehicleEnterFailReason failReason)
    {
        var replyPacket = new VehicleInOutPacket()
        {
            PedId = client.Player.Id,
            VehicleId = vehicle.Id,
            FailReason = failReason,
            OutActionId = VehicleInOutActionReturns.VehicleAttemptFailed,
            CorrectPosition = (failReason == VehicleEnterFailReason.Distance ? vehicle.Position : Vector3.Zero)
        };
        replyPacket.SendTo(client);
    }

    private void HandleNotifyIn(IClient client, Elements.Vehicle vehicle)
    {
        if (client.Player.VehicleAction == VehicleAction.Entering)
        {
            client.Player.VehicleAction = VehicleAction.None;
            if (client.Player.EnteringVehicle == vehicle)
            {
                client.Player.Vehicle = client.Player.EnteringVehicle;
                client.Player.EnteringVehicle = null;
                vehicle.AddPassenger(client.Player.Seat ?? 0, client.Player, false);

                var replyPacket = new VehicleInOutPacket()
                {
                    PedId = client.Player.Id,
                    VehicleId = vehicle.Id,
                    Seat = client.Player.Seat ?? 0,
                    OutActionId = VehicleInOutActionReturns.NotifyInReturn,
                };
                this.server.BroadcastPacket(replyPacket);
            }
        }
    }

    private void HandleNotifyInAbort(IClient client, Elements.Vehicle vehicle, VehicleInOutPacket packet)
    {
        if (client.Player.VehicleAction == VehicleAction.Entering)
        {
            client.Player.VehicleAction = VehicleAction.None;
            client.Player.EnteringVehicle = null;
            client.Player.Vehicle = null;
            vehicle.RemovePassenger(client.Player, false);

            var replyPacket = new VehicleInOutPacket()
            {
                PedId = client.Player.Id,
                VehicleId = vehicle.Id,
                Seat = packet.Seat,
                Door = packet.Door,
                DoorOpenRatio = packet.DoorOpenRatio,
                OutActionId = VehicleInOutActionReturns.NotifyInAbortReturn,
            };
            this.server.BroadcastPacket(replyPacket);
        }
    }

    private void HandleRequestOut(IClient client, Elements.Vehicle vehicle, VehicleInOutPacket packet)
    {
        if (client.Player.VehicleAction != VehicleAction.None)
        {
            var errorReplyPacket = new VehicleInOutPacket()
            {
                PedId = client.Player.Id,
                VehicleId = vehicle.Id,
                OutActionId = VehicleInOutActionReturns.VehicleAttemptFailed,
            };
            errorReplyPacket.SendTo(client);
            return;
        }

        if (vehicle.CanExit != null && !vehicle.CanExit(client.Player, vehicle, packet.Seat))
        {
            var cancelReplyPacket = new VehicleInOutPacket()
            {
                PedId = client.Player.Id,
                VehicleId = vehicle.Id,
                OutActionId = VehicleInOutActionReturns.VehicleAttemptFailed,
            };
            cancelReplyPacket.SendTo(client);
            return;
        }

        client.Player.VehicleAction = VehicleAction.Exiting;

        var replyPacket = new VehicleInOutPacket()
        {
            PedId = client.Player.Id,
            VehicleId = vehicle.Id,
            OutActionId = VehicleInOutActionReturns.RequestOutConfirmed,
            Door = packet.Door
        };
        this.server.BroadcastPacket(replyPacket);
    }

    private void HandleNotifyOut(IClient client, Elements.Vehicle vehicle, VehicleInOutPacket packet)
    {
        if (client.Player.VehicleAction != VehicleAction.Exiting)
            return;

        client.Player.Vehicle = null;
        client.Player.EnteringVehicle = null;
        client.Player.VehicleAction = VehicleAction.None;

        if (!vehicle.Occupants.ContainsValue(client.Player))
            return;

        vehicle.RemovePassenger(client.Player, false);

        var replyPacket = new VehicleInOutPacket()
        {
            PedId = client.Player.Id,
            VehicleId = vehicle.Id,
            OutActionId = VehicleInOutActionReturns.NotifyOutReturn,
            Seat = packet.Seat
        };
        this.server.BroadcastPacket(replyPacket);
    }

    private void HandleNotifyOutAbort(IClient client, Elements.Vehicle vehicle, VehicleInOutPacket packet)
    {
        if (client.Player.VehicleAction != VehicleAction.Exiting)
            return;

        if (!vehicle.Occupants.ContainsValue(client.Player))
            return;

        client.Player.VehicleAction = VehicleAction.None;


        var replyPacket = new VehicleInOutPacket()
        {
            PedId = client.Player.Id,
            VehicleId = vehicle.Id,
            OutActionId = VehicleInOutActionReturns.NotifyOutAbortReturn,
            Seat = packet.Seat
        };
        this.server.BroadcastPacket(replyPacket);
    }

    private void HandleNotifyFellOff(IClient client, Elements.Vehicle vehicle, VehicleInOutPacket packet)
    {
        if (!vehicle.Occupants.ContainsValue(client.Player))
            return;

        client.Player.VehicleAction = VehicleAction.None;
        client.Player.Vehicle = null;
        client.Player.EnteringVehicle = null;
        vehicle.RemovePassenger(client.Player, false);

        var replyPacket = new VehicleInOutPacket()
        {
            PedId = client.Player.Id,
            VehicleId = vehicle.Id,
            OutActionId = VehicleInOutActionReturns.NotifyFellOffReturn,
            Seat = packet.Seat
        };
        this.server.BroadcastPacket(replyPacket);
    }

    private void HandleNotifyJack(IClient client, Elements.Vehicle vehicle, VehicleInOutPacket packet)
    {
        if (client.Player.VehicleAction != VehicleAction.Jacking)
            return;

        if (vehicle.Driver == null)
            return;

        var jackedPed = vehicle.Driver;
        if (jackedPed is Elements.Player jackedPlayer)
        {
            jackedPlayer.Vehicle = null;
            jackedPlayer.VehicleAction = VehicleAction.None;
            jackedPlayer.Disconnected -= HandleDisconnected;
            Console.WriteLine("unsubscribe jacked player 1");

            vehicle.JackingPed = null;

            client.Player.Vehicle = vehicle;
            client.Player.EnteringVehicle = null;
            client.Player.JackingVehicle = null;
            client.Player.VehicleAction = VehicleAction.None;
            vehicle.AddPassenger(0, client.Player, false);

            vehicle.Jack(jackedPlayer, client.Player);

            var replyPacket = new VehicleInOutPacket()
            {
                PedId = client.Player.Id,
                PlayerInId = client.Player.Id,
                PlayerOutId = jackedPlayer.Id,
                VehicleId = vehicle.Id,
                OutActionId = VehicleInOutActionReturns.NotifyJackReturn,
                Seat = packet.Seat
            };
            this.server.BroadcastPacket(replyPacket);
        }
    }

    private void HandleNotifyJackAbort(IClient client, Elements.Vehicle vehicle, VehicleInOutPacket packet)
    {
        if (client.Player.VehicleAction != VehicleAction.Jacking)
            return;

        client.Player.VehicleAction = VehicleAction.None;
        client.Player.JackingVehicle = null;
        vehicle.JackingPed = null;

        var replyPacket = new VehicleInOutPacket()
        {
            PedId = client.Player.Id,
            VehicleId = vehicle.Id,
            OutActionId = VehicleInOutActionReturns.NotifyInAbortReturn,
            Seat = packet.Seat
        };
        this.server.BroadcastPacket(replyPacket);

        var jackedPed = vehicle.Driver;
        if (jackedPed == null)
            return;

        if(jackedPed is Elements.Player jackedPlayer)
        {
            Console.WriteLine("unsubscribe jacked player 2");
            jackedPlayer.Disconnected -= HandleDisconnected;
        }

        if (packet.StartedJacking)
        {
            jackedPed.Vehicle = null;
            vehicle.RemovePassenger(jackedPed, false);

            var jackReplyPacket = new VehicleInOutPacket()
            {
                PedId = jackedPed.Id,
                VehicleId = vehicle.Id,
                OutActionId = VehicleInOutActionReturns.NotifyOutReturn,
                Seat = packet.Seat
            };
            this.server.BroadcastPacket(jackReplyPacket);
        }
    }
}
