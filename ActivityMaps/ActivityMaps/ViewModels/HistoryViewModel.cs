using System;
using System.Collections.Generic;
using System.Text;
using ActivityMaps.Models;

namespace ActivityMaps.ViewModels
{
	public class HistoryViewModel
	{
		#region Atributos
		private List<User> user;
		#endregion

		#region Contructores
		public HistoryViewModel()
		{
		}

		public HistoryViewModel(List<User> user)
		{
			this.user = user;
		}
		#endregion
	}
}
