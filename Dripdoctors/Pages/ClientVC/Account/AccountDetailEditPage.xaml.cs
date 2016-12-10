using System;
using Xamarin.Forms;
using Rg.Plugins.Popup.Extensions;
using XLabs.Cryptography;

namespace Dripdoctors
{
	public partial class AccountDetailEditPage : ContentPage
	{
		private APIManager apiManager;
		public AccountDetailEditPage()
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
			btnDetailBack.Clicked += OnBackButtonClicked;
			btnDetailSave.Clicked += OnSaveButtonClicked;
			apiManager = new APIManager();
			txtEmail.Text = Singleton.sharedInstance().user.email;
		}

		private bool checkInputValue() {
			if (txtEmail.Text == null)
			{
				Device.BeginInvokeOnMainThread(() =>
				{
					Navigation.PushPopupAsync(new AlertPopup("Warring", "Input the E-mail please!", "OK"));
				});
				return false;
			}
			if (txtOldPassword.Text == null) 
			{
				Device.BeginInvokeOnMainThread(() =>
				{
					Navigation.PushPopupAsync(new AlertPopup("Warring", "Input the OldPassword please!", "OK"));
				});
				return false;	
			}
			var oldPass = MD5.GetMd5String(txtOldPassword.Text);
			if (oldPass.Equals(Singleton.sharedInstance().user.password))
			{
				Device.BeginInvokeOnMainThread(() =>
				{
					Navigation.PushPopupAsync(new AlertPopup("Warring", "Input the age OldPassword!", "OK"));
				});
				return false;
			}
			if (txtNewPassword.Text == null) { 
				Device.BeginInvokeOnMainThread(() =>
				{
					Navigation.PushPopupAsync(new AlertPopup("Warring", "Input the age new password!", "OK"));
				});
				return false;
			}
			if (txtConfirmPassword.Text == null) {
				Device.BeginInvokeOnMainThread(() =>
				{
					Navigation.PushPopupAsync(new AlertPopup("Warring", "Input the age confirm password!", "OK"));
				});
				return false;
			}
			if (!txtNewPassword.Text.Equals(txtConfirmPassword.Text)) { 
				Device.BeginInvokeOnMainThread(() =>
				{
					Navigation.PushPopupAsync(new AlertPopup("Warring", "Input the age confirm password!", "OK"));
				});
				return false;
			}
			return true;
		}

		public void OnBackButtonClicked(object sender, EventArgs e)
		{
			Navigation.PopAsync();
		}

		public async void OnSaveButtonClicked(object sender, EventArgs e)
		{
			var user = Singleton.sharedInstance().user;
			await apiManager.updateUserPassword(user, txtNewPassword.Text, txtConfirmPassword.Text);
		}
	}
}
