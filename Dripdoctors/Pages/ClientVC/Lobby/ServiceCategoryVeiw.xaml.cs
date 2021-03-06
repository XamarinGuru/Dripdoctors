using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace Dripdoctors
{
	public partial class ServiceCategoryVeiw : PopupPage
	{
		//private APIManager apiManager;
		private List<ServiceItem> products;
		public ServiceCategoryVeiw()
		{
			InitializeComponent();
			//apiManager = new APIManager();
			//progressControl.IsVisible = true;
			products = new List<ServiceItem>();
			closeButton.Source = ImageSource.FromFile("close_icon.png");
			var tapGestureRecognizer = new TapGestureRecognizer();
			tapGestureRecognizer.Tapped += OnCloseButtonClicked;
			closeButton.GestureRecognizers.Add(tapGestureRecognizer);
			//Xamarin.Forms.Device.StartTimer(TimeSpan.FromSeconds(.02), OnTimer);
		}

		public ServiceCategoryVeiw(ServiceCategory arg) : this() {
			nameLabel.Text = arg.category_name;
			contentLabel.Text = arg.category_description;
			//loadProducts(arg.category_id);
		}


		//private bool OnTimer()
		//{
		//	var progress = (progressControl.Progress + .01);
		//	if (progress > 1) progress = 0;
		//	progressControl.Progress = progress;
		//	return true;
		//}

		//private async void loadProducts(string category_id) {
		//	products = await apiManager.getServiceProducts(category_id);
		//	progressControl.IsVisible = false;
		//	int i = 0;
		//	foreach (ServiceItem item in products) {
		//		if (item.service_img!= null)
		//		{
		//			var image = new Image
		//			{
		//				Source = ImageSource.FromUri(new Uri(item.service_img_icon)),
		//				HeightRequest = Height * 0.1
		//			};
		//			image.ClassId = item.service_id;
		//			var tapGestureRecognizer = new TapGestureRecognizer();
		//			tapGestureRecognizer.Tapped += OnServiceItemClicked;
		//			image.GestureRecognizers.Add(tapGestureRecognizer);
		//			productsGrid.Children.Add(image, i % 3, i / 3);
		//			i++;
		//		}
		//	}
		//	bodyLayout.IsVisible = true;
		//}



		private void OnServiceItemClicked(object sender, EventArgs e) {
			var item = (Image)sender;
			Singleton.sharedInstance().currentRequestedService =  findService(item.ClassId);
			PopupNavigation.PopAsync();
		}

		private ServiceItem findService(string id) {
			foreach (ServiceItem item in products) {
				if (item.service_id == id) {
					return item;
				}
			}
			return null;
		}

		private void OnCloseButtonClicked(object sender, EventArgs e) {
			PopupNavigation.PopAsync();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();
		}

		protected override bool OnBackButtonPressed()
		{
			return true;
		}
	}
}
