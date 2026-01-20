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
    struct SocketINFO
    {
        public string serverIP;
        public int nPort;
    }
    public class StateObject
    {
        // Client socket.
        public Socket workSocket = null;
        // Size of receive buffer.
        public const int BufferSize = 256;
        // Receive buffer.
        public byte[] buffer = new byte[BufferSize];
        // Received data string.
        public StringBuilder sb = new StringBuilder();
    }

    public partial class Socket_control
    {
        SocketINFO socketInfo;
        Socket curSocket;
        public string socketRev;
        Thread SocketThread;
        private bool isConnect;

        public event System.EventHandler SendMessage;
        protected virtual void OnSendMessage(Byte category, string msgContent)
        {
            if (null != SendMessage)
            {
                MessageEventArgs e = new MessageEventArgs();
                e.category = category;
                e.msgContent = msgContent;
                SendMessage(this, e);
            }
        }
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void SocketLogging(byte category, string msg);

        void Socket_LoggingToConsole(byte category, string msg)
        {
            this.OnSendMessage(category, msg);
        }
        SocketLogging socketLog = null;

       
        public Socket_control()
        {
            //socketInfo = sInfo;
            socketLog = new SocketLogging(Socket_LoggingToConsole);
            isConnect = false;
        }

        private void WriteDebugMessage(string msg)
        {
            string fullMsg = "";
            fullMsg = DateTime.Now.ToString("hh:mm:ss.fff") + " " + msg +"\n";
            this.OnSendMessage(22, fullMsg);
        }
        

        public  bool ConnectSocket(string server, int port)
        {
            Socket s = null;
            IPHostEntry hostEntry = null;

            hostEntry = Dns.GetHostEntry(server);
            foreach (IPAddress address in hostEntry.AddressList)
            {
                if (address.IsIPv6SiteLocal || address.IsIPv6LinkLocal)
                {
                    continue;
                }
                IPEndPoint ipe = new IPEndPoint(address, port);
                Socket tempSocket = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    tempSocket.Connect(ipe);
                }
                catch (System.Exception ex)
                {
                    WriteDebugMessage(ex.ToString());	
                }
                
                
                if (tempSocket.Connected)
                {
                    s = tempSocket;
                    socketRev = "";
                    WriteDebugMessage("---> connect socket " + ipe.AddressFamily.ToString());
                    
                }
                else
                    continue;
            }
            if (s != null)
            {
                curSocket = s;
                isConnect = true;
                
                return true;
            }
            else
            {
                curSocket = null;
                return false;
            }            
        }

        public bool SocketSendAndReceive(string cmd , int timeout,string exp)
        {
            Byte[] bytesSent = Encoding.ASCII.GetBytes(cmd);
            Byte[] bytesReceived = new Byte[256];
            string szBuffer = "";
            socketRev = "";
            curSocket.Send(bytesSent, bytesSent.Length, 0);
            WriteDebugMessage("--->  " + cmd);
            int iTimeout = timeout*10;
            if (SocketThread != null)
            {
                while (iTimeout > 0)
                {
                    Thread.Sleep(100);
                    if (socketRev.Contains(exp))
                    {
                        break;
                    }
                    iTimeout--;

                }
                if (iTimeout <= 0)
                {
                    WriteDebugMessage("Can not receive expect message");
                    return false;
                }
            }
            else
            {
                int bytes = 0;
                do
                {
                    bytes = curSocket.Receive(bytesReceived, bytesReceived.Length, 0);
                    szBuffer += Encoding.ASCII.GetString(bytesReceived, 0, bytes);
                    WriteDebugMessage("<---- " + szBuffer);
                } while (bytes <= 0);
                if (!szBuffer.Contains(exp))
                {
                    WriteDebugMessage("Can not receive expect message");
                    return false;
                }
            }


            return true ;
        }

        public void SocketSendMessage(string cmd)
        {
            Byte[] bytesSent = Encoding.ASCII.GetBytes(cmd);                  
            //socketRev = "";
            curSocket.Send(bytesSent, bytesSent.Length, 0);
        }
        public void SocketReceive()
        {
            StateObject state = new StateObject();
            state.workSocket = curSocket;
            
            curSocket.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
            
            
            //this.com_SFIS.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.com_SFIS_DataReceived);
            
        }
        private void ReceiveCallback(IAsyncResult ar)
        {
            if (curSocket == null)
            {
                return;
            }
            try
            {
                // Retrieve the state object and the client socket 
                // from the asynchronous state object.
                StateObject state = (StateObject)ar.AsyncState;
                Socket client = state.workSocket;

                // Read data from the remote device.
                int bytesRead = client.EndReceive(ar);                   
                if (bytesRead > 0)
                {
                    // There might be more data, so store the data received so far.
                    state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));
                    if (client == null)
                    {
                        return;
                    }
                    // Get the rest of the data.
                    client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                        new AsyncCallback(ReceiveCallback), state);
                }
                else
                {
                    // All the data has arrived; put it in response.
                    if (state.sb.Length > 1)
                    {
                        socketRev += state.sb.ToString();
                        WriteDebugMessage(socketRev);
                    }
                    // Signal that all bytes have been received.
                   
                } 
            }
            catch (Exception e)
            {
                WriteDebugMessage(e.ToString());
            }
            
            
        }
        public void StartRec()
        {
            SocketThread = new Thread(Received);
            SocketThread.IsBackground = true;
            SocketThread.Start();
        }

        private void Received()
        {
            try
            {
                Socket accept = curSocket;
                while (true && isConnect)
                {
                    byte[] rec = new byte[4900];
                    accept.Receive(rec);
                    string buff = Encoding.ASCII.GetString(rec);
                    if (buff.Length > 0)
                    {
                        buff = buff.Trim('\0');
                        WriteDebugMessage(buff);
                        socketRev += buff;
                    }
                    
                }
            }
            catch (Exception e)
            {
                WriteDebugMessage(e.ToString());
            }
        }

        public void SocketDisConnect()
        {
            isConnect = false;
            if (SocketThread != null)
            {
                //SocketThread.Abort();
            }
            curSocket.Disconnect(true);
            curSocket = null;
        }
    }

   
}
