using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MVCWebApplication2.Models;
//using System.Data.Objects;
using System.Data.SqlClient;

namespace MVCWebApplication2.Controllers
{
    public class Employee { public string Name; public int Age; public DateTime HireDate; }
    public class Result { public Employee[] Employees; }
    public class StopTimesController : Controller
    {
        private StopTimes1Context db = new StopTimes1Context();
        public string SqlConnectionString = "Data Source=sql7002.site4now.net;Initial Catalog=DB_A3E75F_gk55;Persist Security Info=True;User ID=DB_A3E75F_gk55_admin;Password=helloWorld55!";

        //private string FindStop(int routeNumber, string s)
        //{
        //    string stop = string.Empty;
        //    /*
        //    Ok, Route#%routeNumber% to %direction%, which stop? Reply with the stop ID or stop name or "List stops" to get a list of stops to %direction%.
        //    */

        //    return stop;
        //}

        public JsonResult SearchForStopMessage(string gather, int routeNumber, int directionId, string destination)
        {
            string msg = "Sorry, I can't find any stop matches for that route and direction. " +
                "Please reply 'List stops' to get a list of stops for that direction #." +
                directionId.ToString();
            string[] stops = TextSearchForStops(gather, routeNumber, directionId).ToArray();
            if (stops != null)
            {
                // Ok, Route#%routeNumber% to %direction%, which stop? Reply with the stop ID or stop name or 
                //"List stops" to get a list of stops to %direction%.
                msg = string.Format("Ok, Route#{0} to {1}, which stop? " +
                    "Reply with the stop ID or stop name or 'List stops' to get a list of stops to direction({1}).", 
                    routeNumber.ToString(), destination);
            }

            return new JsonResult() { Data = msg, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        private List<string> TextSearchForStops(string textTokens, /*List<string> searchContent,*/ int routeNumber, int directionId)
        {
            /*
             * Dictionary<TKey,TValue>.Add(TKey, TValue) Method (System.Collections.Generic) | Microsoft Docs: https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2.add?view=netframework-4.8
             c# - What exception type should be thrown when trying to add duplicate items to a collection? - Stack Overflow : https://stackoverflow.com/questions/1280035/what-exception-type-should-be-thrown-when-trying-to-add-duplicate-items-to-a-col
             c# - When to use IList and when to use List - Stack Overflow: https://stackoverflow.com/questions/17170/when-to-use-ilist-and-when-to-use-list
             c# - OrderBy and List vs. IOrderedEnumerable - Stack Overflow: https://stackoverflow.com/questions/9285426/orderby-and-list-vs-iorderedenumerable/9285461
             linq select condition c# - Google Search: https://www.google.com/search?q=linq+select+condition+c%23&rlz=1C1CHBF_enUS812US812&oq=linq+select+con&aqs=chrome.5.69i57j0l7.10816j0j4&sourceid=chrome&ie=UTF-8
             c# - select object which matches with my condition using linq - Stack Overflow: https://stackoverflow.com/questions/17722670/select-object-which-matches-with-my-condition-using-linq
             c# - What is the best way to iterate over a dictionary? - Stack Overflow: https://stackoverflow.com/questions/141088/what-is-the-best-way-to-iterate-over-a-dictionary
             c# - Converting from IEnumerable to List - Stack Overflow: https://stackoverflow.com/questions/7617771/converting-from-ienumerable-to-list
             Enumerable.Distinct Method (System.Linq) | Microsoft Docs: https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.distinct?view=netframework-4.8
             c# - Get a list of distinct values in List - Stack Overflow: https://stackoverflow.com/questions/10255121/get-a-list-of-distinct-values-in-list
             Various Ways to Get Distinct Values from a List<T> using LINQ - CodeProject: https://www.codeproject.com/Articles/1105173/Various-Ways-to-Get-Distinct-Values-from-a-List-T
             Remove duplicates from a List in C# - Techie Delight: https://www.techiedelight.com/remove-duplicates-from-list-csharp/
             c# - Converting from IEnumerable to List - Stack Overflow: https://stackoverflow.com/questions/7617771/converting-from-ienumerable-to-list            
             */

            //foreach (string s1 in textTokens.Split().ToList().OrderByDescending(s => s.Length))
            //{
            //    keywords.Add(s1);
            //}, out stopHitLinesIndices);

            Stop[] stops = GetStops(routeNumber, directionId);
            if (stops != null)
            {
                List<string> keywords = textTokens.Split().ToList().OrderByDescending(s => s.Length).ToList();
                {
                    int[][] stopHitLinesIndices = FindSearchStopsHitLinesByKeywords(stops, keywords);                    
                    if (stopHitLinesIndices != null)
                        return GetSearchResultsFromIndeces(stopHitLinesIndices, /*searchContent,*/ stops);
                }
            }
            return stops.Select(p=>p.PublishedName).ToList();
        }

        public List<string> GetSearchResultsFromIndeces(int[][] stopIndeces, /*List<string> searchContent,*/ Stop[] stops)
        {
            List<string> hitStrings = new List<string>();// searchContent;
            if (stops != null & stops.ToList().Count > 0)
            {
                for (int i = 0; i < stops.Length; i++)
                {
                    Stop s = stops[i];
                    if (s != null)
                        if (!string.IsNullOrEmpty(s.PublishedName))
                            hitStrings.Add(s.PublishedName);
                }

                HashSet<string> h = new HashSet<string>();
                if (stopIndeces.ToList().Count > 0)
                {
                    stopIndeces.ToList().OrderByDescending(n => n.Length);
                    stopIndeces.Distinct();
                    for (int i = 0; i < stopIndeces.Length; i++)
                    {
                        int[] hitArray = stopIndeces[i];
                        if (hitArray != null)
                        {
                            for (int j = 0; j < hitArray.Length; j++)
                            {
                                string s = stops[j].PublishedName;
                                if (!string.IsNullOrEmpty(s))
                                {
                                    try
                                    {
                                        if (h.Add(s))
                                            hitStrings.Add(s);
                                    }
                                    catch { }
                                }
                            }
                        }
                    }
                }
            }

            return hitStrings;
        }


        private Stop[] GetStops/*ByRouteAndDirection*/(int routeNumber, int directionId)
        {
            List<Stop> stops = new List<Stop>();
            string selectQuery = string.Format("select distinct	Stoptimes.stopId, PublishedName, stoptimes.directionId " +
                " from stoptimes " +
                " join Stops on stoptimes.StopId = stops.StopId " +
                " where routeid = {0} " +
                " and directionid = {1} ", routeNumber.ToString(), directionId.ToString());
            DataTable dt = GetFilledSqlServerDataTable(SqlConnectionString, selectQuery);

            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    stops.Add(new Stop()
                    {
                        StopId = int.Parse(dt.Rows[i][0].ToString()),
                        PublishedName = dt.Rows[i][1].ToString()
                    });
                }
            }

            return stops.ToArray();
        }

        private void ShowRoutes(int routeNumber = -1)
        {
            Console.WriteLine(GetRoutesMessage(GetRoutes(routeNumber)));
        }

        static int FindRouteNumber(string s)
        {
            List<string> numbers = new List<string>();
            bool foundFirstNumber = false;
            int number = -1;
            for (int i = 0; i < s.ToArray().Length; i++)
            {
                string c = s[i].ToString();
                if (!string.IsNullOrEmpty(c.Trim())) //Skip white spaces.
                {
                    if (int.TryParse(c, out number))
                    {
                        foundFirstNumber = true;
                        numbers.Add(number.ToString());
                    }
                    else
                        //Break on 1st non-number.
                        if (foundFirstNumber)
                        i = s.Length + 1;
                }
            }

            return int.Parse(string.Join("", numbers.ToArray()));
        }

        static string GetRoutesMessage(Route[] routes)
        {
            string msg = string.Empty, directions = string.Empty;
            int routeNumber = -1;

            for (int j = 0; j < routes.Length; j++)
            {
                Route r = routes[j];
                if (r.RouteNumber > 1)
                    routeNumber = r.RouteNumber;
                //%direction%(1) & %direction%(2)
                directions += string.Format("{0}({1}) & ", routes[j].Destination, routes[j].DirectionId);
            }
            directions = directions.EndsWith(" & ") ?
                directions.Substring(0, directions.Length - " & ".Length) : directions;
            return string.Format("Ok, Route#{0} headed to directions: {1}. Which direction?",
                routeNumber.ToString(), directions);
        }

        private Route[] GetRoutes(int routeNumber = -1)
        {
            List<Route> routes = new List<Route>();
            string routeId = string.Empty, directionId = string.Empty, destination = string.Empty;
#pragma warning disable CS0219 // Variable is assigned but its value is never used
            string sqlConnectionString = "Data Source=sql7002.site4now.net;Initial Catalog=DB_A3E75F_gk55;Persist Security Info=True;User ID=DB_A3E75F_gk55_admin;Password=helloWorld55!";
#pragma warning restore CS0219 // Variable is assigned but its value is never used
            string selectQuery = string.Format("  select distinct routeid as 'Route ID', DirectionId as 'Direction ID' " +
                ", Destination from stoptimes {0} ",
                 routeNumber > 0 ? " where routeId = " + routeNumber.ToString() : string.Empty);
            DataTable dt = GetFilledSqlServerDataTable(SqlConnectionString, selectQuery);

            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    routes.Add(new Route()
                    {
                        RouteNumber = int.Parse(dt.Rows[i][0].ToString()),
                        DirectionId = int.Parse(dt.Rows[i][1].ToString()),
                        Destination = dt.Rows[i][2].ToString()
                    });
                }
            }

            return routes.ToArray();
        }

        private int[][] FindSearchStopsHitLinesByKeywords(Stop[] stops, List<string> keywords/*, out int[][] hitLineCounts*/)
        {
            if (stops != null)
            {
                if (keywords != null && keywords.Count > 0)
                {
                    int[][] hitLineCounts = new int[stops.Length][];
                    for (int i = 0; i < keywords.Count; i++)
                    {
                        string keyword = keywords[i];
                        for (int j = 0; j < stops.Length; j++)
                        {
                            if (stops[j].PublishedName.ToUpper().Contains(keyword.ToUpper()))
                            {
                                List<int> n1 = new List<int>();
                                if (hitLineCounts[i] == null)
                                {
                                    n1.Add(j);
                                    hitLineCounts[i] = n1.ToArray();
                                }
                                else
                                {
                                    n1 = hitLineCounts[i].ToList();
                                    n1.Add(j);
                                    hitLineCounts[i] = n1.ToArray();
                                }
                            }
                        }
                    }
                    return hitLineCounts;
                }
            }
            return null;
        }

        public JsonResult IsValidDirectionIdMessage(int routeNumber, int stopId, int directionId)
        {
            string message = string.Empty;
            string destination = string.Empty;
            string selectQuery = string.Format(
                  " select distinct direction, destination " +
                  " from StopTimes " +
                  " where routeId = {0} " +
                  " and StopTimes.stopId = {1} " +
                  " and DirectionId = {2} ",
                  routeNumber.ToString(), stopId.ToString(), directionId.ToString());
#pragma warning disable CS0219 // Variable is assigned but its value is never used
            string sqlConnectionString = "Data Source=sql7002.site4now.net;Initial Catalog=DB_A3E75F_gk55;Persist Security Info=True;User ID=DB_A3E75F_gk55_admin;Password=helloWorld55!";
#pragma warning restore CS0219 // Variable is assigned but its value is never used
            DataTable dt = GetFilledSqlServerDataTable(SqlConnectionString, selectQuery);

            if (dt != null && dt.Rows.Count > 0)
            {
                string direction = string.Empty;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(dt.Rows[0][0].ToString()))
                        {
                            direction = dt.Rows[0][0].ToString();
                            destination = dt.Rows[0][1] != null && !string.IsNullOrEmpty(dt.Rows[0][1].ToString()) ?
                                "to " + dt.Rows[0][1].ToString() : string.Empty;
                            message = string.Format("Ok, Route {2} traveling {0} {1} from Stop {3}.",
                                direction, Say.SaySpecialCharacters(destination),
                                Say.SayNumber(routeNumber.ToString()), Say.SayNumber(stopId.ToString()));
                        }
                    }
                    catch { }
                }

                return new JsonResult()
                {
                    Data = new SaidArrivalTimes()
                    {
                        NextStopArrivalTimeMessage = message,
                        NextStopArrivalTimeMessageSms = !string.IsNullOrEmpty(direction) ?
                            string.Format("Ok, Route#{2} traveling {0} {1} from Stop#{3}.",
                            direction.Substring(0, 1).ToLower() + direction.Substring(1, direction.Length - 1),  //lower case direction 
                                destination, routeNumber.ToString(), stopId.ToString()) : "Ok.",
                        NextStopArrivalTimeErrorMessage = "Invalid direction selected"
                    },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }

            throw new InvalidOperationException(string.Format("Direction ID not found on Route#{0}, Stop#{1}:  {2}",
                routeNumber.ToString(), stopId.ToString(), directionId.ToString()));

        }

        public JsonResult IsValidStopIdMessage(int routeNumber, int stopId)
        {
            string message = string.Empty;
            string publishedName = string.Empty;
            string selectQuery = string.Format(
                  //"select distinct Stops.Description " +
                  "select distinct Stops.PublishedName " +
                  "from StopTimes " +
                  "join Stops on StopTimes.StopId = Stops.StopId " +
                  "where routeId = {0} " +
                  "and StopTimes.stopId = {1} ",
                  routeNumber.ToString(), stopId.ToString());
#pragma warning disable CS0219 // Variable is assigned but its value is never used
            string sqlConnectionString = "Data Source=sql7002.site4now.net;Initial Catalog=DB_A3E75F_gk55;Persist Security Info=True;User ID=DB_A3E75F_gk55_admin;Password=helloWorld55!";
#pragma warning restore CS0219 // Variable is assigned but its value is never used
            DataTable dt = GetFilledSqlServerDataTable(SqlConnectionString, selectQuery);

            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    try
                    {
                        publishedName = dt.Rows[0][0].ToString();
                        if (!string.IsNullOrEmpty(dt.Rows[0][0].ToString()))
                            message = string.Format("Ok, Route {2} at Stop {0} {1}.",
                                Say.SayNumber(stopId.ToString()), Say.SaySpecialCharacters(publishedName),
                                Say.SayNumber(routeNumber.ToString()));
                    }
                    catch { }
                }

                return new JsonResult()
                {
                    Data = new SaidArrivalTimes()
                    {
                        NextStopArrivalTimeMessage = message,
                        NextStopArrivalTimeMessageSms = string.Format("Ok, Route#{2} at Stop#{0} {1}.",
                        stopId.ToString(), publishedName, routeNumber.ToString()),
                        NextStopArrivalTimeErrorMessage = "Invalid stop eye dee selected"
                    },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }

            throw new InvalidOperationException(string.Format("Stop ID not found on Route#{0}: {1}",
                routeNumber.ToString(), stopId.ToString()));
        }

        public JsonResult IsValidRouteNumberMessage(int routeNumber)
        {
            string message = string.Empty;

            var result = db.StopTimes.Where(st => st.Route == routeNumber.ToString())
                .Select(st => new { st.Direction, st.Destination, st.Route, st.DirectionId }).Distinct().ToList();
            List<StopTime> stopTimes = new List<StopTime>();
            foreach (var r in result)
            {
                stopTimes.Add(new StopTime()
                {
                    Direction = r.Direction,
                    Destination = r.Destination,
                    DirectionId = r.DirectionId,
                    Route = r.Route
                });
            }

            string selectQuery = string.Format("select distinct routeId from StopTimes where routeId = {0}",
                routeNumber.ToString());
#pragma warning disable CS0219 // Variable is assigned but its value is never used
            string sqlConnectionString = "Data Source=sql7002.site4now.net;Initial Catalog=DB_A3E75F_gk55;Persist Security Info=True;User ID=DB_A3E75F_gk55_admin;Password=helloWorld55!";
#pragma warning restore CS0219 // Variable is assigned but its value is never used
            DataTable dt = GetFilledSqlServerDataTable(SqlConnectionString, selectQuery);

            if (dt != null && dt.Rows.Count > 0)
            {
                message = string.Format("Ok, Route#{0}.", string.IsNullOrEmpty(dt.Rows[0][0].ToString()) ?
                    Say.SayNumber(routeNumber.ToString()) : Say.SayNumber(dt.Rows[0][0].ToString()));

                return new JsonResult()
                {
                    Data = new SaidArrivalTimes()
                    {
                        StopTimes = stopTimes.ToArray(),
                        NextStopArrivalTimeMessage = message,
                        NextStopArrivalTimeMessageSms = string.Format("Ok, Route#{0}.", routeNumber.ToString()),
                        NextStopArrivalTimeErrorMessage = "Invalid route number selected",
                        RouteSpokenName = Say.SayNumber(routeNumber.ToString())
                    },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }

            throw new InvalidOperationException(string.Format("Route Number not found: {0}.", routeNumber.ToString()));
        }

        private DateTime ConvertDateTimeToTimeZone(DateTime dt, string timeZoneName = "Eastern Standard Time")
        {
            return System.TimeZoneInfo.ConvertTimeFromUtc(dt.ToUniversalTime()/*DateTime.UtcNow*/,
                                TimeZoneInfo.FindSystemTimeZoneById(timeZoneName));
        }

        public JsonResult GetNextStopArrivalTimeMessage(int routeNumber, int stopId, int directionId)//ion, string destination)
        {
            string message = "sorry, no scheduled stop times for that route, stop and direction could be processed.";
            string timeOfStop = string.Empty, stopDescription = string.Empty, description = string.Empty,
                direction = string.Empty, destination = string.Empty;
            string smsMessage = string.Empty;

            if (routeNumber > 0)
                if (stopId > 0)
                    if (directionId > 0)
                    {
                        string selectQuery = string.Format(
                            //"select StopTimes.TimeOfStop, Stops.Description, StopTimes.Destination, StopTimes.RouteId, Stops.StopId, StopTimes.Direction" +
                            "select StopTimes.TimeOfStop, Stops.PublishedName, StopTimes.Destination, StopTimes.RouteId, Stops.StopId, StopTimes.Direction" +
                            " from StopTimes " +
                            " join Stops on StopTimes.StopId = Stops.StopId " +
                            " where route = '{0}' " +
                            " and StopTimes.stopId = {1} " +
                            " and directionId = {2} " +
                            " order by TimeOfStop asc ",
                            routeNumber.ToString(), stopId.ToString(), directionId.ToString()/*direction*/);
#pragma warning disable CS0219 // Variable is assigned but its value is never used
                        string sqlConnectionString = "Data Source=sql7002.site4now.net;Initial Catalog=DB_A3E75F_gk55;Persist Security Info=True;User ID=DB_A3E75F_gk55_admin;Password=helloWorld55!";
#pragma warning restore CS0219 // Variable is assigned but its value is never used

                        try
                        {
                            DataTable dt = GetFilledSqlServerDataTable(SqlConnectionString, selectQuery);
                            message = string.Format("Route {0} is NOT scheduled to arrive next at Stop {1} {2} again today.",
                                                Say.SayNumber(routeNumber.ToString()), Say.SayNumber(stopId.ToString()),
                                                Say.SaySpecialCharacters(stopDescription));
                            if (dt != null || dt.Rows.Count > 0)
                            {
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    timeOfStop = dt.Rows[i][0].ToString();
                                    //timeOfStop = "17:29:00.0000000";

                                    if (!string.IsNullOrEmpty(timeOfStop))
                                    {
                                        stopDescription = !string.IsNullOrEmpty(dt.Rows[i][1].ToString()) ?
                                                dt.Rows[i][1].ToString() : string.Empty;
                                        destination = !string.IsNullOrEmpty(dt.Rows[i][2].ToString()) ?
                                                dt.Rows[i][2].ToString() : string.Empty;
                                        direction = !string.IsNullOrEmpty(dt.Rows[i][5].ToString()) ?
                                                dt.Rows[i][5].ToString() : string.Empty;

                                        //c# - How to convert DateTime in Specific timezone? - Stack Overflow - https://stackoverflow.com/questions/9869051/how-to-convert-datetime-in-specific-timezone
                                        DateTime now = System.TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,
                                            TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));
                                        DateTime stopTime = DateTime.Parse(timeOfStop);

                                        if (stopTime.Ticks >= now.Ticks - 3000000000)
                                        {
                                            message = string.Format(!string.IsNullOrEmpty(destination) ? "{3}. Route {0} headed {5} to {4} is scheduled to arrive next at Stop {1} {2} at {3}" :
                                                "Route {0} is scheduled to arrive next at Stop {1} {2} at {3}",
                                                Say.SayNumber(routeNumber.ToString()), Say.SayNumber(stopId.ToString()),
                                                Say.SaySpecialCharacters(stopDescription), DateTime.Parse(timeOfStop).ToLongTimeString(),
                                                Say.SaySpecialCharacters(destination), direction);
                                            smsMessage = string.Format(!string.IsNullOrEmpty(destination) ? "{3}. Route {0} headed {5} to {4} is scheduled to arrive next at Stop {1} {2} at {3}" :
                                                "{3}. Route {0} is scheduled to arrive next at Stop {1} {2} at {3}",
                                                routeNumber.ToString(), stopId.ToString(),
                                                stopDescription, DateTime.Parse(timeOfStop).ToLongTimeString(),
                                                destination, direction);
                                            i = dt.Rows.Count + 1;
                                        }
                                    }
                                }
                            }
                        }

                        catch (Exception ex) { throw ex; }
                    }
            return new JsonResult()
            {
                Data = new SaidArrivalTimes()
                {
                    NextStopArrivalTimeMessage = message,
                    NextStopArrivalTimeMessageSms = smsMessage,
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

        }

        private static DataTable GetFilledSqlServerDataTable(string sqlConnectionString, string qry)
        {
            SqlConnection conn = new SqlConnection(sqlConnectionString);
            DataTable dt = new DataTable();
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(qry, conn);
                SqlDataAdapter oda = new SqlDataAdapter() { SelectCommand = new SqlCommand(qry, conn) };
                oda.Fill(dt);
                conn.Close();
                return dt;
            }
            catch (Exception ex)
            {
                string errmsg = string.Empty;// "Connection Timeout: {0}\n";//conn.ConnectionTimeout
                throw new Exception(string.Format("{0}\n{1}", errmsg, ex.Message));
            }
        }

        public JsonResult GetNextStopArrivalTime(string route, int stopId, string direction)
        {
            const long FiveMinsAsTicks = 3000000000;
            List<string> saidArrivalTimes = new List<string>();

            //select TimeOfStop
            //from StopTimes
            //where route = '301'
            //and stopId = 3137
            //and directionId = 1
            //order by TimeOfStop asc

            int directionId = int.Parse(direction);

            var result = db.StopTimes.Where(st => st.Route == route &&
                st.StopId == stopId && st.DirectionId == directionId);

            foreach (var r in result)
            {
                try
                {
                    DateTime dt = DateTime.Parse(r.TimeOfStop.ToString());
                    if (dt.Ticks >= (DateTime.Now.Ticks - FiveMinsAsTicks))
                        saidArrivalTimes.Add(string.Format("{0}", dt.ToLongTimeString()));
                }
                catch { }
            }

            //saidArrivalTimes.Add("9:00PM");
            //saidArrivalTimes.Add("9:15PM");
            //saidArrivalTimes.Add("9:45PM");
            //saidArrivalTimes.Add("10:15PM");
            //saidArrivalTimes.Add("11:15AM");
            SaidArrivalTimes arrivalTimes = new SaidArrivalTimes()
            {
                ArrivalTimes = saidArrivalTimes.ToArray(),
                NextStopTimesCount = saidArrivalTimes.Count > 0 ? saidArrivalTimes.Count - 1 : 0,
                StopId = stopId
            };

            return new JsonResult()
            {
                Data = arrivalTimes,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetRouteBounds(string routeNumber)
        {
            try
            {
                int routeId = int.Parse(routeNumber);
                if (routeId < 1)
                    throw new InvalidOperationException("Route Number entered is not a number.");
                //    GetRouteBounds()
                //--direction1: North
                //--direction2: South
                //--destination1: Wilmington
                //--destination2: Dover
                //--routeId: 301           

                var result = db.StopTimes.Where(st => st.Route == routeNumber)
                    .Select(st => new { st.Direction, st.Destination, st.Route, st.DirectionId }).Distinct().ToList();//<StopTime>();
                //string queryString =
                //        @"SELECT VALUE product FROM AdventureWorksEntities.Products AS product";
                // = @"select distinct direction, destination, routeId from stoptimes where routeid = 301";

                //    ObjectQuery<Product> productQuery1 =
                //        new ObjectQuery<Product>(queryString, context, MergeOption.NoTracking);

                //    ObjectQuery<Product> productQuery2 = productQuery1.Top("2");

                //    // Iterate through the collection of Product items.
                //    foreach (Product result in productQuery2)
                //        Console.WriteLine("{0}", result.Name);
                List<StopTime> stopTimes = new List<StopTime>();
                foreach (var r in result)
                {
                    stopTimes.Add(new StopTime()
                    {
                        Direction = r.Direction,
                        Destination = r.Destination,
                        DirectionId = r.DirectionId,
                        Route = r.Route
                    });
                }

                return new JsonResult()
                {
                    Data = new RouteBounds() { StopTimes = stopTimes.ToArray(), RouteName = routeNumber, RouteId = routeId },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }

            catch (InvalidOperationException ex)
            {
                return new JsonResult()
                {
                    Data = ex,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }

            //return null;
        }

        public JsonResult TestTwilioParseArrayItems()
        {
            Employee[] employees = new Employee[]
            {
                new Employee() { Name = "Ti Brown", Age=50, HireDate = new DateTime(2020, 03, 20) },
                new Employee() { Name = "T.S. Brown", Age=55, HireDate = new DateTime(2019, 03, 20) },
            };

            return new JsonResult()
            {
                Data = new Result() { Employees = employees },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        // GET: StopTimes
        public ActionResult Index()
        {
            return View(db.StopTimes.ToList());
        }

        // GET: StopTimes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StopTime stopTime = db.StopTimes.Find(id);
            if (stopTime == null)
            {
                return HttpNotFound();
            }
            return View(stopTime);
        }

        // GET: StopTimes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StopTimes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StopTimeId,Direction,Destination,StopId,TimeOfStop,Route")] StopTime stopTime)
        {
            if (ModelState.IsValid)
            {
                db.StopTimes.Add(stopTime);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(stopTime);
        }

        // GET: StopTimes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StopTime stopTime = db.StopTimes.Find(id);
            if (stopTime == null)
            {
                return HttpNotFound();
            }
            return View(stopTime);
        }

        // POST: StopTimes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StopTimeId,Direction,Destination,StopId,TimeOfStop,Route")] StopTime stopTime)
        {
            if (ModelState.IsValid)
            {
                db.Entry(stopTime).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(stopTime);
        }

        // GET: StopTimes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StopTime stopTime = db.StopTimes.Find(id);
            if (stopTime == null)
            {
                return HttpNotFound();
            }
            return View(stopTime);
        }

        // POST: StopTimes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StopTime stopTime = db.StopTimes.Find(id);
            db.StopTimes.Remove(stopTime);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
