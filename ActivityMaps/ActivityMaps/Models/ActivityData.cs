using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ActivityMaps.Models
{
    public class ActivityData
    {
        public static ObservableCollection<Activity> Activities { get; private set; }
        static ActivityData()
        {
            Activities = new ObservableCollection<Activity>();
            Activities.Add(new Activity
            {
                Id = "1",
                Name = "BasketBall 3pa3",
                Activity_Loc_Id = "3",
                Activity_Cat_Code = "2"
                
            });

            Activities.Add(new Activity
            {
                Id = "2",
                Name = "BasketBall Doble",
                Activity_Loc_Id = "1",
                Activity_Cat_Code = "2"
                
                
            });
            Activities.Add(new Activity
            {
                Id = "3",
                Name = "Ping Pong de 4 Sets",
                Activity_Loc_Id = "3",
                Activity_Cat_Code = "1"
           
            });
            Activities.Add(new Activity
            {
                Id = "4",
                Name = "BasketBall 3pa3",
                Activity_Loc_Id = "2",
                Activity_Cat_Code = "2"
                
                
            });

            Activities.Add(new Activity
            {
                Id = "5",
                Name = "BasketBall Doble",
                Activity_Loc_Id = "1",
                Activity_Cat_Code = "2"
                
                
            });
            Activities.Add(new Activity
            {
                Id = "6",
                Name = "Merengue",
                Activity_Loc_Id = "3",
                Activity_Cat_Code = "3"
                
            });
            Activities.Add(new Activity
            {
                Id = "7",
                Name = "BasketBall 3pa3",
                Activity_Loc_Id = "3",
                Activity_Cat_Code = "2"
               
            });

            Activities.Add(new Activity
            {
                Id = "8",
                Name = "BasketBall Guerilla",
                Activity_Loc_Id = "2",
                Activity_Cat_Code = "2"
                
            });
            Activities.Add(new Activity
            {
                Id = "9",
                Name = "Salsa",
                Activity_Loc_Id = "1",
                Activity_Cat_Code = "3"
               
            });
        }
    }
}
