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
				Name = "DANCE"
			});
			Filters.Add(new Filter
			{
				Name = "PING PONG"
			});
            Filters.Add(new Filter
            {
                Name = "NO FILTER"
            });

            //another one++

        }
	}
}
