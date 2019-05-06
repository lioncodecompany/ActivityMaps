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
		public string Description { get; set; }
		public int Status { get; set; }
        public bool IsService { get; set; }
        public string Activity_Cat_Code { get; set; } 
        public bool deleted { get; set; }
        public string Activity_Loc_Id { get; set; }
		public string Color{ get; set; }
        public int CountPeople { get; set; }
        public string StartActFormat { get; set; }
    }
}
