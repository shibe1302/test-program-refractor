using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace CPEI_MFG
{
    public partial class SocketServer
    {
        public Socket socketServer;
        Thread threadService;
        ManualResetEvent manager = new ManualResetEvent(false);
        ManualResetEvent reviceManager = new ManualResetEvent(false);

        public event System.EventHandler SendMessage;
        public delegate void SocketServerLog(byte category, string msgContent);
        SocketServerLog socketServertLog = null;

        public string socketRecv;
        protected virtual void OnSendMessage(byte category, string msgContent)
        {
            if (null != SendMessage)
            {
                MessageEventArgs e = new MessageEventArgs();
                e.category = category;
                e.msgContent = msgContent;
                SendMessage(this, e);
            }
        }
        private void WriteDebugMessage(string msg)
        {
            this.OnSendMessage(22, msg);
        }

        public SocketServer(string ipAddress,int nPort)
        {
            socketServertLog += new SocketServerLog(OnSendMessage); 
            try
            {
                var address = IPAddress.Parse(ipAddress);
                var ipPoint = new IPEndPoint(address, nPort);
                socketServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socketServer.Bind(ipPoint);
                socketServer.Listen(100);
                threadService = new Thread(StartSocketService);
                threadService.IsBackground = true;
                threadService.Start();
            }
            catch (System.Exception ex)
            {
                WriteDebugMessage(ex.Message);	
            }
        }
        private void StartSocketService()
        {
            while (true)
            {
                try
                {
                    //將事件狀態設置為非終止狀態, 導致線程阻塞
                    manager.Reset();
                    //開始監聽客戶端連接請求
                    var args = new SocketAsyncEventArgs();
                    args.Completed += args_Completed;
                    socketServer.AcceptAsync(args);
                    //阻止當前線程, 直到WaitHandle 信號
                    manager.WaitOne();
                }
                catch (Exception ex)
                {
                    WriteDebugMessage(ex.Message);
                }
            }
        }
        private void args_Completed(object sender, SocketAsyncEventArgs e)
        {
            //監聽完成客戶端的請求, 一旦監聽到返回新的套接字
            var clientSocket = e.AcceptSocket;
            if (clientSocket == null) return;
            //啟動線程獲取客戶端發來的消息
            var t = new Thread(GetClientMsg);
            t.IsBackground = true;
            t.Start(clientSocket);
            WriteDebugMessage(clientSocket.RemoteEndPoint + "  Connected");
            //將事件狀態設置為終止狀態, 允許一個或多個等待線程繼續
            manager.Set();
        }
        private void GetClientMsg(object socket)
        {
            var socketClient = socket as Socket;
            if (socketClient == null) return;
            while (true)
            {
                try
                {
                    reviceManager.Reset();
                    var bytes = new byte[1024 * 1024 * 4];
                    var receiveArgs = new SocketAsyncEventArgs();
                    receiveArgs.SetBuffer(bytes, 0, bytes.Length);
                    //開始異步接收
                    receiveArgs.Completed += receiveArgs_Completed;
                    reviceManager.WaitOne();
                }
                catch (System.Exception ex)
                {
                    WriteDebugMessage(ex.Message);
                }
            }
        }
        //接收消息完成回調事件
        private void receiveArgs_Completed(object sender,SocketAsyncEventArgs e)
        {
            var sockerClient = sender as Socket;
            var bytes = e.Buffer;
            string result = System.Text.Encoding.UTF8.GetString(bytes);
            this.socketRecv += result;
            WriteDebugMessage(result);
            reviceManager.Set();
        }

        public void SendCmd(string cmd)
        {
            if (socketServer != null)
            {
                socketRecv = "";
                byte[] b = System.Text.Encoding.UTF8.GetBytes(cmd);
                socketServer.Send(b);

            }
        }
        public bool SendCmdAndWait(string cmd, string exp, int timeout)
        {
            if (socketServer != null)
            {
                socketRecv = "";
                byte[] b = System.Text.Encoding.UTF8.GetBytes(cmd);
                socketServer.Send(b);
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
