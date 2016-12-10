using System;
using Xamarin.Forms;

namespace Dripdoctors
{
	public partial class CellView : BaseCellView
	{
		public CellView() : base()
		{
			InitializeComponent();
			optionButton.Clicked +=  OnOptionClicked;
			trackNurseButton.Clicked += OnTrackNurseClicked;
			editBookingButton.Clicked += OnEditBookingClicked;
			sendMeessageButton.Clicked += OnSendMessageClicked;
			cancelBookingButton.Clicked += OnCancelBookingClicked;
			sendMeessageButton1.Clicked += OnSendMessageClicked;
			cancelBookingButton1.Clicked += OnCancelBookingClicked;
			applyButton.Clicked += OnApplyClicked;
			removeButton.Clicked += OnRemoveClicked;
			Xamarin.Forms.Device.StartTimer(TimeSpan.FromSeconds(.01), OnTimer);
		}

		private bool OnTimer()
		{
			var booking = (Booking)BindingContext;
			if (booking != null)
			{
				bookingStateLabel.Text = bookingStatus[booking.status - 1];
				dateLabel.Text = Functions.getDateFormatByString(booking.booking_date) + " " + booking.booking_time;
				servicePriceLabel.Text = "$" + booking.service_id.price;
			}
			return false;
		}

		//public void setServieIcon(Image img) {
		//	serviceImage = img;
		//}

		//public void setNurseIcon(Image img) {
		//	nurseImage = img;
		//}

		public void showPopup(int index) { 
			switch (index)
			{
				case 1:
					popupLayout1.IsVisible = !popupLayout1.IsVisible;
					break;
				case 2:
					popupLayout3.IsVisible = !popupLayout3.IsVisible;
					break;
				case 3:
					popupLayout3.IsVisible = !popupLayout3.IsVisible;
					break;
				case 4:
					popupLayout2.IsVisible = !popupLayout2.IsVisible;
					break;
				default:
					break;
			}
		}

		private void OnOptionClicked(object sender, EventArgs e)
		{
			OptionClick();
		}

		private void OnTrackNurseClicked(object sender, EventArgs e)
		{
			TrackNurseClick();
		}

		private void OnEditBookingClicked(object sender, EventArgs e) 
		{
			EditBookingClick();
		}

		private void OnSendMessageClicked(object sender, EventArgs e)
		{
			SendMessageClick();
		}

		private void OnCancelBookingClicked(object sender, EventArgs e)
		{
			CancelBookingClick();
		}

		private void OnApplyClicked(object sender, EventArgs e) 
		{
			ApplyBookingClick();
		}

		private void OnRemoveClicked(object sender, EventArgs e) 
		{
			RemoveBookingClick();
		}
	}
}
