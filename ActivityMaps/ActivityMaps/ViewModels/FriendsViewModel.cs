using System;
using System.Collections.Generic;
using System.Text;
using ActivityMaps.Models;

namespace ActivityMaps.ViewModels
{
	public class FriendsViewModel
	{
		#region Atributos
		private List<User> user;
		#endregion

		#region Contructores
		public FriendsViewModel(List<User> user)
		{
			this.user = user;
		}
		public FriendsViewModel()
		{
			
		}
		#endregion
	}
}
