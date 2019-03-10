using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ActivityMaps.Models
{
    public class User
    {
		public string Id { get; set; }
		public string Name { get; set; }
		public string Last_Name { get; set; }
		public string Nickname { get; set; }
		public string Gender { get; set; }
		public string Email { get; set; }
		public DateTime Birthdate { get; set; }
		public bool IsActive { get; set; }
		public DateTime Created_Date{ get; set; }
		public bool IsAdmin { get; set; }
		public bool Locked { get; set; }
		public string Address_Id_FK { get; set; }

	}
}
