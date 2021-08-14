﻿using SlipeServer.Server.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlipeServer.Server.Elements.IdGeneration
{
    public class BasicElementIdGenerator : IElementIdGenerator
    {
        private uint idCounter;

        public BasicElementIdGenerator()
        {
            this.idCounter = 1;
        }

        public uint GetId()
        {
            this.idCounter = (this.idCounter + 1) % ElementConstants.MaxElementId;
            if (this.idCounter == 0)
                this.idCounter++;
            return this.idCounter;
        }
    }
}
