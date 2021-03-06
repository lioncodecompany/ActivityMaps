﻿// Helpers/Settings.cs
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace ActivityMaps.Utils
{
  /// <summary>
  /// This is the Settings static class that can be used in your Core solution or in any
  /// of your client applications. All settings are laid out the same exact way with getters
  /// and setters. 
  /// </summary>
  public static class Settings
{
	private static ISettings AppSettings
	{
		get
		{
			return CrossSettings.Current;
		}
	}

	#region Setting Constants

	private const string LastEmailSettingsKey = "Last_Email_key";
	private const string LastIsRememberedSettingsKey = "Last_remembered_key";
	private static readonly string SettingsDefault = string.Empty;

	#endregion


	public static string LastUsedEmail
	{
		get
		{
			return AppSettings.GetValueOrDefault(LastEmailSettingsKey, SettingsDefault);
		}
		set
		{
			AppSettings.AddOrUpdateValue(LastEmailSettingsKey, value);
		}
	}
		public static string LastIsRemembered
		{
			get
			{
				return AppSettings.GetValueOrDefault(LastIsRememberedSettingsKey, SettingsDefault);
			}
			set
			{
				AppSettings.AddOrUpdateValue(LastIsRememberedSettingsKey, value);
			}
		}

	}
}