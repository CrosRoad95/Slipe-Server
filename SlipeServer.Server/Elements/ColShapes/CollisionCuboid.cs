﻿using SlipeServer.Server.Elements.Events;
using System.Numerics;

namespace SlipeServer.Server.Elements.ColShapes;

public class CollisionCuboid : CollisionShape
{
    private Vector3 dimensions;
    public Vector3 Dimensions
    {
        get => this.dimensions;
        set
        {
            var args = new ElementChangedEventArgs<Vector3>(this, this.dimensions, value, this.IsSync);
            this.dimensions = value;
            DimensionsChanged?.Invoke(this, args);
        }
    }


    public CollisionCuboid(Vector3 position, Vector3 dimensions)
    {
        this.Position = position;
        this.dimensions = dimensions;
    }

    public override bool IsWithin(Vector3 position, byte? interior = null, ushort? dimension = null)
    {
        if ((interior != null && this.Interior != interior) || (dimension != null && this.Dimension != dimension))
            return false;

        Vector3 bounds = this.Position + this.Dimensions;

        return
            position.X > this.Position.X && position.X < bounds.X &&
            position.Y > this.Position.Y && position.Y < bounds.Y &&
            position.Z > this.Position.Z && position.Z < bounds.Z;
    }

    public new CollisionCuboid AssociateWith(MtaServer server)
    {
        base.AssociateWith(server);
        return this;
    }

    public event ElementChangedEventHandler<Vector3>? DimensionsChanged;
}
