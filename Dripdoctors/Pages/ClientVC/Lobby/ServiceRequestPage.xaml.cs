using System;
using System.Diagnostics;
using System.Collections.Generic;
using Xamarin.Forms;
using PayPal.Forms;
using PayPal.Forms.Abstractions;
using PayPal.Forms.Abstractions.Enum;
using Rg.Plugins.Popup.Extensions;
using System.Threading.Tasks;


namespace Dripdoctors
{
	public partial class ServiceRequestPage : ContentPage
	{
		private bool timeFlag = false;
		private bool locationFlag = false;
		private APIManager apiManager = null;
		private Dictionary<string, string> clinicItems = null;
		private string clinicId = null;
		private ServiceItem serviceProduct = null;
		private bool isPayClinicClicked = true;
		private int selectedTapIndex = 1;
		public ServiceRequestPage()
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
			backButton.Clicked += (sender, e) =>
			{
				Navigation.PopAsync();
			};

			nextButton.Clicked += OnNextButtonClicked;
			//clinicButton.Clicked += OnClinicButtonClicked;
			//houseButton.Clicked += OnHouseButtonClicked;
			submitButton.Clicked += OnSubmitButtonClicked;
			cancelButton.Clicked += OnCancelButtonClicked;

			var locationTapGestureRecognizer = new TapGestureRecognizer();
			locationTapGestureRecognizer.Tapped += OnLocationCheckButtonClicked;
			addressCheckButton.GestureRecognizers.Add(locationTapGestureRecognizer);
			locationCheckButton.GestureRecognizers.Add(locationTapGestureRecognizer);


			var timeTapGestureRecognizer = new TapGestureRecognizer();
			timeTapGestureRecognizer.Tapped += OnTimeCheckButtonClicked;
			timeCheckButton_c.GestureRecognizers.Add(timeTapGestureRecognizer);
			timeCheckButton_h.GestureRecognizers.Add(timeTapGestureRecognizer);
			soonestCheckButton_c.GestureRecognizers.Add(timeTapGestureRecognizer);
			soonestCheckButton_h.GestureRecognizers.Add(timeTapGestureRecognizer);

			payNowButton.Clicked += OnAdditionPayButtonClicked;
			payClinicButton.Clicked += OnAdditionPayButtonClicked;

			locationCheckButton.Source = ImageSource.FromFile("check_button.png");
			addressCheckButton.Source = ImageSource.FromFile("addressCheckButton");

			locationIconImage.Source = ImageSource.FromFile("pin_icon.png");
			addressIconImage.Source = ImageSource.FromFile("pin_icon.png");
			addressCheckButton.Source = ImageSource.FromFile("uncheck_button.png");
			ellipeIconImage.Source = ImageSource.FromFile("ellipe_icon.png");
			timeIconImage.Source = ImageSource.FromFile("time_icon.png");
			cPinIconImage.Source = ImageSource.FromFile("pin_icon.png");
			cEllipeIconImage.Source = ImageSource.FromFile("ellipe_icon.png");
			soonestCheckButton_h.Source = ImageSource.FromFile("check_button.png");
			soonestCheckButton_c.Source = ImageSource.FromFile("check_button.png");
			timeCheckButton_h.Source = ImageSource.FromFile("uncheck_button.png");
			timeCheckButton_c.Source = ImageSource.FromFile("uncheck_button.png");
			Xamarin.Forms.Device.StartTimer(TimeSpan.FromSeconds(.02), OnTimer);
			clinicItems = new Dictionary<string, string>();
			apiManager = new APIManager();	

		}

		public ServiceRequestPage(ServiceItem arg1, int arg2) : this()
		{
			topProductNameLabel.Text = arg1.service_name;
			bodyProductImage.Source = ImageSource.FromUri(new Uri(arg1.service_img_icon));
			bodyProductNameLabel.Text = arg1.service_name;
			bodyProductPriceLabel.Text = "$" + arg1.price;
			locationLabel.Text = Singleton.sharedInstance().locationManager.position;
			serviceProduct = arg1;
			selectedTapIndex = arg2;
			loadClinic();
			updateBody();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();
		}

		bool OnTimer()
		{
			payNowButton.BackgroundColor = Color.FromHex("#c2c2c2");
			payClinicButton.BackgroundColor = Color.FromHex("#01b3f0");
			return false;
		}

		private void updateBody() {
			if (selectedTapIndex == 1)
			{
				homeLayout.IsVisible = false;
				clinicLayout.IsVisible = true;
			}else{
				homeLayout.IsVisible = true;
				clinicLayout.IsVisible = false;
			}

		}

		private void loadClinic() {

			foreach (Clinic item in Singleton.sharedInstance().clinics) {
				clinicItems.Add(item.clinic_name,item.clinic_id);
				clinicDataPicker.Items.Add(item.clinic_name);
			}
			clinicDataPicker.SelectedIndexChanged += (sender, args) =>
			{
				if (clinicDataPicker.SelectedIndex != -1)
				{
					string clinicName = clinicDataPicker.Items[clinicDataPicker.SelectedIndex];
					clinicId = clinicItems[clinicName];
				}
			};
		}

		private async void submitBooking(string transCode = null) {
			var bookingType = "";
			bool isGPS;
			double lat = 0.0, longi = 0.0;
			var address = "";
			int timeType = 0;
			DateTime book_date = DateTime.Now;
			var serviceId = "";
			int userId = 0;
			var amount = "";
			int transactionType = 1;
			var transactionCode = "0";
			int payTypeId = 0;

			if (transCode != null)
				transactionCode = transCode;
			
			if (selectedTapIndex == 1)
			{
				bookingType = "clinic";
				if (isPayClinicClicked) {
					transactionType = 0;
					payTypeId = 1;
				}
				if(!timeFlag)
					timeType = 1;
				if (timeType == 0)
				{
					book_date = specificDatePicker2.Date + specificTimePicker2.Time;
				}
				if (clinicId == null)
				{
					await Navigation.PushPopupAsync(new AlertPopup("Warning", "Please select a clinic location.", "OK"));
					return;
				}
			}
			else 
			{
				if (selectedTapIndex == 2)
				{
					bookingType = "house";
				}
				else { 
					bookingType = "van";
				}
				payTypeId = 0;
				if (!timeFlag)
					timeType = 1;
				if (timeType == 0)
				{
					book_date = specificDatePicker1.Date + specificTimePicker1.Time;
				}
			}
			if (selectedTapIndex == 1)
			{

				if (!isPayClinicClicked)
				{
					var payRewult = await payServicePaypal();
					if (payRewult != "Cancelled" && payRewult != "Error")
					{
						transactionCode = payRewult;
					}
					else {
						return;
					}
				}
			}
			else { 
				var payRewult = await payServicePaypal();
				if (payRewult != "Cancelled" && payRewult != "Error")
				{
					transactionCode = payRewult;
				}
				else {
					return;
				}
			}

			isGPS = true;

			if (locationFlag)
				address = addressEntry.Text;
			else
				address = Singleton.sharedInstance().locationManager.position;

			lat = Singleton.sharedInstance().locationManager.latitude;
			longi = Singleton.sharedInstance().locationManager.longitude;
			serviceId = serviceProduct.service_id;
			userId = Singleton.sharedInstance().user.userid;
			amount = serviceProduct.price;

			var result = await apiManager.sendBookingRequest(bookingType, isGPS, lat, longi, address, clinicId, timeType, book_date, serviceId, userId, transactionType, transactionCode, amount, payTypeId);
			if (result is bool)
				await Navigation.PushAsync(new ServiceConfirmPage(serviceProduct));
			else
				await Navigation.PushPopupAsync(new AlertPopup("Warning", (string)result, "OK"));
		}

		private async Task<string> payServicePaypal() {
			string resultState = "SUCCESS";
			var result = await CrossPayPalManager.Current.Buy(new PayPalItem(serviceProduct.service_name, new Decimal(Double.Parse(serviceProduct.price)), "USD"), new Decimal(0.00));
			if (result.Status == PayPalStatus.Cancelled)
			{
				resultState = "Cancelled";

			}
			else if (result.Status == PayPalStatus.Error)
			{
				resultState = "Error";
			}
			else if (result.Status == PayPalStatus.Successful)
			{
				resultState = result.ServerResponse.Response.Id;
			}
			return resultState;
		}


		/* Events */
		private void OnLocationCheckButtonClicked(object sender, EventArgs e) {
			locationFlag = !locationFlag;
			if (locationFlag)
			{
				addressCheckButton.Source = ImageSource.FromFile("check_button.png");
				locationCheckButton.Source = ImageSource.FromFile("uncheck_button.png");
				addressEntry.IsEnabled = true;
			}
			else { 
				addressCheckButton.Source = ImageSource.FromFile("uncheck_button.png");
				locationCheckButton.Source = ImageSource.FromFile("check_button.png");
				addressEntry.IsEnabled = false;
			}
		}

		private void OnTimeCheckButtonClicked(object sender, EventArgs e) {
			timeFlag = !timeFlag;
			if (timeFlag)
			{
				timeCheckButton_c.Source = ImageSource.FromFile("check_button.png");
				timeCheckButton_h.Source = ImageSource.FromFile("check_button.png");
				soonestCheckButton_c.Source = ImageSource.FromFile("uncheck_button.png");
				soonestCheckButton_h.Source = ImageSource.FromFile("uncheck_button.png");
				specificTimePicker1.IsEnabled = true;
				specificTimePicker2.IsEnabled = true;
				specificDatePicker1.IsEnabled = true;
				specificDatePicker2.IsEnabled = true;
			}
			else { 
				timeCheckButton_c.Source = ImageSource.FromFile("uncheck_button.png");
				timeCheckButton_h.Source = ImageSource.FromFile("uncheck_button.png");
				soonestCheckButton_c.Source = ImageSource.FromFile("check_button.png");
				soonestCheckButton_h.Source = ImageSource.FromFile("check_button.png");
				specificTimePicker1.IsEnabled = false;
				specificTimePicker2.IsEnabled = false;
				specificDatePicker1.IsEnabled = false;
				specificDatePicker2.IsEnabled = false;
			}
		}

		private void OnAdditionPayButtonClicked(object sender, EventArgs e) 
		{
			payNowButton.BackgroundColor = Color.FromHex("#c2c2c2");
			payClinicButton.BackgroundColor = Color.FromHex("#c2c2c2");
			var button = (Button)sender;
			if (button == payNowButton)
			{
				isPayClinicClicked = false;
				payNowButton.BackgroundColor = Color.FromHex("#01b3f0");
			}
			else {
				isPayClinicClicked = true;
				payClinicButton.BackgroundColor = Color.FromHex("#01b3f0");
			}
		}

		private void OnNextButtonClicked(object sender, EventArgs e)
		{
			submitBooking();
		}

		private void OnSubmitButtonClicked(object sender, EventArgs e) {
			submitBooking();
		}

		private void OnCancelButtonClicked(object sender, EventArgs e)
		{
			Navigation.PopToRootAsync();
		}
	}
}
