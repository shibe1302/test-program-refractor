using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPEI_MFG.Core
{
    public class StationInfo
    {
        public string Model { get; set; }
        public string Station { get; set; }
        public string StationNo { get; set; }
        public string LocalLog { get; set; }
        public string LocalLogNoSfc { get; set; }
        public string JavaPath { get; set; }
        public string LogPass { get; set; }
        public string LogFail { get; set; }
        public string JavaWindows { get; set; }
        public string NustreamWindows { get; set; }
        public string ModelFileName { get; set; }
        public string DConfPathName { get; set; }
        public string CConfPathName { get; set; }
        public string ServerProgram { get; set; }
        public string ServerMoc { get; set; }
        public string TeLocalLog { get; set; }
        public string ServerLog { get; set; }
        public string SfisCom { get; set; }
        public string FixCom { get; set; }
        public string DutCom { get; set; }
        public string FixUsbCom { get; set; }
        public bool AutoClose { get; set; }
        public int SfisMode { get; set; }
        public int ScanMode { get; set; }
        public int Sftp { get; set; }
        public int SaveMode { get; set; }
        public string Hostname { get; set; }
        public string StationName { get; set; }
        public string IpAdd1 { get; set; }
        public string IpAdd2 { get; set; }
        public int BIqFactTest { get; set; }
        public string IqCsv { get; set; }
        public int BNeedResetUsb { get; set; }
        public int BNeedRescan { get; set; }
        public int UsbCableNum { get; set; }
        public int RfProbeNum { get; set; }
        public int Rj45Num { get; set; }
        public int PowerConnectNum { get; set; }
        public int TestPassNum { get; set; }
        public int TestFailNum { get; set; }
        public int SubCamNum { get; set; }
        public int MainCamNum { get; set; }
        public int FixProbeNum { get; set; }
        public string SoftLedCom { get; set; }
        public string FwVersion { get; set; }
        public string FcdVersion { get; set; }
        public string BomVersion { get; set; }
        public string ApFwVersion { get; set; }
        public string BcFwVersion { get; set; }
        public string FsFwVersion { get; set; }
        public bool NeedGpib { get; set; }
        public int GpibAddress { get; set; }
        public string LedCom { get; set; }
        public string LteTestFile { get; set; }
        public string LteTestFileGolden { get; set; }
        public string LteTestTitle { get; set; }
        public string GoldenSn1 { get; set; }
        public string GoldenSn2 { get; set; }
        public string GoldenFlag { get; set; }
        public string NgSn1 { get; set; }
        public string NgSn2 { get; set; }
        public string NgFlag { get; set; }
        public double GoldenTime { get; set; }
        public int NgTime { get; set; }
        public int ContinueFailNum { get; set; }
        public int AutoScan { get; set; }
        public string AutoScanTitle { get; set; }
        public string FtuDut { get; set; }
        public string MoDut { get; set; }
        public string Region { get; set; }
        public string FtuKill { get; set; }
        public string FtuSau { get; set; }
        public string FtuTruoc { get; set; }
    }
}
