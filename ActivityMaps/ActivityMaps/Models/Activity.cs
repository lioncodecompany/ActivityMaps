using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityMaps.Models
{
    public class Activity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime Created_Date { get; set; }
        public DateTime Start_Act_Datetime { get; set; }
        public DateTime End_Act_Datetime { get; set; }
        public bool IsPrivate { get; set; }
        public int Status { get; set; }
        public bool IsService { get; set; }
        public string Activity_Cat_Code { get; set; } 
       // public string CategoryName { get; set; }
        //public string LocationPlaceName { get; set; } 
        public string Activity_Loc_Id { get; set; }

    }
}
