using SlipeServer.Server.Elements.ColShapes;
using SlipeServer.Server.Enums;
using System;
using System.Numerics;

namespace SlipeServer.Server.Elements.Events
{
    public class CollisionPolygonPointPositionChangedArgs : ElementEventArgs<CollisionPolygon>
    {
        public uint Index { get; set; }
        public Vector2 Position { get; set; }

        public CollisionPolygonPointPositionChangedArgs(CollisionPolygon polygon, uint index, Vector2 position) : base(polygon)
        {
            this.Index = index;
            this.Position = position;
        }
    }
}
