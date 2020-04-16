using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCWebApplication2.Models
{
    public class Booking
    {
        public string BookingId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public string DARTID { get; set; }
        public DateTime RequestDateTime { get; set; }
        //public DateTime RequestedTime { get; set; }
        public string RequestRecipientName { get; set; }
        public string RequestRecipientEmail { get; set; }
        public string DateTime { get; set; }
        //public string Time { get; set; }
        //public string Companion { get; set; }
        public string CompanionName { get; set; }
        public int MobilityDeviceType { get; set; }
        public string TripPurpose { get; set; }
        public int PickupLocationType { get; set; }
        public string PickupLocationAddress { get; set; }
        public string PickupLocationCity { get; set; }
        public string PickupLocationZipcode { get; set; }
        public DateTime DropoffArrivalDateTime { get; set; }
        //public DateTime DropoffArrivalTime { get; set; }
        public int DropoffLocationType { get; set; }
        public string DropoffLocationName { get; set; }
        public string DropoffLocationAddress { get; set; }
        public string DropoffLocationCity { get; set; }
        public string DropoffLocationZipcode { get; set; }
        public string DropoffLocationPhone { get; set; }
        public bool IsRoundtrip { get; set; }
        public DateTime RoundtripLocationPickupDateTime { get; set; }
        public string Notes { get; set; }
    }
}