using System;

using Xamarin.Forms;

namespace Dripdoctors
{
	public class BaseCellView : ViewCell
	{
		protected string[] bookingStatus = { "Booking Pending", "Booking Confirmed", "Booking Declined", "Booking Completed", "Booking Removed" };
		public event EventHandler OnOptionButtonClicked;
		public event EventHandler OnTrackNurseButtonClicked;
		public event EventHandler OnEditBookingButtonClicked;
		public event EventHandler OnSendMessageButtonClicked;
		public event EventHandler OnCancelBookingButtonClicked;
		public event EventHandler OnApplyAgainButtonClicked;
		public event EventHandler OnRemoveBookingButtonClicked;
		public event EventHandler OnReportNurseButtonClicked;
		public event EventHandler OnRefundButtonClicked;

		public BaseCellView() : base(){
			
		}


		public void OptionClick()
		{
			if (OnOptionButtonClicked != null)
				OnOptionButtonClicked(this, new EventArgs());
		}

		public void TrackNurseClick()
		{
			if (OnTrackNurseButtonClicked != null)
				OnTrackNurseButtonClicked(this, new EventArgs());
		}
		public void EditBookingClick()
		{
			if (OnEditBookingButtonClicked != null)
				OnEditBookingButtonClicked(this, new EventArgs());
		}

		public void SendMessageClick()
		{
			if (OnSendMessageButtonClicked != null)
				OnSendMessageButtonClicked(this, new EventArgs());
		}

		public void CancelBookingClick()
		{
			if (OnCancelBookingButtonClicked != null)
				OnCancelBookingButtonClicked(this, new EventArgs());
		}

		public void ApplyBookingClick()
		{
			if (OnApplyAgainButtonClicked != null)
				OnApplyAgainButtonClicked(this, new EventArgs());
		}

		public void RemoveBookingClick()
		{
			if (OnRemoveBookingButtonClicked != null)
				OnRemoveBookingButtonClicked(this, new EventArgs());
		}

		public void ReportNurseClick()
		{
			if (OnReportNurseButtonClicked != null)
				OnReportNurseButtonClicked(this, new EventArgs());
		}

		public void RefundClick()
		{
			if (OnRefundButtonClicked != null)
				OnRefundButtonClicked(this, new EventArgs());
		}
	}
}

