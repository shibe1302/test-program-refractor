using System;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Threading;
using System.Runtime.InteropServices;
using System.Diagnostics;

using System.IO.Ports;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Xml;
using FluentFTP;
using CPEI_MFG.Core;
using CPEI_MFG.CORE;
using CPEI_MFG.Infrastructure;

namespace CPEI_MFG
{
    public partial class Form1 : Form
    {

        // ==========================================================
        // KHAI BÁO CÁC ĐỐI TƯỢNG DỮ LIỆU (MỚI)
        // ==========================================================

        // Khởi tạo các class từ thư mục Core
        private StationInfo stpInfo = new StationInfo();
        private TestInfo curTestInfo = new TestInfo();
        private TestResult testResult = new TestResult(); 
        private LaserInfo laserInfo = new LaserInfo();
        private NustreamInfo nustreamInfo = new NustreamInfo();
        private EndInfo endInfo = new EndInfo();
        private Etab5Test eTAB5Test = new Etab5Test();
        private NecTest nECTest = new NecTest();
        MFG_DialogWindow DialogWindow = new MFG_DialogWindow();
        CmdMessage m_pAdbCMD;

        [DllImport("user32.dll", EntryPoint = "FindWindowEx", CharSet = CharSet.Auto)]
        private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);
        [DllImport("user32.dll", EntryPoint = "keybd_event")]
        private static extern int keybd_event
            (
           byte bVk,
           byte bScan,
           int dwFlags,
           int dwExtraInfo
            );

        string csvTile = "", csvText = "";

        int useTimePerItem = 0;
        private void AddCsvContext(string tile, string context)
        {
            if (csvTile.Length != 0)
            {
                csvTile = csvTile + ",";
                csvText = csvText + ",";
            }
            csvTile = csvTile + tile;
            csvText = csvText + context;
        }
        
   
        delegate void SetTextCallback(string msg, int iColor = 0);
        private void SetText(string msg, int iColor = 0)
        {
            if (this.richTextBox1.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { msg, iColor });
            }
            else
            {
                if (iColor == 1)
                {
                    this.richTextBox1.SelectionColor = Color.Blue;
                    this.richTextBox1.SelectionStart = richTextBox1.Text.Length;
                    this.richTextBox1.SelectedText = msg + Environment.NewLine;
                }
                else if (iColor == 2)
                {
                    this.richTextBox1.SelectionColor = Color.Red;
                    this.richTextBox1.SelectionStart = richTextBox1.Text.Length;
                    this.richTextBox1.SelectedText = msg + Environment.NewLine;
                }
                else
                {
                    this.richTextBox1.SelectionColor = Color.Black;
                    this.richTextBox1.SelectionStart = richTextBox1.Text.Length;
                    this.richTextBox1.SelectedText = msg + Environment.NewLine;
                }

                this.richTextBox1.ScrollToCaret();
            }
        }
        delegate void SetListViewCallback(string szTestName, string szValue, string szLow, string szHigh, string szUnit, bool bResult, int nTestTime);
        private void SetTestSummary(string szTestName, string szValue, string szLow, string szHigh, string szUnit, bool bResult, int nTestTime)
        {
            if (this.listView2.InvokeRequired)
            {
                SetListViewCallback d = new SetListViewCallback(SetTestSummary);
                this.Invoke(d, new object[] { szTestName, szValue, szLow, szHigh, szUnit, bResult, nTestTime });

            }
            else
            {
                int iCount = listView2.Items.Count;
                listView2.Items.Add(szTestName, iCount);
                listView2.Items[iCount].SubItems.Add(szValue);
                listView2.Items[iCount].SubItems.Add(szLow);
                listView2.Items[iCount].SubItems.Add(szHigh);
                listView2.Items[iCount].SubItems.Add(szUnit);
                if (bResult)
                    listView2.Items[iCount].SubItems.Add("PASS");
                else
                    listView2.Items[iCount].SubItems.Add("FAIL");
                listView2.Items[iCount].SubItems.Add(nTestTime.ToString());
            }
        }
        public Form1()
        {
            InitializeComponent();
            InitListView1();
            this.tabPage2.Parent = null;//add in 20160531 for 隱藏  test LOG
            this.tabPage2.Parent = this.tabControl1;//add in 20160531 for 顯示 the test LOG
        }
        
        private Thread workThread;         //Test Thread 
        bool bSendEMP;
        public InstrBaseClass mPowersupply_Battery;
        public InstrBaseClass mPowersupply_USB;
        int nFailNum;
        public String bound;
        public IntPtr hTemp_Main;
        private void Form1_Load(object sender, EventArgs e)
        {
            nFailNum = 0;
            mPowersupply_Battery = null;
            mPowersupply_USB = null;
            bSendEMP = true;
            bool Exist = true;

            //--------------------------Read ScreenInfo----------------------------------------------------------
            int height = Screen.PrimaryScreen.Bounds.Height;
            int width = Screen.PrimaryScreen.Bounds.Width;


            //-------------------------Open .bat----------------------------------------------------------------
            string settingFile = ".\\Setup.ini";
            if (!File.Exists(settingFile))
            {
                MessageBox.Show("can not find " + settingFile);
                Environment.Exit(0);
            }
            IniFile ini = new IniFile(settingFile);
            string station = ini.ReadString("MAIN", "Station", "");


            Mutex newMutex = new Mutex(true, "Already", out Exist);
            if (!Exist)
            {
                MessageBox.Show("Test Program already running", "Warning");
            }

            //-------------------------Read Info(IP/Hostname)----------------------------------------------------------------
            stpInfo.IpAdd1 = "0.0.0.0";
            stpInfo.IpAdd2 = "0.0.0.0";
            IPHostEntry host;
            host = Dns.GetHostEntry(Dns.GetHostName());
            for (int i = 0; i < host.AddressList.Count(); i++)
            {
                if (host.AddressList[i].IsIPv6LinkLocal) continue;
                if (host.AddressList[i].IsIPv6Multicast) continue;
                if (host.AddressList[i].IsIPv6SiteLocal) continue;
                if (host.AddressList[i].IsIPv6Teredo) continue;
                if (stpInfo.IpAdd1 == "0.0.0.0")
                {
                    stpInfo.IpAdd1 = host.AddressList[i].ToString();
                    continue;
                }
                if (stpInfo.IpAdd2 == "0.0.0.0")
                {
                    stpInfo.IpAdd2 = host.AddressList[i].ToString();
                    break;
                }
            }

            stpInfo.Hostname = Environment.MachineName.Trim();
            LoadLocalSetting();
            string ftpPath = Path.Combine(stpInfo.FtuKill, "run_windows_mvc.bat");
            if (File.Exists(ftpPath))
            {
                File.Delete(ftpPath);
            }
            //if (!stpInfo.StationNo.Contains("PT2"))
            //{
            //    while (true)
            //    {
            //        MessageBox.Show("Vui long kiem tra lai ten may tinh la " + stpInfo.StationNo + " khong chinh xac voi chuong trinh hien tai la " + stpInfo.Station + "\r\nVui long tim TE online kiem tra ten may tinh theo dinh dang EAV24P0" + stpInfo.Station + "XX !!!!", "CANH BAO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    }
            //}

            //if (!stpInfo.StationNo.Contains("EAV24P0"))
            //{
            //    while (true)
            //    {
            //        MessageBox.Show("Vui long kiem tra lai ten may tinh la " + stpInfo.StationNo + " khong chinh xac voi chuong trinh hien tai la " + stpInfo.Station + "\r\nVui long tim TE online kiem tra ten may tinh theo dinh dang EAV24P0" + stpInfo.Station + "XX!!!!", "CANH BAO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    }
            //}
            //if (stpInfo.StationNo.Length != 12)
            //{
            //    while (true)
            //    {
            //        MessageBox.Show("Vui long kiem tra lai ten may tinh la " + stpInfo.StationNo + " khong chinh xac voi chuong trinh hien tai la " + stpInfo.Station + "\r\nVui long tim TE online kiem tra ten may tinh theo dinh dang EAV24P0" + stpInfo.Station + "XX!!!!", "CANH BAO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    }
            //}
            //InitNustream
            /*bool initStatus=InitNustream();
            if (!initStatus)
            {
                return;
            }*/

            InitializeFormUI();
            InitializeFormSetting();
            InitListView1(); //IP/Hostname Info
            InitialTestSummary(); //Result Info
            curTestInfo.PassNum = stpInfo.TestPassNum;
            curTestInfo.FailNum = stpInfo.TestFailNum;
            if (stpInfo.AutoScan == 1)
                ScanProgramInit();
            if (stpInfo.NeedGpib)
            {
                if (!Battery_Poweroff())
                {
                    MessageBox.Show("Control PowerSupply GPIB Fail,Please Check!");
                    Environment.Exit(0);
                }
            }
            if (stpInfo.Station == "PT2")
            {
                curTestInfo.TestItemCsvLogContent = new string[12];
                curTestInfo.OracleLogContent = new string[12];
            }
            if (stpInfo.Station == "FT1")
            {
                curTestInfo.TestItemCsvLogContent = new string[7];
                curTestInfo.OracleLogContent = new string[7];
                curTestInfo.CsvLogContent = new string[126];
            }

            if (stpInfo.Station == "PT2")
            {
                curTestInfo.TestItemCsvLogContent = new string[7];
                curTestInfo.OracleLogContent = new string[7];
                curTestInfo.CsvLogContent = new string[126];
            }

            if (stpInfo.Station == "PT2")
            {
                curTestInfo.TestItemCsvLogContent = new string[7];
                curTestInfo.OracleLogContent = new string[7];
                curTestInfo.CsvLogContent = new string[126];
            }
            if (stpInfo.Station == "FT")
            {
                curTestInfo.TestItemCsvLogContent = new string[12];
                curTestInfo.OracleLogContent = new string[12];
                curTestInfo.CsvLogContent = new string[15];
            }
            if (stpInfo.Station == "FT1")
            {
                curTestInfo.TestItemCsvLogContent = new string[12];
                curTestInfo.OracleLogContent = new string[12];
                curTestInfo.CsvLogContent = new string[15];
            }
            if (stpInfo.Station == "ATO")
            {
                curTestInfo.TestItemCsvLogContent = new string[9];
                curTestInfo.OracleLogContent = new string[9];
            }

            if (stpInfo.Station == "RC")
            {
                curTestInfo.TestItemCsvLogContent = new string[10];
                curTestInfo.OracleLogContent = new string[10];
            }

            if (stpInfo.Station == "DEG")
            {
                curTestInfo.TestItemCsvLogContent = new string[8];
                curTestInfo.OracleLogContent = new string[8];
            }

            if (stpInfo.Station == "PT2")
            {
                curTestInfo.TestItemCsvLogContent = new string[8];
                curTestInfo.OracleLogContent = new string[8];
            }

            this.InitTestInfomation();
            UpdateListView = new UPDATELISTVIEW(AddToTestSummary);

            ActiveControl = textBox1;
            pictureBox1.SendToBack();

            if (this.stpInfo.AutoScan == 1)
                this.timer_autoScan.Enabled = true;
        }
        private void ScanProgramInit()
        {
            string snFile = "c:\\sn.txt";
            string scanTitle = this.stpInfo.AutoScanTitle;
            string scanExe = Directory.GetCurrentDirectory() + "\\Autoscan\\scan_barcode.exe";
            WindowControl winCtrl = new WindowControl();
            IntPtr hwnd = winCtrl.GetMainWindow(null, scanTitle);
            if (File.Exists(snFile))
            {
                File.Delete(snFile);
            }

            if (hwnd == IntPtr.Zero)
            {
                try
                {
                    Process.Start(scanExe);
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

            }
        }
        delegate void SetPictureBox(bool bFront);
        private void PictureBoxControl(bool bFront)
        {
            if (this.pictureBox1.InvokeRequired)
            {
                SetPictureBox sPictureBox = new SetPictureBox(PictureBoxControl);
                this.Invoke(sPictureBox, new object[] { bFront });
            }
            else
            {
                if (bFront)
                    pictureBox1.BringToFront();
                else
                    pictureBox1.SendToBack();
            }
        }
        //-------------------------Read Count.dat and Setup.dat----------------------------------------------------------------
        private void LoadLocalSetting()
        {
            
            //--------------------------------------------Read Setup.ini------------------------------------
            string countFile = ".\\Count.dat";
            if (!File.Exists(countFile))
            {
                File.Create(countFile);
                File.SetAttributes(countFile, FileAttributes.Hidden);
                Thread.Sleep(500);
                if (!File.Exists(countFile))
                {
                    MessageBox.Show("can not create " + countFile);
                    Environment.Exit(0);
                }

            }
            IniFile countInt = new IniFile(countFile);
            stpInfo.Rj45Num = countInt.ReadInt("COUNT", "RJ45", 0);
            stpInfo.PowerConnectNum = countInt.ReadInt("COUNT", "PowerConnect", 0);
            stpInfo.ContinueFailNum = countInt.ReadInt("COUNT", "FailNum", 0);

            //--------------------------------------------Read Setup.ini------------------------------------
            string settingFile = ".\\Setup.ini";
            if (!File.Exists(settingFile))
            {
                MessageBox.Show("can not find " + settingFile);
                Environment.Exit(0);
            }
            IniFile ini = new IniFile(settingFile);
            ini.WriteString("MAIN", "Station", "PT2");
            ini.WriteString("MAIN", "StationNO", stpInfo.Hostname);
            stpInfo.Model = ini.ReadString("MAIN", "Model", "EMS00000T02");
            stpInfo.Station = ini.ReadString("MAIN", "Station", "DEBUG");
            stpInfo.StationNo = ini.ReadString("MAIN", "StationNO", "DEBUG-01");
            stpInfo.LocalLog = ini.ReadString("LC_PARAM", "LocalLogPath", "");
            stpInfo.LocalLogNoSfc = ini.ReadString("LC_PARAM", "LocalLogPathNoSFC", "");
            stpInfo.ServerLog = ini.ReadString("LC_PARAM", "ServerLogPath", "");
            stpInfo.ServerProgram = ini.ReadString("LC_PARAM", "ServerProgramPath", "");
            stpInfo.ServerMoc = ini.ReadString("LC_PARAM", "ServerMOCPath", "");
            stpInfo.SfisMode = ini.ReadInt("LC_PARAM", "SFISMode", 0);
            stpInfo.ScanMode = ini.ReadInt("LC_PARAM", "ScanMode", 0);
            stpInfo.SfisCom = ini.ReadString("LC_PARAM", "COM_SFIS", "");
            stpInfo.FixCom = ini.ReadString("LC_PARAM", "COM_FIX", "");
            stpInfo.DutCom = ini.ReadString("LC_PARAM", "COM_DUT", "");
            stpInfo.AutoScan = ini.ReadInt("LC_PARAM", "AutoScan", 0);
            stpInfo.AutoScanTitle = ini.ReadString("LC_PARAM", "AutoScanTitle", "Auto Scan - Ver: 3.1.0.0");
            stpInfo.FwVersion = ini.ReadString("LC_PARAM", "FWVersion", "");
            stpInfo.FcdVersion = ini.ReadString("LC_PARAM", "FCDVersion", "");
            stpInfo.BomVersion = ini.ReadString("LC_PARAM", "BOMVersion", "");
            stpInfo.JavaPath = ini.ReadString("LC_PARAM", "JavaPath", "");
            stpInfo.LogPass = ini.ReadString("LC_PARAM", "LogPASS", "");
            stpInfo.LogFail = ini.ReadString("LC_PARAM", "LogFAIL", "");
            stpInfo.JavaWindows = ini.ReadString("LC_PARAM", "JavaWindows", "");
            stpInfo.NustreamWindows = ini.ReadString("LC_PARAM", "NustreamWindows", "");
            stpInfo.ModelFileName = ini.ReadString("LC_PARAM", "ModelFileName", "");
            stpInfo.DConfPathName = ini.ReadString("LC_PARAM", "DConfPathName", "");
            stpInfo.CConfPathName = ini.ReadString("LC_PARAM", "CConfPathName", "");
            stpInfo.LedCom = ini.ReadString("LC_PARAM", "COM_LED", "");
            stpInfo.GoldenSn1 = ini.ReadString("LC_PARAM", "Golden_SN1", "");
            stpInfo.GoldenSn2 = ini.ReadString("LC_PARAM", "Golden_SN2", "");
            stpInfo.GoldenFlag = ini.ReadString("LC_PARAM", "Golden_Flag", "");
            stpInfo.NgSn1 = ini.ReadString("LC_PARAM", "NG_SN1", "");
            stpInfo.NgSn2 = ini.ReadString("LC_PARAM", "NG_SN2", "");
            stpInfo.NgFlag = ini.ReadString("LC_PARAM", "NG_Flag", "");
            stpInfo.GoldenTime = ini.ReadInt("LC_PARAM", "Golden_Time", 0);
            stpInfo.NgTime = ini.ReadInt("LC_PARAM", "NG_Time", 0);
            stpInfo.Sftp = ini.ReadInt("LC_PARAM", "SFTP", 0);
            stpInfo.TestFailNum = ini.ReadInt("COUNT", "TestFail_Num", 0);
            stpInfo.TestPassNum = ini.ReadInt("COUNT", "TestPass_Num", 0);

            //FTU va MO load
            stpInfo.MoDut = ini.ReadString("LC_PARAM", "Mo_Current", "");
            stpInfo.FtuDut = ini.ReadString("LC_PARAM", "FTU_Current", "");
            stpInfo.Region = ini.ReadString("LC_PARAM", "Region", "");
            if (stpInfo.Model.Contains("TS1"))
            {
                ini.WriteString("LC_PARAM", "Region", "WORLD");
                stpInfo.Region = ini.ReadString("LC_PARAM", "Region", "");
            }
            if (stpInfo.Model.Contains("TS2"))
            {
                ini.WriteString("LC_PARAM", "Region", "US");
                stpInfo.Region = ini.ReadString("LC_PARAM", "Region", "");
            }
            if (stpInfo.Model.Contains("T01"))
            {
                ini.WriteString("LC_PARAM", "Region", "WORLD");
                stpInfo.Region = ini.ReadString("LC_PARAM", "Region", "");
            }
            if (stpInfo.Model.Contains("T02"))
            {
                ini.WriteString("LC_PARAM", "Region", "US");
                stpInfo.Region = ini.ReadString("LC_PARAM", "Region", "");
            }
            stpInfo.FtuKill = ini.ReadString("LC_PARAM", "FTU_Kill", "");
            stpInfo.FtuSau = ini.ReadString("LC_PARAM", "FTU_Sau", "");
            stpInfo.FtuTruoc = ini.ReadString("LC_PARAM", "FTU_truoc", "");
            //
            /*    if (stpInfo.SfisMode == 0)
                {
                    label20.Text = "行为52.52";
                }
                else if (stpInfo.SfisMode == 1)
                {
                    label20.Text = "行为36.36";
                }
                else if (stpInfo.SfisMode == 2)
                {
                    label20.Text = "行为35.35";
                }
                else if (stpInfo.SfisMode == 3)
                {
                    label20.Text = "行为35.35";
                }
            */
            //--------------------------------Setup Fixture COM/USB-------------------------------------
            stpInfo.FixUsbCom = ini.ReadString("LC_PARAM", "COM_FIX_USB", "");
            if (stpInfo.FixCom.Length > 3)
            {
                try
                {
                    this.com_fixture.PortName = stpInfo.FixCom;
                    this.com_fixture.Close();
                    this.com_fixture.BaudRate = 9600;
                    this.com_fixture.Open();
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    Environment.Exit(0);
                }
            }
            if (stpInfo.FixUsbCom.Length > 3)
            {
                try
                {
                    this.com_fixtureUSB.PortName = stpInfo.FixUsbCom;
                    this.com_fixture.BaudRate = 9600;
                    if (!this.com_fixtureUSB.IsOpen)
                    {
                        this.com_fixtureUSB.Open();
                    }

                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    Environment.Exit(0);
                }
            }
            //--------------------------------Setup LED-------------------------------------
            stpInfo.SoftLedCom = ini.ReadString("LC_PARAM", "COM_Soft_LED", "");
            if (stpInfo.SoftLedCom.Length > 3)
            {
                try
                {
                    this.comm_SoftLED.PortName = stpInfo.SoftLedCom;
                    this.com_fixture.BaudRate = 9600;
                    this.com_fixture.Open();
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    Environment.Exit(0);
                }
            }
            InitialSFISCom();
            InitialUUTCom();
        }

        //--------------------------Write Setup.ini-------------------------------
        private void WriteLocalSetting()
        {

            string settingFile = ".\\Setup.ini";
            if (!File.Exists(settingFile))
            {
                MessageBox.Show("can not find " + settingFile);
                Environment.Exit(0);
            }

            IniFile ini = new IniFile(settingFile);
            ini.WriteString("MAIN", "Model", stpInfo.Model);
            ini.WriteString("MAIN", "Station", stpInfo.Station);
            ini.WriteString("MAIN", "StationNO", stpInfo.StationNo);
            ini.WriteString("LC_PARAM", "LocalLogPath", stpInfo.LocalLog);
            ini.WriteString("LC_PARAM", "LocalLogPathNoSFC", stpInfo.LocalLogNoSfc);
            ini.WriteString("LC_PARAM", "ServerLogPath", stpInfo.ServerLog);
            ini.WriteString("LC_PARAM", "ServerProgramPath", stpInfo.ServerProgram);
            ini.WriteString("LC_PARAM", "ServerMOCPath", stpInfo.ServerMoc);
            ini.WriteString("LC_PARAM", "COM_SFIS", stpInfo.SfisCom);
            ini.WriteString("LC_PARAM", "COM_FIX", stpInfo.FixCom);
            ini.WriteString("LC_PARAM", "COM_DUT", stpInfo.DutCom);
            ini.WriteString("LC_PARAM", "FWVersion", stpInfo.FwVersion);
            ini.WriteString("LC_PARAM", "FCDVersion", stpInfo.FcdVersion);
            ini.WriteString("LC_PARAM", "BOMVersion", stpInfo.BomVersion);
            ini.WriteString("LC_PARAM", "JavaPath", stpInfo.JavaPath);
            ini.WriteString("LC_PARAM", "LogPASS", stpInfo.LogPass);
            ini.WriteString("LC_PARAM", "LogFAIL", stpInfo.LogFail);
            ini.WriteString("LC_PARAM", "JavaWindows", stpInfo.JavaWindows);
            ini.WriteString("LC_PARAM", "NustreamWindows", stpInfo.NustreamWindows);
            ini.WriteString("LC_PARAM", "ModelFileName", stpInfo.ModelFileName);
            ini.WriteString("LC_PARAM", "DConfPathName", stpInfo.DConfPathName);
            ini.WriteString("LC_PARAM", "CConfPathName", stpInfo.CConfPathName);
        }

        private void InitializeFormUI()
        {
            this.label_Model.Text = stpInfo.Model;
            this.label_Station.Text = stpInfo.Station;
            this.label_StationNO.Text = stpInfo.StationNo;
            this.label_Result.Text = "Standby";
            this.label_Result.ForeColor = Color.Black;
        }
        private void InitializeFormSetting()
        {
            this.S_Model.Text = stpInfo.Model;
            this.S_Station.Text = stpInfo.Station;
            this.S_StationNO.Text = stpInfo.StationNo;
            this.S_LocalLogPath.Text = stpInfo.LocalLog;
            this.S_ServerLogPath.Text = stpInfo.ServerLog;
            this.S_ServerProgramPath.Text = stpInfo.ServerProgram;
            this.S_ServerMOCPath.Text = stpInfo.ServerMoc;
            this.S_SFISCom.Text = stpInfo.SfisCom;
            this.S_DUTCom.Text = stpInfo.DutCom;
            this.S_FIXCom.Text = stpInfo.FixCom;
            this.S_FWVersion.Text = stpInfo.FwVersion;
            this.S_FCDVersion.Text = stpInfo.FcdVersion;
            this.S_BOMVersion.Text = stpInfo.BomVersion;
        }
        //------------------------Initial SFIS COM-----------------------------
        private void InitialSFISCom()
        {
            if (stpInfo.SfisCom.Length != 0)
            {
                com_SFIS.PortName = stpInfo.SfisCom;
                com_SFIS.Close();
                com_SFIS.BaudRate = 9600;
                //com_SFIS.Open();
            }
        }
        private void InitialUUTCom()
        {


            string[] ports = SerialPort.GetPortNames();
            for (int i = 0; i < ports.Length; i++)
            {
                if ((!ports[i].Equals("COM1")) && (!ports[i].Equals("COM7")) && (!ports[i].Equals("COM3")) && (!ports[i].Equals("COM8")))
                {
                    string st = ports[i].ToString();
                    SerialPort com1 = new SerialPort(st);
                    //if (com1.IsOpen == true)
                    //{
                    com1.PortName = st;
                    //com1.Close();
                    com1.PortName = "COM3";
                    com1.BaudRate = 9600;
                    //com1.Open();
                    // }                   
                }

            }
        }

        private void UpdatePassFailNum()
        {

            this.Label_Pass.Text = Convert.ToString(curTestInfo.PassNum);
            this.Label_Fail.Text = Convert.ToString(curTestInfo.FailNum);
            if (Convert.ToDecimal(curTestInfo.FailNum) == 0)
            {
                this.Label_Retest.Text = "0.00%";
            }
            else
            {
                Decimal nRetestRate = Convert.ToDecimal(curTestInfo.FailNum) / Convert.ToDecimal(curTestInfo.FailNum + curTestInfo.PassNum) * 100;
                if (nRetestRate > 5)
                {
                    this.Label_Retest.ForeColor = Color.Red;

                }
                else
                    this.Label_Retest.ForeColor = Color.Black;
                this.Label_Retest.Text = Convert.ToString(nRetestRate);
                if (Label_Retest.Text.Length > 7)
                    this.Label_Retest.Text = Label_Retest.Text.Substring(0, 5) + "%";
                else
                    this.Label_Retest.Text += "%";
            }
            if (curTestInfo.BLoopTest)
            {
                Decimal nRetestRate = Convert.ToDecimal(this.loop_Fail.Text) / (Convert.ToDecimal(this.loop_Pass.Text) + Convert.ToDecimal(this.loop_Fail.Text)) * 100;
                if (nRetestRate > 5)
                {
                    this.loop_Retest.ForeColor = Color.Red;

                }
                else
                    this.loop_Retest.ForeColor = Color.Black;
                this.loop_Retest.Text = Convert.ToString(nRetestRate);
                if (loop_Retest.Text.Length > 7)
                    this.loop_Retest.Text = Label_Retest.Text.Substring(0, 5) + "%";
                else
                    this.loop_Retest.Text += "%";
            }

        }
        private void InitListView1()
        {
            this.listView1.View = View.Details;
            this.listView1.GridLines = true;
            this.listView1.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            this.listView1.Clear();

            this.listView1.Columns.Add("ItemList", 100, HorizontalAlignment.Left);
            this.listView1.Columns.Add("Value", 150, HorizontalAlignment.Center);
            this.listView1.Items.Add("HostName", 1);
            this.listView1.Items.Add("IPAddress_1", 2);
            this.listView1.Items.Add("IPAddress_2", 3);
            this.listView1.Items.Add("SFIS_COM", 4);
            this.listView1.Items.Add("FIX_COM", 5);
            this.listView1.Items.Add("DUT_COM", 6);
            this.listView1.Items.Add("RJ45", 7);
            this.listView1.Items.Add("PWR_Connector", 8);
            this.listView1.Items.Add("Continue_Fail", 9);
            this.listView1.Items.Add("PASS_Num", 10);
            this.listView1.Items.Add("FAIL_Num", 11);


            this.listView1.Items[0].SubItems.Add(stpInfo.Hostname);
            this.listView1.Items[1].SubItems.Add(stpInfo.IpAdd1);
            this.listView1.Items[2].SubItems.Add(stpInfo.IpAdd2);
            this.listView1.Items[3].SubItems.Add(stpInfo.SfisCom);
            this.listView1.Items[4].SubItems.Add(stpInfo.FixCom);
            this.listView1.Items[5].SubItems.Add(stpInfo.DutCom);
            this.listView1.Items[6].SubItems.Add(stpInfo.Rj45Num.ToString());
            this.listView1.Items[7].SubItems.Add(stpInfo.PowerConnectNum.ToString());
            this.listView1.Items[8].SubItems.Add(stpInfo.ContinueFailNum.ToString());

            this.listView1.Items[10].SubItems.Add(stpInfo.FixProbeNum.ToString());
            this.listView1.Visible = true;
        }
        private void InitialTestSummary()
        {
            listView2.View = View.Details;
            listView2.GridLines = true;
            listView2.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            listView2.Columns.Add("Test Name", 300, HorizontalAlignment.Left);
            listView2.Columns.Add("Current Value", 50, HorizontalAlignment.Center);
            listView2.Columns.Add("Low Limit", 50, HorizontalAlignment.Center);
            listView2.Columns.Add("High Limit", 50, HorizontalAlignment.Center);
            listView2.Columns.Add("Unit", 50, HorizontalAlignment.Center);
            listView2.Columns.Add("Result", 100, HorizontalAlignment.Center);
            listView2.Columns.Add("Test Time", 80, HorizontalAlignment.Center);

        }
        delegate void TextBox1ControlCallback(bool bEnable);
        private void TextBox1Control(bool bEnable)
        {
            if (this.textBox1.InvokeRequired)
            {
                TextBox1ControlCallback d = new TextBox1ControlCallback(TextBox1Control);
                this.Invoke(d, new object[] { bEnable });
            }
            else
            {
                if (bEnable)
                {
                    textBox1.Enabled = true;
                }
                else
                {
                    textBox1.Enabled = false;
                }
            }
        }
        private void updateCountNum()
        {
            //if (this.stpInfo.Station == "BURN_IN" ||this.stpInfo.Station == "PT2" || this.stpInfo.Station == "FT")
            //{
            //    stpInfo.RfProbeNum++;
            //}
            //if (stpInfo.Station == "PT" || stpInfo.Station == "BURN_IN" || stpInfo.Station == "PT2")
            //{
            //    stpInfo.FixProbeNum++;
            //}
            stpInfo.Rj45Num++;
            stpInfo.PowerConnectNum++;
            listView1.Items[6].SubItems.RemoveAt(1);
            listView1.Items[6].SubItems.Add(stpInfo.Rj45Num.ToString());
            listView1.Items[7].SubItems.RemoveAt(1);
            listView1.Items[7].SubItems.Add(stpInfo.PowerConnectNum.ToString());

            this.WriteCountNumToFile();
        }
        private void updateContinueFail(bool bAdded)
        {
            if (bAdded)
                stpInfo.ContinueFailNum++;
            else
                stpInfo.ContinueFailNum = 0;
            listView1.Items[6].SubItems.RemoveAt(1);
            listView1.Items[6].SubItems.Add(stpInfo.ContinueFailNum.ToString());

            this.WriteCountNumToFile();
        }
        private void SetStatusFlag(StatusFlag flag, string errorCode = "")
        {
            switch (flag)
            {
                case StatusFlag.RUN:
                    this.curTestInfo.NWaitSfc = 0;
                    this.curTestInfo.CycleTime = 0;
                    this.curTestInfo.OnTest = true;
                    TextBox1Control(false);
                    Label_Time.Text = curTestInfo.CycleTime.ToString() + "s";
                    this.label_Result.Text = "Running";
                    this.label_Result.ForeColor = Color.Yellow;
                    this.richTextBox1.Clear();
                    this.listView2.Items.Clear();
                    timer_Count.Enabled = true;
                    curTestInfo.LastPsn = curTestInfo.SfcPsn;
                    this.label_Error.Visible = false;
                    this.btn_Laser.Enabled = false;
                    this.button2.Enabled = false;
                    timer_CheckFinish.Enabled = true;
                    var tempErrorCode = endInfo.ErrorCode;
                    if (stpInfo.NeedGpib)
                    {
                        if (!Battery_PowerOn_New(ref tempErrorCode))
                        {
                            endInfo.ErrorCode = tempErrorCode;
                            MessageBox.Show("Control PowerSupply GPIB Fail,Please Check!");
                        }
                    }
                    return;
                case StatusFlag.PASS:
                    this.curTestInfo.OnTest = false;
                    nFailNum = 0;
                    if (this.nECTest.NeedFixture && this.stpInfo.Station == "RC" && this.nECTest.AutoClose)
                    {
                        this.WriteFixtureComAndWait(this.nECTest.PowerOff + "\r\n", 2, this.nECTest.PowerOffReply);
                    }
                    if (this.rb_SfisON.Checked)
                    {
                        this.SendToSFC(true);
                        for (int i = 0; i < 30; i++)
                        {
                            Thread.Sleep(300);
                            if (this.curTestInfo.MSfcRecv.Length > 20 && this.curTestInfo.MSfcRecv.Contains("PASS"))
                            {
                                //this.SFISMode0(curTestInfo.MSfcRecv.Trim());
                                this.curTestInfo.MSfcRecv = "";
                                curTestInfo.SfcReply = true;
                            }
                            if (this.curTestInfo.SfcReply) break;
                            if (this.curTestInfo.MSfcRecv.Contains("CALL PQE SETUP FTU AT CONFIG75 SSN5"))
                            {
                                MessageBox.Show("CALL PQE SETUP FTU AT CONFIG75 SSN5");
                                this.label_Result.Text = "FAIL";
                                this.label_Result.ForeColor = Color.Red;
                                this.timer_Count.Enabled = false;
                                break;

                            }
                            if (this.curTestInfo.MSfcRecv.Contains("ERRO"))
                            {
                                MessageBox.Show("SFC haven't response");
                                this.label_Result.Text = "FAIL";
                                this.label_Result.ForeColor = Color.Red;
                                this.timer_Count.Enabled = false;
                                break;
                            }


                        }
                    }
                    this.timer_Count.Enabled = false;
                    this.label_Result.Text = "PASS";
                    this.label_Result.ForeColor = Color.Green;
                    this.curTestInfo.PassNum++;
                    updateContinueFail(false);
                    endInfo.ResultPass = true;
                    endInfo.ResultFail = false;
                    updateCountNum();
                    if (this.rb_SfisON.Checked)
                    {
                        //TE_LogSave(endInfo.Result);//add in 20160316 For Save Te_Log when SFIS Passed
                    }
                    if (curTestInfo.BLoopTest)
                    {
                        this.curTestInfo.NLoopPass++;
                        this.loop_Pass.Text = curTestInfo.NLoopPass.ToString();
                    }
                    if (this.rb_Golden.Checked)
                    {
                        TE_Golden_Tes_Time_golden(endInfo.Result);//change in 20160515 for Golden Test
                    }
                    break;
                case StatusFlag.FAIL:
                    this.curTestInfo.OnTest = false;
                    if (nECTest.NeedFixture && this.stpInfo.Station == "RC" && this.nECTest.AutoClose)
                    {
                        this.WriteFixtureComAndWait(nECTest.PowerOff + "\r\n", 2, this.nECTest.PowerOffReply);
                    }

                    if (this.rb_SfisON.Checked)
                    {
                        if (endInfo.ErrorCode.Length != 6)
                            endInfo.ErrorCode = "ERROCD";
                        if (!(endInfo.ErrorCode.Contains("GPIB") || endInfo.ErrorCode.Contains("FIXUC") || endInfo.ErrorCode.Contains("BATT")))
                            this.SendToSFC(false);
                    }
                    this.label_Result.Text = "FAIL";
                    this.label_Result.ForeColor = Color.Red;
                    this.label_Error.Text = "ErrorCode : \n " + this.endInfo.ErrorCode;
                    this.label_Error.Visible = true;
                    this.timer_Count.Enabled = false;
                    this.curTestInfo.FailNum++;
                    updateContinueFail(true);
                    endInfo.ResultFail = true;
                    endInfo.ResultPass = false;
                    updateCountNum();
                    if (this.rb_SfisON.Checked)
                    {
                        //TE_LogSave(endInfo.Result);//add in 20160316 For Save Te_Log when FAIL
                    }
                    if (curTestInfo.BLoopTest)
                    {
                        this.curTestInfo.NLoopFail++;
                        this.loop_Fail.Text = curTestInfo.NLoopFail.ToString();
                    }
                    if (stpInfo.BNeedResetUsb == 1 && nFailNum >= 2)
                    {
                        string usbReset = ".\\USB_Reset.bat";
                        Process p = Process.Start(usbReset);

                    }
                    if (this.rb_Golden.Checked)
                    {
                        TE_Golden_Tes_Time_golden(endInfo.Result);//change in 20160515 for Golden Test
                    }
                    break;
                case StatusFlag.ERROR:
                    this.label_Result.Text = "SN fail!!";
                    this.label_Result.ForeColor = Color.Red;
                    this.textBox1.Enabled = true;
                    this.curTestInfo.OnTest = false;
                    this.timer_Count.Enabled = false;
                    if (!rb_SfisON.Checked)
                    {
                        button1.Enabled = true;
                    }
                    break;
                case StatusFlag.STANDBY:
                    this.label_Result.Text = "Standby";
                    this.textBox1.Enabled = true;
                    this.label_Result.ForeColor = Color.Blue;
                    this.curTestInfo.OnTest = false;
                    this.timer_Count.Enabled = false;
                    break;
                case StatusFlag.RESCAN:
                    this.label_Result.Text = "Rescan";

                    this.curTestInfo.OnTest = false;
                    this.curTestInfo.Rescan = false;

                    MFG_DialogWindow mfg_dialog = new MFG_DialogWindow();
                    string psn = mfg_dialog.AskInput("Please Rescan PSN ");
                    if (psn != curTestInfo.SfcPsn)
                    {
                        psn = mfg_dialog.AskInput("Rescan Error , please rescan again");
                        if (psn != curTestInfo.SfcPsn)
                        {
                            endInfo.ErrorCode = "RESCAN  Rescan PSN failed";
                            this.SetStatusFlag(StatusFlag.FAIL, endInfo.ErrorCode);
                            return;
                        }
                    }
                    this.SetStatusFlag(StatusFlag.PASS);
                    return;

            }

        INITIAL:
            if (this.nECTest.NeedFixture && this.stpInfo.Station == "RC" && this.stpInfo.AutoClose)
            {
                this.WriteFixtureComAndWait(this.nECTest.PowerOff + "\r\n", 2, this.nECTest.PowerOffReply);
            }

            InitTestInfomation();
            UpdatePassFailNum();
            this.btn_Laser.Enabled = true;
            this.button2.Enabled = true;
            if (!this.cbEnableLoop.Checked)
            {
                curTestInfo.BLoopTest = false;
            }
            if (stpInfo.NeedGpib)
            {
                if (!Battery_Poweroff())
                {
                    MessageBox.Show("Control PowerSupply GPIB Fail,Please Check!");
                }
            }
            if (this.stpInfo.AutoScan == 1)
                this.timer_autoScan.Enabled = true;
            if (this.curTestInfo.BLoopTest)
            {
                this.txLoopNum.Text = Convert.ToString(int.Parse(txLoopNum.Text) - 1);
                if (int.Parse(txLoopNum.Text) <= 0)
                {
                    curTestInfo.NLoopPass = 0;
                    curTestInfo.NLoopFail = 0;
                    cbEnableLoop.Checked = false;
                    return;
                }

                this.Start_RUN();
            }
        }
        private bool TE_Golden_Now()
        {
            string destTxt = "";

            //StreamWriter wlog;
            string StrLogTmp;
            //string strtmp;

            StrLogTmp = string.Format("{0}\\{1}\\{2}\\{3}\\{4}_now.txt",
                                        "D:",
                                        "test_program",
                                        stpInfo.Model,
                                        stpInfo.Station,
                                        "Verify"
                                     );
            string simPage = string.Format("Test Time:{0} {1}\r\n",
                                            DateTime.Now.ToString("yyyy-MM-dd"),
                                            DateTime.Now.ToString("HH:mm:ss")
                                          );

            destTxt = string.Format("{0}", StrLogTmp);
            try
            {
                string buff = Path.GetDirectoryName(destTxt);
                if (!Directory.Exists(buff))
                {
                    Directory.CreateDirectory(buff);
                }
            }
            catch (System.Exception ex)
            {
                SetText(ex.ToString());
                endInfo.ErrorCode = "SVLOG1 Save Server Log error";
                SetText(endInfo.ErrorCode);
                return false;
            }
            //wlog = File.AppendText(StrLogTmp);
            //wlog.Write("{0}", simPage);
            //wlog.Flush();
            System.IO.File.WriteAllText(StrLogTmp, simPage, Encoding.Default);
            return true;
        }
        private bool TE_Golden_Tes_Time_golden(bool bResult)
        {

            /* string s = "5555512h\r\n";
             System.IO.File.WriteAllText("D:/2.txt", s, Encoding.Default);//在D：/2.txt文檔能寫入“5555512h”，但第二次寫入會覆蓋第一次的信息
             */
            string destTxt = "";
            //if (!bResult && endInfo.ErrorCode.Length < 6)
            //{
            //    endInfo.ErrorCode = "EEEEEE";
            //}
            //StreamWriter wlog;
            string StrLogTmp, StrLogTmpNG;
            string strtmp;
            if (bResult) strtmp = "PASS,";
            else strtmp = "FAIL," + endInfo.ErrorCode.Substring(0, 6);
            StrLogTmp = string.Format("{0}\\{1}\\{2}\\{3}\\{4}_golden.txt",
                                        "D:",
                                        "test_program",
                                        stpInfo.Model,
                                        stpInfo.Station,
                                        "Verify"
                                     );
            StrLogTmpNG = string.Format("{0}\\{1}\\{2}\\{3}\\{4}_NG.txt",
                            "D:",
                            "test_program",
                            stpInfo.Model,
                            stpInfo.Station,
                            "Verify"
                         );
            string simPage = string.Format("Test Time:{0} {1}\r\n",
                                            DateTime.Now.ToString("yyyy-MM-dd"),
                                            DateTime.Now.ToString("HH:mm:ss")
                                          );

            destTxt = string.Format("{0}", StrLogTmp);
            try
            {
                string buff = Path.GetDirectoryName(destTxt);
                if (!Directory.Exists(buff))
                {
                    Directory.CreateDirectory(buff);
                }
            }
            catch (System.Exception ex)
            {
                SetText(ex.ToString());
                endInfo.ErrorCode = "SVLOG1 Save Server Log error";
                SetText(endInfo.ErrorCode);
                return false;
            }
            destTxt = string.Format("{0}", StrLogTmpNG);
            try
            {
                string buff = Path.GetDirectoryName(destTxt);
                if (!Directory.Exists(buff))
                {
                    Directory.CreateDirectory(buff);
                }
            }
            catch (System.Exception ex)
            {
                SetText(ex.ToString());
                endInfo.ErrorCode = "SVLOG1 Save Server Log error";
                SetText(endInfo.ErrorCode);
                return false;
            }

            if (stpInfo.Station == "PT2" || stpInfo.Station == "PT2" || stpInfo.Station == "PT" || stpInfo.Station == "PT3")
            {
                if ((curTestInfo.DutPsn == stpInfo.GoldenSn1 || curTestInfo.DutPsn == stpInfo.GoldenSn2) && bResult)
                {
                    System.IO.File.WriteAllText(StrLogTmp, simPage, Encoding.Default);
                }
                if ((curTestInfo.DutPsn == stpInfo.NgSn1 || curTestInfo.DutPsn == stpInfo.NgSn2) && !bResult)
                {
                    System.IO.File.WriteAllText(StrLogTmpNG, simPage, Encoding.Default);
                }
            }
            if (stpInfo.Station == "FT" || stpInfo.Station == "RC")
            {
                if ((curTestInfo.DutMac == stpInfo.GoldenSn1 || curTestInfo.DutMac == stpInfo.GoldenSn2) && bResult)
                {
                    System.IO.File.WriteAllText(StrLogTmp, simPage, Encoding.Default);
                }
                if ((curTestInfo.DutMac == stpInfo.NgSn1 || curTestInfo.DutMac == stpInfo.NgSn2) && !bResult)
                {
                    System.IO.File.WriteAllText(StrLogTmpNG, simPage, Encoding.Default);
                }
            }

            return true;
        }
        private void InitTestInfomation()
        {
            this.groupBox3.Enabled = true;
            this.curTestInfo.NWaitSfc = 0;
            curTestInfo.SfcPanel = "";
            curTestInfo.SfcPsnStart = "";
            curTestInfo.SfcPsnEnd = "";
            curTestInfo.SfcImei = "";
            this.curTestInfo.SfcMeid = "";
            this.curTestInfo.SfcPsn = "";
            curTestInfo.SfcFw = "";
            this.curTestInfo.DutFw = "";
            this.curTestInfo.DutImei = "";
            this.curTestInfo.DutMeid = "";
            this.curTestInfo.DutPsn = "";
            this.curTestInfo.DutMac = "";//new
            this.curTestInfo.ScanCsn = "";
            this.curTestInfo.CountryCode = "";
            this.curTestInfo.CycleTime = 0;
            this.curTestInfo.OnTest = false;
            this.curTestInfo.SfcReply = false;
            this.curTestInfo.MSfcRecv = "";
            this.endInfo.ErrorCode = "";
            this.endInfo.Result = false;
            this.endInfo.ResultPass = false;
            this.endInfo.ResultFail = false;
            this.endInfo.ResultGolden = false;
            this.endInfo.Finished = false;
            this.curTestInfo.CsvLog = "";
            this.curTestInfo.BNeedRunIn = true;
            this.curTestInfo.DutMainCamera = "";
            this.curTestInfo.DutSubCamera = "";
            this.curTestInfo.DutTp = "";
            mPowersupply_Battery = null;
            mPowersupply_USB = null;
            csvTile = "";
            csvText = "";
            for (int i = 0; i < curTestInfo.TestItemCsvLogContent.Length; i++)
            {
                this.curTestInfo.TestItemCsvLogContent[i] = "";
            }
            for (int i = 0; i < curTestInfo.OracleLogContent.Length; i++)
            {
                this.curTestInfo.OracleLogContent[i] = "";
            }
            /*if (stpInfo.Station == "PT" || stpInfo.Station == "PT2" || stpInfo.Station == "PT2" || stpInfo.Station == "PT3" || stpInfo.Station == "FT")
            {
                for (int i = 0; i < curTestInfo.CsvLogContent.Length; i++)
                {
                    this.curTestInfo.CsvLogContent[i] = "";
                }
            }*/

            TextBox1Control(true);
            this.textBox1.Text = "";
            this.textBox1.Focus();
            if (!rb_SfisON.Checked)
            {
                button1.Enabled = true;
            }
        }

        private bool logparth_Golden_Test()
        {

            string StrLogTmp_now, StrLogTmp_Golden, str_NG;
            StrLogTmp_now = string.Format("{0}\\{1}\\{2}\\{3}\\{4}_now.txt",
                                        "D:",
                                        "test_program",
                                        stpInfo.Model,
                                        stpInfo.Station,
                                        "Verify"
                                     );
            StrLogTmp_Golden = string.Format("{0}\\{1}\\{2}\\{3}\\{4}_golden.txt",
                                        "D:",
                                        "test_program",
                                        stpInfo.Model,
                                        stpInfo.Station,
                                        "Verify"
                                     );
            str_NG = string.Format("{0}\\{1}\\{2}\\{3}\\{4}_NG.txt",
                                        "D:",
                                        "test_program",
                                        stpInfo.Model,
                                        stpInfo.Station,
                                        "Verify"
                                     );

            TE_Golden_Now();
            Thread.Sleep(1000);
            if (!File.Exists(StrLogTmp_now))
            {
                MessageBox.Show("can not find :" + StrLogTmp_now);
                return false;
            }
            if (!File.Exists(StrLogTmp_Golden))
            {
                MessageBox.Show("can not find :" + StrLogTmp_Golden);
                return false;
            }
            if (!File.Exists(str_NG))
            {
                MessageBox.Show("can not find :" + str_NG);
                return false;
            }

            StreamReader SR_tmp = new StreamReader(StrLogTmp_now);
            string fileText = SR_tmp.ReadLine();
            string Time1 = "";
            do
            {
                if (fileText.Contains("Test Time:"))
                {
                    Time1 = fileText.Substring(fileText.IndexOf("Test Time:") + 10, 19); ;
                }
                fileText = SR_tmp.ReadLine();
            } while (fileText != null);
            SR_tmp.Close();

            StreamReader SR_tmp1 = new StreamReader(StrLogTmp_Golden);
            string Time11 = "";
            string fileText1 = SR_tmp1.ReadLine();
            do
            {
                if (fileText1.Contains("Test Time:"))
                {

                    Time11 = fileText1.Substring(fileText1.IndexOf("Test Time:") + 10, 19);

                }
                fileText1 = SR_tmp1.ReadLine();
            } while (fileText1 != null);
            SR_tmp1.Close();

            StreamReader SR_tmp2 = new StreamReader(str_NG);
            string fileText2 = SR_tmp2.ReadLine();
            string Time12 = "";
            do
            {
                if (fileText2.Contains("Test Time:"))
                {
                    Time12 = fileText2.Substring(fileText2.IndexOf("Test Time:") + 10, 19);
                }
                fileText2 = SR_tmp2.ReadLine();
            } while (fileText2 != null);
            SR_tmp2.Close();

            DateTime t1 = DateTime.Parse(Time1);
            DateTime t2 = DateTime.Parse(Time11);
            DateTime t2_NG = DateTime.Parse(Time12);
            System.TimeSpan t3 = t1 - t2;
            double getMinute = t3.TotalMinutes;
            System.TimeSpan t4 = t1 - t2_NG;
            double getMinute1 = t4.TotalMinutes;
            if (getMinute / 60 > stpInfo.GoldenTime)
            {
                MessageBox.Show("請用Golden Sample 校驗機台!!!");
                return false;
            }
            if (getMinute1 / 60 > stpInfo.NgTime)
            {
                MessageBox.Show("請用NG Sample 校驗機台!!!");
                return false;
            }
            return true;
        }

        public delegate void UPDATELISTVIEW(string szTestName, string szValue, string szLow, string szHigh, string szUnit, bool bResult, double nTestTime);
        public UPDATELISTVIEW UpdateListView;
        private void AddToTestSummary(string szTestName, string szValue, string szLow, string szHigh, string szUnit, bool bResult, double nTestTime)
        {
            if (this.listView2.InvokeRequired)
            {
                this.Invoke(UpdateListView, new object[] { szTestName, szValue, szLow, szHigh, szUnit, bResult, nTestTime });
            }
            else
            {
                int iCount = listView2.Items.Count;
                listView2.Items.Add(szTestName, iCount);
                listView2.Items[iCount].SubItems.Add(szValue);
                listView2.Items[iCount].SubItems.Add(szLow);
                listView2.Items[iCount].SubItems.Add(szHigh);
                listView2.Items[iCount].SubItems.Add(szUnit);
                if (bResult)
                {
                    listView2.Items[iCount].SubItems.Add("PASS");
                    listView2.Items[iCount].ForeColor = Color.Green;
                }
                else
                {
                    listView2.Items[iCount].SubItems.Add("FAIL");
                    listView2.Items[iCount].ForeColor = Color.Red;
                }
                listView2.Items[iCount].SubItems.Add(nTestTime.ToString());
            }

        }
        private void txtMacInput_01(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == (char)Keys.Return)
            {
                textBox1.Focus();
                string str = textBox1.Text.Trim();
                if (str.Length == 12)
                {
                    if (!CheckParamPreStart())
                    {
                        SetStatusFlag(StatusFlag.STANDBY);
                        return;
                    }
                    curTestInfo.ScanPsn = textBox1.Text.Trim().ToUpper();
                    curTestInfo.ScanMac = textBox1.Text.Substring(0, 12).Trim().ToUpper();
                    curTestInfo.SfcPsn = textBox1.Text.Substring(0, 12).Trim().ToUpper();

                    if (rb_SfisOff.Checked)
                    {
                        button1.Enabled = false;
                        Start_RUN();

                    }
                    if (rb_SfisON.Checked)
                    {
                        if (CheckCondition.IsFailedTimeOutOfSpec)
                        {
                            MessageBox.Show($"Máy đã fail quá {CheckCondition.GetSpec} lần.\r\n Vui lòng gọi TE online");
                            SetStatusFlag(StatusFlag.STANDBY);
                            return;
                        }
                        if (CheckCondition.IsOldMac(curTestInfo.ScanMac))
                        {
                            MessageBox.Show($"Bản đã test fail trước đó vui lòng đổi bản khác");
                            SetStatusFlag(StatusFlag.STANDBY);
                            return;
                        }
                        //textBox1.Enabled = false;
                        this.timer_checkSFC.Enabled = true;
                        if (0 == stpInfo.ScanMode)
                        {
                            scanMode_0(textBox1.Text.Substring(0, 12).Trim().ToUpper());
                        }
                        else if (1 == stpInfo.ScanMode)
                        {
                            scanMode_1(textBox1.Text.Substring(0, 12).Trim().ToUpper());
                        }
                        else if (2 == stpInfo.ScanMode)
                        {
                            scanMode_2(textBox1.Text.Trim().ToUpper());
                        }
                        else if (8 == stpInfo.ScanMode)
                        {
                            scanMode_8(textBox1.Text.Trim().ToUpper());
                        }

                    }
                }
                else
                {
                    textBox1.Text = "";
                    this.SetStatusFlag(StatusFlag.ERROR);
                }

            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            //if (this.nECTest.NeedFixture && this.stpInfo.Station == "RC" && this.nECTest.AutoClose)
            if (this.nECTest.NeedFixture && this.nECTest.AutoClose)
            {
                this.WriteFixtureComAndWait(this.nECTest.PowerOn + "\r\n", 2, this.nECTest.PowerOnReply);
            }
            button1.Enabled = false;
            if (textBox1.Text.Length != 7)
                this.SetStatusFlag(StatusFlag.ERROR);
            return;
            Start_RUN();

        }
        private bool CheckParamPreStart()
        {

            KiemTra();
            return true;

        }
        private void Start_RUN()
        {
            curTestInfo.OnTest = true;
            textBox1.Enabled = false;
            groupBox3.Enabled = false;

            string snFile = "c:\\sn.txt";
            if (File.Exists(snFile))
                File.Delete(snFile);

            if (cbEnableLoop.Checked)
                curTestInfo.BLoopTest = true;
            else
                curTestInfo.BLoopTest = false;
            if (rb_SfisON.Checked)
            {
                curTestInfo.BLoopTest = false;
                cbEnableLoop.Checked = false;
            }

            if (this.rb_SfisON.Checked)
            {
                if (stpInfo.GoldenFlag != "0")
                {
                    if (!logparth_Golden_Test())
                    {

                        SetStatusFlag(StatusFlag.STANDBY);
                        return;
                    }
                }
            }

            if (rb_Golden.Checked)
            {
                if (stpInfo.GoldenFlag == "0" || stpInfo.NgFlag == "0")
                {
                    MessageBox.Show("該測試工站不需要Golden Test，請確認!!!", "Warning", MessageBoxButtons.OK);
                    SetStatusFlag(StatusFlag.STANDBY);
                    return;
                }
            }

            SetStatusFlag(StatusFlag.RUN);

            workThread = new Thread(Station_Test);
            workThread.Start();


        }
        private void rb_SfisON_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_SfisON.Checked)
            {
                button1.Enabled = false;
                this.BackColor = Color.LightSkyBlue;
            }
            else
            {

                button1.Enabled = true;
                this.BackColor = Color.Yellow;
            }
        }
        private void timer_CheckFinish_Tick(object sender, EventArgs e)
        {

            if (endInfo.Finished)
            {
                timer_CheckFinish.Enabled = false;

                //CsvLogSave(endInfo.Result);//change 20160307 for save Csv
                //LogSave(endInfo.Result);
                //if (this.curTestInfo.SfcReply)
                // {
                // TE_LogSave(endInfo.Result);
                // }

                if (endInfo.Result)
                {
                    this.SetStatusFlag(StatusFlag.PASS);
                    // TE_LogSave(endInfo.Result);
                    return;
                }
                else
                {
                    this.SetStatusFlag(StatusFlag.FAIL, endInfo.ErrorCode);
                    //TE_LogSave(endInfo.Result);
                    return;
                }
            }

        }
        private void timer_Count_Tick(object sender, EventArgs e)
        {
            /*IntPtr hTemp_Main = new WindowControl().GetMainWindow(null, "Test");
            while (hTemp_Main != IntPtr.Zero)
            {
                new WindowControl().RaiseWindowProcess(hTemp_Main);
                Thread.Sleep(100);
            }*/

            curTestInfo.CycleTime++;
            this.Label_Time.Text = curTestInfo.CycleTime.ToString() + "s";
            if (curTestInfo.CycleTime == 500)
            {
                if (stpInfo.Station == "PT2")
                {
                    // MFG_DialogWindow diagWindow = new MFG_DialogWindow();
                    // diagWindow.ShowWarning("提示", "現在可以插入新的產品進行預熱");

                }
                if (stpInfo.Station == "PT3")
                {
                    // MFG_DialogWindow diagWindow = new MFG_DialogWindow();
                    // diagWindow.ShowWarning("提示", "現在可以插入新的產品進行預熱");

                }

            }
        }
        private void com_SFIS_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            if (rb_SfisOff.Checked)
                return;
            if (curTestInfo.OnTest)
                return;
            string RevcData = this.com_SFIS.ReadLine();
            RevcData = RevcData.Trim();
            this.curTestInfo.MSfcRecv = RevcData;
        }

        private void SFISMode0(string revcData)//SFIS-->PC to Start test (SFISMode0 for sbb\nec\yatai)
        {
            string szModel, szPSN;
            //string szStation, szPSN, szIMEI, szWIFI, szBT, szCSN, szFW;
            string errorMsg = "";
            SetText("SFIS -->" + revcData);
            if (revcData.Contains("ERRO"))
            {
                if (this.stpInfo.AutoScan == 1)
                    this.timer_autoScan.Enabled = true;
                textBox1.Text = "";
                return;
            }
            //25PSN+12MAC+25ModelName+10MO+PASS
            if (revcData.Length == 25 + 12 + 25 + 10 + 4 && revcData.Contains("PASS"))
            {
                szModel = revcData.Substring(37, 25).Trim().ToUpper();
                if (szModel != stpInfo.Model.ToUpper())
                {
                    errorMsg = "SFC Model : " + szModel + " , Setting Model : " + stpInfo.Model;
                    SetText(errorMsg);
                    //ShowMessage ("提示", "Model不匹配，請確認model名!!!!");

                    SetStatusFlag(StatusFlag.ERROR, "SFC0_FF");
                    return;
                }
                szPSN = revcData.Substring(0, 25).Trim().ToUpper();
                if (szPSN != textBox1.Text.Trim().ToUpper())
                {
                    errorMsg = "SFC PSN : " + szPSN + " ,Scan PSN : " + textBox1.Text.Trim().ToUpper();
                    SetText(errorMsg, 2);
                    SetStatusFlag(StatusFlag.ERROR, "SFC1_FF");
                    return;
                }

                curTestInfo.SfcPsn = szPSN;
                curTestInfo.DutMo = revcData.Substring(25 + 12 + 20, 10).Trim().ToUpper();
                this.curTestInfo.OnTest = true;
                Start_RUN();
                return;
            }
            //25PSN+12MAC+15BOM+25FW+15FCD+10QRCode+12Station
            else if (revcData.Length == 25 + 12 + 15 + 25 + 15 + 10 + 12 + 4 && revcData.Contains("PASS"))//SFIS->PC for Test PASS
            {
                curTestInfo.SfcReply = true;
                return;
            }
            else
                return;
        }
        private void SFISMode1(string revcData) //It is the SFISModel1 for ETAB_5
        {
            string szModel, szPSN, szMO;
            string errorMsg = "";
            SetText("SFIS -->" + revcData);
            if (revcData.Contains("ERRO"))
            {
                if (this.stpInfo.AutoScan == 1)
                    this.timer_autoScan.Enabled = true;
                textBox1.Text = "";
                SetStatusFlag(StatusFlag.ERROR, "SFC0FF");
                return;
            }
            //25PSN+12MAC+25ModelName+10MO+PASS
            if (revcData.Length == 25 + 12 + 25 + 10 + 4 && revcData.Contains("PASS"))
            {
                szModel = revcData.Substring(37, 25).Trim().ToUpper();
                if (szModel != stpInfo.Model.ToUpper())
                {
                    errorMsg = "SFC Model : " + szModel + " , Setting Model : " + stpInfo.Model;
                    SetText(errorMsg);
                    //ShowMessage ("提示", "Model不匹配，請確認model名!!!!");

                    SetStatusFlag(StatusFlag.ERROR, "SFC0FF");
                    return;
                }

                szPSN = revcData.Substring(0, 12).Trim().ToUpper();
                if (szPSN != textBox1.Text.Substring(0, 12).Trim().ToUpper())
                {
                    errorMsg = "SFC PSN : " + szPSN + " ,Scan PSN : " + textBox1.Text.Substring(0, 12).Trim().ToUpper();
                    SetText(errorMsg, 2);
                    SetStatusFlag(StatusFlag.ERROR, "SFC1FF");
                    return;
                }
                szMO = revcData.Substring(62, 10).Trim().ToUpper();
                curTestInfo.DutMo = szMO;
                curTestInfo.SfcPsn = szPSN;
                this.curTestInfo.OnTest = true;
                Start_RUN();
                return;
            }
            //25PSN+12MAC+15BOM+25FW+15FCD+10QRCode+12Station
            else if (revcData.Length == 25 + 12 + 4 && revcData.Contains("PASS"))//SFIS->PC for Test PASS
            {
                curTestInfo.SfcReply = true;
                return;
            }
            else
                return;
        }
        private void SFISMode2(string revcData)
        {
            //string szModel, szStation;
            string szPanel, szPSN_Start, szPSN_End;
            //string errorMsg = "";
            SetText("SFIS -->" + revcData);
            if (revcData.Contains("ERRO"))
            {
                textBox1.Text = "";
                return;
            }

            if (stpInfo.Station == "LASER")
            {
                //25PSN + 12ModelName + 15 Group + PASS
                if (revcData.Length == 25 + 25 + 25 + 4 && revcData.Contains("PASS"))
                {
                    szPanel = revcData.Substring(0, 25).Trim().ToUpper();

                    szPSN_Start = revcData.Substring(25, 25).Trim().ToUpper();

                    szPSN_End = revcData.Substring(50, 25).Trim().ToUpper();
                    textBox1.Text = szPSN_Start;
                    curTestInfo.SfcPsn = szPSN_Start;
                    curTestInfo.SfcPsnStart = szPSN_Start;
                    curTestInfo.SfcPsnEnd = szPSN_End;
                    curTestInfo.SfcPanel = szPanel;
                    curTestInfo.OnTest = true;
                    Start_RUN();
                    return;

                }
                //MO(20)+PANEL(25)+PSN_Start(25)+PSN_End(25)+PC_Name(12)+PASS(4)
                else if (revcData.Length == 20 + 25 + 25 + 25 + 12 + 4 && revcData.Contains("PASS"))
                {
                    curTestInfo.SfcReply = true;
                    return;
                }
                else
                    return;
            }
        }
        
        private void SendToSFC(bool bResult)//PC-->SFIS For Test Finish
        {
            string senddate = "";
            if (bResult)
            {
                //25PSN+12MAC+15BOM+25FW+15FCD+12Station上次上傳格式.
                //25PSN+12MAC+15BOM+25FW+15FCD+10QRCode+12Station
                if (stpInfo.SfisMode == 0 || stpInfo.SfisMode == 1)
                {
                    //add country
                    senddate = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}", curTestInfo.SfcPsn.PadRight(25), curTestInfo.SfcPsn.PadRight(12), stpInfo.BomVersion.PadRight(15), stpInfo.FwVersion.PadRight(25), stpInfo.FcdVersion.PadRight(15), stpInfo.Region.PadRight(26), stpInfo.FtuDut.PadRight(30), stpInfo.StationNo.PadRight(12));
                    //  senddate = string.Format("{0}{1}{2}{3}{4}{5}{6}", curTestInfo.SfcPsn.PadRight(25), curTestInfo.SfcPsn.PadRight(12), stpInfo.BomVersion.PadRight(15), stpInfo.FwVersion.PadRight(19), curTestInfo.CountryCode.PadRight(6), stpInfo.FcdVersion.PadRight(15), stpInfo.StationNo.PadRight(12,'-'));
                    //senddate = string.Format("{0}{1}{2}{3}{4}{5}", curTestInfo.SfcPsn.PadRight(25), curTestInfo.SfcPsn.PadRight(12), stpInfo.BomVersion.PadRight(15), stpInfo.FwVersion.PadRight(25), stpInfo.FcdVersion.PadRight(15), stpInfo.StationNo.PadRight(12));
                }
                if (stpInfo.SfisMode == 2)
                {
                    if (stpInfo.Station == "LASER")
                    {
                        // senddate = string.Format("{0}{1}{2}{3}{4}", tb_MoNum.Text.PadRight(20), curTestInfo.SfcPanel.PadRight(25),curTestInfo.SfcPsnStart.PadRight(25),curTestInfo.SfcPsnEnd.PadRight(25), stpInfo.Hostname);
                    }
                    else if (stpInfo.Station == "ATO" && stpInfo.Model == "T08B004.01")
                    {
                        senddate = string.Format("{0}{1}{2}", curTestInfo.SfcPsn.PadRight(49), curTestInfo.SfcPsn.PadRight(139), stpInfo.Hostname);
                    }
                    else if (stpInfo.Station == "ATO" && stpInfo.Model == "J18B134.01")
                    {
                        senddate = string.Format("{0}{1}{2}", curTestInfo.SfcPsn.PadRight(49), curTestInfo.SfcPsn.PadRight(24), curTestInfo.ScanCsn.PadRight(115), stpInfo.Hostname);
                    }
                    else if (stpInfo.Station == "RC" && stpInfo.Model == "T08B004.01")
                    {
                        senddate = string.Format("{0}{1}{2}{3}", curTestInfo.SfcPsn.PadRight(49), curTestInfo.SfcPsn.PadRight(24), curTestInfo.ScanCsn.PadRight(57), stpInfo.Hostname);
                    }
                    else if (stpInfo.Station == "RC" && stpInfo.Model == "J18B134.01")
                    {
                        senddate = string.Format("{0}{1}{2}{3}", curTestInfo.SfcPsn.PadRight(49), curTestInfo.SfcPsn.PadRight(24), curTestInfo.ScanCsn.PadRight(57), stpInfo.Hostname);
                    }
                }
            }
            else
            {

                //25PSN+12MAC+15BOM+25FW+15FCD+10QRCode+12Station+6ErrorCode
                if (stpInfo.SfisMode == 0 || stpInfo.SfisMode == 1)
                {
                    senddate = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}", curTestInfo.SfcPsn.PadRight(25), curTestInfo.SfcPsn.PadRight(12), stpInfo.BomVersion.PadRight(15), stpInfo.FwVersion.PadRight(25), stpInfo.FcdVersion.PadRight(15), stpInfo.Region.PadRight(26), stpInfo.FtuDut.PadRight(30), stpInfo.StationNo.PadRight(12), endInfo.ErrorCode.Substring(0, 6));
                    //senddate = string.Format("{0}{1}{2}{3}{4}{5}{6}", curTestInfo.SfcPsn.PadRight(25), curTestInfo.SfcPsn.PadRight(12), stpInfo.BomVersion.PadRight(15), stpInfo.FwVersion.PadRight(25), stpInfo.FcdVersion.PadRight(15), stpInfo.StationNo.PadRight(12),endInfo.ErrorCode.Substring(0, 6));
                }
                if (stpInfo.SfisMode == 2)
                {
                    if (stpInfo.Station == "LASER")
                    {
                        //senddate = string.Format("{0}{1}{2}", tb_MoNum.Text.PadRight(20), curTestInfo.SfcPanel.PadRight(25),endInfo.ErrorCode.PadRight(50), stpInfo.Hostname);
                    }
                    else if (stpInfo.Station == "ATO" && stpInfo.Model == "T08B004.01")
                    {
                        senddate = string.Format("{0}{1}{2}{3}", curTestInfo.SfcPsn.PadRight(49), curTestInfo.SfcPsn.PadRight(139), stpInfo.Hostname, endInfo.ErrorCode.Substring(0, 6));
                    }
                    else if (stpInfo.Station == "ATO" && stpInfo.Model == "J18B134.01")
                    {
                        senddate = string.Format("{0}{1}{2}{3}{4}", curTestInfo.SfcPsn.PadRight(49), curTestInfo.SfcPsn.PadRight(24), curTestInfo.ScanCsn.PadRight(115), stpInfo.Hostname, endInfo.ErrorCode.Substring(0, 6));
                    }
                    else if (stpInfo.Station == "RC" && stpInfo.Model == "T08B004.01")
                    {
                        senddate = string.Format("{0}{1}{2}{3}", curTestInfo.SfcPsn.PadRight(49), curTestInfo.SfcPsn.PadRight(81), stpInfo.Hostname, endInfo.ErrorCode.Substring(0, 6));
                    }
                    else if (stpInfo.Station == "RC" && stpInfo.Model == "J18B134.01")
                    {
                        senddate = string.Format("{0}{1}{2}{3}{4}", curTestInfo.SfcPsn.PadRight(49), curTestInfo.SfcPsn.PadRight(24), curTestInfo.ScanCsn.PadRight(57), stpInfo.Hostname, endInfo.ErrorCode.Substring(0, 6));
                    }
                }
            }
            this.curTestInfo.MSfcRecv = "";
            com_SFIS.Write(senddate + "\r\n");
            SetText(senddate, 1);
        }
        private int Golden_ReadPSN()
        {
            WriteDebugMessage("-----Check Golden Sample MAC------");
            WriteDebugMessage(" Read PSN is: " + textBox1.Text);
            //  if ((curTestInfo.DutPsn!= stpInfo.GoldenSn1)||(curTestInfo.DutPsn!= stpInfo.GoldenSn2)||(curTestInfo.DutPsn!= stpInfo.NgSn1)||(curTestInfo.DutPsn!=stpInfo.NgSn2))
            if ((textBox1.Text == stpInfo.GoldenSn1) || (textBox1.Text == stpInfo.GoldenSn2))
            {
                WriteDebugMessage("-----is Golden Sample PSN ------");
                return 1;
            }
            else if ((textBox1.Text == stpInfo.NgSn1) || (textBox1.Text == stpInfo.NgSn2))
            {
                WriteDebugMessage("-----is NG Sample PSN ------");
                return 2;
            }

            return 0;
        }
        
        private bool serialPort_SFIS_SendAndWaitResult(string cmd, int timeout, string exp)
        {
            string command = cmd;
            curTestInfo.MSfcRecv = "";
            try
            {
                com_SFIS.Write(command);
                int nTimeout = timeout * 10;
                while (nTimeout > 0)
                {
                    if (curTestInfo.MSfcRecv.Contains(exp)) break;
                    else
                        Thread.Sleep(100);
                    nTimeout--;
                }
                if (nTimeout <= 0)
                {
                    return false;
                }
                else
                    return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("SFIS COM異常：" + ex.Message);
                return false;
            }
        }




        private void scanMode_0(string scanData)//change In 20160310 For T08B004,J01B030,PC-->SFIS For Start test
        {
            string strEMP = "SYANTE5";
            string strSend = "";
            this.curTestInfo.MSfcRecv = "";
            if (bSendEMP)
            {
                strSend = string.Format("{0}END\r\n", strEMP.PadRight(20));
                com_SFIS.Write(strSend);
                //bSendEMP = false;
                Thread.Sleep(1500);
            }
            int GoldenStatue = Golden_ReadPSN();
            //TE_golden_Time();
            if (GoldenStatue == 1)
            {
                bSendEMP = true;
                Start_RUN();

            }
            else if (GoldenStatue == 2)
            {

                bSendEMP = true;
                Start_RUN();
            }
            else
            {
                //  TE_golden_Time();//tracy 1016

                strSend = string.Format("{0}{1}{2}{3}END\r\n", scanData.PadRight(25), scanData.PadRight(12), stpInfo.Station.PadRight(25), stpInfo.StationNo.PadRight(25));
                //   com_SFIS.Write(strSend);
                if (serialPort_SFIS_SendAndWaitResult(strSend, 10, "PASS"))
                {
                    string sfcmode = curTestInfo.MSfcRecv.Substring(37, 25).ToString().Trim();
                    //  MessageBox.Show(sfcmode);
                    if (stpInfo.Model == sfcmode.Trim())
                    {

                    }
                    else
                    {
                        this.SetStatusFlag(StatusFlag.ERROR);
                        MessageBox.Show("Model name do not match,Please check the model name");
                    }
                }
                else
                {
                    this.SetStatusFlag(StatusFlag.ERROR);
                }
            }

            System.Diagnostics.Process[] excelProcess = System.Diagnostics.Process.GetProcessesByName("WindowsFormsApplication1");
            foreach (System.Diagnostics.Process p in excelProcess)
            {
                p.Kill();
            }


        }
        private void scanMode_1(string scanData)
        {
            string strEMP = "SYANTE5";
            string strSend = "";
            this.curTestInfo.MSfcRecv = "";
            if (bSendEMP)
            {
                strSend = string.Format("{0}END\r\n", strEMP.PadRight(20));
                com_SFIS.Write(strSend);
                bSendEMP = false;
                Thread.Sleep(1500);
            }


            if (scanData.Trim().Length < 12)
            {
                MessageBox.Show("掃描MAC有誤，請重新掃描", "Warning", MessageBoxButtons.OK);
                this.SetStatusFlag(StatusFlag.STANDBY);
                return;
            }
            this.curTestInfo.ScanMac = scanData.Trim();
            MFG_DialogWindow diaWin = new MFG_DialogWindow();
            string hhsn = diaWin.AskInput("Please Scan HH ");
            if (hhsn.Trim().Length != 9)
            {
                MessageBox.Show("掃描HH有誤，請重新掃描", "Warning", MessageBoxButtons.OK);
                diaWin.InitialDialog();
                hhsn = diaWin.AskInput("Scan HH is wrong , Please Rescan HH ");
                // if (csn.Length != 12 || csn.Substring(0, 5) != "FXICB")
                if (hhsn.Length != 9)
                {
                    MessageBox.Show("掃描HH有誤，請確認HH是否是9位", "Warning", MessageBoxButtons.OK);
                    this.SetStatusFlag(StatusFlag.STANDBY);
                    return;
                }
            }
            curTestInfo.ScanPsn = hhsn;
            strSend = string.Format("{0}{1}{2}END\r\n", this.curTestInfo.ScanMac.PadRight(25), this.curTestInfo.ScanMac.PadRight(12), this.curTestInfo.ScanPsn.PadRight(25));

            com_SFIS.Write(strSend);
        }
        private void scanMode_2(string scanData)
        {
            string strEMP = "SYANTE5";
            string strSend = "";
            curTestInfo.MSfcRecv = "";
            if (bSendEMP)
            {
                strSend = string.Format("{0}END\r\n", strEMP.PadRight(20));
                com_SFIS.Write(strSend);
                bSendEMP = false;
                Thread.Sleep(1500);
            }

            if (stpInfo.Station == "RC")
            {
                if (scanData.Trim().Length != 12)
                {
                    MessageBox.Show("掃描MAC有誤，請重新掃描", "Warning", MessageBoxButtons.OK);
                    this.SetStatusFlag(StatusFlag.STANDBY);
                    return;
                }
                MFG_DialogWindow diaWin = new MFG_DialogWindow();
                string csn = diaWin.AskInput("RC Please Scan CSN ");
                if (csn.Trim().Length != 13 || !checkDigit_Femto(csn))
                {
                    MessageBox.Show("掃描CSN有誤，請重新掃描", "Warning", MessageBoxButtons.OK);
                    diaWin.InitialDialog();
                    csn = diaWin.AskInput("Scan CSN is wrong , Please Rescan CSN ");
                    // if (csn.Length != 12 || csn.Substring(0, 5) != "FXICB")
                    if (csn.Length != 13 || !checkDigit_Femto(csn))
                    {
                        MessageBox.Show("掃描CSN有誤，請確認CSN是否是13位，或者第13位是否正確", "Warning", MessageBoxButtons.OK);
                        this.SetStatusFlag(StatusFlag.STANDBY);
                        return;
                    }
                }
                this.curTestInfo.ScanCsn = csn;
                strSend = string.Format("{0}END\r\n", scanData.PadRight(25));
            }
            else if (stpInfo.Station == "ATO")
            {
                if (scanData.Trim().Length != 12)
                {
                    MessageBox.Show("掃描MAC有誤，請重新掃描", "Warning", MessageBoxButtons.OK);
                    this.SetStatusFlag(StatusFlag.STANDBY);
                    return;
                }

                MFG_DialogWindow diaWin = new MFG_DialogWindow();
                string csn = diaWin.AskInput("Please Scan CSN ");
                if (csn.Trim().Length != 13 || !checkDigit_Femto(csn))
                {
                    MessageBox.Show("掃描CSN有誤，請重新掃描", "Warning", MessageBoxButtons.OK);
                    diaWin.InitialDialog();
                    csn = diaWin.AskInput("Scan CSN is wrong , Please Rescan CSN ");
                    // if (csn.Length != 12 || csn.Substring(0, 5) != "FXICB")
                    if (csn.Length != 13 || !checkDigit_Femto(csn))
                    {
                        MessageBox.Show("掃描CSN有誤，請確認CSN是否是13位，或者第13位是否正確", "Warning", MessageBoxButtons.OK);
                        this.SetStatusFlag(StatusFlag.STANDBY);
                        return;
                    }
                }
                this.curTestInfo.ScanCsn = csn;
                strSend = string.Format("{0}END\r\n", scanData.PadRight(25));
            }
            else if (stpInfo.Station == "RW")
            {
                strSend = string.Format("{0}{1}END\r\n", scanData.PadRight(25), scanData.PadRight(25));
            }
            com_SFIS.Write(strSend);



        }
        
        private void scanMode_8(string scanData)
        {
            // for No sfc control
            string buff = scanData.Trim();
            if (stpInfo.Station.Contains("PT"))
            {
                if (buff.Length == 9)
                {
                    curTestInfo.SfcPsn = buff;
                }
                Start_RUN();
            }
            else if (stpInfo.Station == "FT4")
            {
                if (buff.Length == 17)
                {
                    curTestInfo.SfcCsn = buff;
                }
                Start_RUN();
            }

        }
        
        private void com_fixture_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            //string buff = com_fixture.ReadLine();
            string buff = com_fixture.ReadExisting();
            curTestInfo.FixRecv += buff;
            SetText("Fix Com : " + buff + "\n");
        }
       
        public bool WriteFixtureComAndWait(string cmd, int timeout, string exp)
        {
            this.curTestInfo.FixRecv = "";
            com_fixture.Write(cmd);
            int nTimeout = timeout * 10;
            while (nTimeout > 0)
            {
                Thread.Sleep(100);
                if (curTestInfo.FixRecv.Contains(exp)) return true;
                nTimeout--;
            }
            return false;
        }
        
        private void com_DUT_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            string buff = com_DUT.ReadExisting();
            curTestInfo.ComDutRecv += buff;
            SetText("DUT COM -->" + buff);
        }
 

        private void WriteDebugMessage(string msg, int iColor = 0)
        {
            if (msg.Contains("---") && !msg.Contains("OK"))
            {
                //設置每個Item 開始測試時的時間
                useTimePerItem = string2Int(this.Label_Time.Text.Trim().Trim('s'));
            }

            if (msg.Contains("---") && msg.Contains("OK"))
            {
                //所花時間-開始測試的時間
                useTimePerItem = string2Int(this.Label_Time.Text.Trim().Trim('s')) - useTimePerItem;
                SetText("Test time : " + useTimePerItem.ToString(), 1);
            }
            string fullmsg = DateTime.Now.ToString("HH:mm:ss.fff") + " " + msg;
            if (msg == null)
                //SetText(fullmsg + "\r", iColor);
                //WriteDebugMessage("\r\n");
                SetText("\n\r");
            else
                SetText(fullmsg, iColor);
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (stpInfo.DutCom.Length != 0)
                {
                    if (this.com_DUT.IsOpen)
                    {
                        com_DUT.Close();
                    }
                }
                //adbCMD.SendCmd("taskkill /F /IM execFemtoTest.exe");
                new CmdMessage("cmd.exe", false).SendCmd($"start taskkill /F /IM \"{stpInfo.FtuKill}.exe\"");

                // Process.Start("taskkill /F /IM cmd.exe");
            }
            catch (Exception exp)
            {
                return;
            }
            WriteCountNumToFile();
            System.Environment.Exit(0);
        }
        private bool WriteCountNumToFile()
        {
            string countFile = ".\\Count.dat";
            //string countFile = ".\\Setup.ini";
            if (!File.Exists(countFile))
            {
                File.Create(countFile);
                File.SetAttributes(countFile, FileAttributes.Hidden);
                Thread.Sleep(500);
                if (!File.Exists(countFile))
                {
                    return false;
                }
            }
            IniFile countIni = new IniFile(countFile);
            try
            {
            
                countIni.WriteInt("COUNT", "RF_Probe", stpInfo.RfProbeNum);
                countIni.WriteInt("COUNT", "RJ45", stpInfo.Rj45Num);
                countIni.WriteInt("COUNT", "PowerConnect", stpInfo.PowerConnectNum);
             
                countIni.WriteInt("COUNT", "fixProbeNum", stpInfo.FixProbeNum);
                countIni.WriteInt("COUNT", "FailNum", stpInfo.ContinueFailNum);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }

            countFile = ".\\Setup.ini";
            countIni = new IniFile(countFile);
            try
            {
                countIni.WriteInt("COUNT", "RF_Probe", stpInfo.RfProbeNum);
                countIni.WriteInt("COUNT", "RJ45", stpInfo.Rj45Num);
                countIni.WriteInt("COUNT", "PowerConnect", stpInfo.PowerConnectNum);
                countIni.WriteInt("COUNT", "fixProbeNum", stpInfo.FixProbeNum);
                countIni.WriteInt("COUNT", "FailNum", stpInfo.ContinueFailNum);

                countIni.WriteInt("COUNT", "TestPass_Num", curTestInfo.PassNum);
                countIni.WriteInt("COUNT", "TestFail_Num", curTestInfo.FailNum);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
            return true;
        }
        private void com_fixtureUSB_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            string buff = com_fixtureUSB.ReadLine();
            curTestInfo.UsbFixRecv += buff;
            SetText("USB Fix Com : " + buff + "\n");
        }

        private void timer_checkSFC_Tick(object sender, EventArgs e)
        {
            string RevcData = "";
            this.curTestInfo.NWaitSfc++;
            if (curTestInfo.NWaitSfc >= 50)
            {
                curTestInfo.NWaitSfc = 0;
                if (this.stpInfo.AutoScan == 1)
                    this.timer_autoScan.Enabled = true;
            }
            if (this.curTestInfo.MSfcRecv.Contains("SYANTE5"))
            {
                curTestInfo.MSfcRecv = "";
                return;
            }
            if (this.curTestInfo.MSfcRecv.Length < 25)
            {
                curTestInfo.MSfcRecv = "";
                return;
            }

            RevcData = curTestInfo.MSfcRecv;
            RevcData = RevcData.Trim();
            this.timer_checkSFC.Enabled = false;
            switch (stpInfo.SfisMode)
            {
                case 0:
                    SFISMode0(RevcData);
                    break;
                case 1:
                    SFISMode1(RevcData);
                    break;
                case 2:
                    SFISMode2(RevcData);
                    break;
            }
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.pictureBox1.SendToBack();
        }
        private void listView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (curTestInfo.OnTest)
                return;

            if (e.Button == MouseButtons.Right)
            {
                MFG_DialogWindow dialog = new MFG_DialogWindow();
                string result = dialog.AskPassword("請輸入密碼").Trim().ToUpper();
                if (stpInfo.Station == "PT" || stpInfo.Station == "FT1" || stpInfo.Station == "PT2" || stpInfo.Station == "PT3")
                {
                    if (result == "FIXTURE")
                    {
                        stpInfo.FixProbeNum = 0;

                        this.listView1.Items[6].SubItems.RemoveAt(1);
                        this.listView1.Items[6].SubItems.Add(stpInfo.FixProbeNum.ToString());


                    }
                    else if (result == "RF_PROBE")
                    {

                        stpInfo.RfProbeNum = 0;

                        this.listView1.Items[7].SubItems.RemoveAt(1);
                        this.listView1.Items[7].SubItems.Add(stpInfo.RfProbeNum.ToString());
                    }
                    else
                    {
                        MessageBox.Show("密碼錯誤,請重新嘗試");
                    }
                }

                textBox1.Focus();
            }

        }
        private void comm_SoftLED_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            string buff = comm_SoftLED.ReadExisting();
            curTestInfo.SoftRecv += buff;
            SetText("Soft LED COM -->" + buff);
        }
        private void com_Laser_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            string buff = com_Laser.ReadExisting();
            curTestInfo.LaserRecv += buff;
            SetText("Laser COM -->" + buff);
        }
        private void btn_Laser_Click(object sender, EventArgs e)
        {

            MFG_DialogWindow dialog = new MFG_DialogWindow();
            string result = dialog.AskPassword("請輸入密碼").Trim().ToUpper();
            if (result == "LOGOFF")
            {
                this.tabPage2.Parent = null;
            }
            else if (result == "LOGON")
            {
                this.tabPage2.Parent = this.tabControl1;
            }
            else
            {
                MessageBox.Show("密碼錯誤，請重新嘗試");
            }

        }
        private void Com_LedTestRecData(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {

            string buff = Com_LedTest.ReadExisting();
            curTestInfo.ComLedTestRecv += buff;
            SetText("LED COM -->" + buff);
        }
        private int string2Int(string str)
        {
            try
            {
                return Convert.ToInt32(str);
            }

            catch (Exception ex)
            {
                return -1;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            MFG_DialogWindow dialog = new MFG_DialogWindow();
            string result = dialog.AskPassword("請輸入密碼").Trim().ToUpper();
            if (result == "RJ45")
            {
                stpInfo.Rj45Num = 0;
                this.listView1.Items[6].SubItems.RemoveAt(1);
                this.listView1.Items[6].SubItems.Add(stpInfo.FixProbeNum.ToString());
            }
            else if (result == "PWR")
            {
                stpInfo.PowerConnectNum = 0;
                this.listView1.Items[7].SubItems.RemoveAt(1);
                this.listView1.Items[7].SubItems.Add(stpInfo.RfProbeNum.ToString());
            }
            else if (result == "UBNT")
            {
                updateContinueFail(false);
            }
            else
            {
                MessageBox.Show("密碼錯誤,請重新嘗試");
            }
        }
        private string GetStrBetween2Keys(String strBuffer, String strStart, String strEnd)
        {
            if ((strBuffer == "") || (strStart == "") || (strEnd == ""))
                return "";
            if ((strBuffer.IndexOf(strStart) < 0) || (strBuffer.IndexOf(strEnd) < 0))
                return "";
            int nStart = 0, nEnd = 0;
            String strTemp = strBuffer;
            nStart = strTemp.IndexOf(strStart) + strStart.Length;
            if (strEnd == "")
                nEnd = strTemp.Length;
            else
                nEnd = strTemp.IndexOf(strEnd, nStart);
            strTemp = strTemp.Substring(nStart, nEnd - nStart);
            strTemp = strTemp.Trim();

            strTemp = strTemp.Replace("\r", "").Replace("\n", "");
            return strTemp;
        }
        //nustreamInfo
        private void GetNustreamsLogMsg(string configPath)
        {

            XmlDocument config = new XmlDocument();
            config.Load(configPath);
            nustreamInfo.LogPath = config.SelectSingleNode(@"Config/Environment/LogPath").InnerText;
            nustreamInfo.ModelName = config.SelectSingleNode(@"Config/Environment/ModelName").InnerText;
            nustreamInfo.LogName = config.SelectSingleNode(@"Config/Environment/LogName").InnerText;


        }
        private bool GetNustreamsInfoMsg()
        {
            bool final_result = false;
            bool C_result = false;
            bool D_result = false;

            //D盤程式下 config    D:\\test_program\\XG24\\AF6LR000T02_FT_FCD012240_BOM10_V2\\CPEI_MFG\\bin\\Debug
            string config_path_D = Environment.CurrentDirectory + stpInfo.DConfPathName;


            XmlDocument D_config = new XmlDocument();
            try
            {
                D_config.Load(config_path_D);
                GetNustreamsLogMsg(config_path_D);
            }
            catch (System.Exception ex)
            {
                SetText(ex.ToString());
                MessageBox.Show("請聯繫TE檢查D盤配置檔！！！");
                return final_result;
            }
            //check CRC
            XmlNodeList D_crcless = D_config.SelectNodes(@"Config/TaskItems/Task/crc_less2");
            foreach (XmlNode crc in D_crcless)
            {
                string CRC_value = crc.InnerText;
                if (CRC_value != "0")
                {
                    D_result = false;
                    endInfo.ErrorCode = "CRCDFF";
                    MessageBox.Show("D盤下config CRC有誤 ! !");
                    break;
                }
                else
                {
                    D_result = true;
                }
            }

            //check FrameLength
            if (D_result)
            {
                XmlNodeList D_FrameLength = D_config.SelectNodes(@"Config/TaskItems/Task/FrameLength");
                foreach (XmlNode fl in D_FrameLength)
                {
                    string frameLength = fl.InnerText;
                    if (frameLength != "60")
                    {
                        D_result = false;
                        endInfo.ErrorCode = "CONFFL";
                        MessageBox.Show("D盤下config FrameLength有誤 ! !");
                        break;
                    }
                    else
                    {
                        D_result = true;
                    }
                }
            }
            //check Utilization
            if (D_result)
            {
                XmlNodeList D_Utilization = D_config.SelectNodes(@"Config/TaskItems/Task/Utilization");
                foreach (XmlNode util in D_Utilization)
                {
                    string utilization = util.InnerText;
                    if (utilization != "100.0000")
                    {
                        D_result = false;
                        endInfo.ErrorCode = "CONFUL";
                        MessageBox.Show("D盤下config Utilization有誤 ! !");
                        break;
                    }
                    else
                    {
                        D_result = true;
                    }
                }
            }
            //check ToleranceLoss
            if (D_result)
            {
                XmlNodeList D_ToleranceLoss = D_config.SelectNodes(@"Config/TaskItems/Task/ToleranceLoss");
                foreach (XmlNode Loss in D_ToleranceLoss)
                {
                    string toleranceLoss = Loss.InnerText;
                    if (toleranceLoss != "0")
                    {
                        D_result = false;
                        endInfo.ErrorCode = "CONFTL";
                        MessageBox.Show("D盤下config ToleranceLoss有誤 ! !");
                        break;
                    }
                    else
                    {
                        D_result = true;
                    }
                }
            }
            //check FrameCount
            if (D_result)
            {
                XmlNodeList D_Task = D_config.SelectNodes(@"Config/TaskItems/Task");
                foreach (XmlNode task in D_Task)
                {
                    string task_name = task.Attributes["name"].Value;
                    XmlNode frameCount = task.SelectSingleNode("FrameCount");
                    string count = frameCount.InnerText;
                    if (task_name == "PT2-UC-1G")
                    {
                        if (count != "10000000")
                        {
                            D_result = false;
                            endInfo.ErrorCode = "CONFFC";
                            MessageBox.Show("D盤下config FrameCount有誤 ! !");
                            break;
                        }
                        else
                        {
                            D_result = true;
                        }
                    }
                    else if (task_name == "PT2-UC-10G")
                    {
                        if (count != "100000000")
                        {
                            D_result = false;
                            endInfo.ErrorCode = "CONFFC";
                            MessageBox.Show("D盤下config FrameCount有誤 ! !");
                            break;
                        }
                        else
                        {
                            D_result = true;
                        }
                    }
                    else if (task_name == "PT2-UC-2.5G")
                    {
                        if (count != "25000000")
                        {
                            D_result = false;
                            endInfo.ErrorCode = "CONFFC";
                            MessageBox.Show("D盤下config FrameCount有誤 ! !");
                            break;
                        }
                        else
                        {
                            D_result = true;
                        }
                    }
                }
            }

            //检查测试item数目



            //C盤APMPT下 config
            XmlDocument C_config = new XmlDocument();
            try
            {
                C_config.Load(stpInfo.CConfPathName);
            }
            catch (System.Exception ex)
            {
                SetText(ex.ToString());
                MessageBox.Show("請聯繫TE檢查C盤配置檔！！！");
                return final_result;
            }
            //check CRC
            XmlNodeList C_crcless = C_config.SelectNodes(@"Config/TaskItems/Task/crc_less2");
            foreach (XmlNode crc in C_crcless)
            {
                string CRC_value = crc.InnerText;
                if (CRC_value != "0")
                {
                    C_result = false;
                    endInfo.ErrorCode = "CRCDFF";
                    MessageBox.Show("C盤下config CRC有誤 ! !");
                    break;
                }
                else
                {
                    C_result = true;
                }
            }
            //check FrameLength
            if (C_result)
            {
                XmlNodeList C_FrameLength = C_config.SelectNodes(@"Config/TaskItems/Task/FrameLength");
                foreach (XmlNode fl in C_FrameLength)
                {
                    string frameLength = fl.InnerText;
                    if (frameLength != "60")
                    {
                        C_result = false;
                        endInfo.ErrorCode = "CONFFL";
                        MessageBox.Show("C盤下config FrameLength有誤 ! !");
                        break;
                    }
                    else
                    {
                        C_result = true;
                    }
                }
            }
            //check Utilization
            if (C_result)
            {
                XmlNodeList C_Utilization = C_config.SelectNodes(@"Config/TaskItems/Task/Utilization");
                foreach (XmlNode util in C_Utilization)
                {
                    string utilization = util.InnerText;
                    if (utilization != "100.0000")
                    {
                        C_result = false;
                        endInfo.ErrorCode = "CONFUL";
                        MessageBox.Show("C盤下config Utilization有誤 ! !");
                        break;
                    }
                    else
                    {
                        C_result = true;
                    }
                }
            }
            //check ToleranceLoss
            if (C_result)
            {
                XmlNodeList C_ToleranceLoss = C_config.SelectNodes(@"Config/TaskItems/Task/ToleranceLoss");
                foreach (XmlNode Loss in C_ToleranceLoss)
                {
                    string toleranceLoss = Loss.InnerText;
                    if (toleranceLoss != "0")
                    {
                        C_result = false;
                        endInfo.ErrorCode = "CONFTL";
                        MessageBox.Show("C盤下config ToleranceLoss有誤 ! !");
                        break;
                    }
                    else
                    {
                        C_result = true;
                    }
                }
            }

            //check FrameCount
            if (C_result)
            {
                XmlNodeList C_Task = C_config.SelectNodes(@"Config/TaskItems/Task");
                foreach (XmlNode task in C_Task)
                {
                    string task_name = task.Attributes["name"].Value;
                    XmlNode frameCount = task.SelectSingleNode("FrameCount");
                    string count = frameCount.InnerText;
                    if (task_name == "PT2-UC-1G")
                    {
                        if (count != "10000000")
                        {
                            C_result = false;
                            endInfo.ErrorCode = "CONFFC";
                            MessageBox.Show("C盤下config FrameCount有誤 ! !");
                            break;
                        }
                        else
                        {
                            C_result = true;
                        }
                    }
                    else if (task_name == "PT2-UC-10G")
                    {
                        if (count != "100000000")
                        {
                            C_result = false;
                            endInfo.ErrorCode = "CONFFC";
                            MessageBox.Show("C盤下config FrameCount有誤 ! !");
                            break;
                        }
                        else
                        {
                            C_result = true;
                        }
                    }
                    else if (task_name == "PT2-UC-2.5G")
                    {
                        if (count != "25000000")
                        {
                            C_result = false;
                            endInfo.ErrorCode = "CONFFC";
                            MessageBox.Show("C盤下config FrameCount有誤 ! !");
                            break;
                        }
                        else
                        {
                            C_result = true;
                        }
                    }
                }
            }
            //結果
            if (C_result && D_result)
            {
                final_result = true;
                WriteDebugMessage("-----GetNustreamsInfoMsg PASS------");
            }
            else if (C_result == false)
            {
                MessageBox.Show("C盤下config 配置檔有誤，請通知TE確認 ! !");
                final_result = false;
                WriteDebugMessage("-----C盤下config 配置檔有誤，請通知TE確認------");
            }
            else if (D_result == false)
            {
                MessageBox.Show("D盤下config 配置檔有誤，請通知TE確認 ! !");
                final_result = false;
                WriteDebugMessage("-----D盤下config 配置檔有誤，請通知TE確認------");
            }
            else
            {
                final_result = false;
                WriteDebugMessage("-----GetNustreamsInfoMsg Fail------");
            }
            return final_result;
        }

        //-------------------------snake test for nustream----------------------------------------------------------------



        void KiemTra()
        {
            // Kiểm tra caplock có bật không
            while (IsKeyLocked(Keys.CapsLock))
            {
                MessageBox.Show("Vui long tat Caplocks");
            }

        }


        private void Station_Test()
        {
            
            m_pAdbCMD = new CmdMessage("cmd.exe", false);
            if (stpInfo.Station == "PT2") StartTest_01();

            WriteDebugMessage("Total Test time : " + Label_Time.Text);
        }
        private void StartTest_01()
        {

            WindowControl control;
            string Curtime = DateTime.Now.ToString("HH:mm:ss");
            string Lasttime = DateTime.Now.ToString("HH:mm:ss");
            double duration = 0.0;
            bool result = false;
            string analyse = null;
            Process[] java = Process.GetProcessesByName(stpInfo.FtuKill);
            if (java.Length != 0)
            {
                java[0].Kill();
            }
            IntPtr hconnetui;



            try
            {

                init();
                WriteDebugMessage("-----PT2 Test start------");
                string current = Directory.GetCurrentDirectory();
                control = new WindowControl();
                CmdMessage cmd = new CmdMessage("cmd.exe", false);
                cmd.SendCmd("arp -d");
                //bool pingStatue = DetectDevices("192.168.2.20");
                //pingStatue == true;
                if (true)
                {

                    cmd.SendCmd($"cd \"./{stpInfo.FtuKill}\" && \"{stpInfo.FtuKill}.exe\" -p=UniFiSwitch --mvc");
                    Thread.Sleep(3000);

                    IntPtr hTemp = new WindowControl().GetMainWindow(null, stpInfo.JavaWindows);
                    int Wait_time = 0;
                    while (Wait_time < 1000)
                    {
                        Thread.Sleep(200);
                        hTemp = new WindowControl().GetMainWindow(null, stpInfo.JavaWindows);
                        if (hTemp != new IntPtr(0))
                        {
                            new WindowControl().RaiseWindowProcess(hTemp);
                            new WindowControl().SetWindowMaximize(hTemp);
                            break;
                        }
                    }

                    new WindowControl().RaiseWindowProcess(hTemp);
                    new WindowControl().SetWindowMaximize(hTemp);
                    IntPtr Wnd = new IntPtr(0);
                    IntPtr sWnd = new IntPtr(0);
                    IntPtr txt = new IntPtr(0);
                    IntPtr botten1 = new IntPtr(0);
                    IntPtr botton2 = new IntPtr(0);
                    IntPtr txt1 = new IntPtr(0);
                    IntPtr Wnd1 = new IntPtr(0);

                    Wnd = FindWindowEx((IntPtr)0, (IntPtr)0, "", stpInfo.JavaWindows);

                    Win32Api.SetForegroundWindow(Wnd);
                    Thread.Sleep(1000);

                    hTemp = new WindowControl().GetMainWindow(null, stpInfo.JavaWindows);
                    hconnetui = new WindowControl().GetMainWindow(null, "Notice! Cannot Connect UI Cloud");
                    while (hconnetui != IntPtr.Zero)
                    {
                        hconnetui = new WindowControl().GetMainWindow(null, "Notice! Cannot Connect UI Cloud");
                        new WindowControl().SetWindowToTop(hconnetui);
                        Thread.Sleep(500);
                        new WindowControl().ClickTarget(689, 419);
                        Thread.Sleep(1000);


                    }
                    SendKeys.SendWait(textBox1.Text);
                    Thread.Sleep(1000);
                    SendKeys.SendWait("{F2}");

                    analyse = saveLog();
                    if (analyse.Contains("PASS"))
                    {

                        result = true;
                    }

                    if (analyse.Contains("FAIL"))
                    {

                        result = false;
                    }

                }
                else
                {
                    endInfo.ErrorCode = "EthErr";
                    result = false;
                }
                affichMain();
            }
            catch (System.Exception ex)
            {
                WriteDebugMessage(ex.Data.ToString());
                endInfo.ErrorCode = "Catch a error";
                result = false;
                goto FT1_STOP;
            }
            finally
            {
                if (result)
                {
                    CheckCondition.SetPass();
                }
                else
                {
                    CheckCondition.SetMac(curTestInfo.ScanMac);
                    CheckCondition.SetFailed(endInfo.ErrorCode);
                }
            }
            Curtime = DateTime.Now.ToString("HH:mm:ss");
            DateTime t1 = DateTime.Parse(Lasttime);
            DateTime t2 = DateTime.Parse(Curtime);
            System.TimeSpan t3 = t2 - t1;
            duration = t3.TotalSeconds;
            if (result)
            {
                AddCsvContext("PT2", "PASS");
                AddToTestSummary("PT2", "---", "---", "---", "---", result, Convert.ToInt32(duration));
                goto FT1_STOP;
            }
            else
            {
                AddCsvContext("PT2", "FAIL");
                AddToTestSummary("PT2", "---", "---", "---", "---", result, Convert.ToInt32(duration));
                goto FT1_STOP;
            }

        FT1_STOP:
            if (stpInfo.Station == "PT2")
            {
                curTestInfo.TestItemCsvLogContent[6] = result ? "PASS" : "FAIL";
                curTestInfo.OracleLogContent[6] = string.Format("{0},{1},{2},{3},{4},{5},{6}", curTestInfo.ScanPsn, stpInfo.Station, "PT Test", result ? "PASS" : "FAIL", result ? "PASS" : "FAIL", 7, DateTime.Now.ToString("yyyyMMdd HH:mm:ss"));
                if (result)
                {
                    WriteDebugMessage("PT2 Test Pass");
                    endInfo.Result = true;
                    endInfo.Finished = true;
                    return;
                }
                else
                {
                    WriteDebugMessage("PT2 Test Failed");
                    endInfo.Result = false;
                    endInfo.Finished = true;
                    return;
                }
            }
        }

        private void affichMain()
        {

            hTemp_Main = new WindowControl().GetMainWindow(null, "Test");
            Thread.Sleep(200);
            new WindowControl().RaiseWindowProcess(hTemp_Main);
        }


        private bool checkResult(string logfile)
        {
            if (!File.Exists(logfile)) return checkResult(logfile);
            StreamReader SR_tmp = new StreamReader(logfile);
            string fileText = SR_tmp.ReadToEnd();
            SR_tmp.Close();
            SR_tmp.Dispose();
            WriteDebugMessage("\r\n");
            WriteDebugMessage(fileText);
            if (fileText.Contains("*  P A S S  *")) return true;
            else if (fileText.Contains("*  F A I L  *")) return false;
            else return false;
        }

        //add for J18B134.01 to check CSN
        private bool checkDigit_Femto(string strSN)
        {
            string strTmpSN = strSN.Trim();
            UInt32[] uiSN = new UInt32[13];
            char[] chSN = new char[13];
            UInt32[] uiSum = new UInt32[12]; //不需要第13位，第13位為checksum
            UInt32 SUM = 0;
            UInt32 checkSum = 0;

            // check SN length
            if (strTmpSN.Length != 13)
            {
                MessageBox.Show("Input SN length error, request is 13");
                return false;
            }
            /*Translate string to digit list*/
            //123456789: chSN[0]=1; chSN[1]=2;
            chSN = strTmpSN.ToCharArray();
            for (int i = 0; i < chSN.Length; i++)
            {
                try
                {
                    if (chSN[i] < '0' || chSN[i] > '9')
                    {
                        return false;
                    }
                    uiSN[i] = Convert.ToUInt32(chSN[i]) - Convert.ToUInt32('0'); ;

                }
                catch (System.Exception ex)
                {
                    return false;
                }
            }

            for (int i = 0; i < uiSN.Length - 1; i++)
            {
                if (i % 2 == 0) uiSum[i] = uiSN[i] * 1; //i =0,2,4,6,8,10;
                else uiSum[i] = uiSN[i] * 2; //i = 1,3,5,7,9,11;
            }

            /*1. Start from rear, multiply by 2, 1, 2, 1.... Calculate the sum*/
            SUM = uiSum[11] / 10 + uiSum[11] % 10 + uiSum[10] / 10 + uiSum[10] % 10 + uiSum[9] / 10 + uiSum[9] % 10
                + uiSum[8] / 10 + uiSum[8] % 10 + uiSum[7] / 10 + uiSum[7] % 10 + uiSum[6] / 10 + uiSum[6] % 10
                + uiSum[5] / 10 + uiSum[5] % 10 + uiSum[4] / 10 + uiSum[4] % 10 + uiSum[3] / 10 + uiSum[3] % 10
                + uiSum[2] / 10 + uiSum[2] % 10 + uiSum[1] / 10 + uiSum[1] % 10 + uiSum[0] / 10 + uiSum[0] % 10;

            /*2. The sum modulus by 10*/
            checkSum = SUM % 10;

            /*3. 10 substract remainder, then get the check digit*/
            checkSum = 10 - checkSum;
            if (checkSum == 10)
            {
                checkSum = 0;
            }

            if (checkSum == uiSN[12]) return true;
            else return false;

        }


        public bool Battery_Poweroff()
        {
            string errorCode = "";
            WriteDebugMessage("  -------- Power Off ------------ ");
            if (mPowersupply_Battery == null)
            {
                mPowersupply_Battery = new InstrBaseClass();

                if (!mPowersupply_Battery.OpenSession(0, eTAB5Test.GpibAddrBattery, 20))
                {
                    errorCode = "GPIB00  Power supply of Battery Open Session error";
                    SetText(errorCode);
                    return false;
                }
            }
            string cmd = "OUTP OFF";
            if (mPowersupply_Battery != null)
                mPowersupply_Battery.WriteGPIBCmd(cmd);
            if (mPowersupply_USB != null)
                mPowersupply_USB.WriteGPIBCmd(cmd);

            return true;
        }

        public bool Battery_PowerOn_New(ref string errorCode)
        {
            WriteDebugMessage("  -------- Power On ------------ ");
            if (mPowersupply_Battery == null)
            {
                mPowersupply_Battery = new InstrBaseClass();

                if (!mPowersupply_Battery.OpenSession(0, eTAB5Test.GpibAddrBattery, 20))
                {
                    errorCode = "GPIB00  Power supply of Battery Open Session error";
                    SetText(errorCode);
                    AddCsvContext("Battery_PowerOn", "FAIL");
                    return false;
                }
            }
            string cmd = "OUTP OFF";
            string cmd1 = "";

            mPowersupply_Battery.WriteGPIBCmd(cmd);

            cmd = string.Format("VOLT {0}", "12");
            cmd1 = string.Format("CURR {0}", "1.5");
            mPowersupply_Battery.WriteGPIBCmd(cmd);
            mPowersupply_Battery.WriteGPIBCmd(cmd1);
            cmd = "OUTP ON";
            mPowersupply_Battery.WriteGPIBCmd(cmd);



            //string str = mPowersupply_Battery.QueryGPIBCmd("*IDN?");
            //str = mPowersupply_Battery.QueryGPIBCmd("MEAS:CURR?");

            WriteDebugMessage("  -------- Power On OK!------------ ");
            AddCsvContext("Battery_PowerOn", "PASS");
            return true;
        }
        private void rb_Golden_CheckedChanged(object sender, EventArgs e)
        {

        }
        private void label_Station_Click(object sender, EventArgs e)
        {

        }
        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }
        private void timer_autoScan_Tick(object sender, EventArgs e)
        {
            if (this.rb_SfisON.Checked && this.stpInfo.AutoScan == 1)
            {
                timer_autoScan.Enabled = false;
                this.textBox1.Text = "";
                string snFile = "c:\\sn.txt";
                string scanTitle = this.stpInfo.AutoScanTitle;

                string scanExe = Directory.GetCurrentDirectory() + "\\Autoscan\\scan_barcode.exe";
                WindowControl winCtrl = new WindowControl();
                IntPtr hwnd = winCtrl.GetMainWindow(null, scanTitle);
                if (File.Exists(snFile))
                {
                    StreamReader sReader = new StreamReader(snFile);
                    string buff = sReader.ReadToEnd().Trim();
                    sReader.Close();
                    if (this.curTestInfo.LastPsn != buff)
                    {
                        this.textBox1.Text = buff;
                        KeyPressEventArgs keyEven = new KeyPressEventArgs('\r');
                        txtMacInput_01(sender, keyEven);
                        File.Delete(snFile);
                        return;
                    }
                }
                if (hwnd == IntPtr.Zero)
                {
                    try
                    {
                        Process.Start(scanExe);
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
                timer_autoScan.Enabled = true;
            }
        }
        private void S_OK_Click(object sender, EventArgs e)
        {
            if (this.S_PWD.Text == "te")
            {
                stpInfo.Model = this.S_Model.Text;
                stpInfo.Station = this.S_Station.Text;
                stpInfo.StationNo = this.S_StationNO.Text;
                stpInfo.LocalLog = this.S_LocalLogPath.Text;
                stpInfo.ServerLog = this.S_ServerLogPath.Text;
                stpInfo.ServerProgram = this.S_ServerProgramPath.Text;
                stpInfo.ServerMoc = this.S_ServerMOCPath.Text;
                stpInfo.SfisCom = this.S_SFISCom.Text;
                stpInfo.DutCom = this.S_DUTCom.Text;
                stpInfo.FixCom = this.S_FIXCom.Text;
                stpInfo.FwVersion = this.S_FWVersion.Text;
                stpInfo.FcdVersion = this.S_FCDVersion.Text;
                stpInfo.BomVersion = this.S_BOMVersion.Text;
                stpInfo.JavaPath = this.S_JavaPath.Text;
                stpInfo.JavaWindows = this.S_JavaWindows.Text;
                WriteLocalSetting();
                InitializeFormUI();
            }
            else
            {
                MessageBox.Show("Password is wrong!!");
            }
        }
        private void S_Cancel_Click(object sender, EventArgs e)
        {
            LoadLocalSetting();
            InitializeFormSetting();
        }
        private void S_PWD_TextChanged(object sender, EventArgs e)
        {

        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }
        private void label_Error_Click(object sender, EventArgs e)
        {

        }
        private void label_Result_Click(object sender, EventArgs e)
        {

        }
        //add to 20180528
        private void init()
        {
            string strTime_Now = System.DateTime.Now.ToString("yyyy.MM.dd");
            //string read_Path = @"D:\unifi\" + strTime_Now+"\\";
            string read_Path = @".\\" + stpInfo.LogPass + "logs";

            string temp_Path = @"D:\\temp_EAV24P0.log";
            string path_600i = @"C:\\APMPT-4.log\\APMPT-4 v2.1b045\\USWMP48FINAL\\log\\" + strTime_Now;
            try
            {
                if (Directory.Exists(read_Path))
                {
                    Directory.Delete(read_Path, true);
                    Thread.Sleep(500);
                }
                else
                {
                    Thread.Sleep(500);
                }

                //jifa
                if (File.Exists(temp_Path))
                {
                    File.Delete(temp_Path);
                    Thread.Sleep(500);
                }
                else
                {
                    Thread.Sleep(500);
                }
                if (Directory.Exists(path_600i))
                {
                    Directory.Delete(path_600i, true);
                    Thread.Sleep(500);
                }
                else
                {
                    Thread.Sleep(500);
                }

            }
            catch (System.Exception ex)
            {
                SetText(ex.ToString());
                while (true)
                {
                    MessageBox.Show("Can't delete the old logs\r\nKhong the xoa duoc log cu");
                }
            }
        }

  


        private void deleteLogSFTP(String file, String result)
        {
            bool bsftp = false;
            //MessageBox.Show("1");
            string strTime_Now = System.DateTime.Now.ToString("yyyy-MM-dd");
            string strTime_log = System.DateTime.Now.ToString("yyyyMMdd");
            string strTime_log_fail = System.DateTime.Now.ToString("yyyyMMddhhmmss");
            string saveLog = "";
            // var ftpSV = new FtpClient("200.166.2.202", "user", "ubnt");
            // ftpSV.Connect();
            string FilePathName = "";
            if (this.rb_SfisON.Checked)
            {
                saveLog = stpInfo.LocalLog + "\\" + stpInfo.Model + "\\" + stpInfo.StationNo + "\\" + strTime_Now + "\\";
            }
            else
            {
                saveLog = stpInfo.LocalLogNoSfc + "\\" + stpInfo.Model + "\\" + stpInfo.StationNo + "\\" + strTime_Now + "\\";
            }
            if (!Directory.Exists(saveLog))
            {
                Directory.CreateDirectory(saveLog);
            }
            Thread.Sleep(500);
            if (result == "PASS")
            {
                File.Copy(file, saveLog + result + "_" + curTestInfo.SfcPsn.Substring(0, 12) + "_" + stpInfo.Model + "_" + stpInfo.Station + "_" + stpInfo.StationNo + "_" + strTime_log_fail + "_" + curTestInfo.DutMo + ".log");
                FilePathName = saveLog + result + "_" + curTestInfo.SfcPsn.Substring(0, 12) + "_" + stpInfo.Model + "_" + stpInfo.Station + "_" + stpInfo.StationNo + "_" + strTime_log_fail + "_" + curTestInfo.DutMo + ".log";
            }
            if (result == "FAIL")
            {
                File.Copy(file, saveLog + result + "_" + curTestInfo.SfcPsn.Substring(0, 12) + "_" + stpInfo.Model + "_" + stpInfo.Station + "_" + stpInfo.StationNo + "_" + strTime_log_fail + "_" + curTestInfo.DutMo + "_" + endInfo.ErrorCode + ".log");
                FilePathName = saveLog + result + "_" + curTestInfo.SfcPsn.Substring(0, 12) + "_" + stpInfo.Model + "_" + stpInfo.Station + "_" + stpInfo.StationNo + "_" + strTime_log_fail + "_" + curTestInfo.DutMo + "_" + endInfo.ErrorCode + ".log";
            }
            Thread.Sleep(2000);
            //SFIS ON, on envoie les logs au serveur
            /*  string destTxt = string.Format("{0}\\{1}\\{2}\\{3}\\", "\\UBNT_Test_Logs", stpInfo.Model, stpInfo.StationNo, strTime_Now);
              string buff = Path.GetDirectoryName(destTxt);
              if (!Directory.Exists(buff))
              {
                  Directory.CreateDirectory(buff);
              }
              if (!ftpSV.DirectoryExists(destTxt))
              {
                  ftpSV.CreateDirectory(destTxt);

              }
              */

            string serverLog = "\\UBNT_Test_Logs\\" + stpInfo.Model + "\\" + stpInfo.StationNo + "\\" + strTime_Now + "\\";
            SFtp.SFtpTransmit sftp = new SFtp.SFtpTransmit("200.166.2.202", "4422", "user", "ubnt");
            if (this.rb_SfisON.Checked)
            {
                try
                {
                    string destTxt = serverLog;

                    if (result == "PASS")
                    {
                        //ftp.UploadFile(file, destTxt + "PASS" + "_" + Path.GetFileName(file));
                        FilePathName = saveLog + result + "_" + curTestInfo.SfcPsn.Substring(0, 12) + "_" + stpInfo.Model + "_" + stpInfo.Station + "_" + stpInfo.StationNo + "_" + strTime_log_fail + "_" + curTestInfo.DutMo + ".log";
                    }
                    if (result == "FAIL")
                    {
                        //ftp.UploadFile(file, destTxt + "FAIL" + "_" + curTestInfo.error_Code + "_" + Path.GetFileName(file));
                        FilePathName = saveLog + result + "_" + curTestInfo.SfcPsn.Substring(0, 12) + "_" + stpInfo.Model + "_" + stpInfo.Station + "_" + stpInfo.StationNo + "_" + strTime_log_fail + "_" + curTestInfo.DutMo + "_" + endInfo.ErrorCode + ".log";
                    }
                    /// fi.MoveTo(newFile);

                    bsftp = sftp.VNUpLoadLogToSftp(FilePathName, destTxt);
                    Thread.Sleep(500);
                    while (bsftp == false)
                    {
                        MessageBox.Show("Please check connection to sever, save log to SFTP FAIL");
                    }
                }


                catch (Exception ex)
                {
                    while (true)
                    {
                        MessageBox.Show(ex.ToString());
                    }

                }
                Thread.Sleep(500);
            }
            File.Delete(file);
        }
        public void click_FTU_Yes_No_windows(bool result)
        {
            IntPtr hPOE;
            IntPtr hTemp = new WindowControl().GetMainWindow(null, stpInfo.JavaWindows);
            Thread.Sleep(200);
            new WindowControl().RaiseWindowProcess(hTemp);
            new WindowControl().SetWindowMaximize(hTemp);
            if (result == true)
            {
                hPOE = new WindowControl().GetMainWindow(null, "Work fine?");
                Thread.Sleep(200);
                new WindowControl().RaiseWindowProcess(hPOE);
                new WindowControl().SetWindowToTop(hPOE);
                //   Thread.Sleep(2000);
                Thread.Sleep(100);
                //SendReturnKey(h600iTip);
                SendKeys.SendWait("y");

            }
            else
            {
                hPOE = new WindowControl().GetMainWindow(null, "Work fine?");
                new WindowControl().RaiseWindowProcess(hPOE);
                new WindowControl().SetWindowToTop(hPOE);
                //   Thread.Sleep(2000);
                Thread.Sleep(200);
                //SendReturnKey(h600iTip);
                SendKeys.SendWait("n");

            }
        }
        public void click_Instruction_windows(string child_Main)
        {
            IntPtr hPOE;
            IntPtr hTemp = new WindowControl().GetMainWindow(null, stpInfo.JavaWindows);
            Thread.Sleep(100);
            new WindowControl().RaiseWindowProcess(hTemp);
            Thread.Sleep(100);
            new WindowControl().SetWindowMaximize(hTemp);
            Thread.Sleep(200);
            hPOE = new WindowControl().GetMainWindow(null, child_Main);
            Thread.Sleep(200);
            new WindowControl().RaiseWindowProcess(hPOE);
            new WindowControl().SetWindowToTop(hPOE);
            Thread.Sleep(200);
            keybd_event((byte)Keys.Tab, 0, 0, 0);
            Thread.Sleep(20);
            keybd_event((byte)Keys.F2, 0, 2, 0);
            Thread.Sleep(20);
            keybd_event((byte)Keys.Space, 0, 0, 0);
            Thread.Sleep(20);
            keybd_event((byte)Keys.Space, 0, 2, 0);
        }
        //add in 20180528
        private string saveLog()
        {
            //trouver le chemin de log
            String date = DateTime.Now.ToString("yyyyMMdd");
            //string log_PathPass = @"D:\unifi\" + date + "\\" + "U-LTE"+ "\\" +"pass"+ "\\";
            //string log_PathFail = @"D:\unifi\" + date + "\\" + "U-LTE"+ "\\" +"fail"+ "\\";
            string log_PathPass = @".\\" + stpInfo.LogPass + "logs\\Pass\\";
            string log_PathFail = @".\\" + stpInfo.LogFail + "logs\\Fail\\";
            string log_PathTemp = @".\\" + stpInfo.LogPass + "logs\\";
            string temp_Path = @"D:\\temp_EAV24P0.log";
            //creer le dossier par la date 
            //string saveLog = stpInfo.LocalLog + "\\" + stpInfo.Model + "\\" + stpInfo.StationNo + "\\" + date + "\\";
            string result = "FAIL";
            string erroCode600i = "";
            bool result_600i = false;
            bool flag_600i = true;
            bool flag_600i0 = true;
            int i = 0;

            bool Poe_check = true;
            bool Copy_log = true;

            bool Poe_10W = true;
            bool Poe_0W = true;
            bool hw1to5 = true;
            bool Put1_4net = true;
            bool Out1_4net = true;

            IntPtr h600iTip;
            IntPtr hFTU;
            IntPtr hPOE;


            bool flag_loop = true;
            int tipCount = 0;
            bool flag_Burn = true;
            bool flag_100G = true;
            bool flag_MGMT = true;
            bool flag_LED_port_Blink = true;
            int leght = 0, count = 0;
            try
            {
                if (!Directory.Exists(log_PathPass))
                {
                    Directory.CreateDirectory(log_PathPass);
                }
                if (!Directory.Exists(log_PathFail))
                {
                    Directory.CreateDirectory(log_PathFail);
                }
                while (i == 0)
                {


                    string[] fileListPass = Directory.GetFiles(log_PathPass, "*.log");
                    if (fileListPass.Length != 0)
                    {
                        foreach (string file in fileListPass)
                        {
                            Process[] java = Process.GetProcessesByName(stpInfo.FtuKill);
                            java[0].Kill();

                            System.Diagnostics.Process[] cmd = System.Diagnostics.Process.GetProcessesByName("cmd");
                            foreach (System.Diagnostics.Process p in cmd)
                            {
                                p.Kill();
                            }
                            // bool checkPOWLog = checkPOEpower10W(file);
                            bool checkresult = CheckPassLog(file);
                            getFTUVersion(file);
                            if (checkresult == false)
                            {
                                result = "FAIL";
                            }
                            else
                            {
                                result = "PASS";

                            }
                            //CountryCheck(file);

                            if (stpInfo.Sftp == 1)
                            {
                                deleteLogSFTP(file, result);
                            }
                            else
                            {
                                deleteLog(file, result);
                            }

                            i = 1;
                        }
                    }

                    string[] fileListFail = Directory.GetFiles(log_PathFail, "*.log");
                    if (log_PathFail.Length != 0)
                    {
                        foreach (string file in fileListFail)
                        {
                            Process[] java = Process.GetProcessesByName(stpInfo.FtuKill);
                            java[0].Kill();

                            System.Diagnostics.Process[] cmd = System.Diagnostics.Process.GetProcessesByName("cmd");
                            foreach (System.Diagnostics.Process p in cmd)
                            {
                                p.Kill();
                            }
                            result = "FAIL";
                            catchErrocode(file);
                            getFTUVersion(file);
                            //CountryCheck(file);
                            if (stpInfo.Sftp == 1)
                            {
                                deleteLogSFTP(file, result);
                            }
                            else
                            {
                                deleteLog(file, result);
                            }
                            i = 1;
                        }
                    }
                    //wait for snake test
                    if (Copy_log)
                    {
                        if (fileListFail.Length != 0)
                        {
                            result_600i = false;
                            break;
                        }
                        string[] fileListTemp = Directory.GetFiles(log_PathTemp, "*.log");
                        Thread.Sleep(100);
                        if (log_PathTemp.Length != 0)
                        {
                            foreach (string file in fileListTemp)
                            {
                                if (file.Contains(curTestInfo.SfcPsn.Substring(0, 12)))
                                {
                                    File.Copy(file, temp_Path, true);
                                    Thread.Sleep(300);
                                    if (File.Exists(temp_Path))
                                    {

                                        string strbuff = null;
                                        string totalLog = null;
                                        StreamReader SR = new StreamReader(temp_Path, Encoding.Default);
                                        strbuff = SR.ReadLine();
                                        totalLog = strbuff;
                                        while (strbuff != null)//read log
                                        {
                                            strbuff = SR.ReadLine();
                                            totalLog += strbuff;
                                        }
                                        SR.Close();

                                        if (flag_MGMT)
                                        {
                                            if (totalLog.Contains("Is MGMT port LED GREEN?"))
                                            {
                                                Thread.Sleep(100);
                                                hmessageWorkFind Mshow_image = new hmessageWorkFind();
                                                Mshow_image.TopMost = true;
                                                bool res = Mshow_image.Show_string("Vui lòng kiểm tra đèn LED cổng MGMT có sáng xanh không\r\nNếu CÓ ấn YES, KHÔNG SÁNG ấn NO", "./image/MGMT.jpg");
                                                click_FTU_Yes_No_windows(res);
                                                flag_MGMT = false;
                                            }
                                        }
                                        //SFP
                                        if (flag_loop)
                                        {
                                            if (totalLog.Contains("Please loop port")
                                                && totalLog.Contains("[1, 2],[3, 4],[5, 6],[7, 8],[9, 10],[11, 12],")
                                                && totalLog.Contains("[13, 14],[15, 16],[17, 18],[19, 20],[21, 22],[23, 24] with Cat6a cable."))
                                            {
                                                Thread.Sleep(100);
                                                hmessage3 Mshow_image = new hmessage3();
                                                Mshow_image.TopMost = true;
                                                Mshow_image.Show_string("Vui lòng đẩy tất cả cần gạt vào sản phẩm\r\nSau đó nhấn OK", "./image/all_port.jpg");
                                                Thread.Sleep(200);
                                                click_Instruction_windows("Instruction");
                                                flag_loop = false;
                                            }
                                        }
                                        if (flag_100G)
                                        {
                                            if (totalLog.Contains("Please loop port")
                                                && totalLog.Contains("[25, 26],[27, 28] with SPEED_100G QSFP cable."))
                                            {
                                                Thread.Sleep(100);
                                                hmessage3 Mshow_image = new hmessage3();
                                                Mshow_image.TopMost = true;
                                                Mshow_image.Show_string("Vui lòng cắm dây quang 100G AOC vào cổng [25, 26] và [27, 28]\r\n Kiểm tra xem tất cả các đèn LED đã sáng chưa\r\nSau đó nhấn OK", "./image/SFP_light.jpg");
                                                Thread.Sleep(200);
                                                click_Instruction_windows("Instruction");
                                                flag_100G = false;
                                            }
                                        }
                                        //1. Are all etherlight LEDs breathing light?
                                        if (flag_LED_port_Blink)
                                        {
                                            if (totalLog.Contains("Please check if")
                                                && totalLog.Contains("1. Are all etherlight LEDs breathing light?"))
                                            {
                                                Thread.Sleep(100);
                                                hmessageWorkFind Mshow_image = new hmessageWorkFind();
                                                Mshow_image.TopMost = true;
                                                bool res = Mshow_image.Show_string("Vui lòng kiểm tra tất cả cổng mạng có sáng nhấp nháy hay không\r\nNếu sáng ấn YES, không sáng ấn NO", "./image/light_breathing.jpg");
                                                click_FTU_Yes_No_windows(res);
                                                flag_LED_port_Blink = false;
                                            }
                                        }
                                        if (flag_Burn)
                                        {
                                            if (totalLog.Contains("Please do following before power cycle.")
                                                && totalLog.Contains("Loop ports:")
                                               && totalLog.Contains("[1, 2],[3, 4],[5, 6],[7, 8],[9, 10],[11, 12],")
                                               && totalLog.Contains("[13, 14],[15, 16],[17, 18],[19, 20],[21, 22],[23, 24]")
                                               && totalLog.Contains("Loop sfp ports with QSFP28 cable:")
                                                && totalLog.Contains("[25, 26],[27, 28]"))
                                            {

                                                Thread.Sleep(100);
                                                click_Instruction_windows("Instruction");
                                                flag_Burn = false;
                                            }
                                        }

                                    }
                                    else
                                    {
                                        MessageBox.Show("temp_EAV24P0 文件不存在 ! !");
                                    }
                                }


                                else
                                {
                                    MessageBox.Show("存在多餘log ! !");
                                }

                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                SetText(ex.ToString());
                while (true)
                {
                    MessageBox.Show("clear and copy Log error", "消息", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                }
            }
            return result;
        }


        private void deleteLog(String file, String result)
        {
   
            string strTime_Now = System.DateTime.Now.ToString("yyyy-MM-dd");
            string strTime_log = System.DateTime.Now.ToString("yyyyMMdd");
            string strTime_log_fail = System.DateTime.Now.ToString("yyyyMMddhhmmss");
            var ftpSV = new FtpClient("200.166.2.200", "user", "ubnt");
            ftpSV.Connect();
            string saveLog = null;
            if (this.rb_SfisON.Checked)
            {
                saveLog = stpInfo.LocalLog + "\\" + stpInfo.Model + "\\" + stpInfo.StationNo + "\\" + strTime_Now + "\\";
            }
            else
            {
                saveLog = stpInfo.LocalLogNoSfc + "\\" + stpInfo.Model + "\\" + stpInfo.StationNo + "\\" + strTime_Now + "\\";
            }
            if (!Directory.Exists(saveLog))
            {
                Directory.CreateDirectory(saveLog);
            }
            Thread.Sleep(500);
            if (this.rb_SfisON.Checked)
            {
                try
                {
                    string destTxt = string.Format("{0}\\{1}\\{2}\\{3}\\", "\\UBNT_Test_Logs", stpInfo.Model, stpInfo.StationNo, strTime_Now);
                    string buff = Path.GetDirectoryName(destTxt);
                    if (!Directory.Exists(buff))
                    {
                        Directory.CreateDirectory(buff);
                    }
                    if (!ftpSV.DirectoryExists(destTxt))
                    {
                        ftpSV.CreateDirectory(destTxt);

                    }

                    string strFtpPath = "/UBNT_Test_Logs/" + stpInfo.Model + "/" + stpInfo.StationNo + "/" + strTime_Now + "/";
                    if (result == "PASS")
                    {
                        File.Copy(file, saveLog + result + "_" + curTestInfo.SfcPsn.Substring(0, 12) + "_" + stpInfo.Model + "_" + stpInfo.StationNo.Substring(0, 9) + "_" + strTime_log_fail + "_" + curTestInfo.DutMo + ".log");
                        ftpSV.UploadFile(file, strFtpPath + result + "_" + curTestInfo.SfcPsn.Substring(0, 12) + "_" + stpInfo.Model + "_" + stpInfo.StationNo.Substring(0, 9) + "_" + strTime_log_fail + "_" + curTestInfo.DutMo + ".log");
                    }
                    if (result == "FAIL")
                    {
                        File.Copy(file, saveLog + result + "_" + curTestInfo.SfcPsn.Substring(0, 12) + "_" + stpInfo.Model + "_" + stpInfo.StationNo.Substring(0, 9) + "_" + strTime_log_fail + "_" + curTestInfo.DutMo + "_" + endInfo.ErrorCode + ".log");
                        ftpSV.UploadFile(file, strFtpPath + result + "_" + curTestInfo.SfcPsn.Substring(0, 12) + "_" + stpInfo.Model + "_" + stpInfo.StationNo.Substring(0, 9) + "_" + strTime_log_fail + "_" + curTestInfo.DutMo + "_" + endInfo.ErrorCode + ".log");
                    }
                    Thread.Sleep(500);
                }

                catch (Exception ex)
                {
                    SetText(ex.ToString());
                    MessageBox.Show("Save the log to server failed,Please check the connnection to the server!");
                }
            }
            File.Delete(file);


        }

        private void catchErrocode(string fs)
        {
            StreamReader SR_tmp = new StreamReader(fs);
            StringBuilder sb = new StringBuilder();
            string fileText = SR_tmp.ReadToEnd();

            SR_tmp.Close();
            SR_tmp.Dispose();
            WriteDebugMessage("\n\r");
            WriteDebugMessage(fileText);

            string erroCode = null;

            if (fileText.IndexOf("4  Check MAC address                 1       0/1") >= 0)
            {
                erroCode = "CMACFF";
            }
            else if (fileText.IndexOf("5  Check BOM revision                1       0/1") >= 0)
            {
                erroCode = "CBOMFF";
            }
            //16  Check Activation Code             1       1/0
            else if (fileText.IndexOf("2001  Check firmware version            1       0/1") >= 0)
            {
                erroCode = "FWVERF";
            }
            else if (fileText.IndexOf("752  Check UART port                   1       0/1") >= 0)
            {
                erroCode = "UARTFF";
            }
            else if (fileText.IndexOf("753  Check MGMT port                   1       0/1") >= 0)
            {
                erroCode = "MGMTFF";
            }
            else if (fileText.IndexOf("17  Check A12 status                  1       0/1") >= 0)
            {
                erroCode = "CA12FF";
            }
            else if (fileText.IndexOf("108  LED MCU version test              1       0/1") >= 0)
            {
                erroCode = "LEDMCU";
            }
            else if (fileText.IndexOf("330  Port link test                    1       0/1") >= 0)
            {
                erroCode = "LINKFF";
                int count = 0;
                for (int i = 1; i <= 24; ++i)
                {
                    if (i < 10)
                    {
                        if (fileText.IndexOf("Failed to check 'Link' of port " + i) >= 0)
                        {
                            erroCode = "PORT" + i + "F";
                        }

                    }
                    else
                    {
                        if (fileText.IndexOf("Failed to check 'Link' of port " + i) >= 0)
                        {
                            erroCode = "PORT" + i;
                        }
                    }
                }
            }

            else if (fileText.IndexOf("9108  Check PTP UART and Clock Gen FW Version    1       0/1") >= 0)
            {
                erroCode = "PTPUAT";
            }
            else if (fileText.IndexOf("1000  Enable burnin test                1       0/1") >= 0)
            {
                erroCode = "SEBURN";
            }
            else
            {
                erroCode = "UNKNFF";
            }
            endInfo.ErrorCode = erroCode.ToString();

        }
        public void getFTUVersion(string file)
        {
            StreamReader SR_tmp = new StreamReader(file);
            StringBuilder sb = new StringBuilder();
            string fileText = SR_tmp.ReadToEnd();
            SR_tmp.Close();
            SR_tmp.Dispose();
            string[] charsToRemove = new string[] { "@", ",", ".", ";", "'", "_", "-" };
            string resultFTU = GetStrBetween2Keys(fileText, stpInfo.FtuTruoc, stpInfo.FtuSau);
            foreach (var c in charsToRemove)
            {
                resultFTU = resultFTU.Replace(c, string.Empty).Trim();
            }
     
            string settingFile = ".\\Setup.ini";
            if (!File.Exists(settingFile))
            {
                MessageBox.Show("can not find " + settingFile);
                Environment.Exit(0);
            }

            IniFile ini = new IniFile(settingFile);
            ini.WriteString("LC_PARAM", "FTU_Current", resultFTU);
            ////Write FTU to Setup.ini

            stpInfo.FtuDut = ini.ReadString("LC_PARAM", "FTU_Current", "");
        }
        //add in 20191114


        private bool CheckPassLog(string fs)
        {
            bool checkresult = false;
            StreamReader SR_tmp = new StreamReader(fs);
            StringBuilder sb = new StringBuilder();
            string fileText = SR_tmp.ReadToEnd();

            SR_tmp.Close();
            SR_tmp.Dispose();
            WriteDebugMessage("\r\n");
            WriteDebugMessage(fileText);

            if (fileText.IndexOf("Total Test Items: [4, 5, 2001, 17, 752, 753, 330, 108, 9108, 1000]") >= 0)
            {
                checkresult = true;

            }
            else
            {
                checkresult = false;
                endInfo.ErrorCode = "CONFIG";
            }
            if (checkresult)
            {
                //Pass Items: 4 16 5 2001 17 330 108 1000
                if (fileText.IndexOf("Pass Items: 4 5 2001 17 752 753 330 108 9108 1000") >= 0
                    && fileText.IndexOf("times=220") >= 0)
                {
                    checkresult = true;

                }
                else
                {
                    checkresult = false;
                    endInfo.ErrorCode = "CONFIG";
                }
            }

            return checkresult;
        }

        //add in 20180519



        private void label_Ver_Click(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }
    }

    public class MessageEventArgs : System.EventArgs
    {
        public byte category;
        public string msgContent;

    }

}