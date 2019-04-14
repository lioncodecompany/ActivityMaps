using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityMaps.Models
{
    public class RatingData
    {
		public static IList<Rating> Ratings { get; private set; }

		static RatingData()
		{
			Ratings = new List<Rating>();

			Ratings.Add(new Rating
			{
				FeedbackName = "1"
			});

			Ratings.Add(new Rating
			{
				FeedbackName = "2"
			});
			Ratings.Add(new Rating
			{
				FeedbackName = "3"
			});
			Ratings.Add(new Rating
			{
				FeedbackName = "4"
			});
			Ratings.Add(new Rating
			{
				FeedbackName = "5"
			});

		}
	}
}
