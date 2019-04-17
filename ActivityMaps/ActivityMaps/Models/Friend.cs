using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityMaps.Models
{
    public class Friend
    {

		public string Id { get; set; }
		public string User_Id_FK1 { get; set; }
		public string User_Friend_Id_FK2 { get; set; }
		public int Type { get; set; }
		public string Nickname { get; set; }
	}
}
