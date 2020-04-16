using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCWebApplication2.Models
{
    public class RouteBounds
    {
        public StopTime[] StopTimes;
        public int RouteId;
        public string RouteName;

        private string sayRouteName;

        public RouteBounds() { }
        public RouteBounds(string sayRouteName)
        {
            switch (sayRouteName)
            {
                case "301":
                    SayRouteName = "three owe one";
                    break;
            }
        }

        public string SayRouteName
        {
            get
            {
                string spokenRouteId = RouteId.ToString();
                if (RouteId > 100)// && RouteId < 1000)
                {
                    spokenRouteId = string.Empty;
                    char[] chars = RouteId.ToString().ToArray();
                    for (int i = 0; i < chars.Length; i++)
                    {
                        spokenRouteId += chars[i] + " ";
                        spokenRouteId = spokenRouteId.Replace('0', 'o');
                    }
                }
                sayRouteName = spokenRouteId;
                return sayRouteName;
            }
 
            set
            {
                sayRouteName = value;
            }
        }
    }
}