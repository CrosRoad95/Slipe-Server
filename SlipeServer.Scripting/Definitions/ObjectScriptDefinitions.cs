using SlipeServer.Server;
using SlipeServer.Server.Elements;
using System;
using System.Numerics;

namespace SlipeServer.Scripting.Definitions
{
    public class ObjectScriptDefinitions
    {
        private readonly MtaServer server;
        private readonly ScriptHookContext scriptHookContext;

        public ObjectScriptDefinitions(MtaServer server, ScriptHookContext scriptHookContext)
        {
            this.server = server;
            this.scriptHookContext = scriptHookContext;
        }

        [ScriptFunctionDefinition("createObject")]
        public WorldObject CreateObject(ushort model, Vector3 position, Vector3? rotation = null, bool isLowLod = false)
        {
            WorldObject worldObject = new WorldObject(model, position)
            {
                Rotation = rotation ?? Vector3.Zero,
                IsLowLod = isLowLod
            }.AssociateWith(this.server);

            this.scriptHookContext.Hook?.ElementCreated(worldObject);
            return worldObject;
        }


    }
}
