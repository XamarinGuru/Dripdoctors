using System;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace Dripdoctors
{
	public partial class ConfirmedCellView : BaseCell
	{
		private Booking booking;
		private String[] bookingStatus = { "Booking Confirmed", "Pending Confirmation", "Booking Declined" };
		public ConfirmedCellView()
		{
			InitializeComponent();

			optionButton.Clicked +=  OnOptionClicked;
			trackNurseButton.Clicked += OnTrackNurseClicked;
			sendMeessageButton.Clicked += OnSendMessageClicked;
			cancelBookingButton.Clicked += OnCancelBookingClicked;
		}

		public ConfirmedCellView(Booking arg) : this()
		{
			booking = arg;
			if (booking == null) return;
			serviceImage.Source = ImageSource.FromUri(new Uri(booking.service_id.service_img));
			if (booking.serviceInfo != null)
			{
				serviceNameLabel.Text = booking.serviceInfo.category_name;
			}
			if (booking.service_id != null)
			{
				servicePriceLabel.Text = booking.service_id.service_name + " $" + booking.service_id.price;
			}

			dateLabel.Text = Functions.getDateFormatByString(booking.booking_date) + ", " + booking.booking_time;
			bookingTypeLabel.Text = booking.booking_type;
			addressLabel.Text = booking.client_address;
			bookingStateLabel.Text = bookingStatus[booking.status - 1];
			if (booking.nurseInfo != null)
			{
				nurseImage.Source = ImageSource.FromUri(new Uri(booking.nurseInfo.img_url));
				nurseNameLabel.Text = booking.nurseInfo.fname;
			}
		}

		public void OnOptionClicked(object sender, EventArgs e)
		{
			popupLayout.IsVisible = !popupLayout.IsVisible;
		}

		public void OnTrackNurseClicked(object sender, EventArgs e)
		{
			Navigation.PushModalAsync(new TrackNursePage(booking.nurseId));
		}

		public void OnSendMessageClicked(object sender, EventArgs e)
		{
			
		}

		public void OnCancelBookingClicked(object sender, EventArgs e)
		{
			Navigation.PushPopupAsync(new CancelPopup(booking.booking_id));
		}
	}
}
