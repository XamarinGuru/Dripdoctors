using System;
using System.Collections.Generic;
using Dripdoctors.Resx;
using Xamarin.Forms;

namespace Dripdoctors
{
	public partial class ServiceConfirmPage : ContentPage
	{
		ServiceItem serviceProduct;
		public ServiceConfirmPage()
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
			bookingButton.Clicked += OnSubmitButtonClicked;
			anotherButton.Clicked += OnAnotherButtonClicked;
			logoButton.Source = ImageSource.FromFile("drip_off.png");
		}

		public ServiceConfirmPage(ServiceItem arg) : this(){
			serviceProduct = arg;
			welcomeLabel1.Text = AppResources.ServiceWelcomeSentence1;
			welcomeLabel2.Text = AppResources.ServiceWelcomeSentence2;
			welcomeLabel2.HorizontalTextAlignment = TextAlignment.Center;
			serviceImage.Source = ImageSource.FromUri(new Uri(serviceProduct.service_img));
			userButton.Text = Singleton.sharedInstance().user.fname;
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();
		}

		public void OnSubmitButtonClicked(object sender, EventArgs e)
		{
			var pages = Navigation.NavigationStack;
			var page = pages[0];
			var main = (ClientMainPage)page;
			main.loadBody(3);
			Navigation.PopToRootAsync();
		}

		public void OnAnotherButtonClicked(object sender, EventArgs e) {
			var pages = Navigation.NavigationStack;
			var page = pages[0];
			var main = (ClientMainPage)page;
			main.loadBody(1);
			Navigation.PopToRootAsync();
		}
	}
}

