using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Dripdoctors
{
	public partial class NurseMainPage : BaseContentPage
	{
		public int pageIndex = 1;
		Button rightButton = null;
		APIManager apiManager = null;
		public NurseMainPage()
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
			apiManager = new APIManager();
			activeButton.Clicked += OnActiveButtonClicked;
			oncallButton.Clicked += OnCallButtonClicked;
			unavailableButton.Clicked += OnUnableButtonClicked;
			activeCallButton.Clicked += OnActiveCallButtonClicked;


			var serviceTapGestureRecognizer = new TapGestureRecognizer();
			serviceTapGestureRecognizer.Tapped += (sender, e) => { pageIndex = 1; titleLabel.Text = "Dashboard"; loadBody(); };
			dashboardButton.GestureRecognizers.Add(serviceTapGestureRecognizer);

			var nurseTapGestureRecognizer = new TapGestureRecognizer();
			nurseTapGestureRecognizer.Tapped += (sender, e) => { pageIndex = 2;titleLabel.Text = "Schedule"; loadBody(); };
			scheduleButton.GestureRecognizers.Add(nurseTapGestureRecognizer);

			var bookingTapGestureRecognizer = new TapGestureRecognizer();
			bookingTapGestureRecognizer.Tapped += (sender, e) => { pageIndex = 3;titleLabel.Text = "Requests"; loadBody(); };
			requestButton.GestureRecognizers.Add(bookingTapGestureRecognizer);

			var accountTapGestureRecognizer = new TapGestureRecognizer();
			accountTapGestureRecognizer.Tapped += (sender, e) => { pageIndex = 4;titleLabel.Text = "Account"; if (statusLayout.IsVisible) { statusLayout.IsVisible = false; } loadBody(); };
			accountButton.GestureRecognizers.Add(accountTapGestureRecognizer);


			if (Singleton.sharedInstance().nureseStatus == "Active")
			{
				activeButton.TextColor = Color.FromHex("#01b3f0");
			}
			else if (Singleton.sharedInstance().nureseStatus == "On Call")
			{
				oncallButton.TextColor = Color.FromHex("#01b3f0");
			}
			else { 
				unavailableButton.TextColor = Color.FromHex("#01b3f0");
			}
			loadBody();
		}

		public void loadBody()
		{
			bodyLayout.Children.Clear();
			if(rightButton != null)
				rightButton.Text = Singleton.sharedInstance().nureseStatus;
			dashboardButton.Source = "dashboard_normal.png";
			scheduleButton.Source = "schedule_normal.png";
			requestButton.Source = "request_normal.png";
			accountButton.Source = "account_normal.png";
			switch (pageIndex)
			{
				case 1:
					dashboardButton.Source = "dashboard_selected.png";
					var serviceView = new DashboardMainView(this);
					activeChild = serviceView;
					activeChildIndex = 1;
					OnChildViewSelectionChanged();
					bodyLayout.Children.Add(serviceView);
					break;
				case 2:
					scheduleButton.Source = "schedule_selected.png";
					var nurseView = new ScheduleMainView(this);
					activeChild = nurseView;
					activeChildIndex = 2;
					OnChildViewSelectionChanged();
					bodyLayout.Children.Add(nurseView);
					break;
				case 3:
					requestButton.Source = "request_selected.png";
					var bookingView = new RequestMainView(this);
					activeChild = bookingView;
					activeChildIndex = 3;
					OnChildViewSelectionChanged();
					bodyLayout.Children.Add(bookingView);
					break;
				case 4:
					accountButton.Source = "account_selected.png";
					var accountView = new AccountMainView(this);
					activeChild = accountView;
					activeChildIndex = 4;
					accountView.loadBody(1);
					bodyLayout.Children.Add(accountView);
				break;
				default:
					dashboardButton.Source = "dashboard_selected.png";
					var dflt = new DashboardMainView(this);
					activeChild = dflt;
					activeChildIndex = 1;
					OnChildViewSelectionChanged();
					bodyLayout.Children.Add(dflt);
					break;
			}
		}

		private async void OnActiveButtonClicked(object sender, EventArgs e)
		{
			if (Singleton.sharedInstance().nureseStatus == "On Call")
			{
				await DisplayAlert("Warning", "You can only change to an On Call status choosing a scheduled call.", "OK");
				return;
			}
			statusLayout.IsVisible = !statusLayout.IsVisible;
			if (Singleton.sharedInstance().nureseStatus == "Active") return;

			var result = await apiManager.sendNurseStatus(Singleton.sharedInstance().nurseId,2);
			if (((string)result).Contains("success"))
			{
				Singleton.sharedInstance().locationManager.setThreadValues(true, 5);
				Singleton.sharedInstance().nureseStatus = "Active";
				rightButton.Text = "Active";
				activeButton.TextColor = Color.FromHex("#01b3f0");
				oncallButton.TextColor = Color.FromHex("#000");
				unavailableButton.TextColor = Color.FromHex("#000");
			}
		}

		private void OnCallButtonClicked(object sender, EventArgs e)
		{
			DisplayAlert("Warning", "You can only change to an On Call status choosing a scheduled call.", "OK");
			return;
		}

		private async void OnUnableButtonClicked(object sender, EventArgs e)
		{
			if (Singleton.sharedInstance().nureseStatus == "On Call")
			{
				await DisplayAlert("Warning", "You can only change to an On Call status choosing a scheduled call.", "OK");
				return;
			}

			statusLayout.IsVisible = !statusLayout.IsVisible;
			if (Singleton.sharedInstance().nureseStatus == "Unavailable") return;

			var result = await apiManager.sendNurseStatus(Singleton.sharedInstance().nurseId, 1);
			if (((string)result).Contains("success")) { 
				Singleton.sharedInstance().locationManager.setThreadValues(false);
				Singleton.sharedInstance().nureseStatus = "Unavailable";
				rightButton.Text = "Unavailable";
				unavailableButton.TextColor = Color.FromHex("#01b3f0");
				oncallButton.TextColor = Color.FromHex("#000");
				activeButton.TextColor = Color.FromHex("#000");
			}
		}

		private void OnActiveCallButtonClicked(object sender, EventArgs e) 
		{
			if (Singleton.sharedInstance().currentActiveCall == null)
			{
				DisplayAlert("Warning", "You have no Active Call.", "OK");
			}
			else {
				Navigation.PushAsync(new CallDetailPage(Singleton.sharedInstance().currentActiveCall));
				activeStatusLayout.IsVisible = false;
			}
		}

		public override Object RightNavButton()
		{
			if (pageIndex != 4)
			{
				return new CustomButton
				{
					TextColor = Color.White,
					HorizontalOptions = LayoutOptions.EndAndExpand,
					VerticalOptions = LayoutOptions.FillAndExpand,
					FontSize = 12,
					Text = Singleton.sharedInstance().nureseStatus
				};
			}
			else
			{
				return null;
			}
		}

		public override bool OnRightNavButton_Clicked()
		{
			if (pageIndex == 4) { 
				if (statusLayout.IsVisible)
					statusLayout.IsVisible = false;
				return true;
			}

			if (Singleton.sharedInstance().nureseStatus == "On Call")
			{
				activeStatusLayout.IsVisible = !activeStatusLayout.IsVisible;
			}
			else {
				statusLayout.IsVisible = !statusLayout.IsVisible;
			}
			return false;
		}

		public override void OnContentViewSelectionChanged()
		{
			Object navRightButton = GetRightNavButton();
			if (navRightButton != null)
			{
				rightButton = (Button)navRightButton;
				logoGrid.Children.RemoveAt(2);
				logoGrid.Children.Add(rightButton, 2, 0);
				rightButton.Clicked += OnRightNavButton;
			}
		}
	}
}