using Microsoft.Extensions.Logging;
using MoonSharp.Interpreter;
using SlipeServer.Console.LuaDefinitions;
using SlipeServer.Lua;
using SlipeServer.Scripting;
using SlipeServer.Server.Services;
using System;
using System.Collections.Generic;
using System.IO;

namespace SlipeServer.Console
{
    public class LuaTestLogic
    {
        public LuaTestLogic(IScriptEventRuntime eventRuntime, LuaService luaService)
        {
            eventRuntime.LoadDefaultEvents();

            luaService.LoadDefaultDefinitions();

            luaService.LoadDefinitions<CustomMathDefinition>();
            luaService.LoadDefinitions<TestDefinition>();

            using FileStream testLua = File.OpenRead("test.lua");
            using StreamReader reader = new StreamReader(testLua);
            var hook = new LuaTestHook();
            try
            {
                luaService.LoadScript("test.lua", reader.ReadToEnd(), hook);
            }
            catch (InterpreterException ex)
            {
                System.Console.WriteLine("Failed to load script\n\t{0}", ex.DecoratedMessage);
            }
            System.Console.WriteLine("test.lua created: {0} elements.", hook.Counter);
        }
    }
}
