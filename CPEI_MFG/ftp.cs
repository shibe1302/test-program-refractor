using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Windows.Forms;

namespace CPEI_MFG
{
    class ftp
    {
        private static string ftphost = "ftp://200.166.2.202";
        private static string ftpusername = "user";
        private static string ftppassword = "ubnt";


        public static bool UploadFile(string ftppath,string name)
        {
            string info = "";
            FileInfo f = new FileInfo(name);
            string ftplogfilename = "";

            ftplogfilename = ftphost + "/UBNT_Test_Logs/" + ftppath + "/" + f.Name;
            FtpWebRequest reqFtp = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftplogfilename));
            reqFtp.UseBinary = true;
            reqFtp.Credentials = new NetworkCredential(ftpusername, ftppassword);
            reqFtp.KeepAlive = false;
            reqFtp.Method = WebRequestMethods.Ftp.UploadFile;

            reqFtp.ContentLength = f.Length;
            int BuffLengh = 2048;
            byte[] buff=new byte[BuffLengh];
            int ContentLengh;
            FileStream fs = f.OpenRead();
            //long BuffLengh = fs.Length();

            try
            {
                Stream strm = reqFtp.GetRequestStream();
                ContentLengh = fs.Read(buff,0,BuffLengh);
                while(ContentLengh!=0)
                {
                    strm.Write(buff,0,BuffLengh);
                    ContentLengh = fs.Read(buff, 0, BuffLengh);
                    if (ContentLengh<2048)
                    {
                        strm.Write(buff, 0, ContentLengh);
                        ContentLengh = fs.Read(buff, 0, BuffLengh);
                    }

                }
                strm.Close();
                fs.Close();
                //info = "finish!!!";
                return true;
            }
            catch(Exception ex)
            {
                //info = string.Format("{0},fail",ex.Message);
                //MessageBox.Show(info);
                return false;
            }

        }
        public static Boolean CreateDir(string path1,string path2,string dirctoryname)
        {
            string info = "";
            string temppath = "";

            temppath = ftphost + "/UBNT_Test_Logs/" + path1 + "/" + path2 + "/" + dirctoryname;
                    
            FtpWebRequest reqFtp = (FtpWebRequest)FtpWebRequest.Create(new Uri(temppath));
            reqFtp.UseBinary = true;
            reqFtp.Credentials = new NetworkCredential(ftpusername, ftppassword);
            reqFtp.KeepAlive = false;
            reqFtp.Method = WebRequestMethods.Ftp.MakeDirectory;
            try
            {
                
                FtpWebResponse response = (FtpWebResponse)reqFtp.GetResponse();
                response.Close();
                
            }

            catch(Exception ex)
            {
                throw new Exception("create dir fail" + ex.Message);
                info = string.Format("{0},fail", ex.Message);
                MessageBox.Show(info);
            }
            reqFtp.Abort();
            return true;
        }
        public static string[] GetFilesDetailList(string path1, string path2)// get directory list 
        {
            string[] downloadFiles;
            string buffer;
            string ftpURI = "";

            ftpURI = ftphost + "/UBNT_Test_Logs/" + path1 + "/" + path2 + "/";
            try
            {
                StringBuilder result = new StringBuilder();
                FtpWebRequest reqFtp;
                reqFtp = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpURI));
                reqFtp.Credentials = new NetworkCredential(ftpusername, ftppassword);
                reqFtp.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                WebResponse response;
                response = reqFtp.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default);
                //buffer = reader.ReadToEnd();
               
 
  
                string line = reader.ReadLine();

                if (line == null)
                {
                    result = null;
                    reader.Close();
                    response.Close();
                    return result.ToString().Split('\n');
                }
                while (line != null)
                {
                    result.Append(line);
                    result.Append("\n");
                    line = reader.ReadLine();
                }
                result.Remove(result.ToString().LastIndexOf("\n"), 1);
                reader.Close();
                response.Close();
                return result.ToString().Split('\n');
            }
            catch (Exception ex)
            {
                downloadFiles = null;
               // MessageBox.Show(ex.Message);
                
                return downloadFiles;
            }
        }
        /// <summary>
        /// directory
        /// </summary>
        /// <returns></returns>
        public static string[] GetDirectoryList(string path1,string path2)
        {
            string m = string.Empty;
            string[] drectory = GetFilesDetailList(path1,path2);
            if (drectory == null)
            {
                //drectory = GetFilesDetailList(type, path1, path2);
                return m.Split('\n');
            }
            
            foreach (string str in drectory)
            {
                int dirPos = str.IndexOf("<DIR>");
                if (dirPos > 0)
                {
                    /*unknow*/
                    m += str.Substring(dirPos + 5).Trim() + "\n";
                }
                else if (str.Trim().Substring(0, 1).ToUpper() == "D")
                {
                    /*window*/
                    string dir = str.Substring(49).Trim();
                    if (dir != "." && dir != "..")
                    {
                        m += dir + "\n";
                    }
                }
            }
            char[] n = new char[] { '\n' };
            return m.Split(n);
        }

        public static bool DirectoryExist(string path1,string path2,string RemoteDirectoryName)
        {
            string[] dirList = GetDirectoryList(path1,path2);
            if(dirList==null)
            {
                return false;
            }
            foreach (string str in dirList)
            {
                if(str.Trim()=="")
                {
                    return false;
                }
                if (str.Trim().Contains(RemoteDirectoryName.Trim()))
                {
                    return true;
                }
            }
            return false;
        }




    }
}
