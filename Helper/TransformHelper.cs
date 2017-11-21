using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AAAAAAAAAAA
{
    /// <summary>  
    /// 编码转换工具  
    /// 作者：caolh  
    /// 日期：2016-12-13
    /// </summary>  
    public class TransformHelper
    {

        //定义转换的枚举类型  
        public enum TransType
        {
            ASCIIToUnicode,
            UnicodeToASCII,
            UnicodeToCN,
            CNToUnicode,
            Clear
        }

        /// <summary>  
        /// 根据指定的转换类型转换字符串  
        /// </summary>  
        /// <param name="type">转换类型</param>  
        public static string TransText(TransType type, string source)
        {
            string dest = string.Empty;
            switch (type)
            {
                case TransType.ASCIIToUnicode:
                    dest = string.Empty;
                    for (int i = 0; i < source.Length; i++)
                    {
                        dest += "&#" + ((int)source[i]).ToString() + ";";
                    }
                    break;
                case TransType.UnicodeToASCII:
                    dest = string.Empty;
                    MatchCollection mc = Regex.Matches(source, "\\w+");
                    foreach (Match m in mc)
                    {
                        dest += ((char)int.Parse(m.Value)).ToString();
                    }
                    break;
                case TransType.UnicodeToCN:
                    dest = string.Empty;
                    string[] arr = source.Replace("\\", "").Split('u');
                    for (int i = 1; i < arr.Length; i++)
                    {
                        dest += (char)int.Parse(arr[i], NumberStyles.HexNumber);
                    }
                    break;
                case TransType.CNToUnicode:
                    dest = string.Empty;
                    for (int i = 0; i < source.Length; i++)
                    {
                        dest += "\\u" + ((int)source[i]).ToString("x");
                    }
                    break;
                case TransType.Clear:

                    break;
            }
            return dest;
        }
    }  
}
