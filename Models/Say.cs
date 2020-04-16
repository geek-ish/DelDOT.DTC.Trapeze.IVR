using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCWebApplication2.Models
{
    public class Say
    {
        public static string SilentString = "-1";   
        public static long SilentInteger = -1;
        //public static DateTime SilentDateTime = DateTime.Parse("-1");

        public static string SayNumber(string numberString)
        {
            string saidNumber = numberString;
            if (!string.IsNullOrEmpty(numberString))
            {
                //for (int i = 0; i < numberString.Length; i++)
                //{
                //    saidNumber += numberString[i] + " ";
                //    saidNumber = saidNumber.Replace("0", "o");
                //}
                if (int.Parse(numberString) > 100)
                {
                    saidNumber = string.Empty;
                    char[] chars = numberString.ToArray();
                    for (int i = 0; i < chars.Length; i++)
                    {
                        saidNumber += chars[i] + " ";
                        saidNumber = saidNumber.Replace('0', 'o');
                    }
                }
            }
            return saidNumber;
        }

        public static string SayDateTime(DateTime dt)
        {
            return string.Format("{0}", dt.ToString("D"));
        }

        public static string SaySpecialCharacters(string s1)
        {
            string s = s1;
            if (s.Trim().Length > 0)
            {
                s = s.Replace("FOULK & NAAMANS", "falk and nay mens road");
                s = s.Replace(" LN", " lane");
                //s = s.Replace(" DR", " drive");
                s = s.Substring(s.Length - 3, 3).ToUpper() == " DR" ?
                   s.Replace(s.Substring(s.Length - 3, 3), " drive") : s;
                s = s.Replace("ROSA PK", "Rosa Parks");
                s = s.Replace("&", "and");
                s = s.Replace("@", "at");
                s = s.Replace(" ST/", " street and ");
                s = s.Replace(@"/", "and");
                s = s.Replace(" ST ", " street ");
                s = s.EndsWith(" ST") ? s.Substring(0, s.Length - 3) + " street" : s;
                //s = s.Substring(s.Length - 3, 3).ToUpper() == " ST" ?
                //    s.Replace(s.Substring(s.Length - 3, 3), " street") : s;
                s = s.Replace(" N ", " north ");
                s = s.Replace(" AVE", " avenue");
            }

            return s;
        }
    }

    public class SayDateTime
    {
        public static string Say(DateTime dt)
        {
            return string.Format("{0}", dt.ToString("D"));
        }
    }
}