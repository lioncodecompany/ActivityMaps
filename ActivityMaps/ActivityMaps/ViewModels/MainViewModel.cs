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

		#endregion

		#region Constructors

		public MainViewModel()
		{
			instance = this;
			this.Login = new LoginViewModel();
			this.Register = new RegisterViewModel();
			this.EndRegister = new EndRegisterViewModel();
			
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
