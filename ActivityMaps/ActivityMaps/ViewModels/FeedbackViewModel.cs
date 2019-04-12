using System;
using System.Collections.Generic;
using System.Text;
using ActivityMaps.Models;

namespace ActivityMaps.ViewModels
{
	public class FeedbackViewModel
	{
		#region Atributos
		private List<User> user;
		#endregion

		#region Contructores
		public FeedbackViewModel(List<User> user)
		{
			this.user = user;
		}
		public FeedbackViewModel()
		{

		}
		#endregion

	}
}
