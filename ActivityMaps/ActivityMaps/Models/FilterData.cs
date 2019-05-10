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
				Name = "BASKETBALL"
			});
			Filters.Add(new Filter
			{
				Name = "Salsa"
			});
			Filters.Add(new Filter
			{
				Name = "Table Tennis"
			});
			Filters.Add(new Filter
			{
				Name = "Baseball"
			});
			Filters.Add(new Filter
			{
				Name = "Tennis"
			});
			Filters.Add(new Filter
			{
				Name = "Football"
			});
			Filters.Add(new Filter
			{
				Name = "Volleyball"
			});
			Filters.Add(new Filter
			{
				Name = "Merengue"
			});
			Filters.Add(new Filter
			{
				Name = "Zumba"
			});
			Filters.Add(new Filter
			{
				Name = "Event"
			});
			Filters.Add(new Filter
			{
				Name = "Walk"
			});
			Filters.Add(new Filter
			{
				Name = "Golf"
			});
			Filters.Add(new Filter
            {
                Name = "NO FILTER"
            });

            //another one++

        }
	}
}
