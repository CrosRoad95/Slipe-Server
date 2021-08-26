using SlipeServer.Server.Elements;
using System;

namespace SlipeServer.Server.Extensions
{
    public static class ElementExtensions
    {
        public static float DistanceTo(this Element from, Element to)
        {
            return (from.Position - to.Position).Length();
        }
    }
}
