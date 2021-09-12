using SlipeServer.Packets.Structs;
using SlipeServer.Server;
using SlipeServer.Server.Elements;
using SlipeServer.Server.Resources;
using SlipeServer.Server.Resources.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlipeServer.Console.Providers
{
    public class ExtendedFileSystemResourceProvider : FileSystemResourceProvider
    {
        public ExtendedFileSystemResourceProvider(MtaServer mtaServer, RootElement rootElement, Configuration configuration) : base(mtaServer, rootElement, configuration)
        {

        }

        public override IEnumerable<ResourceFile> GetFilesForResource(Resource resource)
        {
            yield return CreateResourceFileFromFile("./Resources/TestResource", "./Resources/global.lua");
            foreach (var item in base.GetFilesForResource(resource))
            {
                yield return item;
            }
        }
    }
}
