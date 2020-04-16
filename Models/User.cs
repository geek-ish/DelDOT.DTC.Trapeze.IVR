using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCWebApplication2.Models
{
    public class User
    {
        public int UserId { get; set; }
        public int ClientId { get; set; } 
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TextNumber { get; set; }
        public int? Passcode { get; set; }
        public User() { }
    }
}