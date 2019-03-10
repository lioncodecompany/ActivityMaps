
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ActivityMaps.Models
{
    public class Address
	{
		
		public string Id { get; set; }
		//public int Id_PK { get; set; }
		public string Phone { get; set; }
		public string Address1 { get; set; }
		public string Address2 { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string Country { get; set; }
		public string Zipcode { get; set; }
	}
}