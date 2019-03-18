using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityMaps.Models
{
    public class Activity_Category
    {
        public string Id { get; set; } 
        public string Parent { get; set; } 
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
