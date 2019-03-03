using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityMaps.Models
{
    public static class CountryData
    {
		public static IList<Country> Countries { get; private set; }

		static CountryData()
		{
			Countries = new List<Country>();

			Countries.Add(new Country
			{
				Name = "Puerto Rico"
			});



		}
	}
}
