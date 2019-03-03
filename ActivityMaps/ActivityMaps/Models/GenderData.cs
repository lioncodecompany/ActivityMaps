using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityMaps.Models
{
    public static class GenderData
    {
		public static IList<Gender> Genders { get; private set; }

		static GenderData()
		{
			Genders = new List<Gender>();

			Genders.Add(new Gender
			{
				Name = "Male"
			});

			Genders.Add(new Gender
			{
				Name = "Female"
			});

	
		}
	}
}
