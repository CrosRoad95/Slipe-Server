﻿using SlipeServer.Packets.Definitions.Lua.ElementRpc.Element;
using SlipeServer.Packets.Definitions.Lua.ElementRpc.WorldObject;
using SlipeServer.Packets.Definitions.Lua.Rpc.Destroys;
using SlipeServer.Server.Elements;

namespace SlipeServer.Server.PacketHandling.Factories;

public static class WorldObjectPacketFactory
{
    public static SetElementModelRpcPacket CreateSetModelPacket(WorldObject worldObject)
    {
        return new SetElementModelRpcPacket(worldObject.Id, worldObject.Model);
    }

    public static SetWorldObjectScaleRpcPacket CreateSetScalePacket(WorldObject worldObject)
    {
        return new SetWorldObjectScaleRpcPacket(worldObject.Id, worldObject.Scale);
    }

    public static DestroyAllWorldObjectsRpcPacket CreateDestroyAllPacket()
    {
        return DestroyAllWorldObjectsRpcPacket.Instance;
    }

    public static SetWorldObjectVisibileInAllDimensionsPacket CreateSetVisibleInAllDimensionsPacket(WorldObject worldObject)
    {
        return new SetWorldObjectVisibileInAllDimensionsPacket(worldObject.Id, worldObject.IsVisibleInAllDimensions);
    }
}
