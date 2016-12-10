using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms.Maps;
using Xamarin.Forms;

namespace Dripdoctors
{
	public partial class CallDirectionPage : ContentPage
	{
		TrackMap _map;
		Call selectedCall = null;
		CustomPin myPosition = null;
		double lat = 0.0, lon = 0.0;
		private CallDirectionPage()
		{
			InitializeComponent();
			_map = new TrackMap
			{
				MapType = MapType.Street,
				WidthRequest = 1000,
				HeightRequest = 1000
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
		}

		public CallDirectionPage(Call call) : this()
		{
			
			selectedCall = call;
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
				Xamarin.Forms.Device.StartTimer(TimeSpan.FromSeconds(5),trackNurse);
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

		protected override void OnAppearing()
		{
			base.OnAppearing();
		}

		protected override void OnDisappearing()
		{
			_map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(0, 0), Distance.FromMiles(5)));
			_map.Pins.Clear();
			mapLayout.Children.Clear();
			base.OnDisappearing();
		}
	}
}
