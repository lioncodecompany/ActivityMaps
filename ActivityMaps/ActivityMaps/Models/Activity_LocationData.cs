using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ActivityMaps.Models
{
    public class Activity_LocationData
    {

        public static ObservableCollection<Activity_Location> Locations { get; private set; }
        static Activity_LocationData()
        {
            Locations = new ObservableCollection<Activity_Location>();

            Locations.Add(new Activity_Location
            {
                Id = "1",
                Nameplace = "April Garden",
                City = "Las Piedras"
            });

            



        }
    }
}
