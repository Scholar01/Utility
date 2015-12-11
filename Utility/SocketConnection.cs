using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Scholar.Net
{
   public class SocketConnection : IDisposable
    {
        private Socket socket;
        private EndPoint epServer;
        private byte[] receiveBuf = new byte[10240];
        public SocketConnection()
        {
            socket = GetSocket();
        }
        private Socket GetSocket()
        {
            if (socket == null)
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            }
            return socket;
        }
        public bool Connect(IPEndPoint ip)
        {
            if (socket != null && socket.Connected)
            {
                Dispose();
            }
            int retry = 0;
        Connect:
            try
            {
                socket = GetSocket();
                if (ip != null)
                    this.epServer = ip;
                socket.Connect(epServer);
                BeginDataReceive(this.socket);
                return true;
            }
            catch (Exception e)
            {
                retry++;
                if (retry < 3)
                {
                    goto Connect;
                }
                return false;
            }
        }
        protected void BeginDataReceive(Socket socket)
        {
            if (this.socket == null || !this.socket.Connected)
            {
                return;
            }
            receiveBuf.Initialize();
            socket.BeginReceive(receiveBuf, 0, receiveBuf.Length, SocketFlags.None, new AsyncCallback(EndDataReceive), socket);
        }
        protected void EndDataReceive(IAsyncResult ar)
        {
           
            int cnt = 0;
            try
            {
                Socket socket = (Socket)ar.AsyncState;
                cnt = socket.EndReceive(ar);
                if (cnt != 0)
                {
                    ByteBuffer byteBuffer = new ByteBuffer(receiveBuf, 0, cnt);
                    OnReceiveBuffer(byteBuffer);
                    receiveBuf.Initialize();
                }
                BeginDataReceive(socket);
            }
            catch (Exception e)
            {
                OnNetworkError(e);
            }
        }
        public virtual void BeginSendData(ByteBuffer sendBuf)
        {
            this.BeginSendData(sendBuf.ToByteArray());
        }
        internal void BeginSendData(byte[] buf)
        {
            try
            {

                socket.BeginSend(buf, 0, buf.Length, SocketFlags.None, new AsyncCallback(EndSendData), buf);
            }
            catch (Exception e)
            {
                OnNetworkError(e);
            }
        }
        protected virtual void EndSendData(IAsyncResult ar)
        {
            try
            {
                int sendCount = socket.EndSend(ar);
            }
            catch (Exception e)
            {
                OnNetworkError(e);
            }
        }

        /// <summary>
        /// 获取服务器ip
        /// </summary>
        /// <returns></returns>
        public static IPEndPoint GetServer(string host,int port)
        {
            IPAddress ipAddress;
            IPAddress.TryParse(host, out ipAddress);
            if (ipAddress == null)
            {
                try
                {

                    System.Net.IPHostEntry ipHostEntry = System.Net.Dns.GetHostEntry(host);
                    ipAddress = ipHostEntry.AddressList[0];
                }
                catch { }
            }
            return new IPEndPoint(ipAddress, port);
        }

        public void Dispose()
        {
            this.socket.Dispose();
        }

        public event EventHandler<EventArgs> ReceiveBufferEvent;
        public void OnReceiveBuffer(ByteBuffer buf)
        {
            if (ReceiveBufferEvent != null)
            {
                ReceiveBufferEvent(buf, new EventArgs());
            }
        }
        public event EventHandler<EventArgs> NetworkError;
        public void OnNetworkError(Exception e)
        {
            if (NetworkError != null)
            {
                NetworkError(e, new EventArgs());
            }
        }
    }
}
