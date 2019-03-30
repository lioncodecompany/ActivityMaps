using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityMaps.Models
{
	public class Activity_History
	{
		public string Id { get; set; }
		public string Activity_Code_Id { get; set; }
		public string Name { get; set; }
		public DateTime Created_Date { get; set; }
		public DateTime Start_Act_Date { get; set; }
		public DateTime End_Act_Date { get; set; }
		public bool IsPrivate { get; set; }
		public string Description { get; set; }
		public int Status { get; set; }
		public bool IsService { get; set; }
		public string Activity_Cat_code { get; set; }
		public string Activity_Loc_Id_FK { get; set; }
	}
}
