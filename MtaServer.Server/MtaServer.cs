using MtaServer.Packets.Enums;
using MtaServer.Server.Elements;
using MtaServer.Server.PacketHandling;
using MtaServer.Server.Repositories;
using MTAServerWrapper.Server;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace MtaServer.Server
{
    public class MtaServer
    {
        private readonly NetWrapper netWrapper;
        private readonly PacketReducer packetReducer;
        private readonly Dictionary<NetWrapper, Dictionary<uint, Client>> clients;

        public Element Root { get; }
        public Logic.Commands Commands { get; }
        public Logic.Console Console { get; }
        public IElementRepository ElementRepository { get; private set; }

        public MtaServer(string directory, string netDllPath, string host, ushort port, IElementRepository elementRepository)
        {
            this.ElementRepository = elementRepository;

            this.Root = new Element();
            this.Console = new Logic.Console();
            this.Commands = new Logic.Commands(this);

            this.packetReducer = new PacketReducer();
            this.clients = new Dictionary<NetWrapper, Dictionary<uint, Client>>();

            this.netWrapper = CreateNetWrapper(directory, netDllPath, host, port);
        }

        public void onNetStarted()
        {
            Console.Output("Server started and is ready to accept connection!");
            Console.Output("Type 'help' for a list of commands.");
        }

        public void OnServerShuttingDown()
        {
            Console.Output("Stopping server...");
            Thread.Sleep(500);
        }

        public void Shutdown()
        {
            OnServerShuttingDown();
            Process.GetCurrentProcess().CloseMainWindow();
            Process.GetCurrentProcess().Close();
        }

        public void RegisterPacketQueueHandler(PacketId packetId, IQueueHandler queueHandler)
        {
            this.packetReducer.RegisterQueueHandler(packetId, queueHandler);
        }

        private NetWrapper CreateNetWrapper(string directory, string netDllPath, string host, ushort port)
        {
            NetWrapper netWrapper = new NetWrapper(directory, netDllPath, host, port);
            netWrapper.OnPacketReceived += EnqueueIncomingPacket;

            this.clients[netWrapper] = new Dictionary<uint, Client>();

            return netWrapper;
        }

        private void EnqueueIncomingPacket(NetWrapper netWrapper, uint binaryAddress, PacketId packetId, byte[] data)
        {
            if (!this.clients[netWrapper].ContainsKey(binaryAddress))
            {
                this.clients[netWrapper][binaryAddress] = new Client(binaryAddress, netWrapper);
            }
            this.packetReducer.EnqueuePacket(this.clients[netWrapper][binaryAddress], packetId, data);
        }
    }
}
