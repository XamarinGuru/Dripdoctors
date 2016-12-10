using System;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System.Diagnostics;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Extensions;
namespace Dripdoctors
{
	public class LocationManager
	{
		public string city = "Los Angeles";
		public string position = "";
		public string state = "";
		public string postalCode = "";
		public string region;
		public bool regions = false;

		public bool isThread = false;
		private int timeStampValue = 300;
		private int threadIndex = 0;

		public double latitude = 0.0;
		public double longitude = 0.0;

		private APIManager apimanager;
		private IGeolocator locator = null;

		public LocationManager()
		{
			apimanager = new APIManager();
			locator = CrossGeolocator.Current;
			locator.DesiredAccuracy = 50;
			getLocation();
		}

		private async void getLocation()
		{
			try
			{
				if (!locator.IsGeolocationAvailable)
				{
					return;
				}
				if (!locator.IsGeolocationEnabled)
				{
					return;
				}

				var position = await locator.GetPositionAsync(timeoutMilliseconds: 10000);
				latitude = position.Latitude;
				longitude = position.Longitude;
				Debug.WriteLine(latitude +":"+ longitude);
				initCityName();
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}
		}

		public bool sendNurseLocation() 
		{
			if (!isThread)
				return false;
			threadIndex += 5;
			if (threadIndex  == timeStampValue) 
			{
				getLocation();
				threadIndex = 0;
				//var result = apimanager.sendNurseLocation(Singleton.sharedInstance().nurseId, latitude, longitude);
			}
			return true;
		}

		public void setThreadValues(bool thread, int timeValue = 300) {
			timeStampValue = timeValue;
			if (!isThread && thread) {
				Xamarin.Forms.Device.StartTimer(TimeSpan.FromSeconds(5), sendNurseLocation);
			}
			isThread = thread;
		}

		public async void initCityName() {
			var result = await apimanager.getGeoCode(longitude, latitude);
			if (!(result is string))
			{
				return;
			}
			position = (string)result;
			char[] splitChar = {','};
			if (position != null)
			{
				var strArr = position.Split(splitChar);
				if(strArr.Length > 2)
					city = strArr[strArr.Length - 2];
			}
		}
	}
}
