/*
MIT License

Copyright (c) 2018 Pavel Anpin

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharp.CrestronSockets;

namespace UDPSocket
{
    public class Server
    {
        
        #region "Variables"
        //public bool Debug { get; set; }
        private UDPServer udpServer;
        private Int32 BufferSize = 65535;
        private Int32 Port { get; set; }
        private IPEndPoint Endpoint { get; set; }
        private bool initialized = false;
        public bool Initialized
        {
            get
            {
                return initialized;
            }
        }
        private bool started = false;
        public bool Started
        {
            get
            {
                return started;
            }
        }
        #endregion
        #region "Classes"
        
        #endregion
        #region "Events"
        public class OnDataReceivedArgs : EventArgs
        {
            private string incomingDataBuffer;
            private string lastMsgReceivedFrom;

            public OnDataReceivedArgs(string incomingDataBuffer, string lastMsgReceivedFrom)
            {
                this.incomingDataBuffer = incomingDataBuffer;
                this.lastMsgReceivedFrom = lastMsgReceivedFrom;
            }
            public string IncomingDataBuffer
            {
                get
                {
                    return incomingDataBuffer;
                }
            }
            public string LastMsgReceivedFrom
            {
                get
                {
                    return lastMsgReceivedFrom;
                }
            }
        }
        
        public delegate void OnDataReceivedHandler(object sender, OnDataReceivedArgs Args);
       
        public event EventHandler<EventArgs> OnServerStart;
        public event EventHandler<EventArgs> OnServerStarted;
        public event EventHandler<EventArgs> OnServerStopping;
        public event EventHandler<EventArgs> OnServerStopped;
        public event OnDataReceivedHandler OnDataReceived;
        public event OnErrorDelegate OnError;

        #endregion
        #region "Functions"
        public void Init(String IpAddress, Int32 Port)
        {
            try
            {
                IPAddress ip = IPAddress.Parse(IpAddress);
                // Check the IpAddress to make sure that it is valid
                if (Port < 1024 && Port > 65535)
                {
                    throw new ArgumentException("The argument 'Port' is not valid. Please select a value greater than 1024 and less than 65535.");
                }
                else
                {
                    this.Port = Port;
                    this.Endpoint = new IPEndPoint(ip, Port);
                    initialized = true;
                }
            }
            catch (Exception e)
            {
                ErrorLog.Error("UDPSocket init error {0}", e.Message);
                OnError(this, new OnErrorArgs(e));
            }
        }
        public void Start()
        {
            try
            {
                OnServerStart(this, EventArgs.Empty);
                udpServer = new UDPServer(this.Endpoint, this.Port, this.BufferSize);
                SocketErrorCodes errorEnable = udpServer.EnableUDPServer(this.Endpoint.Address, this.Endpoint.Port);
                if (errorEnable == SocketErrorCodes.SOCKET_OK)
                {
                    OnServerStarted(this, EventArgs.Empty);
                    started = true;
                    SocketErrorCodes errorRx = udpServer.ReceiveDataAsync(DataReceived);
                    if(errorRx != SocketErrorCodes.SOCKET_OK)
                        throw new ApplicationException(SocketError.Text(errorRx));
                }
                else
                {
                    throw new ApplicationException(SocketError.Text(errorEnable));
                }

            }
            catch (Exception e)
            {
                ErrorLog.Error("UDPSocket start error {0}", e.Message);
                OnError(this, new OnErrorArgs(e));
            }
        }
        public void Stop()
        {
            try
            {
                OnServerStopping(this, EventArgs.Empty);
                SocketErrorCodes error = udpServer.DisableUDPServer();
                if (error == SocketErrorCodes.SOCKET_OK)
                {
                    OnServerStopped(this, EventArgs.Empty);
                    started = false;
                }
                else
                    throw new ApplicationException(SocketError.Text(error));
            }
            catch (Exception e)
            {
                ErrorLog.Error("UDPSocket stop error {0}", e.Message);
                OnError(this, new OnErrorArgs(e));
            }
        }
        private void DataReceived(UDPServer server, int numberOfBytesReceived)
        {
            try
            {
                if (server.DataAvailable)
                {

                    String buffer = Encoding.ASCII.GetString(server.IncomingDataBuffer, 0, numberOfBytesReceived);
                    OnDataReceived(this,new OnDataReceivedArgs(buffer, server.IPAddressLastMessageReceivedFrom));
                }
                SocketErrorCodes errorRx = udpServer.ReceiveDataAsync(DataReceived);
                if (errorRx != SocketErrorCodes.SOCKET_OK)
                    throw new ApplicationException(SocketError.Text(errorRx));
            }
            catch (Exception e)
            {
                ErrorLog.Error("UDPSocket receive error {0}", e.Message);
                OnError(this, new OnErrorArgs(e));
            }
        }
        #endregion

    }
}