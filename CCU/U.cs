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
    }
}
