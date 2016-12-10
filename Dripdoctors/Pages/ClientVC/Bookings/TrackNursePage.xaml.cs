using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Extensions;
using System.Diagnostics;

namespace Dripdoctors
{
	public partial class TrackNursePage : ContentPage
	{
		APIManager apiManager;
		Booking booking;
		TrackMap _map;
		public static List<Nurse> nurses;
		public TrackNursePage()
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
			_map = new TrackMap
			{
				MapType = MapType.Street,
				WidthRequest = 500,
				HeightRequest = 500
			};

			mapLayout.Children.Add(_map);
			Pin myLocation = new Pin
			{
				Position = new Position(Singleton.sharedInstance().locationManager.latitude, Singleton.sharedInstance().locationManager.longitude),
				Label = " " + Singleton.sharedInstance().user.fname
			};
			_map.Pins.Add(myLocation);
			_map.addPosition(Singleton.sharedInstance().user.userid+"", new Position(Singleton.sharedInstance().locationManager.latitude, Singleton.sharedInstance().locationManager.longitude));
			backButton.Clicked += (sender, e) => {
				_map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(0, 0), Distance.FromMiles(5)));
				_map.Pins.Clear();
				mapLayout.Children.Clear();
				Navigation.PopAsync();
			};
			apiManager = new APIManager();
			initPins();
		}

		public TrackNursePage(Booking arg) : this()
		{
			booking = arg;
			Xamarin.Forms.Device.StartTimer(TimeSpan.FromSeconds(5), OnTimer);
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			_map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(36.114823, -115.172695), Distance.FromMiles(5)));
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();
		}
		private bool OnTimer()
		{
			trackNurse();
			return true;
		}

		public static async Task<bool> getNurses(string id) { 
			
			var result = await new APIManager().getNurseAvailable4Booking(id);
			if ((result is List<Nurse>))
			{
				nurses = (List<Nurse>)result;
				return true;
			}
			else {
				return false;
			}
		}

		private void initPins() {
			if (nurses == null)
				return;

			var pinList = new List<CustomPin>();
			foreach (Nurse item in nurses)
			{
				var pin = new CustomPin(
					item.nurse_id,
					new Pin
					{
						Address = item.sname,
						Label = item.fname,
						Type = PinType.Generic,
						Position = new Position(item.last_latitud, item.last_longitud)
					},
					item.img_url
				);
				_map.Pins.Add(pin.Pin);
				pinList.Add(pin);
				_map.addPosition(item.nurse_id, new Position(item.last_latitud, item.last_longitud));
			}
			_map.CustomPins = pinList;
		}

		private async void trackNurse() 
		{
			var result = await apiManager.getNurseAvailable4Booking(booking.booking_id);
			if (!(result is List<Nurse>))
			{
				return;
			}
			var nurses = (List<Nurse>)result;
			if (nurses == null) return;

			foreach (Nurse item in nurses) {
				if (findPin(item.nurse_id) != null)
				{
					var pin = findPin(item.nurse_id);
					pin.Pin.Position = new Position(item.last_latitud, item.last_longitud);
				}
				else {
					var pin = new CustomPin(
						item.nurse_id,
						new Pin
						{
							Address = item.sname,
							Label = item.fname,
							Type = PinType.Generic,
							Position = new Position(item.last_latitud, item.last_longitud)
						},
						item.img_url
					);
					_map.CustomPins.Add(pin);
					_map.Pins.Add(pin.Pin);
				}
				_map.addPosition(item.nurse_id, new Position(item.last_latitud, item.last_longitud));
			}
		}

		private CustomPin findPin(string id) {
			if (_map.CustomPins.Count <= 0) return null;
			foreach (CustomPin pin in _map.CustomPins) {
				if (pin.Id == id)
					return pin;
			}
			return null;
		}
	}
}
