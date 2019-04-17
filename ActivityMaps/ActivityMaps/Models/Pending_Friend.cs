using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityMaps.Models
{
   public class Pending_Friend
    {
		public string Id { get; set; }
		public string Requested_By_FK1 { get; set; }
		public string Requested_To_FK2 { get; set; }
		public string Status { get; set; }
		public string Nickname { get; set; }
	}
}
