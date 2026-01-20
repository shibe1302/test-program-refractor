using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace CPEI_MFG
{
    class IniFile
    {
        private string FFileName;
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileInt(
        string lpAppName,
        string lpKeyName,
        int nDefault,
        string lpFileName
        );
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(
        string lpAppName,
        string lpKeyName,
        string lpDefault,
        StringBuilder lpReturnedString,
        int nSize,
        string lpFileName
        );
        [DllImport("kernel32")]
        private static extern bool WritePrivateProfileString(
        string lpAppName,
        string lpKeyName,
        string lpString,
        string lpFileName
        );

        public IniFile(string filename)
        {
            FFileName = filename;
        }
        public int ReadInt(string section, string key, int def)
        {
            return GetPrivateProfileInt(section, key, def, FFileName);
        }
        public string ReadString(string section, string key, string def)
        {
            StringBuilder temp = new StringBuilder(1024);
            GetPrivateProfileString(section, key, def, temp, 1024, FFileName);
            return temp.ToString();
        }
        public void WriteInt(string section, string key, int iVal)
        {
            WritePrivateProfileString(section, key, iVal.ToString(), FFileName);
        }
        public void WriteString(string section, string key, string strVal)
        {
            WritePrivateProfileString(section, key, strVal, FFileName);
        }
        public void DelKey(string section, string key)
        {
            WritePrivateProfileString(section, key, null, FFileName);
        }
        public void DelSection(string section)
        {
            WritePrivateProfileString(section, null, null, FFileName);
        }

    }
}
