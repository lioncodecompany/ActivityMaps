using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityMaps.Models
{
    public class FilterData
    {
		public static IList<Filter> Filters { get; private set; }

		static FilterData()
		{
			Filters = new List<Filter>();

			Filters.Add(new Filter
			{
				Name = "Activity"
			});
			Filters.Add(new Filter
			{
				Name = "Category"
			});
			Filters.Add(new Filter
			{
				Name = "Location"
			});

			//another one++

		}
	}
}
