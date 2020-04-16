using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCWebApplication2.Models
{
    public class Caller
    {
        public string From;
        public string City;
        public string State;
        public string Country;
        public string Zip;

        public Caller() { }
        public Caller(string from, string city, string state, string country, string zip)
        {
            From = from;
            City = city;
            State = state;
            Country = country;
            Zip = zip;
        }
    }
}