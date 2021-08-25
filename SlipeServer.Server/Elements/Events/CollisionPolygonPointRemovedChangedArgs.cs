using SlipeServer.Server.Elements.ColShapes;
using SlipeServer.Server.Enums;
using System;
using System.Numerics;

namespace SlipeServer.Server.Elements.Events
{
    public class CollisionPolygonPointRemovedChangedArgs : ElementEventArgs<CollisionPolygon>
    {
        public int Index { get; set; }

        public CollisionPolygonPointRemovedChangedArgs(CollisionPolygon polygon, int index) : base(polygon)
        {
            this.Index = index;
        }
    }
}
