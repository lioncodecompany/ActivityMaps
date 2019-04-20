using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityMaps.Models
{
    public class Activity_Location
    {
        public string Id { get; set; }
        public string Nameplace { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public decimal CreatorOriginalPinLongitude { get; set; }
        public decimal CreatorOriginalPinLatitude { get; set; }
    }
}
