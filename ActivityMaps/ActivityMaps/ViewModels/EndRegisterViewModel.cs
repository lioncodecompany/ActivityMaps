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
using System.IO;
using System.Net.Mail;

namespace ActivityMaps.ViewModels
{
    public class EndRegisterViewModel : BaseViewModel
    {
		#region Atributos
		private bool isRunning;
		public static bool allValidated = false;
		private Address currentAddress;
		private User currentUser;
		private string email;
		private string password;
		private string reEnterPassword;
		private ImageSource image = null;
		private string source;
		private string uploadedFilename;
		public byte[] byteData;
		private bool isReady = false;


		#endregion

		#region Propiedades

		public bool IsRunning
		{
			get { return this.isRunning; }
			set { SetValue(ref this.isRunning, value); }
		}

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
			this.IsRunning = false;
		}

		public EndRegisterViewModel()
		{
			this.IsRunning = false;
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
			CheckConnectionInternet.checkConnectivity();
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

			this.isReady = true;
				
		}

		private async void TakePicture()
		{
			CheckConnectionInternet.checkConnectivity();
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
					CompressionQuality = 92,
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
			this.isReady = true;

		}

		private async void NextActivityPage()
		{

			CheckConnectionInternet.checkConnectivity();


			if (string.IsNullOrEmpty(this.Email))
			{
				await Application.Current.MainPage.DisplayAlert(
					"Error",
					"You must enter an Email.",
					"Accept");
				return;
			}
			if (!IsValid(this.Email))
			{
				await Application.Current.MainPage.DisplayAlert(
					"Error",
					"You must enter a real Email.",
					"Accept");
				return;
			}

			var checkEmail = await App.MobileService.GetTable<User>().Where(p => p.Email == this.Email).ToListAsync();
			if (checkEmail.Count > 0)
			{
				await Application.Current.MainPage.DisplayAlert(
					"Error",
					"This email has been registered.",
					"Try Another");
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

			if (this.Password.Length < 7)
			{
				await Application.Current.MainPage.DisplayAlert(
					"Error",
					"Your Password length must be greather than 6.",
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
			if (!this.isReady)
			{
				await Application.Current.MainPage.DisplayAlert(
					"Error",
					"You must take a picture or select one",
					"Accept");
				return;
			}

			this.IsRunning = true;

			CurrentUser.Email = Email;
			CurrentUser.IsActive = true;
			CurrentUser.Created_Date = DateTime.Now;


			byte[] encryted = System.Text.Encoding.Unicode.GetBytes(Password);
			var result = System.Convert.ToBase64String(encryted);
			int len = RandomId.length.Next(5, 10);

			User_Password password = new User_Password
			{
				Id = RandomId.RandomString(len),
				Password = result,
				User_Id_FK = CurrentUser.Id
			};

			byteData = AzureStorage.Convert.ToByteArray(source);
			uploadedFilename = await AzureStorage.AzureStorage.UploadFileAsync(AzureStorage.ContainerType.Image, new MemoryStream(byteData));

			string[] arr = source.Split('/');

			File_Path filepath = new File_Path
			{
				Id = RandomId.RandomString(len),
				Type = "Image",
				Path = uploadedFilename,
				Filename = arr[arr.Length - 1],
				FileUrl = source,
				Saved_Date = DateTime.Today,
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
			try
			{
				await App.MobileService.GetTable<File_Path>().InsertAsync(filepath);
			}
			catch (Exception ex)
			{
				await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
			}
			/*
			//logic azure upload blob
			string storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=lioncode;AccountKey=oY4jZN5nSf4g/4ATkgHIPAgjVRxF3fYS/R1BfhT1k9Li98e7vEYq4/DY4y38LHQ9zjvsvIXI8qEDYQWeeHbxHQ==;EndpointSuffix=core.windows.net";
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
			// Upload a local blob
			var task = TransferManager.UploadAsync(
				sourcePath, destBlob, null, context, CancellationToken.None);
			task.Wait();
			*/

		
			

			this.IsRunning = false;

			MainViewModel.GetInstance().Login = new LoginViewModel(CurrentUser.Email);
			await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());

		}
		public bool IsValid(string emailaddress)
		{
			try
			{
				MailAddress m = new MailAddress(emailaddress);

				return true;
			}
			catch (FormatException)
			{
				return false;
			}
		}
		#endregion
	}
}
