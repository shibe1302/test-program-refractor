using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;

namespace CPEI_MFG
{

    class DeviceControl
    {
      //  FOXCONN_CFT_API int     __stdcall FindDUT(
      //  const char * szDevDes,
    //    bool bIfGetDriverInfo,
   //     char * szDriverVer,
    //    char * szDriverDate);
        [DllImport("CommDLL.dll", CallingConvention = CallingConvention.StdCall,CharSet = CharSet.Ansi,EntryPoint = "FindDUT")]
        private extern static int FindDUT(string szDevDes, bool bIfGetDriverInfo, StringBuilder szDriverVer, StringBuilder szDriverDate);
       //FOXCONN_CFT_API bool    __stdcall ChangeDeviceState	(const char * szDevDesc, bool bState);

        [DllImport("CommDLL.dll",CallingConvention = CallingConvention.StdCall,CharSet=CharSet.Ansi, EntryPoint = "ChangeDeviceState")]
        private extern static bool ChangeDeviceState(string szDevDesc, bool bState);
        public DeviceControl()
        {

        }
        public int FindDevice(string szDevDesc, bool bGetDriver, ref string szDriver, ref string szDriverDate)
        {
            string desc = szDevDesc;
            StringBuilder driverVer = new StringBuilder(512);
            StringBuilder driverDate = new StringBuilder(512);
           

            int ret = FindDUT(desc, bGetDriver,  driverVer,  driverDate); 
           
            if (bGetDriver)
            {
                szDriver = driverVer.ToString();
                szDriverDate = driverDate.ToString();
            }
            else
            {
                szDriver = "";
                szDriverDate = "";
            }
            return ret;           
        }
        public bool EnableDevice(string sDevDesc,int timeout)
        {
            string desc = sDevDesc;


            while (timeout > 0)
            {
                if (ChangeDeviceState(desc, true))
                    break;
                Thread.Sleep(500);
                timeout--;
            }
            if (timeout <= 0)
            {
                return false;
            }
            return true;
        }
        public bool DisableDevice(string sDevDesc,int timeout)
        {
            string desc = sDevDesc;
            while (timeout > 0)
            {
                if(ChangeDeviceState(desc,false))
                     break;
                else
                    Thread.Sleep(500);
                timeout--;
            }
            if (timeout <= 0)
            {
                return false;
            }
            return true;
        }

    }
}
