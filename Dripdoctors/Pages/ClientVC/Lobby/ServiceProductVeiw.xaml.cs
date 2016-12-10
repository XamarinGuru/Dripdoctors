using System;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace Dripdoctors
{
	public partial class ServiceProductVeiw : PopupPage
	{
		public ServiceProductVeiw()
		{
			InitializeComponent();
			closeButton.Source = ImageSource.FromFile("close_icon.png");
			var tapGestureRecognizer = new TapGestureRecognizer();
			tapGestureRecognizer.Tapped += OnCloseButtonClicked;
			closeButton.GestureRecognizers.Add(tapGestureRecognizer);
		}

		public ServiceProductVeiw(ServiceItem arg, double screenHeight) : this() {
			nameLabel.Text = arg.service_name;
			contentLabel.Text = arg.service_description;
			//productImage.Source = ImageSource.FromUri(new Uri(arg.service_img_icon));
			//productImage.HeightRequest = screenHeight * 0.25;
		}
		private void OnCloseButtonClicked(object sender, EventArgs e)
		{
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
