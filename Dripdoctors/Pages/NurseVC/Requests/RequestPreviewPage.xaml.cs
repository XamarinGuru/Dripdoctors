using System;
using System.Collections.Generic;
using Xamarin.Forms.Maps;
using Xamarin.Forms;

namespace Dripdoctors
{
	public partial class RequestPreviewPage : ContentPage
	{
		TrackMap _map;
		Call selectedCall = null;
		APIManager apiManager = null;
		CustomPin myPosition = null;
		double lat = 0.0, lon = 0.0;
		private RequestPreviewPage()
		{
			InitializeComponent();
			apiManager = new APIManager();
			NavigationPage.SetHasNavigationBar(this, false);
			_map = new TrackMap
			{
				MapType = MapType.Street
			};
			mapLayout.Children.Add(_map);
			if (!Singleton.sharedInstance().locationManager.latitude.Equals(0) && !Singleton.sharedInstance().locationManager.longitude.Equals(0))
			{
				myPosition = new CustomPin
				(
					Singleton.sharedInstance().user.userid + "",
					new Pin
					{
						Label = Singleton.sharedInstance().user.fname,
						Address = Singleton.sharedInstance().user.address,
						Position = new Position(Singleton.sharedInstance().locationManager.latitude, Singleton.sharedInstance().locationManager.longitude)
					},
					Singleton.sharedInstance().user.img_url
				);
				_map.Pins.Add(myPosition.Pin);
				_map.CustomPins.Add(myPosition);
				_map.addPosition(Singleton.sharedInstance().user.userid + "", new Position(Singleton.sharedInstance().locationManager.latitude, Singleton.sharedInstance().locationManager.longitude));
				_map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(Singleton.sharedInstance().locationManager.latitude, Singleton.sharedInstance().locationManager.longitude), Distance.FromMiles(500)));
			}
			rightButton.Clicked += (sender, e) => { 
				var pages = Navigation.NavigationStack;
				var page = pages[0];
				var main = (NurseMainPage)page;
				main.pageIndex = 2;
				main.loadBody();
				Navigation.PopToRootAsync();
			};
			backButton.Clicked += (sender, e) => { 
				_map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(0, 0), Distance.FromMiles(5)));
				_map.Pins.Clear();
				mapLayout.Children.Clear();
				Navigation.PopAsync();
			};
		}

		public RequestPreviewPage(Call call) : this()
		{
			selectedCall = call;
			string[] titleArr = { "Request Details", "Call Missed", "Call Declined", "Call Scheduled", "Call Active", "Call Completed", "Call Cancelled" };
			titleLabel.Text = titleArr[selectedCall.callstep_id - 1];
			if (!selectedCall.gps_latitud.Equals(0) && !selectedCall.gps_longitud.Equals(0))
			{
				CustomPin clientPosition = new CustomPin
				(
					selectedCall.clientInfo.userid + "",
					new Pin
					{
						Label = selectedCall.clientInfo.fname,
						Address = selectedCall.clientInfo.address,
						Position = new Position(selectedCall.gps_latitud, selectedCall.gps_longitud)
					},
					selectedCall.clientInfo.img_url
				);
				_map.Pins.Add(clientPosition.Pin);
				_map.CustomPins.Add(clientPosition);
				_map.addPosition(selectedCall.clientInfo.userid + "", new Position(selectedCall.gps_latitud, selectedCall.gps_longitud));
			}

			if (Singleton.sharedInstance().nureseStatus == "On Call")
				Xamarin.Forms.Device.StartTimer(TimeSpan.FromSeconds(5), trackNurse);
			
			profileImage.Source = ImageSource.FromUri(new Uri(selectedCall.clientInfo.img_url));
			clientNameLabel.Text = selectedCall.clientInfo.fname + " " + selectedCall.clientInfo.sname;
			clientPhoneLabel.Text = selectedCall.clientInfo.mobilenum;
			callTypeLabel.Text = selectedCall.booking_type.ToUpper() + " CALL";
			if (selectedCall.callstep_id != 1)
			{
				string[] callstatuses = { "requests", "missed", "declined", "scheduled", "active", "completed", "cancelled" };
				statusLabel.Text = callstatuses[selectedCall.callstep_id -1];
			}
			else 
			{ 
				statusLabel.Text = Functions.getExpireTime(selectedCall.booking_date+" "+selectedCall.booking_time);
			}

			addressLabel.Text = selectedCall.clientInfo.address + " " + selectedCall.clientInfo.city + ", " + selectedCall.clientInfo.state + " " + selectedCall.clientInfo.zip;
			serviceImage.Source = ImageSource.FromUri(new Uri(selectedCall.serviceInfo.service_img_icon));
			categoryNameLabel.Text = selectedCall.serviceInfo.category.category_name;
			productNameLabel.Text = selectedCall.serviceInfo.service_name;
		}

		private bool trackNurse()
		{
			if (lat == Singleton.sharedInstance().locationManager.latitude && lon == Singleton.sharedInstance().locationManager.longitude)
				return true;
			lat = Singleton.sharedInstance().locationManager.latitude;
			lon = Singleton.sharedInstance().locationManager.longitude;
			if (myPosition != null)
			{
				myPosition.Pin.Position = new Position(lat, lon);
			}
			else {
				myPosition = new CustomPin
				(
					Singleton.sharedInstance().user.userid + "",
					new Pin
					{
						Label = Singleton.sharedInstance().user.fname,
						Address = Singleton.sharedInstance().user.address,
						Position = new Position(lat, lon)
					},
					Singleton.sharedInstance().user.img_url
				);
				_map.Pins.Add(myPosition.Pin);
				_map.CustomPins.Add(myPosition);
			}

			_map.addPosition(Singleton.sharedInstance().user.userid + "", new Position(lat, lon));
			return true;
		}

		private async void OnAccept(object sender, EventArgs e)
		{
			selectedCall.callstep_id = 4;
			var result = await apiManager.updateCallStatus(selectedCall.nurse_id, selectedCall.call_id, selectedCall.callstep_id);
			if (((string)result).Contains("success"))
			{
				await Navigation.PushAsync(new RequestAcceptedPage(selectedCall));
			}
			else
			{
				await App.Current.MainPage.DisplayAlert("Warning", result + "", "OK");
			}
		}

		private async void OnDecline(object sender, EventArgs e)
		{
			selectedCall.callstep_id = 3;
			var result = await apiManager.updateCallStatus(selectedCall.nurse_id, selectedCall.call_id, selectedCall.callstep_id);
			if (((string)result).Contains("success"))
			{
				var pages = Navigation.NavigationStack;
				var page = pages[0];
				var main = (NurseMainPage)page;
				main.pageIndex = 3;
				main.loadBody();
				await Navigation.PopToRootAsync();
			}
			else
			{
				await App.Current.MainPage.DisplayAlert("Warning", result + "", "OK");
			}
		}
	}
}
