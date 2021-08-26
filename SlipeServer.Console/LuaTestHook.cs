using SlipeServer.Lua;
using SlipeServer.Scripting;
using SlipeServer.Server.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlipeServer.Console
{
    public class LuaTestHook : IScriptHook
    {
        public int Counter { get; set; }
        public void ElementCreated(Element element)
        {
            Counter++;
        }
    }
}
