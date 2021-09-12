using Force.Crc32;
using SlipeServer.Packets.Structs;
using SlipeServer.Server.Elements;
using SlipeServer.Server.Elements.Enums;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace SlipeServer.Server.Resources.Providers
{
    public class FileSystemResourceProvider : IResourceProvider
    {
        private readonly MtaServer mtaServer;
        private readonly RootElement rootElement;
        private readonly Configuration configuration;
        protected readonly Dictionary<string, Resource> resources;
        private ushort netId = 0;

        public FileSystemResourceProvider(MtaServer mtaServer, RootElement rootElement, Configuration configuration)
        {
            this.mtaServer = mtaServer;
            this.rootElement = rootElement;
            this.configuration = configuration;
            this.resources = new();
            this.Refresh();
        }

        public Resource GetResource(string name)
        {
            return this.resources[name];
        }

        public IEnumerable<Resource> GetResources()
        {
            return this.resources.Values;
        }

        public void Refresh()
        {
            var resources = IndexResourceDirectory(this.configuration.ResourceDirectory);
            this.resources.Clear();

            foreach (var resource in resources)
                this.resources[resource.Name] = resource;
        }

        private IEnumerable<Resource> IndexResourceDirectory(string directory)
        {
            var directories = Directory.EnumerateDirectories(directory, "*", SearchOption.TopDirectoryOnly);
            foreach (var subDirectory in directories)
            {
                if (subDirectory.StartsWith("[") && subDirectory.EndsWith("]"))
                    foreach (var resource in IndexResourceDirectory(subDirectory))
                        yield return resource;

                var name = Path.GetFileName(subDirectory)!;
                if (this.resources.ContainsKey(name))
                {
                    yield return this.resources[name];
                } else
                {
                    var resource = new Resource(this.mtaServer, this.rootElement, this, name, subDirectory)
                    {
                        NetId = this.netId++
                    };
                    yield return resource;
                }
            }
        }

        public virtual ResourceFile CreateResourceFileFromFile(string path, string file)
        {
            using (var md5 = MD5.Create())
            {
                byte[] content = File.ReadAllBytes(file);
                var hash = md5.ComputeHash(content);
                var checksum = Crc32Algorithm.Compute(content);

                string fileName2 = Path.GetRelativePath(Path.Join(this.configuration.ResourceDirectory, path), file);
                string fileName = Path.GetFileName(file);
                var fileType = (fileName.EndsWith(".lua") || fileName.EndsWith(".luac")) ? ResourceFileType.ClientScript : ResourceFileType.ClientFile;
                return new ResourceFile()
                {
                    Name = fileName,
                    AproximateSize = content.Length,
                    IsAutoDownload = fileType == ResourceFileType.ClientFile ? true : null,
                    CheckSum = checksum,
                    FileType = (byte)fileType,
                    Md5 = hash
                };
            }
        }

        public virtual IEnumerable<ResourceFile> GetFilesForResource(string name) => GetFilesForResource(this.resources[name]);

        public virtual IEnumerable<ResourceFile> GetFilesForResource(Resource resource)
        {
            string path = resource.Path;
            foreach (var file in Directory.GetFiles(path))
            {
                yield return CreateResourceFileFromFile(path, file);
            }
        }
    }
}
