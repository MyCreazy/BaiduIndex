using BaiduIndex.Util;
using Better517Na.Http.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BaiduIndex.Bus
{
    /// <summary>
    /// 百度抓取
    /// </summary>
    public class BaiduCraw
    {
        /// <summary>
        /// cookie
        /// </summary>
        private string cookie = string.Empty;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="cookie">cookie</param>
        public BaiduCraw(string cookie)
        {
            this.cookie = cookie;
        }

       /// <summary>
       /// 抓取指数
       /// </summary>
        public void ExceuteCrawIndex()
        {
            try
            {
                List<string> keywordsList = new List<string>();
                ////从文本读取关键词
                using (System.IO.StreamReader sr = new System.IO.StreamReader("F:\\phicommwork\\斐讯大数据文档\\游戏画像\\百度指数\\gamekeyword.txt", Encoding.GetEncoding("GB2312")))
                {
                    string str;
                    while ((str = sr.ReadLine()) != null)
                    {
                        keywordsList.Add(str);
                    }
                }

                MessagePipe.ExcuteWriteMessageEvent("取到关键词" + keywordsList.Count+"条", 0);
                ////开始遍历关键词
                foreach (string keyword in keywordsList)
                {
                    try
                    {
                        MessagePipe.ExcuteWriteMessageEvent("开始处理关键词【" + keyword + "】", 0);
                        HttpRequestParam param = new HttpRequestParam();
                        string tempword = System.Web.HttpUtility.UrlEncode(keyword, System.Text.Encoding.GetEncoding("GB2312"));
                        tempword = tempword.ToUpper();
                        param.URL = string.Format("http://index.baidu.com/?tpl=crowd&word={0}", tempword);
                        param.Method = "get";
                        param.AllowAutoRedirect = false;
                        param.IsIEProxy = true;
                        param.Cookie = this.cookie;
                        param.Timeout = 7 * 1000;
                        param.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/62.0.3202.94 Safari/537.36";
                        param.ResultType = ResultType.String;
                        HttpResult result = HttpHelper.GetHttpRequestData(param);
                        String temphtml = result.Html;
                        temphtml = Regex.Replace(temphtml, @"\r|\n|\s|\t", string.Empty, RegexOptions.IgnoreCase);
                        Regex regextemp = new Regex("PPval.ppt=\'(?<str>.*?)\',");
                        Match matchresult = regextemp.Match(temphtml);
                        string tempstr = matchresult.Groups["str"].Value;
                        tempstr += "&res2=";
                        ////拼凑字符串，进行解析
                        string temprefer = param.URL;
                        param.Referer = temprefer;
                        param.URL = string.Format("http://index.baidu.com/Interface/Social/getSocial/?res={0}", tempstr);
                        param.Header.Add("X-Requested-With", "XMLHttpRequest");
                        result = HttpHelper.GetHttpRequestData(param);
                        string jsonstr = result.Html;
                        jsonstr = jsonstr.Replace("\"", string.Empty);
                        regextemp = new Regex("str_age:\\{(?<age>.*?)\\},str_sex:\\{(?<sex>.*?)\\}");
                        matchresult = regextemp.Match(jsonstr);
                        string ageregion = matchresult.Groups["age"].Value;
                        string sexstr = matchresult.Groups["sex"].Value;
                        List<string> agelist = ageregion.Split(',').ToList();
                        List<string> sexlist = sexstr.Split(',').ToList();
                        ////解析数据
                        string content = string.Empty;
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
                        MessagePipe.ExcuteWriteMessageEvent("关键词【" + keyword + "】指数数据添加" + content, 0);
                    }
                    catch (Exception ex)
                    {
                        MessagePipe.ExcuteWriteMessageEvent("处理关键词【" + keyword + "】发生异常:"+ex.Message, 1);
                    }
                }
            }
            catch (Exception ex)
            { }
        }
    }
}
