using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityMaps.Models
{
    public class User_Setting
    {
		public string Id { get; set; }
		public bool SendNotification { get; set; }
		public bool AlertSound { get; set; }
		public string Lang_Id_FK1 { get; set; }
		public string User_Id_FK2 { get; set; }
		
	}
}
