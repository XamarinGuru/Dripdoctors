using System;
using Xamarin.Forms;
using Rg.Plugins.Popup.Extensions;

namespace Dripdoctors
{
	public partial class SignUpPage : ContentPage
	{
		APIManager apiManager;
		public SignUpPage()
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
			apiManager = new APIManager();
			btnSignin.Clicked += OnSignInClicked;
			btnSignup.Clicked += OnSignUpClicked;
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

		void OnSignInClicked(object sender, EventArgs e)
		{
			Navigation.PopAsync();
		}

		async void OnSignUpClicked(object sender, EventArgs e)
		{
			if (checkInputValue())
			{
				string mail = txt_mail.Text;
				string pwd = txt_pwd.Text;
				var result = await apiManager.signUpAsync(mail, pwd);
				if (result is User)
				{
					Singleton.sharedInstance().user = (User)result;
					await Navigation.PopAsync();
				}
				else {
					await Navigation.PushPopupAsync(new AlertPopup("Warning", (string)result, "OK"));
				}
			}
		}

		private bool checkInputValue() {
			if (txt_mail.Text == null || txt_mail.Text == "") {
				Navigation.PushPopupAsync(new AlertPopup("Warning", "Please enter your email.", "OK"));
				return false;
			}

			if (txt_pwd.Text == null || txt_pwd.Text == "") {
				Navigation.PushPopupAsync(new AlertPopup("Warning", "Please enter your password", "OK"));
				return false;
			}
			if (!txt_pwd.Text.Equals(txt_cpwd.Text)){
				Navigation.PushPopupAsync(new AlertPopup("Warning", "These passwords don't match. Try again", "OK"));
				return false;
			}
			return true;
		}
	}
}

