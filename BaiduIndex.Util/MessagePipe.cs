using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaiduIndex.Util
{
    /// <summary>
    /// 自定义向屏幕写信息代理
    /// </summary>
    /// <param name="msg">信息</param>
    /// <param name="color">颜色</param>
    public delegate void WriteMessageOnScreen(string msg, Color color);

    /// <summary>
    /// 消息公共类
    /// </summary>
    public class MessagePipe
    {
        /// <summary>
        /// 写信息事件
        /// </summary>
        public static event WriteMessageOnScreen WriteMessageEvent;

        /// <summary>
        /// 执行写信息事件
        /// </summary>
        /// <param name="message">信息</param>
        /// <param name="color">颜色</param>
        public static void ExcuteWriteMessageEvent(string message, int colorflag)
        {
            if (WriteMessageEvent != null)
            {
                Color color = Color.Green;
                if (colorflag == 1)
                {
                    color = Color.Red;
                }

                WriteMessageEvent(message, color);
            }
        }
    }
}
