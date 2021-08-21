using SlipeServer.Packets.Definitions.Lua.ElementRpc.Ped;
using SlipeServer.Server.Elements;
using SlipeServer.Server.Elements.ColShapes;
using SlipeServer.Server.Elements.Events;
using SlipeServer.Server.Repositories;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;

namespace SlipeServer.Server.Behaviour
{
    public class RadarAreaBehaviour : ElementBehaviourBase<RadarArea>
    {
        public RadarAreaBehaviour(MtaServer server, IElementRepository elementRepository) : base(server, elementRepository, ElementType.RadarArea)
        {

        }

        protected override void OnElementAdded(RadarArea radarArea)
        {
            radarArea.ColorChanged += ColorChanged;
            radarArea.SizeChanged += SizeChanged;
            radarArea.FlashingStateChanged += FlashingStateChanged;
        }

        private void FlashingStateChanged(Element sender, ElementChangedEventArgs<RadarArea, bool> args)
        {
            this.server.BroadcastPacket(new SetRadarAreaFlashingPacket(args.Source.Id, args.NewValue));
        }

        private void ColorChanged(Element sender, ElementChangedEventArgs<RadarArea, Color> args)
        {
            this.server.BroadcastPacket(new SetRadarAreaColorPacket(args.Source.Id, args.NewValue));
        }

        private void SizeChanged(Element sender, ElementChangedEventArgs<RadarArea, Vector2> args)
        {
            this.server.BroadcastPacket(new SetRadarAreaSizePacket(args.Source.Id, args.NewValue));
        }
    }
}
