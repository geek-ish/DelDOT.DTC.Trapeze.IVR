﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCWebApplication2.Models
{
    public class ClientDevice
    {
        public int ClientDeviceId { get; set; }
        public int ClientId { get; set; }
        public string DeviceList { get; set; }
    }
}