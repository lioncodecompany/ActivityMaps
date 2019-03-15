using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ActivityMaps.Models
{
    public class Activity_CategoryData
    {

        public static ObservableCollection<Activity_Category> Categories { get; private set; }
        static Activity_CategoryData()
        {
            Categories = new ObservableCollection<Activity_Category>();

            Categories.Add(new Activity_Category
            {
                Id = "1",
                Parent = "0",
                Name = "Ping Pong"
                
            });

            Categories.Add(new Activity_Category
            {
                Id = "2",
                Parent = "0",
                Name = "Basketball"
                
            });

            Categories.Add(new Activity_Category
            {
                Id = "3",
                Parent = "0",
                Name = "Dance"
               
            });
        }

    }
}
