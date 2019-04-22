using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityMaps.Models
{
    public class Reset_Password_Token
    {
		public string Id { get; set; }
		public string User_Id_FK { get; set; }
		public string Token { get; set; }
		public bool IsUsed { get; set; }
	
	}
}
