using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCU
{
    /// <summary>
    /// 工具类
    /// </summary>
    public static class U
    {
        /// <summary>
        /// 判断当前字符串是不是数字
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static bool IsNumber(string s)
        {
            int flag = 0;
            char[] str = s.ToCharArray();
            int lenght = str.Length;
            for (int i = 0; i < lenght; i++)
            {
                if (char.IsNumber(str[i])) flag++;
                else
                {
                    flag = -1;
                    break;
                }
            }
            return flag > 0;
        }

        /// <summary>
        /// 解决换行符失效问题
        /// </summary>
        /// <param name="sourceStr"></param>
        /// <returns></returns>
        public static string Unescape(string sourceStr)
        {
            return System.Text.RegularExpressions.Regex.Unescape(sourceStr);
        }
        /// <summary>
        /// 解决换行符失效和空格自动换行问题
        /// </summary>
        /// <param name="sourceStr"></param>
        /// <param name="space"></param>
        /// <returns></returns>
        public static string Unescape(string sourceStr, bool space)
        {
            return ReplaceSpace(System.Text.RegularExpressions.Regex.Unescape(sourceStr));
        }
        /// <summary>
        /// 解决空格自动换行问题
        /// </summary>
        /// <param name="sourceStr"></param>
        /// <returns></returns>
        public static string ReplaceSpace(string sourceStr)
        {
            return sourceStr.Replace(" ", "<color=#00000000>.</color>");
        }
    }


}
