using System;
using Xamarin.Forms;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Rg.Plugins.Popup.Extensions;

namespace Dripdoctors
{
	public partial class AccountProfileEditPage : ContentPage
	{
		private APIManager apiManager;
		private FileImageSource file = null;
		public AccountProfileEditPage()
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
			btnProfileBack.Clicked += OnBackButtonClicked;
			btnProfileSave.Clicked += OnSaveButtonClicked;

			btnProfileCamera.Source = ImageSource.FromFile("profile_camera.png");
			btnProfileUpload.Source = ImageSource.FromFile("profile_upload.png");
			btnProfileCancel.Source = ImageSource.FromFile("profile_cancel.png");

			var cameraTapGestureRecognizer = new TapGestureRecognizer();
			cameraTapGestureRecognizer.Tapped += OnCameraClicked;
			btnProfileCamera.GestureRecognizers.Add(cameraTapGestureRecognizer);

			var uploadTapGestureRecognizer = new TapGestureRecognizer();
			uploadTapGestureRecognizer.Tapped += OnUploadClicked;
			btnProfileUpload.GestureRecognizers.Add(uploadTapGestureRecognizer);

			var cancelTapGestureRecognizer = new TapGestureRecognizer();
			cancelTapGestureRecognizer.Tapped += OnCancelClicked;
			btnProfileCancel.GestureRecognizers.Add(cancelTapGestureRecognizer);

			initBody();
			apiManager = new APIManager();
		}

		private void initBody()
		{
			txtFname.Text = Singleton.sharedInstance().user.fname;
			txtSname.Text = Singleton.sharedInstance().user.sname;
			txtPhone.Text = Singleton.sharedInstance().user.mobilenum;
			txtCountry.Text = Singleton.sharedInstance().user.country;
			txtCity.Text = Singleton.sharedInstance().user.city;
			txtAddress.Text = Singleton.sharedInstance().user.address;
			txtZip.Text = Singleton.sharedInstance().user.zip;
			txtGender.Text = Singleton.sharedInstance().user.gender;
			txtAge.Text = "" + Singleton.sharedInstance().user.age;

			if (Singleton.sharedInstance().user.img_url != null)
			{
				imgProfile.Source = ImageSource.FromUri(new Uri(Singleton.sharedInstance().user.img_url));
			}
		}


		private bool checkInputValue() {
			
			if (txtPhone.Text == null) { 
				Device.BeginInvokeOnMainThread(() =>
				{
					Navigation.PushPopupAsync(new AlertPopup("Warring", "Input the phone number please!", "OK"));
				});
				return false;
			}
			if (txtAge.Text == null)
			{
				Device.BeginInvokeOnMainThread(() =>
				{
					Navigation.PushPopupAsync(new AlertPopup("Warring", "Input the age please!", "OK"));
				});
				return false;
			}
			if (txtZip.Text == null)
			{
				Device.BeginInvokeOnMainThread(() =>
				{
					Navigation.PushPopupAsync(new AlertPopup("Warring", "Input the zip please!", "OK"));
				});
				return false;
			}
			if (txtCity.Text == null)
			{
				Device.BeginInvokeOnMainThread(() =>
				{
					Navigation.PushPopupAsync(new AlertPopup("Warring", "Input the city please!", "OK"));
				});
				return false;
			}
			if (txtGender.Text == null)
			{
				Device.BeginInvokeOnMainThread(() =>
				{
					Navigation.PushPopupAsync(new AlertPopup("Warring", "Input the gender please!", "OK"));
				});
				return false;
			}
			if (txtCountry.Text == null)
			{
				Device.BeginInvokeOnMainThread(() =>
				{
					Navigation.PushPopupAsync(new AlertPopup("Warring", "Input the country please!", "OK"));
				});
				return false;
			}
			if (txtAddress.Text == null)
			{
				Device.BeginInvokeOnMainThread(() =>
				{
					Navigation.PushPopupAsync(new AlertPopup("Warring", "Input the address please!", "OK"));
				});
				return false;
			}
			return true;
		}

		public void OnBackButtonClicked(object sender, EventArgs e) {
			Navigation.PopAsync();
		}

		public async void OnSaveButtonClicked(object sender, EventArgs e) {
			if (checkInputValue()) {
				var user = Singleton.sharedInstance().user;
				user.fname = txtFname.Text;
				user.sname = txtSname.Text;
				user.mobilenum = txtPhone.Text;
				user.address = txtAddress.Text;
				user.city = txtCity.Text;
				user.zip = txtZip.Text;
				user.country = txtCountry.Text;
				user.gender = txtGender.Text;
				if (file != null)
				{
					user.profileImage = Functions.ImageToBase64(System.Drawing.Image.FromFile(file));
				}
				var result = await apiManager.updateUserInfo(user);
				if (result is bool) { 
					await Navigation.PopAsync();

				}else
					await Navigation.PushPopupAsync(new AlertPopup("Warning", (string)result, "OK"));
			}
		}

		public async void OnCameraClicked(object sender, EventArgs e) {
			await CrossMedia.Current.Initialize();
			if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
			{
				await Navigation.PushPopupAsync(new AlertPopup("No Camera", "No camera available.", "OK"));
			}
			var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
			{
				SaveToAlbum = true,
				Name = "photo.jpg"
			});
			if (file == null)
			{
				return;
			}
			imgProfile.Source = ImageSource.FromStream(() => file.GetStream());
		}

		public void OnUploadClicked(object sender, EventArgs e) {
		
		}

		public void OnCancelClicked(object sender, EventArgs e) {
			imgProfile.Source = null;
		}
	}
}
