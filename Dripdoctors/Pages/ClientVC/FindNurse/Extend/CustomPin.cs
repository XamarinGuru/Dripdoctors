using System;
using Xamarin.Forms.Maps;
using Xamarin.Forms;
using System.Diagnostics;
namespace Dripdoctors
{
	public class CustomPin
	{
		public string Id { get;}
		public Pin Pin { get;}
		public string Url { get;}
		public event EventHandler Clicked;

		private CustomPin() {
			
		}

		public CustomPin(string id, Pin pin, string url) : this() {
			Pin = pin;
			Id = id;
			Url = url;
		}

		public void OnPinClicked()
		{
			Clicked(this, new EventArgs());
		}
	}
}
