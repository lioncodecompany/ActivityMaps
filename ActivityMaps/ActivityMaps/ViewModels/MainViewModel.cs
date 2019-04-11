using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityMaps.ViewModels
{
	public class MainViewModel
	{
		#region ViewModels

		public LoginViewModel Login { get; set; }
		public RegisterViewModel Register { get; set; }
		public EndRegisterViewModel EndRegister { get; set; }
        public ActivityViewModel Activity_Child { get; set; }
		public CreateActivityViewModel CreateActivity { get; set; }
		public LocationViewModel Location { get; set; }
		public ActivityJoinViewModel ActivityJoin { get; set; }
		public MenuViewModel Menu { get; set; }
		public ProfileViewModel Profile { get; set; }
 

        #endregion

        #region Constructors

        public MainViewModel()
		{
			instance = this;
			this.Login = new LoginViewModel();
			this.Register = new RegisterViewModel();
			this.EndRegister = new EndRegisterViewModel();
            this.Activity_Child = new ActivityViewModel();
			this.CreateActivity = new CreateActivityViewModel();
			this.Location = new LocationViewModel();
			this.ActivityJoin = new ActivityJoinViewModel();
			this.Menu = new MenuViewModel();
			this.Profile = new ProfileViewModel();
		}


		#endregion

		#region SignLeton
		private static MainViewModel instance;

		public static MainViewModel GetInstance()
		{
			if(instance == null)
			{
				return new MainViewModel();
			}
			return instance;
		}
		#endregion
	}
}
