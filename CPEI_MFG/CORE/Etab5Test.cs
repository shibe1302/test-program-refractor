using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPEI_MFG.Core
{
    public class Etab5Test
    {
        public int GpibAddrUsb { get; set; }
        public int GpibAddrBattery { get; set; }
        public float VolUsb { get; set; }
        public float VolBat { get; set; }
        public float AmpUsb { get; set; }
        public float AmpBat { get; set; }
        public float LeakageHighLimit { get; set; }
        public float LeakageLowLimit { get; set; }
        public float ChargeUsbHighlimit { get; set; }
        public float ChargeUsbLowlimit { get; set; }
        public float ChargeAdapterHighlimit { get; set; }
        public float ChargeAdapterLowlimit { get; set; }
        public int LowBatVol { get; set; }
        public int HighBatVol { get; set; }
        public int WifiRssiLowlimit { get; set; }
        public int BtRssiLowlimit { get; set; }
        public int GpsCnLowlimit { get; set; }
        public int TestItemNum { get; set; }
        public string[] UiResult { get; set; } = new string[100];
        public string DutDesc { get; set; }
        public string DutDriInfo { get; set; }
        public string DutDriDate { get; set; }
        public string FwVersion { get; set; }
        public string FwVersion1 { get; set; }
        public string KeyLed { get; set; }
        public int SpeakerAmpL { get; set; }
        public int SpeakerAmpR { get; set; }
        public int SpeakerAmpD { get; set; }
        public int HeadsetAmpL { get; set; }
        public int HeadsetAmpR { get; set; }
        public int HeadsetAmpD { get; set; }
        public string SpeakerLCmd { get; set; }
        public string SpeakerRCmd { get; set; }
        public string SpeakerDCmd { get; set; }
        public string HeadsetLCmd { get; set; }
        public string HeadsetRCmd { get; set; }
        public string HeadsetDCmd { get; set; }
        public int SpeakerLFre { get; set; }
        public int SpeakerRFre { get; set; }
        public int SpeakerDFre { get; set; }
        public int HeadsetLFre { get; set; }
        public int HeadsetRFre { get; set; }
        public int HeadsetDFre { get; set; }
        public int RunIn { get; set; }
        public int Display { get; set; }
        public int Touch { get; set; }
        public int Digitizer { get; set; }
        public int Key { get; set; }
        public int Led { get; set; }
        public int Headset { get; set; }
        public int Gps { get; set; }
        public int Wifi { get; set; }
        public int Bt { get; set; }
        public int Charge { get; set; }
        public int Lsensor { get; set; }
        public int Gsensor { get; set; }
        public int Msensor { get; set; }
        public int Gyro { get; set; }
        public int SpeakerL { get; set; }
        public int SpeakerR { get; set; }
        public int Microphone { get; set; }
        public int HeadsetL { get; set; }
        public int HeadsetR { get; set; }
        public int HeadsetMic { get; set; }
        public int MainCamera { get; set; }
        public int SubCamera { get; set; }
        public bool NeedFixture { get; set; }
        public bool AutoUsb { get; set; }
        public string UsbDataOpen { get; set; }
        public string UsbDataOpen1 { get; set; }
        public string PowerOn { get; set; }
        public string PowerOnReply { get; set; }
        public string SpeakerOpen { get; set; }
        public string SpeakerOpenReply { get; set; }
        public string VolumeUp { get; set; }
        public string VolumeUpReply { get; set; }
        public string VolumeDown { get; set; }
        public string VolumeDownReply { get; set; }
        public string UsbOn { get; set; }
        public string UsbOnReply { get; set; }
        public string UsbOffReply { get; set; }
    }
}