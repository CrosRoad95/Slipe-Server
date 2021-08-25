using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlipeServer.Server.Elements.Events
{
    public abstract class ElementEventArgs<TElement> : EventArgs
    {
        public TElement Source { get; set; }

        public ElementEventArgs(TElement source)
        {
            this.Source = source;
        }
    }
}
