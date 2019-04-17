using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityMaps.Models
{
    public class Feedback
    {
		public string Id { get; set; }
		public string Comment{ get; set; }
		public string User_Id_FK { get; set; }
		public string Rating { get; set; }

	}
}
