using SlipeServer.Server.Elements;
using SlipeServer.Server.Elements.ColShapes;
using SlipeServer.Server.Elements.Events;
using SlipeServer.Server.PacketHandling.Factories;
using SlipeServer.Server.Repositories;
using System.Collections.Generic;
using System.Numerics;

namespace SlipeServer.Server.Behaviour
{
    /// <summary>
    /// Behaviour responsible for triggering collision shape enter and exit events when an element's position changes
    /// </summary>
    public class CollisionShapeBehaviour : ElementBehaviourBase<CollisionShape>
    {
        public CollisionShapeBehaviour(MtaServer server, IElementRepository elementRepository) : base(server, elementRepository, ElementType.Colshape)
        {
        }


        protected override void OnElementAdded(CollisionShape collisionShape)
        {
            if (collisionShape is CollisionCircle collisionCircle)
            {
                collisionCircle.RadiusChanged += OnRadiusChange;
            }
        }

        protected override void OnElementCreate(Element element)
        {
            if (element is not CollisionShape)
            {
                element.PositionChanged += OnElementPositionChange;
            }
            base.OnElementCreate(element);
        }

        private void OnRadiusChange(Element sender, ElementChangedEventArgs<float> args)
        {
            this.server.BroadcastPacket(CollisionShapePacketFactory.CreateSetRadius(args.Source, args.NewValue));
        }

        private void RefreshColliders(Element element)
        {
            foreach (var shape in this.elements)
            {
                shape.CheckElementWithin(element);
            }
        }

        private void OnElementPositionChange(object sender, ElementChangedEventArgs<Vector3> eventArgs)
        {
            RefreshColliders(eventArgs.Source);
        }
    }
}
