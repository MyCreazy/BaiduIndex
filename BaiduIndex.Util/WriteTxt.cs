using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaiduIndex.Util
{
    public class WriteTxt
    {
        public static void WriteAppendTxt(string path, string content)
        {
            FileStream fs = null;
            StreamWriter sw = null;
            try
            {
                //if (!File.Exists(path))
                //{
                //   ////创建
                //    File.Create(path);
                //}

                fs = new FileStream(path, FileMode.Append);
                sw = new StreamWriter(fs);
                sw.WriteLine(content);
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                }

                if (fs != null)
                {
                    fs.Close();
                }
            }
        }
    }
}
