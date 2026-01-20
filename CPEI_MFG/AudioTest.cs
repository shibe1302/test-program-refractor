using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;

namespace CPEI_MFG
{
    public partial class AudioTest
    {
        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        private extern static IntPtr FindWindow(string lpClassName, string lpWindowName);

        //AMMA_API int AMMA_Open(void **pInstance);
        [DllImport("AMMA.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto, EntryPoint = "AMMA_Open")]
        private extern static int AMMA_Open(ref IntPtr pInstance);

        //AMMA_API void AMMA_Close(void *pInstance);
        [DllImport("AMMA.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto, EntryPoint = "AMMA_Close")]
        private extern static void AMMA_Close(IntPtr pInstance);

        //AMMA_API void AMMA_Mes(void *pInstance, int argc, char* argv[], char * fBuf,long length);
        [DllImport("AMMA.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto, EntryPoint = "AMMA_Mes")]
        private extern static void AMMA_Mes(IntPtr pInstance,int argc,string[] argv,ref string fBuf,long length);

        private IntPtr m_pInstance;

        public bool GetAudioFreAmp(ref double dFre_Left, ref double dFre_Right, ref double dAmp_Left, ref double dAmp_Right)
        {
            dFre_Left = 0.0;
            dFre_Right = 0.0;
            dAmp_Left = 0.0;
            dAmp_Right = 0.0;
            int argc = 3;
            string[] argv = new string[3];
            argv[0] = "TestAMMA.exe";
            argv[1] = "-t";
            argv[2] = "freq";
            string sResult = "";
            m_pInstance = new IntPtr(0);
            if (AMMA_Open(ref m_pInstance) == 0)
            {
                return false;
            }

            AMMA_Mes(m_pInstance, argc, argv, ref sResult, 1024);

            AMMA_Close(m_pInstance);
            return true;
        }
    }
}
