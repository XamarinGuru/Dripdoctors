using System;
using System.Collections.Generic;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace Dripdoctors
{
	public partial class BookingHistoryPage : ContentPage
	{
		List<Booking> bookings;
		APIManager apiManager;

		public BookingHistoryPage()
		{
			InitializeComponent();
			apiManager = new APIManager();
			backButton.Clicked += (sender, e) => {
				Navigation.PopModalAsync();
			};
			loadBooking();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			NavigationPage.SetHasNavigationBar(this, false);
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();
		}

		private async void loadBooking()
		{
			var result = await apiManager.getBookingItems(Singleton.sharedInstance().user.userid);
			if (!(result is List<Booking>))
			{
				await Navigation.PushPopupAsync(new AlertPopup("Warning", (string)result, "OK"));
				return;
			}
			bookings = (List<Booking>)result;
			updateBody();
		}

		private void updateBody()
		{
			bodyLayout.Children.Clear();
			if (bookings == null)
			{
				return;
			}
			foreach (Booking item in bookings)
			{
				BaseCellView cell = new BaseCellView();
			}
		}

		private void OnOptionClicked(object sender, EventArgs e)
		{
			var cell = (BaseCellView)sender;
		}

		private void OnApplayBookingClicked(object sender, EventArgs e)
		{
			var cell = (BaseCellView)sender;
			//Navigation.PushModalAsync(new EditBookingPage(cell.booking));
		}

		private void OnTrakNurseClicked(object sender, EventArgs e)
		{
			var cell = (BaseCellView)sender;
			//Navigation.PushModalAsync(new TrackNursePage(cell.booking));
		}

		private void OnEditBookingClicked(object sender, EventArgs e)
		{
			var cell = (BaseCellView)sender;
			//Navigation.PushModalAsync(new EditBookingPage(cell.booking));
		}

		private void OnSendMessageClicked(object sender, EventArgs e)
		{
			var cell = (BaseCellView)sender;

		}

		private void OnCancelBookingClicked(object sender, EventArgs e)
		{
			var cell = (BaseCellView)sender;
			//Navigation.PushPopupAsync(new CancelPopup(cell.booking.booking_id));
		}

		private async void OnRemoveBookingClicked(object sender, EventArgs e)
		{
			var cell = (BaseCellView)sender;
			//var result = await apiManager.removeBooking(cell.booking.booking_id);
			//if (result == "SUCCESS")
			//{
			//	bookings.Remove(findBookingById(cell.booking.booking_id));
			//	updateBody();
			//}
			//else {

			//}
		}

		private Booking findBookingById(String Id) {
			foreach (Booking item in bookings) {
				if (item.booking_id == Id) {
					return item;
				}
			}
			return null;
		}
	}
}
