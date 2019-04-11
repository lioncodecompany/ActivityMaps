using System;
using System.Collections.Generic;
using System.Text;
using ActivityMaps.Models;
using ActivityMaps.ViewModels;


namespace ActivityMaps.ViewModels
{
	public class ProfileViewModel : BaseViewModel
	{
		#region Atributos
		private List<User> user;
		private string nickName;
		#endregion

		#region Propieades
		public string NickName
		{
			get { return this.nickName; }
			set
			{
				SetValue(ref this.nickName, value);
			}
		}
		#endregion
		#region Contructores
		public ProfileViewModel(List<User> user)
		{
			this.user = user;
			this.NickName = user[0].Nickname;
		}
		public ProfileViewModel()
		{

		}
		#endregion
	}
}
