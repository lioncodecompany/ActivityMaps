using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using ActivityMaps.Helpers;
using ActivityMaps.Models;
using ActivityMaps.Views;
using GalaSoft.MvvmLight.Command;
using Xamarin.Forms;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions.Abstractions;
using Plugin.Permissions;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.DataMovement;
using System.Threading;

namespace ActivityMaps.ViewModels
{
    public class EndRegisterViewModel : BaseViewModel
    {
		#region Atributos

		public static bool allValidated = false;
		private Address currentAddress;
		private User currentUser;
		private string email;
		private string password;
		private string reEnterPassword;
		private ImageSource image = null;
		private string source;


		#endregion

		#region Propiedades

		[Xamarin.Forms.TypeConverter(typeof(Xamarin.Forms.ImageSourceConverter))]
		public Xamarin.Forms.ImageSource Image
		{
			get { return this.image; }
			set { SetValue(ref this.image, value); }
		}

		public string Email
		{
			get { return this.email; }
			set { SetValue(ref this.email, value); }
		}

		public string Password
		{
			get { return this.password; }
			set { SetValue(ref this.password, value); }
		}

		public string ReEnterPassword
		{
			get { return this.reEnterPassword; }
			set { SetValue(ref this.reEnterPassword, value); }
		}

		public Address CurrentAddress
		{
			get { return this.currentAddress; }
			set { SetValue(ref this.currentAddress, value); }

		}

		public User CurrentUser
		{
			get { return this.currentUser; }
			set { SetValue(ref this.currentUser, value); }

		}



		#endregion

		#region Contrusctores


		public EndRegisterViewModel(Address newAddress, User newUSer)
		{
			this.CurrentAddress = newAddress;
			this.CurrentUser = newUSer;
		}

		public EndRegisterViewModel()
		{
		}

		#endregion
		#region Comandos

		public ICommand NextCommand
		{
			get
			{
				return new RelayCommand(NextActivityPage);
			}
		}

		public ICommand TakePictureCommand
		{
			get
			{
				return new RelayCommand(TakePicture);
			}
		}

		public ICommand SelectPictureCommand
		{
			get
			{
				return new RelayCommand(SelectPicture);
			}
		}

		private async void SelectPicture()
		{


			var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);

			if (storageStatus != PermissionStatus.Granted)
			{
				var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Storage });
				storageStatus = results[Permission.Storage];
			}


			if (!CrossMedia.Current.IsPickPhotoSupported)
			{
				await Application.Current.MainPage.DisplayAlert(
					"Error",
					"Picking a photo is not supported",
					"Accept");
				return;
			}


			if (storageStatus == PermissionStatus.Granted)
			{
				var file = await CrossMedia.Current.PickPhotoAsync();

				if (file == null)
					return;
				source = file.Path;
				Image = ImageSource.FromStream(() => file.GetStream());
			}
			else
			{
				await Application.Current.MainPage.DisplayAlert("Permissions Denied", "Unable to choose photo.", "OK");
			}

		}

		private async void TakePicture()
		{
			await CrossMedia.Current.Initialize();

			var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
            


            if (cameraStatus != PermissionStatus.Granted)
			{
				var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Camera });
				cameraStatus = results[Permission.Camera];

            }

			if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
			{
				await Application.Current.MainPage.DisplayAlert(
					"Error",
					"No camera avaible",
					"Accept");
				return;
			}

            if (cameraStatus == PermissionStatus.Granted)
			{
				var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
				{
					DefaultCamera = Plugin.Media.Abstractions.CameraDevice.Front,
					SaveMetaData = true,
					Name = "test.jpg"
				});
				if (file == null)
					return;
				source = file.Path;
				Image = ImageSource.FromStream(() => file.GetStream());
			}
			else
			{
				await Application.Current.MainPage.DisplayAlert("Permissions Denied", "Unable to take photos.", "OK");
				//On iOS you may want to send your user to the settings screen.
				//CrossPermissions.Current.OpenAppSettings();
			}


		}

		private async void NextActivityPage()
		{

			
			

			if (string.IsNullOrEmpty(this.Email))
			{
				await Application.Current.MainPage.DisplayAlert(
					"Error",
					"You must enter an Email.",
					"Accept");
				return;
			}

			if (string.IsNullOrEmpty(this.Password))
			{
				await Application.Current.MainPage.DisplayAlert(
					"Error",
					"You must enter an Password.",
					"Accept");
				return;
			}

			if (string.IsNullOrEmpty(this.ReEnterPassword))
			{
				await Application.Current.MainPage.DisplayAlert(
					"Error",
					"You must enter an Password.",
					"Accept");
				return;
			}

			if (!(Password.Equals(ReEnterPassword)))
			{
				await Application.Current.MainPage.DisplayAlert(
					"Error",
					"Passwords not match.",
					"Accept");
				return;
			}

			CurrentUser.Email = Email;
			CurrentUser.IsActive = true;
			CurrentUser.Created_Date = DateTime.Now;


			byte[] encryted = System.Text.Encoding.Unicode.GetBytes(Password);
			var result = Convert.ToBase64String(encryted);
			int len = RandomId.length.Next(5, 10);

			User_Password password = new User_Password
			{
				Id = RandomId.RandomString(len),
				Password = result,
				User_Id_FK = CurrentUser.Id
			};

			try
			{
				await App.MobileService.GetTable<Address>().InsertAsync(CurrentAddress);
			}
			catch (Exception ex)
			{
				await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
			}

			try
			{
				await App.MobileService.GetTable<User>().InsertAsync(CurrentUser);
			}
			catch (Exception ex)
			{
				await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
			}

			try
			{
				await App.MobileService.GetTable<User_Password>().InsertAsync(password);
			}
			catch (Exception ex)
			{
				await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
			}
			/*
			//logic azure upload blob
			string storageConnectionString = "lioncode";
			CloudStorageAccount account = CloudStorageAccount.Parse(storageConnectionString);
			CloudBlobClient blobClient = account.CreateCloudBlobClient();
			CloudBlobContainer blobContainer = blobClient.GetContainerReference("activitymaps");
			blobContainer.CreateIfNotExists();
			string sourcePath = this.source;
			CloudBlockBlob destBlob = blobContainer.GetBlockBlobReference("activitymaps");

			// Setup the number of the concurrent operations
			TransferManager.Configurations.ParallelOperations = 64;
			// Setup the transfer context and track the upload progress
			SingleTransferContext context = new SingleTransferContext();
			context.ProgressHandler = new Progress<TransferStatus>((progress) =>
			{
				Console.WriteLine("Bytes uploaded: {0}", progress.BytesTransferred);
			});
			// Upload a local blob
			var task = TransferManager.UploadAsync(
				sourcePath, destBlob, null, context, CancellationToken.None);
			task.Wait();
			*/
			MainViewModel.GetInstance().Login = new LoginViewModel(CurrentUser.Email);
			await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());

		}
		#endregion
	}
}
