using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityMaps.Models
{
    public class Entered_History
    {
		public string Id { get; set; }
		public string Status { get; set; }
		public bool IsCreator { get; set; }
		public string Activity_Code_FK2 { get; set; }
		public bool deleted { get; set; }
		public string UserJoin { get; set; }
		public string UserCreator { get; set; }
	}
}
