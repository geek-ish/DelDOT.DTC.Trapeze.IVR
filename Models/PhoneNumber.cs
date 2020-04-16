using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;

namespace MVCWebApplication2.Models
{
    //public enum Locale { USA }
    public class PhoneNumber
    {
        public static string Redial_Undefined = "-1";
        public static long RedialCallack_Undefined = -1;

        public static Int64 Parse(string phoneNumber, string cultureInfoName = "en-US")
        {
            CultureInfo ci = string.IsNullOrEmpty(cultureInfoName) ? new CultureInfo("en-US") : new CultureInfo(cultureInfoName);
            long result;
            int length = ci.Name.ToUpper().Equals("EN-US") ? 10 : 10;//TODO=Dunno YET how many characters to parse for other country locales.
            string s = Redial_Undefined;
            if (phoneNumber.Length >= 7)
            {
                s = new string(phoneNumber.Where(c => char.IsNumber(c)).ToArray());
                s = s.Substring((s.Length - length));
            }
                               
            return Int64.TryParse(s, out result) ? result : Int64.Parse(phoneNumber); 
        }
    }
}