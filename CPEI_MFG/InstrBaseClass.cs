
using System;

using NationalInstruments.VisaNS;

namespace CPEI_MFG
{

    public class InstrBaseClass : BaseClass
    {
        
        public MessageBasedSession mbSession;

        /// <summary>
        /// ResourceName like as "GPIB0::14::INSTR"
        /// </summary>
        /// <param name="Board"></param>
        /// <param name="GPIBAddress"></param>
        /// <param name="Timeout"></param>
        /// <returns></returns>
        public bool OpenSession(int Board,int GPIBAddress,int Timeout)
        {
            string ResourceName = "GPIB" + Board + "::" + GPIBAddress + "::INSTR";
            //string[] resources = ResourceManager.GetLocalManager().FindResources("?*");
            try
            {
                WriteDebugMsg(ResourceName);
                mbSession = (MessageBasedSession)ResourceManager.GetLocalManager().Open(ResourceName,AccessModes.NoLock,5000);
                mbSession.Timeout = Timeout;
                mbSession.Clear();
                mbSession.Write("*CLS\n");
                mbSession.Query("*IDN?\n");
            }
            catch (InvalidCastException)
            {
                WriteDebugMsg("Resource selected must be a message-based session");
                return false;
            }
            catch (Exception exp)
            {
                WriteDebugMsg(exp.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Type of the INSTR as "TCPIP0::192.168.0.188::INSTR"
        /// Type of the SOCKET as "TCPIP0::192.168.20.2::56001::SOCKET"
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="Board"></param>
        /// <param name="IPAddress"></param>
        /// <param name="PortNum"></param>
        /// <param name="Timeout"></param>
        /// <returns></returns>
        public bool OpenSession(TCPIP_Type Type, int Board, string IPAddress, int PortNum, int Timeout)
        {
            string ResourceName = "";
            string strTmp = "";
            //string[] resources = ResourceManager.GetLocalManager().FindResources("?*");
            if (Type == TCPIP_Type.INSTR)
            {
                ResourceName = "TCPIP0" + "::" + IPAddress + "::inst" + Board + "::INSTR";
            }
            else //(Type == TCPIP_Type.SOCKET)
            {
                ResourceName = "TCPIP" + Board + "::" + IPAddress + "::" + PortNum + "::SOCKET";
            }

            try
            {
                WriteDebugMsg(ResourceName);
                mbSession = (MessageBasedSession)ResourceManager.GetLocalManager().Open(ResourceName);                
                mbSession.Timeout = Timeout;

                //System.Threading.Thread.Sleep(200);
                mbSession.Write("*CLS\n");
                strTmp = mbSession.Query("*IDN?\n");
            }
            catch (InvalidCastException)
            {
                WriteDebugMsg("Resource selected must be a message-based session");
                return false;
            }
            catch (Exception exp)
            {
                WriteDebugMsg(exp.Message);
                return false;
            }


            return true;
        }

        public void WriteGPIBCmd(string strCmd)
        {
            try
            {
                WriteGPIBLog(strCmd);
                mbSession.Write(strCmd + "\n");
            }
            catch (Exception exp)
            {
                WriteDebugMsg(exp.Message);
            }
        }

        public string ReadGPIBCmd()
        {
            string ResponseContext = "";
            try
            {
                ResponseContext = mbSession.ReadString();
                WriteGPIBLog(ResponseContext);
            }
            catch (Exception exp)
            {
                WriteDebugMsg(exp.Message);
            }

            return ResponseContext;
        }

        public byte[] ReadGPIBCmd(int countToRead)
        {
            byte[] ResponseContext = new byte[countToRead];
            try
            {
                ResponseContext = mbSession.ReadByteArray(countToRead);
                //WriteGPIBLog(ResponseContext);
            }
            catch (Exception exp)
            {
                WriteDebugMsg(exp.Message);
            }

            return ResponseContext;
        }

        public string QueryGPIBCmd(string strCmd)
        {
            string ResponseContext = "";
            try
            {
                WriteGPIBLog(strCmd);
                ResponseContext = mbSession.Query(strCmd + "\n");
                WriteGPIBLog(ResponseContext);

            }
            catch (Exception exp)
            {
                WriteDebugMsg(exp.Message);
            }

            return ResponseContext;
        }
        


    }
}
