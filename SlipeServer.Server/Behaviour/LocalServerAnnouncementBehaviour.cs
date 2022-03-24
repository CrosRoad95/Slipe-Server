﻿using Microsoft.Extensions.Logging;
using SlipeServer.Server.AllSeeingEye;
using SlipeServer.Server.Constants;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SlipeServer.Server.Behaviour
{
    /// <summary>
    /// Behaviour responsible for responding to local server list queries
    /// </summary>
    public class LocalServerAnnouncementBehaviour
    {
        private readonly Configuration configuration;
        private readonly ILogger logger;

        public LocalServerAnnouncementBehaviour(Configuration configuration, ILogger logger)
        {
            this.configuration = configuration;
            this.logger = logger;

            StartListening(NetworkConstants.LanPort);
        }

        private void OnUdpData(IAsyncResult result)
        {
            if (result.AsyncState is UdpClient socket)
            {
                try
                {
                    IPEndPoint? source = new IPEndPoint(0, 0);
                    byte[] incomingData = socket.EndReceive(result, ref source);
                    string message = Encoding.UTF8.GetString(incomingData);
                    this.logger.LogInformation($"Local server broadcast received from {source?.Address} \"{message}\"");

                    byte[] data = Encoding.UTF8.GetBytes($"MTA-SERVER {this.configuration.Port + 123}");

                    socket.Send(data, data.Length, source);
                    socket.BeginReceive(new AsyncCallback(OnUdpData), socket);
                }
                catch (Exception e)
                {
                    this.logger.LogError(e.Message);
                }
            }
        }

        private void StartListening(ushort port)
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);

            UdpClient socket = new UdpClient(port);
            socket.BeginReceive(new AsyncCallback(OnUdpData), socket);
        }

    }
}
