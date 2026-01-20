using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;

namespace CPEI_MFG
{
    public partial class SocketClient
    {
        private int port;
        private string hostName;
        private Socket client; 
        private byte[] Buffer = new byte[256];
        private char[] czBuffer = new char[256];
        private string socketRev;
        private AsyncCallback ReceivedCallback = null;

        public event System.EventHandler SendMessage;
        protected virtual void OnSendMessage(byte category, string msg)
        {
            if (null != SendMessage)
            {
                MessageEventArgs e = new MessageEventArgs();
                e.category = category;
                e.msgContent = msg;
                SendMessage(this, e);
            }
        }
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void SocketClientLog(byte category, string msg);

         void SocketLogging(byte category, string msg)
        {
            this.OnSendMessage(category, msg);
        }
        SocketClientLog socketLog = null;
        public SocketClient(string hostName,int port)
        {
            this.hostName = hostName;
            this.port = port;
            socketLog = new SocketClientLog(SocketLogging);

        }
        public void Close()
        {          
            //client.Dispose();
            client.Disconnect(true);
            client = null;
        }
        
        public string UdpConnect(string cmd,int timeout = 3)
        {
            byte[] Data = new byte[1024];
            string strRecBuf;
            //richTextBox1.AppendText("Start UDPconnect\n");

            //设置服务端终结点
            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(this.hostName), this.port);
            
            //创建与服务端连接的套接字，指定网络类型，数据连接类型和网络协议
            Socket Server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            //string cmd = string.Concat("/root/mfg/ftmLabelTest 1 1");
            Data = Encoding.ASCII.GetBytes(cmd);
            //给服务端发送测试消息
            Server.SendTo(Data, Data.Length, SocketFlags.None, ipep);
            
            //服务端终结点
            IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
            EndPoint Remote = (EndPoint)sender;
            byte[] RecBufData = new byte[1024 * 1024 * 4];

            try
            {
                //对于不存在的IP地址，加入此行代码后，可以在指定时间内解除阻塞模式限制1
                Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, timeout*1000);
                int recv = Server.ReceiveFrom(RecBufData, ref Remote);
                strRecBuf = System.Text.Encoding.UTF8.GetString(RecBufData);
            }
            catch (System.Exception ex)
            {
                WriteDebugMessage(true, ex.ToString());
                strRecBuf = "Time Out";
            }
            WriteDebugMessage(true, strRecBuf.ToString());
            return (strRecBuf);
        }
        public bool Connect()
        {
            if (ReceivedCallback == null)
            {
                ReceivedCallback = new AsyncCallback(ReadFormSocket);
            }
            if (client == null)
            {
                IPAddress ipAddress = IPAddress.Parse(hostName);
                IPEndPoint ipe = new IPEndPoint(ipAddress, this.port);
                StateObject state = new StateObject();
                
                client = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    client.Connect(ipe);
                    state.workSocket = client;
                    
                    client.BeginReceive(Buffer, 0, this.Buffer.Length, SocketFlags.None, new AsyncCallback(ReceivedCallback), state);
                    return true;
                }
                catch (System.Exception ex)
                {
                    string exception = ex.ToString();
                    WriteDebugMessage(false,ex.ToString());
                    return false;
                }
                
            }
            else
                return true;
        }
        private void ClearBuffer(int bytesRead)
        {
            if (this.Buffer == null)
                return;
            for (int i = 0; i < bytesRead && i < this.Buffer.Length; i++)
            {
                this.Buffer[i] = 0;
            }
        }
        
        private void ReadFormSocket(IAsyncResult asyncResult)
        {
            StateObject state = (StateObject)asyncResult.AsyncState;
            if (asyncResult == null || client == null)
            {
                this.Close();
            }
            try
            {
                int bytesRead = client.EndReceive(asyncResult);
                if (bytesRead > 0)
                {
                    
                    //state.sb.Append(Encoding.ASCII.GetString(Buffer,0,bytesRead));
                    WriteDebugMessage(true,Encoding.ASCII.GetString(Buffer, 0, bytesRead));
                    ClearBuffer(bytesRead);
                    if (client == null)
                    {
                        return;
                    }
                    client.BeginReceive(Buffer, 0, this.Buffer.Length, SocketFlags.None, ReceivedCallback, state);
                } 
                else
                {
                    socketRev += state.sb.ToString();
                    WriteDebugMessage(true,Buffer.ToString());
                }
            }
            catch(System.Exception ex)
            {
                string exception = ex.ToString();
                WriteDebugMessage(false, Buffer.ToString());
            }
        }
        public void SendSocketMessage(string cmd)
        {
            if (client == null)
            {
                return;
            }
            byte[] bytesSend = Encoding.ASCII.GetBytes(cmd+"\r");
            client.Send(bytesSend, bytesSend.Length, 0);
        }
        private void WriteDebugMessage(bool isSocketMsg ,string msg)
        {
            string fullMsg = "";
            fullMsg = DateTime.Now.ToString("hh:mm:ss.fff") + " Socket: " + msg + "\n";
            if (isSocketMsg)
            {
                this.OnSendMessage(33, msg);
            }
            else
                this.OnSendMessage(22, fullMsg);
        }
    }
}
