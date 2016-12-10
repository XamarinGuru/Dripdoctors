using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Rg.Plugins.Popup.Extensions;

namespace Dripdoctors
{
	public partial class SignInPage : ContentPage
	{
		APIManager apiManager;
		public SignInPage()
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
			apiManager = new APIManager();
			btnSignin.Clicked += OnSignInClicked;
			btnSignup.Clicked += OnSignUpClicked;
			Singleton.sharedInstance().locationManager = new LocationManager();
			loadClinic();
		}

		private async void loadClinic()
		{
			var apiManager = new APIManager();
			var result = await apiManager.getClinics();
			if (!(result is List<Clinic>))
			{

				return;
			}
			Singleton.sharedInstance().clinics = (List<Clinic>)result;
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			//isThread = false;
		}

		public async void OnSignInClicked(object sender, EventArgs e)
		{
			if (checkInputValue())
			{
				string mail = txt_mail.Text;
				string pwd = txt_pwd.Text;
				var result = await apiManager.signInAsync(mail, pwd);
				if (result is User)
				{
					Singleton.sharedInstance().user = (User)result;
					if(((User)result).usergroup == 3)
						App.showNurseStroyboard();
					else if(((User)result).usergroup == 4)
						App.showClientStoryboard();
				}
				else {
					await Navigation.PushPopupAsync(new AlertPopup("Warning", (string)result, "OK"));
				}
			}
		}

		public void OnSignUpClicked(object sender, EventArgs e)
		{
			var signinPage = new SignUpPage();
			Navigation.PushAsync(signinPage);
		}

		private bool checkInputValue() {
			if (txt_mail.Text == null || txt_mail.Text == "") {
				Navigation.PushPopupAsync(new AlertPopup("Warning", "Please enter your email.", "OK"));
				return false;
			}
			if (txt_pwd.Text == null || txt_pwd.Text == "") {
				Navigation.PushPopupAsync(new AlertPopup("Warning", "Please enter your password.", "OK"));
				return false;
			}
			return true;
		}
	}
}

