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
        //public int Activity_Cat_Code { get; set; } //ID debemos buscar la forma de traer el texto de la tabla categoria
        public string Category { get; set; }
        public string Location { get; set; } 
        public int Activity_Loc_Id { get; set; }
    }
}
