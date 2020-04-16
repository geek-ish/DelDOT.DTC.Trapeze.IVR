using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MVCWebApplication2.Models
{
    //[Table("StopTimes")]
    public class StopTime
    {
        //    [Key]
        public int StopTimeId { get; set; }
        public string Direction { get; set; }
        public string Destination { get; set; }
        public int StopId { get; set; }
        public TimeSpan TimeOfStop { get; set; }
        public string Route { get; set; }
        public int DirectionId { get; set; }

        public string StopIdSpokenName
        {
            get
            {
                string spokenStopId = StopId.ToString();
                if (StopId > 100)// && StopId < 1000)
                {
                    spokenStopId = string.Empty;
                    char[] chars = StopId.ToString().ToArray();
                    for (int i = 0; i < chars.Length; i++)
                    {
                        spokenStopId += chars[i] + " ";
                        spokenStopId = spokenStopId.Replace('0', 'o');
                    }
                }

                return spokenStopId;
            }

            set
            {
                stopIdSpokenName = value;
            }
        }

        private string stopIdSpokenName;
        //public string TimeOfStopSpokenDateTime
        //{
        //    get
        //    {
        //        return string.Format("{0}", DateTime.Parse(TimeOfStop.ToString()).ToString("D"));
        //    }

        //    set
        //    {
        //        timeOfStopSpokenDateTime = value;
        //    }
        //}
        //private string timeOfStopSpokenDateTime;
    }

    public class SaidArrivalTimes
    {
        public string[] ArrivalTimes;
        //public int NextStopTimesCount;
        public int StopId;
        public int RouteNumber;
        public int DirectionId;

        private int nextStopTimesCount;
        private string sayNextStopTimes;
        private string nextStopTime;
        private string stopIdSpokenName;
        public string NextStopArrivalTimeMessage;
        public string NextStopArrivalTimeErrorMessage;
        public string NextStopDescription;
        public StopTime[] StopTimes;

        public string StopIdSpokenName
        {
            get
            {
                string spokenStopId = StopId.ToString();
                if (StopId > 100)// && StopId < 1000)
                {
                    spokenStopId = string.Empty;
                    char[] chars = StopId.ToString().ToArray();
                    for (int i = 0; i < chars.Length; i++)
                    {
                        spokenStopId += chars[i] + " ";
                        spokenStopId = spokenStopId.Replace('0', 'o');
                    }
                }

                return spokenStopId;
            }

            set
            {
                stopIdSpokenName = value;
            }
        }

        public string NextStopTimesMessage
        {
            get
            {
                //string beVerb = "are", countAdjective = countAdjective = NextStopTimesCount.ToString(), stopsGrammarNumber = "stops";
                //int length = ArrivalTimes.Length;
                //if (NextStopTimesCount == 1)
                //{
                //    beVerb = "is";
                //    stopsGrammarNumber = "stop";
                //}
                //string msg = "There are no more stops scheduled for this stop today.";

                return string.Format("{0}", NextStopTimesCount > 0 ?
                    "Here are the next scheduled times for this stop today.  \n" + SayNextStopTimes.Replace(",", ", and ") : 
                    "There are no more stops scheduled for this stop today.");
            }

            set
            {
                nextStopTime = value;
            }
        }

        //public string NextStopTime
        //{
        //    get
        //    {
        //        return ArrivalTimes.Length > 0 ? ArrivalTimes[0] : string.Empty;
        //    }

        //    set
        //    {
        //        nextStopTime = value;
        //    }
        //}

        public int NextStopTimesCount
        {
            get
            {
                return ArrivalTimes == null || ArrivalTimes.ToList().Count < 1 ? 0 : ArrivalTimes.Length - 1;
            }

            set
            {
                nextStopTimesCount = value;
            }
        }
        
        public string SayNextStopTimes
        {
            get
            {
                return ArrivalTimes == null || ArrivalTimes.ToList().Count < 1 ? string.Empty : string.Join(", ", ArrivalTimes, 1,
                        ArrivalTimes.Length - 1 > 5 ? 5 : ArrivalTimes.Length - 1);
            }

            set
            {
                sayNextStopTimes = value;
            }
        }

        public string RouteSpokenName;
        public string NextStopArrivalTimeMessageSms;

        public SaidArrivalTimes() { }
        public SaidArrivalTimes(int routeNumber, int stopId, int directionId, string stopDescription = "")
        {
            RouteNumber = routeNumber;
            StopId = stopId;
            DirectionId = directionId;
            //NextStopArrivalTimeMessage = string.Format("Route {0} is scheduled to arrive next at Stop {1} {2} at {3}",
            //       Say.SayNumber(routeNumber.ToString()), Say.SayNumber(stopId.ToString()),
            //       stopDescription, ArrivalTimes.ToArray()[0]);
        }
    }

}