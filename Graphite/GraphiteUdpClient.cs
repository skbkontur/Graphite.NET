﻿using System;
using System.Diagnostics;
using SystemUdpClient = System.Net.Sockets.UdpClient;

namespace Graphite
{
    public class GraphiteUdpClient : IGraphiteClient, IDisposable
    {
        public string Hostname { get; private set; }
        public int Port { get; private set; }
        public string KeyPrefix { get; private set; }

        private readonly SystemUdpClient _udpClient;

        public GraphiteUdpClient(string hostname, int port = 2003, string keyPrefix = null)
        {
            Hostname = hostname;
            Port = port;
            KeyPrefix = keyPrefix;

            _udpClient = new SystemUdpClient(Hostname, Port);
        }

        public void Send(string path, int value, DateTime timeStamp)
        {
            try
            {
                var message = new PlaintextMessage(path, value, timeStamp).ToByteArray();

                _udpClient.Send(message, message.Length);
            }
            catch
            {
                // Supress all exceptions for now.
            }
        }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;

            if (_udpClient != null)
            {
                _udpClient.Close();
            }
        }

        #endregion
    }
}