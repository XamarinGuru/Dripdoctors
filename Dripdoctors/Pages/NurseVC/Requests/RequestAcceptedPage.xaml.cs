using System;
using System.Collections.Generic;
using Dripdoctors.Resx;

using Xamarin.Forms;

namespace Dripdoctors
{
	public partial class RequestAcceptedPage : ContentPage
	{
		Call selectedCall = null;
		private RequestAcceptedPage()
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
			backButton.Clicked += (sender, e) => { Navigation.PopAsync();};
			rightButton.Clicked += (sender, e) => { };
			acceptedLabel1.Text = AppResources.CallAcceptSentence1;
			acceptedLabel2.Text = AppResources.CallAcceptSentence2;
			rightButton.Clicked += (sender, e) => {
				var pages = Navigation.NavigationStack;
				var page = pages[0];
				var main = (NurseMainPage)page;
				main.pageIndex = 2;
				main.loadBody();
				Navigation.PopToRootAsync();
			};
			scheduleButton.Clicked += (sender, e) => { 
				var pages = Navigation.NavigationStack;
				var page = pages[0];
				var main = (NurseMainPage)page;
				main.pageIndex = 2;
				main.loadBody();
				Navigation.PopToRootAsync();
			};
			dashboardButton.Clicked += (sender, e) => { 
				var pages = Navigation.NavigationStack;
				var page = pages[0];
				var main = (NurseMainPage)page;
				main.pageIndex = 1;
				main.loadBody();
				Navigation.PopToRootAsync();
			};
		}

		public RequestAcceptedPage(Call call) : this() {
			selectedCall = call;
			serviceImage.Source = ImageSource.FromUri(new Uri(selectedCall.serviceInfo.service_img_icon));
			categoryNameLabel.Text = selectedCall.serviceInfo.category.category_name;
			productNameLabel.Text = selectedCall.serviceInfo.service_name;
			Xamarin.Forms.Device.StartTimer(TimeSpan.FromSeconds(0.02), OnTimer);
		}

		private bool OnTimer()
		{
			backButton.TextColor = Color.White;
			rightButton.TextColor = Color.White;
			return false;
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
		}
	}
}
