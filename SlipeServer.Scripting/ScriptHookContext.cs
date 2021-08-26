using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlipeServer.Scripting
{
    public class ScriptHookContext
    {
        public IScriptHook? Hook { get; set; }
        public ScriptHookContext()
        {

        }
    }
}
