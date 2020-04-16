using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVCWebApplication2.Models;

namespace MVCWebApplication2.Test
{
    public static class TestAssets
    {

        static Stop[] GetDummyStops()
        {
            return new List<Stop>()
            {
                new Stop() { StopId=15, PublishedName="CHRISTIANA MALL @ TARGET" },
                new Stop() { StopId=15, PublishedName="CHRISTIANA MALL PARK & RIDE" },
                new Stop() { StopId=15,  PublishedName="CHURCHMANS RD @ CAVALIER DRIVE" },
                new Stop() { StopId=15, PublishedName="CORPORATE COMMONS ONE READS WAY" },
                new Stop() { StopId=15, PublishedName="KING ST @ 9TH ST" },
                new Stop() { StopId=15, PublishedName="MOORES LANE @ MONTPELIER BOULEVARD" },
                new Stop() { StopId=15, PublishedName="NEW CASTLE 6TH STREET AT DELAWARE ST" },
                new Stop() {StopId=15, PublishedName="NEW CASTLE AVE @ ROGERS RD" }
            }.ToArray();
        }
    }
}