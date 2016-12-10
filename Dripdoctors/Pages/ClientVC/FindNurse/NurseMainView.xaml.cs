using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Dripdoctors;
using Rg.Plugins.Popup.Extensions;

namespace Dripdoctors
{
	public partial class NurseMainView : BaseContentView
	{
		private APIManager apiManager;
		private List<Nurse> nurseItems;
		private FindMap _map;
		//private List<ServiceCategory> serviceCategories;
		//private ServiceCategory selectedService;
		public NurseMainView(BaseElementInterface p) : base(p)
		{
			InitializeComponent();
			_map = new  FindMap{
				MapType = MapType.Street
			};
			mapLayout.Children.Add(_map);
			apiManager = new APIManager();
			nurseItems = new List<Nurse>();
			//serviceCategories = new List<ServiceCategory>();
			//selectedService = new ServiceCategory();

			getNurseItems();
			//getServiceCategories();
			//requestButton.Clicked += OnRequestButtonClicked;
		}

		private async void getNurseItems() {
			var result = await apiManager.getNurse();
			if (!(result is List<Nurse>))
			{
				await Navigation.PushPopupAsync(new AlertPopup("Warning",(string)result,"OK"));
				return;
			}
			nurseItems = (List<Nurse>)result;
			reLoadPins();
		}

		//private async void getServiceCategories() {
		//	var result = await apiManager.getServiceCategory();
		//	if (!(result is List<ServiceCategory>))
		//	{
		//		await Navigation.PushPopupAsync(new AlertPopup("Warning", (string)result, "OK"));
		//		return;
		//	}
		//	serviceCategories = (List<ServiceCategory>)result;
		//}

		private void reLoadPins() {
			_map.Pins.Clear();
			var pinList = new List<CustomPin>();
			foreach (Nurse item in nurseItems) {				
				var pin = new CustomPin(
					item.nurse_id,
					new Pin{
						Address = item.sname,
						Label = item.fname,
						Type = PinType.Generic,
						Position = new Position(item.last_latitud, item.last_longitud)
					},
					item.img_url
				);
				pin.Clicked += OnPinClicked;
				_map.Pins.Add(pin.Pin);
				pinList.Add(pin);
			}
			_map.CustomPins = pinList;
			Xamarin.Forms.Device.StartTimer(TimeSpan.FromSeconds(.3), OnTimer);
		}

		private bool OnTimer() {
			_map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(36.114823, -115.172695), Distance.FromMiles(2000)));
			return false;
		}

		private void OnPinClicked(object sender, EventArgs e) {
			var pin = (CustomPin)sender;
			reLoadProfile(findNurse(pin));
		}

		private async void reLoadProfile(Nurse item) {
			profileImage.Source = ImageSource.FromUri(new Uri(item.img_url));
			profileImage.WidthRequest = Width * 0.38;
			profileImage.HeightRequest = Width * 0.38;
			nameLabel.Text = item.fname + " " + item.sname;
			var result = await apiManager.getGeoCode(item.last_longitud, item.last_latitud);

			addressLabel.Text = (string)result;
			//var layout = new StackLayout
			//{
			//	HorizontalOptions = LayoutOptions.FillAndExpand,
			//	Orientation = StackOrientation.Horizontal
			//};
			//if (item.services != null)
			//{
			//	for (int i = 0; i < item.services.Length; i++)
			//	{
			//		foreach (ServiceCategory service in serviceCategories)
			//		{
			//			if (service.category_id == item.services[i].nurse_service_id)
			//			{
			//				var image = new Image
			//				{
			//					Source = ImageSource.FromUri(new Uri(service.cat_image_url)),
			//					HeightRequest = Width * 0.1,
			//					WidthRequest = Width * 0.1
			//				};
			//				image.ClassId = service.category_id;
			//				//var tapGestureRecognizer = new TapGestureRecognizer();
			//				//tapGestureRecognizer.Tapped += OnServiceClicked;
			//				//image.GestureRecognizers.Add(tapGestureRecognizer);
			//				layout.Children.Add(image);
			//				break;
			//			}
			//		}
			//	}
			//}
			//serviceScrollView.Content = layout;
			completedServiceLabel.Text = "Over " + item.services_completed + " completed services";
			//requestButton.Text = "SEND " + item.fname + " A BOOKING REQUEST";

			profileLayout.IsVisible = true;
		}

		private Nurse findNurse(CustomPin pin)
		{
			foreach (Nurse item in nurseItems) {
				if (item.fname == pin.Pin.Label && item.sname == pin.Pin.Address) {
					return item;
				}
			}
			return null;
		}

		//private ServiceCategory findService(string id) {
		//	foreach (ServiceCategory item in serviceCategories) {
		//		if (item.category_id == id) {
		//			return item;
		//		}
		//	}
		//	return null;
		//}

		//private void OnServiceClicked(object sender, EventArgs e) { 
		//	var service = (Image)sender;
		//	var page = new NurseCategoryVeiw(findService(service.ClassId));
		//	Navigation.PushPopupAsync(page);
		//}

		private void OnRequestButtonClicked(object sender, EventArgs e) {
			if (Singleton.sharedInstance().currentRequestedService == null) {
				return;
			}
			Navigation.PushModalAsync(new ServiceRequestPage(Singleton.sharedInstance().currentRequestedService, 2));		
		}

		public override Object RightNavButton()
		{
			return new CustomButton
			{
				TextColor = Color.White,
				FontSize = 12,
				HorizontalOptions = LayoutOptions.EndAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Text = Singleton.sharedInstance().locationManager.city
			};
		}

		public override bool OnRightNavButton_Clicked()
		{
			//locationLabel.Text = Singleton.sharedInstance().locationManager.position;
			//locationLayout.IsVisible = !locationLayout.IsVisible;
			return false;
		}
	}
}

