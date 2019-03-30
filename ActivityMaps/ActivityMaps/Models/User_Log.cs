using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityMaps.Models
{
    public class User_Log : User
    {
		public new string Id { get; set; }
		public DateTime LogDateTime { get; set; }
		public string LogInfo { get; set; }
		public string Activity_code { get; set; }
		public string User_Equipment_code { get; set; }
		public string User_LogType_Id_FK1 { get; set; }
		public string User_Id_FK2 { get; set; }
	}
}
