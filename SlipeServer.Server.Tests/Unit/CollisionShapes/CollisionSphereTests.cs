﻿using FluentAssertions;
using SlipeServer.Server.Elements.ColShapes;
using System.Numerics;
using Xunit;

namespace SlipeServer.Server.Tests.Unit.CollisionShapes;

public class CollisionSphereTests
{
    [Theory]
    [InlineData(1, 0, 0, 3)]
    [InlineData(1, 1, 0, 3)]
    [InlineData(1, 1, 1, 3)]
    public void PointWithinReturnsTrueTest(float x, float y, float z, float radius)
    {
        var shape = new CollisionSphere(Vector3.Zero, radius);

        var result = shape.IsWithin(new Vector3(x, y, z));

        result.Should().BeTrue();
    }

    [Theory]
    [InlineData(1, 0, 0, 0.5f)]
    [InlineData(0, 1, 0, 0.5f)]
    [InlineData(0, 0, 1, 0.5f)]
    public void PointOutsideReturnsFalseTest(float x, float y, float z, float radius)
    {
        var shape = new CollisionSphere(Vector3.Zero, radius);

        var result = shape.IsWithin(new Vector3(x, y, z));

        result.Should().BeFalse();
    }

    [Theory]
    [InlineData(0, 0, true)]
    [InlineData(1, 0, false)]
    [InlineData(0, 1, false)]
    public void PointInOtherInteriorOrDimension(byte interior, ushort dimension, bool isInside)
    {
        var shape = new CollisionSphere(Vector3.Zero, 5);

        var result = shape.IsWithin(Vector3.Zero, interior, dimension);

        result.Should().Be(isInside);
    }
}
