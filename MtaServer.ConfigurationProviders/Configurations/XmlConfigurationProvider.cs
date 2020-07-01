﻿using MtaServer.Server;
using System.Xml;

namespace MtaServer.ConfigurationProviders.Configurations
{
    public class XmlConfigurationProvider : IConfigurationProvider
    {
        public Configuration configuration { get; private set; }
        public Configuration GetConfiguration() => configuration;

        public XmlConfigurationProvider(string fileName)
        {
            configuration = new Configuration();
            XmlDocument xmlConfig = new XmlDocument();
            xmlConfig.Load(fileName);

            ushort result;

            foreach (XmlNode node in xmlConfig.FirstChild.ChildNodes)
            {
                switch(node.Name)
                {
                    case "serverName":
                        configuration.ServerName = node.InnerText;
                        break;
                    case "host":
                        configuration.Host = node.InnerText;
                        break;
                    case "port":
                        if (ushort.TryParse(node.InnerText, out result))
                            configuration.Port = result;

                        break;
                    case "maxPlayers":
                        if (ushort.TryParse(node.InnerText, out result))
                            configuration.MaxPlayers = result;

                        break;
                    case "password":
                        configuration.Password = node.InnerText;
                        break;
                }
            }
        }
    }
}
