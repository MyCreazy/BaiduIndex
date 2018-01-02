using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaiduIndex.Util
{
    public class WriteLog
    {
        private static StreamWriter streamWriter; //写文件    
        private static object lockobj = new object();
        public static void WriteError(string message)
        {
            lock (lockobj)
            {
                try
                {

                    //DateTime dt = new DateTime();  
                    string directPath = "D:\\";
                    if (!Directory.Exists(directPath))   //判断文件夹是否存在，如果不存在则创建  
                    {
                        Directory.CreateDirectory(directPath);
                    }
                    directPath += string.Format(@"\{0}.log", "error");
                    if (streamWriter == null)
                    {
                        streamWriter = !File.Exists(directPath) ? File.CreateText(directPath) : File.AppendText(directPath);    //判断文件是否存在如果不存在则创建，如果存在则添加。  
                    }

                    if (message != null)
                    {
                        streamWriter.WriteLine(message);
                    }
                }
                finally
                {
                    if (streamWriter != null)
                    {
                        streamWriter.Flush();
                        streamWriter.Dispose();
                        streamWriter = null;
                    }
                }
            }
        }
    }
}
