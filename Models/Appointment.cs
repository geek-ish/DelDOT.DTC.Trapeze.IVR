using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCWebApplication2.Models
{
    /// <summary>
    /// Summary description for Appointment
    /// </summary>
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public DateTime StartDateTime;
        public DateTime EndDateTime;
        public string Title;
        public int TrainerId;
        public string Trainer;
        public int CountyId;
        public string County; // = "New Castle";
        public string AppointmentDisplayName;

        public Appointment()
        {
            Trainer = "Fish, Harry";
            County = "New Castle";
            TrainerId = 90;
            CountyId = 1;
        }

        public Appointment(int trainerId, string trainerName, string countyName, int countyId, DateTime dt)
        {
            //string trainerName = "Fish, Harry";
            string[] trainer = trainerName.ToString().Split(new char[] {','});
            string trainerDisplayName = string.Format("{0}. {1}", trainer[1].Trim().Substring(0, 1), trainer[0].Trim());
            string apptName = string.Format("{0} {1}:{2} {3}", trainerDisplayName,
                int.Parse(dt.ToString("hh")), dt.ToString("mm"), dt.ToString("tt"));

            this.Title = apptName;
            this.StartDateTime = dt;
            this.EndDateTime = new DateTime(dt.Ticks + 36000000000);
            this.County = countyName;
            this.CountyId = countyId + 1;
            this.Trainer = trainerName;
            this.AppointmentDisplayName = apptName;
            this.TrainerId = trainerId;
        }
    }
}