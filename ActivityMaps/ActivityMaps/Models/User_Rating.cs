using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityMaps.Models
{
    public class User_Rating
    {
		public string Id { get; set; }
		public string Comment { get; set; }
		public string User_IdReporter_FK1 { get; set; }
		public string User_IdReported_FK2 { get; set; }
		public string Rating { get; set; }
	}
}
