using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityMaps.Models
{
    public class File_Path
    {
		public string Id { get; set; }
		public string Type { get; set; }
		public string Path { get; set; }
		public string Filename { get; set; }
		public string FileUrl { get; set; }
		public DateTime Saved_Date { get; set; }
		public string User_Id_FK { get; set; }


	}
}
