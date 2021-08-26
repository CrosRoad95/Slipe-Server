using MoonSharp.Interpreter;
using SlipeServer.Scripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlipeServer.Lua
{
    public class LuaScript
    {
        public Script Script { get; set; }
        public IScriptHook? Hook { get; set; }
        public LuaScript(Script script, IScriptHook? hook = null)
        {
            this.Script = script;
            this.Hook = hook;
        }
    }
}
