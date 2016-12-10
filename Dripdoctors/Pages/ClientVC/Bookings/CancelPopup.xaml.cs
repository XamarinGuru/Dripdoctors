using System;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;


namespace Dripdoctors
{
	public partial class CancelPopup : PopupPage
	{
		APIManager apiManager;
		string bookingId = null;
		public event EventHandler OnCancelBookingButtonClick;
		public CancelPopup()
		{
			InitializeComponent();
			closeButton.Clicked += OnCloseButtonClicked;
			cancelBookingButton.Clicked += OnCancelBookingButtonClicked;
		}

		public CancelPopup(string arg) : this() 
		{
			bookingId = arg;
			apiManager = new APIManager();
		}

		private void OnCloseButtonClicked(object sender, EventArgs e)
		{
			PopupNavigation.PopAsync();
		}

		private async void OnCancelBookingButtonClicked(object sender, EventArgs e)
		{
			if (OnCancelBookingButtonClick != null)
			{
				OnCancelBookingButtonClick(this, new EventArgs());
			}

			if (bookingId != null)
			{
				var result = await apiManager.cancelBooking(bookingId);
				if (result != "SUCCESS") {
					MessagingCenter.Send<CancelPopup, string>(this, "Hi", "John");
				}else
					await PopupNavigation.PopAsync();
			}
		}
	}
}
