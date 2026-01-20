using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;


namespace CPEI_MFG
{
    
    public partial class ConsoleMessage
    {
        public Process p;

        public ConsoleMessage(bool bCreateWindow)
        {
            cmdControlLogging = new CmdLogging(Cmd_LoggingToConsole);
            p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardInput = true;            
            p.StartInfo.CreateNoWindow = !bCreateWindow;
            p.Start();
        }
        ~ConsoleMessage()
        {
            p.Close();
        }
        public event System.EventHandler SendMessage;
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
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void CmdLogging(byte category, string msg);

        void Cmd_LoggingToConsole(byte category, string msg)
        {
            this.OnSendMessage(category, msg);
        }
        CmdLogging cmdControlLogging = null;
        public void SendCmd(string msg)
        {
            p.StandardInput.WriteLine(msg);
            WriteDebugMessage(msg);
        }
        public string ReadMessage()
        {
            string szBuffer = p.StandardOutput.ReadToEnd();
            WriteDebugMessage(szBuffer);
            return szBuffer;
        }
        public bool SendCmdAndWait(string cmd, string exp, int nTimeout)
        {
            string szBuffer = "";
            SendCmd(cmd);
            if (exp != null)
            {
                
                while (nTimeout > 0)
                {
                    szBuffer = ReadMessage();
                    if (szBuffer.Contains(exp))
                        break;
                    else
                        Thread.Sleep(1000);
                    nTimeout--;
                }
                if (nTimeout < 0)
                {
                    return false;
                }
            }
            else
            {
                Thread.Sleep(nTimeout * 1000);
            }

            return true;
        }

        public void WriteDebugMessage(string Msg)
        {
            string fullMessage;
            DateTime dt = DateTime.Now;
            fullMessage = dt.ToString("mm:ss.fff") + ", " + Msg;
            this.OnSendMessage(22, fullMessage);
        }
    }

}
