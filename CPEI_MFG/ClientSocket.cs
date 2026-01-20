using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
using System.Net.Sockets;
using System.Net;

namespace CPEI_MFG
{
    public partial class ClientSocket
    {
        public Socket socketClient;
        private Thread clientThread;
        public string socketRecv;

        public event System.EventHandler SendMessage;
        protected virtual void OnSendMessage(byte category, string msgContent)
        {
            if (null != SendMessage)
            {
                MessageEventArgs e = new MessageEventArgs();
                e.category = category;
                e.msgContent = msgContent;
                this.SendMessage(this, e);
            }
        }
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SocketClientLog(byte category, string msg);
        private void SocketClientLoggging(byte categoty, string msg)
        {
            this.OnSendMessage(categoty, msg);
        }
        SocketClientLog socketClientLog = null;
        public ClientSocket(string ipAddress, int nPort)
        {
            socketClientLog += new SocketClientLog(OnSendMessage);

            try
            {
                socketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress ip = IPAddress.Parse(ipAddress);
                IPEndPoint ipPoint = new IPEndPoint(ip, nPort);
                socketClient.Connect(ipPoint);
                clientThread = new Thread(ReceivedService);
                clientThread.IsBackground = true;
                clientThread.Start();
            }
            catch (System.Exception ex)
            {
                	
            }
        }

        private void ReceivedService()
        {
            while (true)
            {
                
                var b = new byte[1024 * 1024 * 4];
                int length = socketClient.Receive(b);
                var msg = System.Text.Encoding.UTF8.GetString(b, 0, length);
                WriteDebugMessage(msg);
                socketRecv += msg;
                
            }
        }
        private void WriteDebugMessage( string msg)
        {
            this.OnSendMessage(22,msg);
        }
        public void SendCmd(string cmd)
        {
            if (socketClient != null)
            {
                socketRecv = "";
                byte[] b = System.Text.Encoding.UTF8.GetBytes(cmd);
                socketClient.Send(b);

            }
        }
        public bool SendCmdAndWait(string cmd, string exp, int timeout)
        {
            if (socketClient != null)
            {
                socketRecv = "";
                byte[] b = System.Text.Encoding.UTF8.GetBytes(cmd);
                socketClient.Send(b);
                while (timeout > 0)
                {
                    if (socketRecv.Contains(exp))
                        return true;
                    else
                        Thread.Sleep(1000);
                    timeout--;
                }
                return false;
            }
            else
                return false;
        }
    }
}
