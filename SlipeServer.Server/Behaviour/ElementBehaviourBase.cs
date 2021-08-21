using SlipeServer.Server.Elements;
using SlipeServer.Server.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlipeServer.Server.Behaviour
{
    public abstract class ElementBehaviourBase<TElement> where TElement : Element
    {
        protected readonly HashSet<TElement> elements;
        protected readonly MtaServer server;

        public ElementBehaviourBase(MtaServer server, IElementRepository elementRepository, ElementType elementType)
        {
            this.elements = new HashSet<TElement>();
            this.server = server;

            foreach (var collisionShape in elementRepository.GetByType<TElement>(elementType))
            {
                AddElement(collisionShape);
            }

            server.ElementCreated += OnElementCreate;
        }

        protected abstract void OnElementAdded(TElement element);
        protected virtual void OnElementRemoved(TElement element) { }

        protected virtual void OnElementCreate(Element element)
        {
            if (element is TElement targetElement)
            {
                AddElement(targetElement);
                OnElementAdded(targetElement);
            }
        }

        private void AddElement(TElement element)
        {
            this.elements.Add(element);
            element.Destroyed += (source) =>
            {
                this.elements.Remove(element);
                OnElementRemoved(element);
            };
        }
    }
}
