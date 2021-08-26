﻿using Microsoft.Extensions.DependencyInjection;
using SlipeServer.Scripting;
using System;

namespace SlipeServer.Lua
{
    public static class LuaServiceCollectionExtensions
    {
        public static void AddLua(this ServiceCollection services)
        {
            services.AddSingleton<IScriptEventRuntime, ScriptEventRuntime>();
            services.AddSingleton<ScriptHookContext>();
            services.AddSingleton<LuaService>();
        }

        public static void AddLua<T>(this ServiceCollection services) where T: class, IScriptEventRuntime
        {
            services.AddSingleton<IScriptEventRuntime, T>();
            services.AddSingleton<ScriptHookContext>();
            services.AddSingleton<LuaService>();
        }
    }
}
