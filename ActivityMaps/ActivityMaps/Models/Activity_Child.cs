using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityMaps.Models
{
    public class Activity_Child:Activity
    {
		
        public string CategoryName { get; set; }
        public string LocationPlaceName { get; set; }
		public string LocationTown { get; set; }
		public string Town { get; set; }
		public string Creator { get; set; }
       // public bool IsActivityFound { get; set; }


    }
}
