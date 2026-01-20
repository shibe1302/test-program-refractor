using System;

namespace CPEI_MFG.Core
{
    public class TestInfo
    {
        public string SfcPanel { get; set; }
        public string SfcPsnStart { get; set; }
        public string SfcPsnEnd { get; set; }
        public string SfcPsn { get; set; }
        public string SfcCsn { get; set; }
        public string SfcImei { get; set; }
        public string SfcMeid { get; set; }
        public string SfcFw { get; set; }
        public string SfcEui { get; set; }
        public string DutPsn { get; set; }
        public string DutMac { get; set; }
        public string DutCsn { get; set; }
        public string DutImei { get; set; }
        public string DutMeid { get; set; }
        public string DutWifiMac { get; set; }
        public string DutBtMac { get; set; }
        public string SfcWifiMac { get; set; }
        public string SfcBtMac { get; set; }
        public string ScanCsn { get; set; }
        public string ScanPsn { get; set; }
        public string ScanMac { get; set; }
        public string LastPsn { get; set; }
        public string CountryCode { get; set; }
        public string StationNo { get; set; }
        public string DutQrCode { get; set; }
        public string DutBom { get; set; }
        public string DutFcd { get; set; }
        public string DutFw { get; set; }
        public string DutMo { get; set; }
        public bool Rescan { get; set; }
        public int CycleTime { get; set; }
        public int PassNum { get; set; }
        public int FailNum { get; set; }
        public bool BNeedRunIn { get; set; }
        public bool OnTest { get; set; }
        public bool SfcReply { get; set; }
        public string LaserRecv { get; set; }
        public string SoftRecv { get; set; }
        public string CsvLog { get; set; }
        public string FixRecv { get; set; }
        public string UsbFixRecv { get; set; }
        public string SockMsg { get; set; }
        public string ComDutRecv { get; set; }
        public string ComLedTestRecv { get; set; }
        public string MSfcRecv { get; set; }
        public bool BLoopTest { get; set; }
        public int NLoopPass { get; set; }
        public int NLoopFail { get; set; }
        public float NLoopRetest { get; set; }
        public string DutMainCamera { get; set; }
        public string DutSubCamera { get; set; }
        public string DutTp { get; set; }
        public string[] CsvLogContent { get; set; } = new string[128];
        public string[] TestItemCsvLogContent { get; set; } = new string[128];
        public string[] OracleLogContent { get; set; } = new string[128];
        public double TestCurrent { get; set; }
        public double TestVoltage { get; set; }
        public int NWaitSfc { get; set; }

        public void Reset()
        {
            SfcPsn = "";
            DutPsn = "";
            DutMac = "";
            CycleTime = 0;
            OnTest = false;
            // Clear các mảng log
            Array.Clear(CsvLogContent, 0, CsvLogContent.Length);
        }
    }
}
