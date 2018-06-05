using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharp.CrestronSockets;

namespace UDPSocket
{
    class SocketError
    {
        public static string Text(SocketErrorCodes e)
        {
            switch (e)
            {
                case SocketErrorCodes.SOCKET_OK:
                    return String.Format("Error [{0}] Success", e);
                case SocketErrorCodes.SOCKET_INVALID_STATE:
                    return String.Format("Error [{0}] Client, udpServer or UDP not in initial state", e);
                case SocketErrorCodes.SOCKET_NO_HOSTNAME_RESOLVE:
                    return String.Format("Error [{0}] Could not resolve specified hostname", e);
                case SocketErrorCodes.SOCKET_INVALID_PORT_NUMBER:
                    return String.Format("Error [{0}] Port not in range of 0-65535", e);
                case SocketErrorCodes.SOCKET_NOT_CONNECTED:
                    return String.Format("Error [{0}] Unable to establish a connection", e);
                case SocketErrorCodes.SOCKET_BUFFER_NOT_ALLOCATED:
                    return String.Format("Error [{0}] Unable to allocate socket buffer", e);
                case SocketErrorCodes.SOCKET_ADDRESS_NOT_SPECIFIED:
                    return String.Format("Error [{0}] Address not specified", e);
                case SocketErrorCodes.SOCKET_OUT_OF_MEMORY:
                    return String.Format("Error [{0}] Out of memory", e);
                case SocketErrorCodes.SOCKET_CONNECTION_IN_PROGRESS:
                    return String.Format("Error [{0}] Socket connection in progress", e);
                case SocketErrorCodes.SOCKET_NOT_ALLOWED_IN_SECURE_MODE:
                    return String.Format("Error [{0}] Sockets are not allowed in the secure mode", e);
                case SocketErrorCodes.SOCKET_SPECIFIED_PORT_ALREADY_IN_USE:
                    return String.Format("Error [{0}] Specified port is already in use", e);
                case SocketErrorCodes.SOCKET_INVALID_CLIENT_INDEX:
                    return String.Format("Error [{0}] Client (socket) index is invalid", e);
                case SocketErrorCodes.SOCKET_MAX_CONNECTIONS_REACHED:
                    return String.Format("Error [{0}] Client connections reached the MAX", e);
                case SocketErrorCodes.SOCKET_INVALID_ADDRESS_ADAPTER_BINDING:
                    return String.Format("Error [{0}] Address specified and the EthernetAdapterToBindTo do not match", e);
                case SocketErrorCodes.SOCKET_OPERATION_PENDING:
                    return String.Format("Error [{0}] Socket operation is pending", e);
                default:
                    return String.Format("Error [{0}] Unexpected error code", e);
            }
        }
    }

    public class OnErrorArgs : EventArgs
    {
        private Exception e;
        public OnErrorArgs(Exception e)
        {
            this.e = e;
        }
        public Exception Exception
        {
            get
            {
                return e;
            }
        }

    }
    public delegate void OnErrorDelegate(object sender, OnErrorArgs Args);
        
}