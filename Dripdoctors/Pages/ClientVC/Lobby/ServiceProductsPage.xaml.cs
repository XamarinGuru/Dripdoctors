using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;
using Rg.Plugins.Popup.Extensions;
using Dripdoctors.Carousel;
namespace Dripdoctors
{
	public partial class ServiceProductsPage : ContentPage
	{
		APIManager apiManager = new APIManager();
		private ServiceCategory serviceCategory = null;
		private List<ServiceItem> products = null;
		private ServiceItem selectedProduct = null;
		private int selectedTabIndex = 1;
		public ServiceProductsPage()
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
			products = new List<ServiceItem>();

			backButton.Clicked += (sender, e) =>
			{
				Navigation.PopAsync();
			};

			nextButton.Clicked += OnNextButtonClicked;
			clinicButton.Clicked += OnClinicButtonClicked;
			houseButton.Clicked += OnHouseButtonClicked;
			vanButton.Clicked += OnVanButtonClicked;

			bookNowButton.Clicked += OnBookNowButtonClicked;
			var tapGestureRecognizer = new TapGestureRecognizer();
			tapGestureRecognizer.Tapped += OnCategoryInforButtonClicked;
			categoryInfoButton.GestureRecognizers.Add(tapGestureRecognizer);
			categoryInfoButton.Source = ImageSource.FromFile("info_icon.png");
		}

		public ServiceProductsPage(ServiceCategory arg, int positionIndex) : this()
		{
			serviceCategory = arg;
			chooseLabel.Text = "Choose a " + serviceCategory.category_name;
			categoryLabel.Text = serviceCategory.category_name;
			categoryTitle.Text = serviceCategory.category_name;
			if (serviceCategory.cat_image_url != null)
			{
				categoryImage.Source = ImageSource.FromUri(new Uri(serviceCategory.cat_image_icon));
			}

			loadProduct(serviceCategory.category_id);
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			updateMenu();
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();
		}

		private void updateMenu()
		{
			clinicButton.BackgroundColor = Color.FromHex("#c2c2c2");
			houseButton.BackgroundColor = Color.FromHex("#c2c2c2");
			vanButton.BackgroundColor = Color.FromHex("#c2c2c2");
			if (selectedTabIndex == 1)
			{
				clinicButton.BackgroundColor = Color.FromHex("#01b3f0");
			}
			else if (selectedTabIndex == 2)
			{
				houseButton.BackgroundColor = Color.FromHex("#01b3f0");
			}
			else {
				vanButton.BackgroundColor = Color.FromHex("#10b3f0");
			}
		}

		private async void showCategoryInfo()
		{
			var page = new ServiceCategoryVeiw(serviceCategory);
			await Navigation.PushPopupAsync(page);
		}

		private async void showProductInfo() { 
			var page = new ServiceProductVeiw(selectedProduct, Height);
			await Navigation.PushPopupAsync(page);
		}

		private async void loadProduct(string productId)
		{
			var result = await apiManager.getServiceProducts(productId);
			if (!(result is List<ServiceItem>)) {
				await Navigation.PushPopupAsync(new AlertPopup("Warning", (string)result, "OK"));
				return;
			}
			products = (List<ServiceItem>)result;

			selectedProduct = products[0];
			CreatePagesCarousel();
		}

		private ServiceItem findItem(string id)
		{
			foreach (ServiceItem item in products)
			{
				if (item.service_id.Equals(id))
				{
					return item;
				}
			}
			return null;
		}

		private void CreatePagesCarousel()
		{
			var carousel = new CarouselLayout
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				ItemTemplate = new DataTemplate(typeof(HomeView))
			};
			carousel.SetBinding(CarouselLayout.ItemsSourceProperty, "Pages");
			carousel.SetBinding(CarouselLayout.SelectedItemProperty, "CurrentPage", BindingMode.TwoWay);

			carousel.OnItemClick += new EventHandler(OnProductItemClick);
			carousel.OnItemInfoClick += new EventHandler(OnProductItemInfoClick);

			List<HomeViewModel> list = new List<HomeViewModel>();

			foreach (ServiceItem item in products)
			{
				var product = new HomeViewModel
				{
					ID = item.service_id,
					Image = ImageSource.FromUri(new Uri(item.service_img)),
					Price = "$ " + item.price,
					Discription = item.service_description,
					WidthRequest = Width * 0.6,
					HeightRequest = Height * 0.2
				};
				list.Add(product);
			}

			carousel.Scrolled += OnScrolled;

			BindingContext = new SwitcherPageViewModel(list);
			bodyLayout.Children.Add(carousel);
		}

		private int getItemIndex(double x) {
			var scrollWidth = x + Width / 2;
			var index = Math.Round(scrollWidth / (Width * 0.7));
			if ((int)index < 1)
				index = 1;
			if ((index) > products.Count)
				index = products.Count;
			return (int)index;
		}


		/* Events */
		private void OnScrolled(object sender, ScrolledEventArgs e)
		{
			var scrollView = (CarouselLayout)sender;
			scrollView.SelectedIndex = getItemIndex(e.ScrollX) - 1;
			selectedProduct = products[getItemIndex(e.ScrollX) - 1];
		}

		private void OnProductItemClick(object sender, EventArgs e)
		{
			CarouselLayout carousel = sender as CarouselLayout;
			CarouselLayout.ItemClickEventArgs args = e as CarouselLayout.ItemClickEventArgs;
			if (carousel == null || args == null)
				return;
			HomeViewModel item = args.item;
			selectedProduct = findItem(item.ID);
		}

		private void OnProductItemInfoClick(object sender, EventArgs e) { 
			CarouselLayout carousel = sender as CarouselLayout;
			CarouselLayout.ItemClickEventArgs args = e as CarouselLayout.ItemClickEventArgs;
			if (carousel == null || args == null)
				return;
			HomeViewModel item = args.item;
			selectedProduct = findItem(item.ID);
			showProductInfo();
		}

		private void OnNextButtonClicked(object sender, EventArgs e)
		{
			if (selectedProduct == null && Singleton.sharedInstance().currentRequestedService != null)
			{
				selectedProduct = Singleton.sharedInstance().currentRequestedService;
			}

			if (selectedProduct != null)
			{
				Navigation.PushAsync(new ServiceRequestPage(selectedProduct, selectedTabIndex));
			}
		}

		private void OnClinicButtonClicked(object sender, EventArgs e)
		{
			selectedTabIndex = 1;
			updateMenu();
		}

		private void OnHouseButtonClicked(object sender, EventArgs e)
		{
			
			selectedTabIndex = 2;
			updateMenu();
		}

		private void OnVanButtonClicked(object sender, EventArgs e) {
			selectedTabIndex = 3;
			updateMenu();
		}

		private void OnCategoryInforButtonClicked(object sender, EventArgs e)
		{
			showCategoryInfo();
		}

		private void OnBookNowButtonClicked(object sender, EventArgs e)
		{
			if (selectedProduct == null && Singleton.sharedInstance().currentRequestedService != null)
			{
				selectedProduct = Singleton.sharedInstance().currentRequestedService;
			}

			if (selectedProduct != null)
			{
				switch (selectedTabIndex)
				{
					case 1:
						
						break;
					case 2:
						if (selectedProduct.inhouse_call != 1) { 
							Navigation.PushPopupAsync(new AlertPopup("Sorry", "This service is only available at your nearest Drip Doctors clinic.", "OK"));
							return;
						}
						break;
					case 3:
						if (selectedProduct.van_call != 1) { 
							Navigation.PushPopupAsync(new AlertPopup("Sorry", "This service is only available at your nearest Drip Doctors clinic.", "OK"));
							return;
						}
						break;
					default:
						break;
				}
				App.Current.MainPage.Navigation.PushAsync(new ServiceRequestPage(selectedProduct, selectedTabIndex));
			}
		}
	}
}
