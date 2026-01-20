using System;
using System.Threading;
using System.Runtime.InteropServices;

namespace CPEI_MFG
{
    public enum TCPIP_Type
    {
        INSTR = 0,
        SOCKET = 1
    }
    public enum ConnectType
    {
        GPIB = 0,
        TCPIP = 1
    }
    class mt8820_Test
    {
        public InstrBaseClass mt8820Control;
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
        delegate void MT8820Logging(byte category,string msgContent);

        void MT8820_LoggingToConsole(byte category,string msgContent)
        {
            this.OnSendMessage(category,msgContent);
        }
        MT8820Logging mt8820Logging = null;

        
        
        public struct MT8820PARAM
        {
            public int gpibAddr;
            public int BoardId;
            public string ipAddr;
            public ConnectType connType;
            public TCPIP_Type tcpipType;
            public int portNum;
            
        }
        MT8820PARAM mt8820para;


        mt8820_Test(MT8820PARAM param)
        {
            mt8820para = param;
            mt8820Logging = new MT8820Logging(MT8820_LoggingToConsole);
        }
       

        public bool InitialEquitment(string errorCode)
        {
            if (mt8820para.connType == ConnectType.GPIB)
            {
                if (!mt8820Control.OpenSession(mt8820para.BoardId, mt8820para.gpibAddr, 2000))
                {
                    string errorMsg = "";
                    errorCode = "INITMT  Initial MT8820 error";
                    errorMsg = "Open session fail Board ID :" + mt8820para.BoardId.ToString() + " GPIB Address :" + mt8820para.gpibAddr.ToString(); 
                    WriteDebugMessage(errorMsg);
                    WriteDebugMessage(errorCode);
                    return false;
                }    
                
            }
            else
            {
                if (!mt8820Control.OpenSession(mt8820para.tcpipType, mt8820para.BoardId, mt8820para.ipAddr, mt8820para.portNum, 2000))
                {
                    string errorMsg = "";
                    errorCode = "INITM0  Open session fail";
                    errorMsg = "Open session fail Board ID :" + mt8820para.BoardId.ToString() + " IP Address :" + mt8820para.ipAddr.ToString() + " Port Num : " + mt8820para.portNum.ToString();
                    WriteDebugMessage(errorMsg);
                    WriteDebugMessage(errorCode);
                    return false;
                }
            }
            string buff = "";
            buff = mt8820Control.QueryGPIBCmd("*IDN?");
            if (!buff.Contains("Anritsu"))
            {
                errorCode = "INITM1  Check Anritsu 8820 connection fail";
                WriteDebugMessage(errorCode);
                return false;
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

        public bool InitialGsmTest(String errorCode,int timeout)
        {
            mt8820Control.WriteGPIBCmd("PRESET");
            for (int i = 0; i <= timeout;i++ )
            {
                Thread.Sleep(1000);
                string buff = mt8820Control.QueryGPIBCmd("CALLRSLT? 4");
                if (buff == "1,0")
                    break;
                

            }
            
            return true;
        }

        public bool InitialGprsTest()
        {
            return true;
        }
    }
}
