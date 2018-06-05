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
    public class Client
    {
        private UDPServer udpServer = new UDPServer();
        public event EventHandler<EventArgs> OnDataSended;
        public event OnErrorDelegate OnError;
        public void Send(string IP, int Port,string String)
        {
            try
            {
                byte[] buffer = Encoding.ASCII.GetBytes(String);
                IPEndPoint ipe = new IPEndPoint(IPAddress.Parse(IP), Port);
                SocketErrorCodes error = udpServer.SendDataAsync(buffer, buffer.Length,ipe, DataSended);
                if (error != SocketErrorCodes.SOCKET_OK)
                    throw new ApplicationException(SocketError.Text(error));
            }
            catch (Exception e)
            {
                ErrorLog.Error("UDPSocket send error {0}", e.Message);
                OnError(this, new OnErrorArgs(e));
            }
           
        }
        private void DataSended(UDPServer myUDPServer, int numberOfBytesSent)
        {
            OnDataSended(this, EventArgs.Empty);
        }

    }
}