using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;
using System.IO;
using System.Windows.Forms;


namespace CPEI_MFG
{
    
    public partial class CmdMessage :Form
    {
        public Process p;
        public StringBuilder sortOutput;
        public StreamWriter sortStreamWriter;
        public string cmdName;
        public string cmdStdRecv;
        public string cmdErrRecv;
        public delegate void ReadDataStdOutput(string result);
        public delegate void ReadDataErrOutput(string result);
        public event ReadDataErrOutput delReadDataErrOutput;
        public event ReadDataStdOutput delReadDataStdOutput;

        public void ReadDataStdAction(string result)
        {
            cmdStdRecv += result;
        }
        public void ReadDataErrAction(string result)
        {
            cmdErrRecv += result;
        }
        public void Init()
        {
            delReadDataStdOutput += new ReadDataStdOutput(ReadDataStdAction);
            delReadDataErrOutput += new ReadDataErrOutput(ReadDataErrAction);
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

        public delegate void DELCmdClosing();
        public void CmdClosing()
        {
            if (this.InvokeRequired)
            {
                DELCmdClosing del = new DELCmdClosing(CmdClosing);
                this.Invoke(del);
            }
            else
            {
                this.CloseCmd();
            }
        }
        public CmdMessage(string exeName,bool bCreateWindow)
        {
            Init();
            cmdName = exeName;
            sortOutput = new StringBuilder("");
            cmdControlLogging = new CmdLogging(Cmd_LoggingToConsole);
            p = new Process();
            p.StartInfo.FileName = exeName;
            //p.StartInfo.Arguments = arg;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.CreateNoWindow = !bCreateWindow;
            p.StartInfo.ErrorDialog = false;
            p.StartInfo.RedirectStandardError = true;
            p.EnableRaisingEvents = true;
            p.OutputDataReceived += new DataReceivedEventHandler(SortOutputHandler);
            //p.OutputDataReceived += Process_OutputDataReceived;
            
            p.Exited += Process_Exited;
            p.ErrorDataReceived += new DataReceivedEventHandler(SortErrorHandler);
            p.Start();
            sortStreamWriter = p.StandardInput;
            p.BeginOutputReadLine();
            p.BeginErrorReadLine();
            
        }
        ~CmdMessage()
        {
               
        }
        public void Process_Exited(object sender, System.EventArgs e)
        {

        }
        
        
        public void CloseCmd()
        {
            //p.StandardInput.WriteLine("exit");
            
           // p.Dispose();
            p.Kill();
            p.WaitForExit();
            //p.Close();
            
        }
        public void SendCmd(string msg)
        {
            cmdStdRecv = "";
            p.StandardInput.WriteLine(msg);
            
            WriteDebugMessage("Send cmd: " + msg);
            //p.StandardOutput.ReadToEnd();
            return;
            
        }
        public string SendCmdAndRet(string msg)
        {
            cmdStdRecv = "";
            p.StandardInput.WriteLine(msg);

            WriteDebugMessage("Send cmd: " + msg);
            Thread.Sleep(1500);
            //p.StandardOutput.ReadToEnd();
            return cmdStdRecv;

        }
        public void SendChar(char cMsg)
        {
            p.StandardInput.Write(cMsg);
        }
        
        public bool SendCmdAndWait(string cmd, string exp, int nTimeout)
        {
            //string szBuffer = "";
            cmdStdRecv = "";
            SendCmd(cmd);

            WriteDebugMessage("Rec: " + cmdStdRecv);
            if (exp != null)
            {
                
                while (nTimeout > 0)
                {
                    //szBuffer = ReadMessage();
                    if (cmdStdRecv.Contains(exp))
                        break;
                    else
                    {
                        Thread.Sleep(1000);                     
                    }
                    nTimeout--;
                }
                if (nTimeout <= 0)
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
        public bool SendCmdAndWait(string cmd, string exp, int nTimeout,string unExp)
        {
            //string szBuffer = "";
            SendCmd(cmd);
            if (exp != null)
            {

                while (nTimeout > 0)
                {
                    //szBuffer = ReadMessage();
                    if (cmdStdRecv.Contains(exp))
                        break;
                    else if (cmdStdRecv.Contains(unExp))
                        return false;
                    else
                    {
                        Thread.Sleep(1000);
                    }
                    nTimeout--;
                }
                if (nTimeout <= 0)
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
            //DateTime dt = DateTime.Now;
            //fullMessage = dt.ToString("hh:mm:ss.fff") + ", " + Msg;
            fullMessage = Msg;
            this.OnSendMessage(22, fullMessage);
        }
        

        private  void SortOutputHandler(object sendingProcess,
            DataReceivedEventArgs outLine)
        {
            // Collect the sort command output.
            if (!String.IsNullOrEmpty(outLine.Data))
            {
               // numOutputLines++;
                WriteDebugMessage(outLine.Data);
                cmdStdRecv += outLine.Data;
                // Add the text to the collected output.
                //sortOutput.Append(Environment.NewLine +outLine.Data);
                
            }
        }
        private void SortErrorHandler(object sender, DataReceivedEventArgs e)
        {
            if (!String.IsNullOrEmpty(e.Data))
            {
                WriteDebugMessage(e.Data);
                cmdStdRecv += e.Data;
            }
        }
        
    }

}
