using ActivityMaps.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityMaps.ViewModels
{
    public class SettingViewModel
    {
		#region Atributos
		private List<User> user;
		#endregion

		#region Contructores
		public SettingViewModel()
		{
		}

		public SettingViewModel(List<User> user)
		{
			this.user = user;
		}
		#endregion
	}
}
