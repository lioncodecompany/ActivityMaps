
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ActivityMaps.Helpers
{
    public class RandomId
    {
		private static Random random = new Random();
		public static Random length = new Random();

		public static string RandomString(int length)
		{
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			return new string(Enumerable.Repeat(chars, length)
			  .Select(s => s[random.Next(s.Length)]).ToArray());
		}
	}
}