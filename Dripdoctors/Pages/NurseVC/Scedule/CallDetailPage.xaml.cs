using System;
using System.Collections.Generic;
using Xamarin.Forms.Maps;
using Xamarin.Forms;
using Plugin.Messaging;
using Rg.Plugins.Popup.Extensions;
namespace Dripdoctors
{
	public partial class CallDetailPage : ContentPage
	{
		int pageIndex = 1;
		Call selectedCall = null;
		APIManager apiManager;

		private CallDetailPage()
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
			apiManager = new APIManager();
			backButton.Clicked += (sender, e) => {
				var pages = Navigation.NavigationStack;
				var page = pages[0];
				var main = (NurseMainPage)page;
				main.pageIndex = 1;
				main.loadBody();
				Navigation.PopToRootAsync();
			};
			rightButton.Clicked += (sender, e) => { 
				Navigation.PushAsync(new CallDirectionPage(selectedCall));
			};
			activeButton.Clicked += (sender, e) => { 
				pageIndex = 1;
				if (selectedCall.callstep_id == 4)
				{
					updateBody();
				}
				else if (selectedCall.callstep_id == 5){
					updateCall(6);
				}

			};
			contactButton.Clicked += (sender, e) => { pageIndex = 2; updateBody();};
			cancelButton.Clicked += (sender, e) => { pageIndex = 3; updateBody();};
		}

		public CallDetailPage(Call call) : this()
		{ 
			selectedCall = call;
			string[] titleArr = { "Request Details", "Call Missed", "Call Declined", "Call Scheduled", "Call Active", "Call Completed", "Call Cancelled" };
			titleLabel.Text = titleArr[selectedCall.callstep_id - 1];
			if (selectedCall.callstep_id == 5) pageIndex = 2;
			profileImage.Source = ImageSource.FromUri(new Uri(selectedCall.clientInfo.img_url));
			clientNameLabel.Text = selectedCall.clientInfo.fname + " " + selectedCall.clientInfo.sname;
			clientPhoneLabel.Text = selectedCall.clientInfo.mobilenum;
			callTypeLabel.Text = selectedCall.booking_type.ToUpper() + " CALL";
			if (selectedCall.callstep_id != 1)
			{
				string[] callstatuses = { "requests", "missed", "declined", "scheduled", "active", "completed", "cancelled" };
				statusLabel.Text = callstatuses[selectedCall.callstep_id - 1];
			}
			else
			{
				statusLabel.Text = Functions.getExpireTime(selectedCall.booking_date + " " + selectedCall.booking_time);
			}

			addressLabel.Text = selectedCall.clientInfo.address + " "+ selectedCall.clientInfo.city + ", " + selectedCall.clientInfo.state + " " + selectedCall.clientInfo.zip;
			serviceImage.Source = ImageSource.FromUri(new Uri(selectedCall.serviceInfo.service_img_icon));
			categoryNameLabel.Text = selectedCall.serviceInfo.category.category_name;
			productNameLabel.Text = selectedCall.serviceInfo.service_name;
			if (selectedCall.callstep_id == 5) {
				activeButton.Text = "COMPLETE";
			}
			Xamarin.Forms.Device.StartTimer(TimeSpan.FromSeconds(0.02), OnTimer);
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
		}

		private bool OnTimer()
		{
			if(selectedCall.callstep_id == 4 || selectedCall.callstep_id == 5)
				updateBody();
			else
				buttonLayout.IsVisible = false;
			return false;
		}
		private void updateBody()
		{
			bodyLayout.Children.Clear();
			activeButton.BackgroundColor = Color.FromHex("#c2c2c2");
			contactButton.BackgroundColor = Color.FromHex("#c2c2c2");
			cancelButton.BackgroundColor = Color.FromHex("#c2c2c2");
			switch (pageIndex)
			{
				case 1:
					activeButton.BackgroundColor = Color.FromHex("#01b3f0");
					if (selectedCall.callstep_id == 4)
					{
						var activeView = new ActiveView();
						activeView.OnActive += (sender, e) => { updateCall(5); };
						bodyLayout.Children.Add(activeView);
					}
					break;
				case 2:
					contactButton.BackgroundColor = Color.FromHex("#01b3f0");
					var contentView = new ContactView();
					contentView.OnCall += (sender, e) => { sendCall();};
					contentView.OnMessage += (sender, e) => { sendMessage();};
					bodyLayout.Children.Add(contentView);
					break;
				default:
					cancelButton.BackgroundColor = Color.FromHex("#01b3f0");
					var cancelView = new CancelView();
					cancelView.OnCancel += (sender, e) => { updateCall(7);};
					bodyLayout.Children.Add(cancelView);
					break;
			}
		}

		public async void updateCall(int status) 
		{
			var result = await apiManager.updateCallStatus(selectedCall.nurse_id, selectedCall.call_id, status);
			if (((string)result).Contains("success"))
			{
				if (status == 5)
				{
					var response = await apiManager.sendNurseStatus(selectedCall.nurse_id, 3);
					if (((string)response).Contains("success"))
					{
						Singleton.sharedInstance().nureseStatus = "On Call";
						Singleton.sharedInstance().locationManager.setThreadValues(true, 5);
						var pages = Navigation.NavigationStack;
						var page = pages[0];
						var main = (NurseMainPage)page;
						main.pageIndex = 1;
						main.loadBody();
						await Navigation.PopToRootAsync();
					}
				}
				else if (status == 6)
				{
					var response = await apiManager.sendNurseStatus(selectedCall.nurse_id, 2);
					if (((string)response).Contains("success"))
					{
						Singleton.sharedInstance().locationManager.setThreadValues(true, 300);
						Singleton.sharedInstance().nureseStatus = "Active";
						await Navigation.PushAsync(new CallCompletePage(selectedCall));
					}
				}
				else 
				{ 
					var response = await apiManager.sendNurseStatus(selectedCall.nurse_id, 2);
					if (((string)response).Contains("success"))
					{
						Singleton.sharedInstance().nureseStatus = "Active";
						var pages = Navigation.NavigationStack;
						var page = pages[0];
						var main = (NurseMainPage)page;
						main.pageIndex = 1;
						main.loadBody();
						await Navigation.PopToRootAsync();
					}
				}
			}
			else
			{
				await App.Current.MainPage.DisplayAlert("Warning", result + "", "OK");
			}
		}

		private void sendMessage()
		{
			var SmsTask = MessagingPlugin.SmsMessenger;
			if (SmsTask.CanSendSms)
				SmsTask.SendSms(selectedCall.clientInfo.mobilenum,"Hello");
		}

		private void sendCall()
		{
			if (selectedCall.clientInfo != null)
			{
				if (selectedCall.clientInfo.mobilenum != null && selectedCall.clientInfo.mobilenum != "")
				{
					var PhoneCallTask = MessagingPlugin.PhoneDialer;
					if (PhoneCallTask.CanMakePhoneCall)
						PhoneCallTask.MakePhoneCall(selectedCall.clientInfo.mobilenum);
				}
				else 
				{
					App.Current.MainPage.DisplayAlert("Warning","Don't exit phone number.","OK");
				}
			}
		}
	}
}
