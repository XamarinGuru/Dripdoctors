using System;
using System.Collections.Generic;
using System.Diagnostics;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using FFImageLoading.Forms;

namespace Dripdoctors
{
	public partial class ServiceMainView : BaseContentView
	{
		List<ServiceCategory> services = new List<ServiceCategory>();
		APIManager apiManager = new APIManager();
		bool addressEnable = false;
		bool isLoaded = false;
		int waitingTime = 0;
		public ServiceMainView(BaseElementInterface p) : base(p)
		{
			InitializeComponent();
			addressButton.Clicked += OnLocationButtonClicked;
			locationButton.Clicked += OnLocationButtonClicked;
			Xamarin.Forms.Device.StartTimer(TimeSpan.FromSeconds(.02), OnTimer);
			loadService();
		}

		private bool OnTimer()
		{
			var progress = (progressControl.Progress + .01);
			if (progress > 1) progress = 0;
			waitingTime++;
			if (waitingTime > 1000)
			{
				Navigation.PushPopupAsync(new AlertPopup("Warning", "Please check your internet connection.", "OK"));
				progressLayout.IsVisible = false;
				return false;
			}
			progressControl.Progress = progress;
			if (isLoaded) { return false; }
			return true;
		}

		private async void loadService() {
			var result = await apiManager.getServiceCategory();
			services = (List<ServiceCategory>)result;
			progressLayout.IsVisible = false;
			isLoaded = true;
			if (!(result is List<ServiceCategory>))
			{
				await Navigation.PushPopupAsync(new AlertPopup("Warning", (string)result, "OK"));
				return;
			}

			int i = 0;
			foreach (ServiceCategory item in services) {
				Uri uri = null;
				if(item.cat_image_url != null)
					uri = new Uri(item.cat_image_url);
				if (uri == null) 
					break;

				var service = new CachedImage
				{
					Source = ImageSource.FromUri(uri),
					HeightRequest = 200
				};

				service.ClassId = item.category_id;
				var tapGestureRecognizer = new TapGestureRecognizer();
				tapGestureRecognizer.Tapped += OnSelectProduct;
				service.GestureRecognizers.Add(tapGestureRecognizer);
				serviceCategoriGrid.Children.Add(service, i % 2, i / 2);
				i++;
			}	
		}

		private void OnSelectProduct(object sender, EventArgs e) {
			Debug.WriteLine("");
			var button = (CachedImage)sender;
			App.Current.MainPage.Navigation.PushAsync(new ServiceProductsPage(findItem(button.ClassId), 1));
		}

		private ServiceCategory findItem(string id) {
			ServiceCategory reItem = new ServiceCategory();
			foreach (ServiceCategory item in services) {
				if (item.category_id.Equals(id)) {
					reItem = item;
					break;
				}
			}
			return reItem;
		}

		private void OnLocationButtonClicked(object sender, EventArgs e) {
			addressEnable = !addressEnable;
			if (addressEnable)
			{
				addressButton.Image = "check_button.png";
				locationButton.Image = "uncheck_button.png";
			}
			else { 
				addressButton.Image = "uncheck_button.png";
				locationButton.Image = "check_button.png";
			}
			addressEntry.IsEnabled = addressEnable;
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

