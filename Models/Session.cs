using System;

namespace MVCWebApplication2.Models
{
    public class Session
    {
        public static string TwilioDefaultDateTimeFormat = "yyyy-MM-dd";
        //private string _unregisteredNumber;
        public string SessionId { get; set; }
        //public int ClientId { get; set; }
        //public string Phone { get; set; }
        public int? Passcode { get; set; }
        //public string Email { get; set; }
        public User User { get; set; }
        public Caller Caller { get; set; }

        public DateTime CreationTime
        {
            get { return creationTime; }
            set { creationTime = value; }
        }

        public string CreationTimeDisplayDate
        {
            get { return creationTime.ToString(TwilioDefaultDateTimeFormat); }
            set { creationTimeDisplayDate = value; }
        }


        public string LoginTimeDisplayDate
        {
            get { return loginTime.ToString(TwilioDefaultDateTimeFormat); }
            set { loginTimeDisplayDate = value; }
        }

        public string LogoutTimeDisplayDate
        {
            get { return logoutTime.ToString(TwilioDefaultDateTimeFormat); }
            set { logoutTimeDisplayDate = value; }
        }

        public string PasscodeSentDisplayDate
        {
            get { return passcodeSent.ToString(TwilioDefaultDateTimeFormat); }
            set { passcodeSentDisplayDate = value; }
        }

        public DateTime LoginTime
        {
            get { return loginTime; }
            set { loginTime = value; }
        }

        public DateTime LogoutTime
        {
            get { return logoutTime; }
            set { logoutTime = value; }
        }

        public DateTime PasscodeSent
        {
            get { return passcodeSent; }
            set { passcodeSent = value; }
        }

        public long UnregisteredNumberCallback
        {
            get { return PhoneNumber.Parse(UnregisteredNumber); }
            set { UnregisteredNumberCallback = value; }
        }

        public string PasscodeSentSpokenDateTime
        {
            get
            {

                //TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                //Console.WriteLine("The date and time are {0} UTC.",
                //                  TimeZoneInfo.ConvertTimeToUtc(PasscodeSent..ToShortTime(), easternZone)
                return string.Format("{0}",
                //return string.Format("{0} at {1}",
                    PasscodeSent.ToString("D")/*ToLongDateString()*//*,//.ToString("dddd, MMMM d, yyyy"),
                    PasscodeSent.ToLocalTime().ToShortTimeString()*/);
            }

            set
            {
                passcodeSentSpokenDateTime = value;
            }
        }

        public Int64? CallerId;
        private DateTime creationTime;
        private DateTime loginTime;
        private DateTime logoutTime;
        public string UnregisteredNumber;
        private string loginTimeDisplayDate;
        private string logoutTimeDisplayDate;
        private string passcodeSentDisplayDate;
        private string creationTimeDisplayDate;
        private DateTime passcodeSent;
        private string passcodeSentSpokenDateTime;

        public Session()
        {
            UnregisteredNumber = PhoneNumber.Redial_Undefined;// "-1";// string.Empty;
            //UnregisteredNumberCallback = PhoneNumber.RedialCallack_Undefined;
            //LoginTime = new DateTime(1, 1, 1);
            //LogoutTime = new DateTime(1, 1, 1);
            //PasscodeSent = new DateTime(1, 1, 1);
            //CreationTime = new DateTime(1, 1, 1);
            //DisplayDate = new DateTime().ToString(TwilioDefaultDateTimeFormat);
        }

        public Session(Caller c)
        {
            UnregisteredNumber = PhoneNumber.Redial_Undefined;
            Caller = c;
        }
    }
}