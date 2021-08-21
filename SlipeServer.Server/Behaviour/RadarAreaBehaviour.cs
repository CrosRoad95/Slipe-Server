﻿using SlipeServer.Packets.Definitions.Lua.ElementRpc.Ped;
using SlipeServer.Server.Elements;
using SlipeServer.Server.Elements.ColShapes;
using SlipeServer.Server.Elements.Events;
using SlipeServer.Server.Repositories;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;

namespace SlipeServer.Server.Behaviour
{
    public class RadarAreaBehaviour
    {
        private readonly MtaServer server;
        private readonly HashSet<RadarArea> radarAreas;

        public RadarAreaBehaviour(MtaServer server, IElementRepository elementRepository)
        {
            this.server = server;
            this.radarAreas = new HashSet<RadarArea>();
            foreach (var radarArea in elementRepository.GetByType<RadarArea>(ElementType.RadarArea))
            {
                AddRadarArea(radarArea);
            }

            server.ElementCreated += OnElementCreate;
        }

        private void OnElementCreate(Element element)
        {
            if (element is RadarArea radarArea)
            {
                AddRadarArea(radarArea);
            }
        }

        private void AddRadarArea(RadarArea radarArea)
        {
            this.radarAreas.Add(radarArea);
            radarArea.Destroyed += (source) => this.radarAreas.Remove(radarArea);
            radarArea.ColorChanged += ColorChanged;
            radarArea.SizeChanged += SizeChanged;
            radarArea.FlashingStateChanged += FlashingStateChanged;
        }

        private void FlashingStateChanged(Element sender, ElementChangedEventArgs<RadarArea, bool> args)
        {
            if (!args.IsSync)
                this.server.BroadcastPacket(new SetRadarAreaFlashingPacket(args.Source.Id, args.NewValue));
        }

        private void ColorChanged(Element sender, ElementChangedEventArgs<RadarArea, Color> args)
        {
            if (!args.IsSync)
                this.server.BroadcastPacket(new SetRadarAreaColorPacket(args.Source.Id, args.NewValue));
        }

        private void SizeChanged(Element sender, ElementChangedEventArgs<RadarArea, Vector2> args)
        {
            if (!args.IsSync)
                this.server.BroadcastPacket(new SetRadarAreaSizePacket(args.Source.Id, args.NewValue));
        }
    }
}
