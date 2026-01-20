using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace CPEI_MFG
{
    public struct MTK_ATE_INFO 
    {
        public string cfgFile;
        public string iniFile;
        public string exeName;
        public string rawLog;
        public string csvLog;
        public string TempLog;
        public string PathLoss;
        
        public string usePort;
        

        public string LogPath;
        public string Caption;
        public int Timeout;
        public string completeMark;
        public string passMark;
        public string failMark;

        public int x_CalNSFT;
        public int y_CalNSFT;
        public int x_Calibration;
        public int y_Calibration;

        public bool bFinish;
        public bool bTestResult;

        
    }
    public partial class MTK_ATE_Program
    {
        public MTK_ATE_INFO mtk_Info;

        public event System.EventHandler SendMessage;


        protected virtual void OnSendMessage(byte category, string content)
        {
            if (null != SendMessage)
            {
                MessageEventArgs e = new MessageEventArgs();
                e.category = category;
                e.msgContent = content;
                SendMessage(this, e);
            }
        }
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void MTKLogging(byte category, string msg);

        void MTK_Logging_Control(byte category, string msg)
        {
            this.OnSendMessage(category, msg);
        }

        MTKLogging mtk_Logging = null;

        public MTK_ATE_Program(string filename)
        {
            ConfigMTK_ATE(filename);
            mtk_Logging = new MTKLogging(MTK_Logging_Control);   
        }

        public void ConfigMTK_ATE(string filename)
        {
            IniFile ateIni = new IniFile(filename);
            mtk_Info.Caption = ateIni.ReadString("MTK_ATE", "Title", "");
            mtk_Info.exeName = ateIni.ReadString("MTK_ATE", "ExeName", "");
            mtk_Info.rawLog = ateIni.ReadString("MTK_ATE", "RawLog", "");
            mtk_Info.TempLog = ateIni.ReadString("MTK_ATE", "TempLog", "");
            mtk_Info.cfgFile = ateIni.ReadString("MTK_ATE", "CFG_File", "");
            mtk_Info.iniFile = ateIni.ReadString("MTK_ATE", "iniFile", "");

            mtk_Info.LogPath = ateIni.ReadString("MTK_ATE", "LogPath", "");
            mtk_Info.completeMark = ateIni.ReadString("MTK_ATE", "CompleteMark", "");
            mtk_Info.passMark = ateIni.ReadString("MTK_ATE", "PassMark", "");
            mtk_Info.failMark = ateIni.ReadString("MTK_ATE", "FailMark", "");

            mtk_Info.PathLoss = ateIni.ReadString("MTK_ATE", "PathLoss", "");
            mtk_Info.Timeout = ateIni.ReadInt("MTK_ATE", "Timeout", 240);
            mtk_Info.x_CalNSFT = ateIni.ReadInt("MTK_ATE", "x_CalNSFT", 0);
            mtk_Info.y_CalNSFT = ateIni.ReadInt("MTK_ATE", "y_CalNSFT", 0);
            mtk_Info.x_Calibration = ateIni.ReadInt("MTK_ATE", "x_Calibration", 0);
            mtk_Info.y_Calibration = ateIni.ReadInt("MTK_ATE", "y_Calibration", 0);

            mtk_Info.bFinish = false;
            mtk_Info.bTestResult = false;

        }

        public void WriteDebugMessage(string Msg)
        {
            string fullMessage;
            DateTime dt = DateTime.Now;
            fullMessage = dt.ToString("hh:mm:ss.fff") + ", " + Msg + "\n";
            this.OnSendMessage(22, fullMessage);
        }

        public bool PreStart_MtkATE(ref string errorCode)
        {
            WriteDebugMessage("Pre start MTK ATE test");
            //Delete Last Log
            if (!DeleteLogFile(mtk_Info.LogPath,"*.*"))
            {
                errorCode = "SYFI05  Delete Last log file error";
                WriteDebugMessage(errorCode);
                return false;
            }
            
            //Check Last Log exist
            if (File.Exists(mtk_Info.rawLog) || File.Exists(mtk_Info.TempLog))
            {
                errorCode = "SYFI06 Check Last Log delete fail";
                WriteDebugMessage(errorCode);
                return false;
            }
            WriteDebugMessage("Delete Last Test Log OK...");

            //Check Execute file exists
            if (!File.Exists(mtk_Info.exeName))
            {
                errorCode = "SYFI07 Check MTK ATE Run file failed";
                WriteDebugMessage(errorCode);
                return false;
            }
            WriteDebugMessage("Check MTK ATE Run file OK...");
            return true;
        }

        public bool DeleteLogFile(string sDir, string expanded)
        {
            try
            {
                string[] txtList = Directory.GetFiles(sDir, expanded);

                foreach (string f in txtList)
                {
                    File.Delete(f);
                }
                return true;
            }
            catch (Exception e)
            {
                WriteDebugMessage(e.ToString());
                return false;
            }
        }

        public bool StartMtkATE(ref string errorCode)
        {
            
            IntPtr hMain = IntPtr.Zero;
            WindowControl winCtl = new WindowControl();
            int iRetry = 3;
            hMain = winCtl.GetMainWindow(null, mtk_Info.Caption);
            if (hMain != IntPtr.Zero)
            {
                // Click Cal&NSFT Button
                //winCtl.Pre_Click(hMain);
                //Thread.Sleep(1000);
                //winCtl.ClickTarget(mtk_Info.x_CalNSFT,mtk_Info.y_CalNSFT);
                winCtl.CloseSpecifiedWindow(hMain);

                ProcessStartInfo pStartInfo = new ProcessStartInfo();
                pStartInfo.FileName = mtk_Info.exeName;
                pStartInfo.UseShellExecute = false;
                pStartInfo.WindowStyle = ProcessWindowStyle.Normal;
                Process.Start(pStartInfo);
                Thread.Sleep(1000);
                int timeout = 10;
                hMain = winCtl.GetMainWindow(null, mtk_Info.Caption);
                while (hMain == IntPtr.Zero && timeout > 0)
                {
                    timeout--;
                    Thread.Sleep(1000);
                    hMain = winCtl.GetMainWindow(null, mtk_Info.Caption);
                }
                if (timeout <= 0)
                {
                    errorCode = "SYPR00 Open MTK ATE tool failed";
                    WriteDebugMessage(errorCode);
                    return false;
                }
                // Click Cal&NSFT Button
                winCtl.Pre_Click(hMain);
                Thread.Sleep(1000);
                winCtl.ClickTarget(mtk_Info.x_CalNSFT, mtk_Info.y_CalNSFT);
                
            }
            else
            {
            //start mte_ate
           
                ProcessStartInfo pStartInfo = new ProcessStartInfo();
                pStartInfo.FileName = mtk_Info.exeName;
                pStartInfo.UseShellExecute = false;
                pStartInfo.WindowStyle = ProcessWindowStyle.Normal;
                Process.Start(pStartInfo);
                Thread.Sleep(1000);
                int timeout = 10;
                hMain = winCtl.GetMainWindow(null, mtk_Info.Caption);
                while (hMain == IntPtr.Zero && timeout > 0)
                {
                    timeout--;
                    Thread.Sleep(1000);
                    hMain = winCtl.GetMainWindow(null, mtk_Info.Caption);
                }
                if (timeout <= 0)
                {
                    errorCode = "SYPR00 Open MTK ATE tool failed";
                    WriteDebugMessage(errorCode);
                    return false;
                }
                // Click Cal&NSFT Button
                winCtl.Pre_Click(hMain);
                Thread.Sleep(1000);
                winCtl.ClickTarget(mtk_Info.x_CalNSFT, mtk_Info.y_CalNSFT);
            }
            for (int i = 0; i < 10; i++ )
            {
                if (File.Exists(mtk_Info.TempLog))
                    break;
                else
                    Thread.Sleep(1000);
                if (9 == i)
                {
                    errorCode = "SYPR01 Click Cal&NSFT Button failed";
                    WriteDebugMessage(errorCode);
                    return false;
                }   
            }
            // Click Calibration Button

            CLICK_CAL:
            
            winCtl.ClickTarget(mtk_Info.x_Calibration, mtk_Info.y_Calibration);
            Thread.Sleep(3000);
            string buff = File.ReadAllText(mtk_Info.TempLog);
            if (!buff.Contains("Calibration Clicked"))
            {
                if (iRetry <= 0)
                {
                    errorCode = "SYPR02 Click Calibration Button failed";
                    WriteDebugMessage(errorCode);
                    return false;
                }
                iRetry--;
                goto CLICK_CAL;
            }
            WriteDebugMessage("Calibration Clicked OK,MTK ATE Tool Start Run");
            return true;
        }
        public string GetSpecifiedFile(string sDir, string extendedName,string expName)
        {
            string sFile = "";
            string[] txtList = Directory.GetFiles(sDir, extendedName);
            foreach (string f in txtList)
            {
                if (f.Contains(expName))
                {
                    sFile = f;
                    return sFile;
                }
            }
            return sFile;

        }
        public bool MonitorMTK_ATE(ref string errorCode)
        {
            WindowControl winCtl = new WindowControl();
            IntPtr hMain = IntPtr.Zero;
            mtk_Info.csvLog = "";
            mtk_Info.rawLog = "";
            for (int i = 0; i <= mtk_Info.Timeout; i++ )
            {
                Thread.Sleep(1000);
                hMain = winCtl.GetMainWindow(null, mtk_Info.Caption);
                if (hMain == IntPtr.Zero)
                {
                    errorCode = "SYPR03 can not find MTK ATE Tool Window";
                    WriteDebugMessage(errorCode);
                    return false;
                }
                string buff = File.ReadAllText(mtk_Info.TempLog);
                if (buff.Contains(mtk_Info.completeMark))
                {
                    mtk_Info.bFinish = true;
                    WriteDebugMessage("MTK ATE Tool Test finished");
                    return true;
                }
                mtk_Info.rawLog = GetSpecifiedFile(mtk_Info.LogPath, "*.txt", "MT0123456");
                mtk_Info.csvLog = GetSpecifiedFile(mtk_Info.LogPath, "*.csv", "MT0123456");
                
                if (mtk_Info.rawLog.Length != 0)
                {
                    mtk_Info.bFinish = true;
                    WriteDebugMessage("MTK ATE Tool Test finished");
                }
                if (i >= mtk_Info.Timeout)
                {
                    errorCode = "SYPR99 MTK ATE Tool Test Timeout";
                    WriteDebugMessage(errorCode);
                    return false;
                }
                                 
                WriteDebugMessage("Waiting for MTK ATE Tool Test running");
            }
            
            return true;
    
        }

        public bool CheckMTK_ATE_Result(ref string errorCode)
        {
            if (mtk_Info.rawLog.Length == 0)
            {
                mtk_Info.rawLog = GetSpecifiedFile(mtk_Info.LogPath, "*.txt", "MT0123456");
                if (mtk_Info.rawLog.Length == 0)
                {
                    errorCode = "SYPR88 can not find MTK ATE Tool Test log";
                    WriteDebugMessage(errorCode);
                    return false;
                }
            }
            string buff = File.ReadAllText(mtk_Info.rawLog, Encoding.Unicode);
            WriteDebugMessage(buff);
            if (mtk_Info.csvLog.Length != 0)
            {
                string buff2 = File.ReadAllText(mtk_Info.csvLog,Encoding.Unicode);
                WriteDebugMessage(buff2);
            }
            // Check Log PASS or FAIL
            if (buff.Contains(mtk_Info.passMark))
            {
                WriteDebugMessage("MTK ATE Tool Test PASS");
                mtk_Info.bTestResult = true;
                return true;
            }
            else
            {
                errorCode = PhaseErrorCode(buff, "Error Code:", "2G Tester:");
                return false;
            }        
        }

        public string PhaseErrorCode(string logContent, string errorStart, string errorEnd)
        {
            string errorCode = "MTKATE MTK ATE Tool Test failed";
            string buff = logContent.Substring(logContent.IndexOf(errorStart), logContent.IndexOf(errorEnd) - logContent.IndexOf(errorStart) - errorStart.Length);
            char[] errorArray = buff.ToArray();
            int i = 0;
            int num;
            string errorNum = "";
            for (i = 0; i < errorArray.Length; i++ )
            {
                
                bool isNum = Int32.TryParse(errorArray[i].ToString(),out num);
                if (isNum)
                {
                    continue;
                }
                else
                {
                    errorNum += num.ToString();
                }
            }
            if (errorNum.Length == 4)
            {
                errorCode = "MT" + errorNum + " " + buff.TrimEnd();
            }
            else if (errorNum.Length == 3)
            {
                errorCode = "MTK" + errorNum + " " + buff.TrimEnd();
            }
            else if(errorNum.Length == 5)
            {
                errorCode = "M" + errorNum + " " + buff.TrimEnd();
            }
            else
            {
                errorCode = "MTK999" + " " +buff.TrimEnd();
            }
            return errorCode;
        }
    }

}
