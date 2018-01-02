
using BaiduIndex.Util;
using Better517Na.Http.Helper;
using Fiddler;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaiduIndex
{
    /// <summary>
    /// 主窗口类
    /// </summary>
    public partial class BaiduMain : Form
    {
        /// <summary>
        /// 构造的内容
        /// </summary>
        private static string content = string.Empty;

        /// <summary>
        /// 所有的关键词
        /// </summary>
        private static List<string> keywordsList = new List<string>();

        /// <summary>
        /// 处理标志
        /// </summary>
        private static int flag = 0;

        /// <summary>
        /// 关键词
        /// </summary>
        private static string keyword = string.Empty;

        /// <summary>
        /// 代理节点信息
        /// </summary>
        private static Proxy oSecureEndpoint;

        /// <summary>
        /// 主机地址
        /// </summary>
        private static string sSecureEndpointHostname = "localhost";

        /// <summary>
        /// 端口
        /// </summary>
        private static int iSecureEndpointPort = 7777;

        /// <summary>
        /// 所有的seesion
        /// </summary>
        private static List<Fiddler.Session> oAllSessions = new List<Fiddler.Session>();

        /// <summary>
        /// 构造函数初始化
        /// </summary>
        public BaiduMain()
        {
            MessagePipe.WriteMessageEvent += this.ShowMessage;
            Control.CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
        }

        /// <summary>
        /// 显示信息
        /// </summary>
        /// <param name="msg">信息</param>
        /// <param name="color">字体颜色</param>
        public void ShowMessage(string msg, Color color)
        {
            if (RtbMsg.InvokeRequired == false)
            {
                if (RtbMsg.Text.Length > 20000)
                {
                    RtbMsg.Clear();
                }

                try
                {
                    int tempTextLength = RtbMsg.Text.Length;
                    string strInfo = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  " + msg;
                    RtbMsg.AppendText(strInfo + Environment.NewLine);
                    RtbMsg.Select(tempTextLength, strInfo.Length);
                    RtbMsg.SelectionColor = color;
                    RtbMsg.Select(RtbMsg.Text.Length, 0);
                    RtbMsg.ScrollToCaret();
                }
                catch (Exception ex)
                {
                    string forStyleCope = "吃掉他" + ex.Message;
                }
            }
            else
            {
                WriteMessageOnScreen delegateShowMsg = new WriteMessageOnScreen(this.ShowMessage);
                this.BeginInvoke(delegateShowMsg, msg, color);
            }
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BaiduMain_Load(object sender, EventArgs e)
        {
            //http://index.baidu.com/?tpl=crowd&word=%B6%E0%CD%E6%D3%CE%CF%B7
            this.webBrowser1.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser1_DocumentCompleted);
            this.webBrowser1.Navigate("http://index.baidu.com/?tpl=crowd&word=%B6%E0%CD%E6%D3%CE%CF%B7");
        }

        /// <summary>
        /// 解压
        /// </summary>
        /// <param name="zippedData"></param>
        /// <returns></returns>
        private byte[] Decompress(byte[] zippedData)
        {
            MemoryStream ms = new MemoryStream(zippedData);
            GZipStream compressedzipStream = new GZipStream(ms, CompressionMode.Decompress);
            MemoryStream outBuffer = new MemoryStream();
            byte[] block = new byte[1024];
            while (true)
            {
                int bytesRead = compressedzipStream.Read(block, 0, block.Length);
                if (bytesRead <= 0)
                    break;
                else
                    outBuffer.Write(block, 0, bytesRead);
            }
            compressedzipStream.Close();
            return outBuffer.ToArray();
        }

        /// <summary>
        /// 数据返回前
        /// </summary>
        /// <param name="oSession"></param>
        private void FiddlerApplication_BeforeResponse(Session oSession)
        {
            if (oSession.fullUrl.Contains("index.baidu.com/Interface/Social/getSocial"))
            {
                try
                {
                    byte[] te = oSession.ResponseBody;
                    byte[] dd = Decompress(te);
                    string jsonstr = System.Text.Encoding.GetEncoding("utf-8").GetString(dd);
                    jsonstr = jsonstr.Replace("\"", string.Empty);
                    Regex regextemp = new Regex("str_age:\\{(?<age>.*?)\\},str_sex:\\{(?<sex>.*?)\\}");
                    Match matchresult = regextemp.Match(jsonstr);
                    string ageregion = matchresult.Groups["age"].Value;
                    string sexstr = matchresult.Groups["sex"].Value;
                    List<string> agelist = ageregion.Split(',').ToList();
                    List<string> sexlist = sexstr.Split(',').ToList();
                    ////解析数据
                    content = "";
                    if (string.IsNullOrEmpty(keyword))
                    {
                        return;
                    }

                    content += keyword + "  ";
                    foreach (string tempage in agelist)
                    {
                        List<string> tempageList = tempage.Split(':').ToList();
                        content += tempageList[1] + "  ";
                    }

                    foreach (string tempsex in sexlist)
                    {
                        List<string> tempsexlist = tempsex.Split(':').ToList();
                        content += tempsexlist[1] + "  ";
                    }

                    ////追加到txt
                    WriteTxt.WriteAppendTxt("F:\\phicommwork\\斐讯大数据文档\\游戏画像\\百度指数\\baidu.txt", content);
                    MessagePipe.ExcuteWriteMessageEvent("添加数据" + content, 0);
                    content = string.Empty;
                    keyword = string.Empty;
                    Monitor.Enter(oAllSessions);
                    oAllSessions.Clear();
                    Monitor.Exit(oAllSessions);
                }
                catch (Exception ex)
                {
                    MessagePipe.ExcuteWriteMessageEvent("捕获到需要的请求，但发生异常:"+ex.Message, 1);
                }
                try
                {
                    Fiddler.FiddlerApplication.Shutdown();
                }
                catch (Exception ex)
                {
                    MessagePipe.ExcuteWriteMessageEvent("释放fillder发生异常:" + ex.Message, 1);
                }
            }
            else
            {
                content = "无";
            }
        }

        /// <summary>
        /// 判断游览器是否加载完毕
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (e.Url.ToString().Contains("index.baidu.com/?tpl=crowd"))
            {
                ////说明加载完毕，可以进行下一条数据处理
                flag++;
            }
        }

        /// <summary>
        /// 获取一个关键词
        /// </summary>
        /// <returns>返回关键词</returns>
        private string GetOneKeyWord()
        {
            string result = string.Empty;
            if (keywordsList != null && keywordsList.Count > 0)
            {
                result = keywordsList[0];
                keywordsList.RemoveAt(0);
            }
            else
            {
                MessagePipe.ExcuteWriteMessageEvent("处理完成", 0);
            }

            return result;
        }

        /// <summary>
        /// 子线程
        /// </summary>
        private void SubThread()
        {
            while (true)
            {
                try
                {
                    MessagePipe.ExcuteWriteMessageEvent("处理标志为:" + flag, 0);
                    if (flag >= 1)
                    {
                        if (content.Contains("无"))
                        {
                            ////说明没有释放
                            try
                            {
                                Fiddler.FiddlerApplication.Shutdown();
                            }
                            catch (Exception ex)
                            {
                                MessagePipe.ExcuteWriteMessageEvent("线程释放fillder发生异常:" + ex.Message, 1);
                            }
                        }

                        flag = 0;
                        this.Excute();
                    }
                }
                catch (Exception ex)
                {
                    MessagePipe.ExcuteWriteMessageEvent("子线程异常:" + ex.Message.ToString(), 0);
                }
                finally
                {
                    MessagePipe.ExcuteWriteMessageEvent("线程休眠15秒", 0);
                    Thread.Sleep(15000);
                }
            }
        }

        /// <summary>
        /// 执行
        /// </summary>
        private void Excute()
        {
            ////调用一个方法取一条
            keyword = this.GetOneKeyWord();
            if (!string.IsNullOrEmpty(keyword))
            {
                MessagePipe.ExcuteWriteMessageEvent("开始处理关键词【" + keyword + "】", 0);

                string tempword = System.Web.HttpUtility.UrlEncode(keyword, System.Text.Encoding.GetEncoding("GB2312"));
                tempword = tempword.ToUpper();
                string url = string.Format("http://index.baidu.com/?tpl=crowd&word={0}", tempword);
                this.webBrowser1.Navigate(url);
                //this.webBrowser1.Document.GetElementById("schword").SetAttribute("value", keyword);
                //this.webBrowser1.Document.GetElementById("schsubmit").InvokeMember("click");

                Fiddler.FiddlerApplication.SetAppDisplayName("FiddlerCoreDemoApp");
                FiddlerApplication.Startup(7777, FiddlerCoreStartupFlags.Default);
                //FiddlerApplication.BeforeRequest += BeforeRequest;
                Fiddler.FiddlerApplication.BeforeRequest += delegate(Fiddler.Session oS)
                {
                    oS.bBufferResponse = true;
                    Monitor.Enter(oAllSessions);
                    oAllSessions.Add(oS);
                    Monitor.Exit(oAllSessions);
                    oS["X-AutoAuth"] = "(default)";
                    if ((oS.oRequest.pipeClient.LocalPort == iSecureEndpointPort) && (oS.hostname == sSecureEndpointHostname))
                    {
                        oS.utilCreateResponseAndBypassServer();
                        oS.oResponse.headers.SetStatus(200, "Ok");
                        oS.oResponse["Content-Type"] = "text/html; charset=UTF-8";
                        oS.oResponse["Cache-Control"] = "private, max-age=0";
                        oS.utilSetResponseBody("<html><body>Request for httpS://" + sSecureEndpointHostname + ":" + iSecureEndpointPort.ToString() + " received. Your request was:<br /><plaintext>" + oS.oRequest.headers.ToString());
                    }
                };
                FiddlerApplication.BeforeResponse += FiddlerApplication_BeforeResponse;
                Fiddler.FiddlerApplication.AfterSessionComplete += delegate(Fiddler.Session oS)
                {
                    //Console.WriteLine("Finished session:\t" + oS.fullUrl); 
                    //MessagePipe.ExcuteWriteMessageEvent ("Session list contains: " + oAllSessions.Count.ToString() + " sessions",0);
                };
            }
        }

        /// <summary>
        /// 游览按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tourbtn_Click(object sender, EventArgs e)
        {
            keywordsList.Clear();
            ////从文本读取关键词
            using (System.IO.StreamReader sr = new System.IO.StreamReader("F:\\phicommwork\\斐讯大数据文档\\游戏画像\\百度指数\\gamekeyword.txt", Encoding.GetEncoding("GB2312")))
            {
                string str;
                while ((str = sr.ReadLine()) != null)
                {
                    keywordsList.Add(str);
                }
            }
            keywordsList = keywordsList.Distinct().ToList();
            MessagePipe.ExcuteWriteMessageEvent("取到关键词" + keywordsList.Count + "条", 0);
            Thread thread = new Thread(new ThreadStart(SubThread));
            thread.IsBackground = true;
            thread.Start();
            flag = 0;
            this.Excute();
        }
    }
}
