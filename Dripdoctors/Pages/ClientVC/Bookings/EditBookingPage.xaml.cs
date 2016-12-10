using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Dripdoctors
{
	public partial class EditBookingPage : ContentPage
	{
		Booking booking;
		public EditBookingPage()
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
			cancelButton.Clicked += (sender, e) => {
				Navigation.PopAsync();
			};
			updateButton.Clicked += OnUpdateButtonClicked;

			serviceInfoEditButton.Clicked += OnServiceInfoButtonClicked;
			bookingInfoEditButton.Clicked += OnBookingInfoButtonClicked;
			nurseInfoEditButton.Clicked += OnNurseInfoButtonClicked;
		}

		public EditBookingPage(Booking arg,bool editable = true) : this()
		{
			booking = arg;
			if (booking.serviceInfo != null)
			{
				serviceNameLabel.Text = booking.serviceInfo.category_name;
			}
			if (booking.service_id != null)
			{
				serviceImage.Source = ImageSource.FromUri(new Uri(booking.service_id.service_img));
				servicePriceLabel.Text = booking.service_id.service_name + " $" + booking.service_id.price;
			}


			dateLabel.Text = booking.created;
			bookingTypeLabel.Text = booking.booking_type;
			addressLabel.Text = booking.client_address;

			if (booking.nurseInfo != null)
			{
				nurseProfileImage.Source = ImageSource.FromUri(new Uri(booking.nurseInfo.img_url));
				nurseNameLabel.Text = booking.nurseInfo.fname;
				serviceCompletedLabel.Text = booking.nurseInfo.services_completed + " completed services";
			}

			serviceInfoEditButton.IsEnabled = editable;
			bookingInfoEditButton.IsEnabled = editable;
			nurseInfoEditButton.IsEnabled = editable;


		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();
		}

		private void OnUpdateButtonClicked(object sender, EventArgs e) { 
		
		}

		private void OnServiceInfoButtonClicked(object sender, EventArgs e) 
		{ 
		
		}

		private void OnBookingInfoButtonClicked(object sender, EventArgs e)
		{

		}

		private void OnNurseInfoButtonClicked(object sender, EventArgs e)
		{

		}
	}
}
