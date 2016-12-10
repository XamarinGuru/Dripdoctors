using System;
using System.Diagnostics;
using Xamarin.Forms;
namespace Dripdoctors
{
	public partial class ClientMainPage : BaseContentPage
	{
		private bool isMenuVisible = false;

		Image serviceButton;
		Image nurseButton;
		Image bookingButton;
		Image accountButton;
		public ClientMainPage()
		{
			InitializeComponent();
			logoButton.Source = ImageSource.FromFile("drip_off.png");
			//btnLogo.Clicked += OnLogoButtonClicked;
		}

		public ClientMainPage(int pageIndex) : this() { 
			initMenu();
			loadBody(pageIndex);
		}

		private void initMenu() {

			serviceButton = new Image
			{
				Source = ImageSource.FromFile("service_selected.png")
			};
			nurseButton = new Image
			{
				Source = ImageSource.FromFile("findnurse_normal.png")
			};
			bookingButton = new Image
			{
				Source = ImageSource.FromFile("booking_normal.png")
			};
			accountButton = new Image
			{
				Source = ImageSource.FromFile("account_normal.png")
			};
	
			var serviceTapGestureRecognizer = new TapGestureRecognizer();
			serviceTapGestureRecognizer.Tapped += OnServiceButtonClicked;
			serviceButton.GestureRecognizers.Add(serviceTapGestureRecognizer);

			var nurseTapGestureRecognizer = new TapGestureRecognizer();
			nurseTapGestureRecognizer.Tapped += OnNurseButtonClicked;
			nurseButton.GestureRecognizers.Add(nurseTapGestureRecognizer);

			var bookingTapGestureRecognizer = new TapGestureRecognizer();
			bookingTapGestureRecognizer.Tapped += OnBookingButtonClicked;
			bookingButton.GestureRecognizers.Add(bookingTapGestureRecognizer);

			var accountTapGestureRecognizer = new TapGestureRecognizer();
			accountTapGestureRecognizer.Tapped += OnAccountButtonClicked;
			accountButton.GestureRecognizers.Add(accountTapGestureRecognizer);

			var grid = new Grid()
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.Start,
				ColumnDefinitions = {
					new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
					new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
					new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
					new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
				}
			};
			grid.WidthRequest = Width;
			grid.Children.Add(serviceButton, 0, 0);
			grid.Children.Add(nurseButton,1,0);
			grid.Children.Add(bookingButton,2,0);
			grid.Children.Add(accountButton,3,0);
			menuLayout.Children.Add(grid);
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			NavigationPage.SetHasNavigationBar(this, false);
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			//isThread = false;
		}

		public void loadBody(int index)
		{
			bodyLayout.Children.Clear();

			switch (index)
			{
				case 1:
					ServiceMainView serviceView = new ServiceMainView(this);
					activeChild = serviceView;
					activeChildIndex = 1;
					OnChildViewSelectionChanged();
					bodyLayout.Children.Add(serviceView);
					break;
				case 2:
					var nurseView = new NurseMainView(this);
					activeChild = nurseView;
					activeChildIndex = 2;
					OnChildViewSelectionChanged();
					bodyLayout.Children.Add(nurseView);
					break;
				case 3:
					BookingsMainView bookingView = new BookingsMainView(this);
					activeChild = bookingView;
					activeChildIndex = 3;
					OnChildViewSelectionChanged();
					bodyLayout.Children.Add(bookingView);
					break;
				case 4:
					AccountMainView accountView = new AccountMainView(this);
					activeChild = accountView;
					activeChildIndex = 4;
					accountView.loadBody(1);
					bodyLayout.Children.Add(accountView);
					break;
				default:
					ServiceMainView am = new ServiceMainView(this);
					activeChild = am;
					activeChildIndex = 1;
					OnChildViewSelectionChanged();
					bodyLayout.Children.Add(am);
					break;
			}
			updateMenu(index);
		}

		private void updateMenu(int index) {

			serviceButton.Source = "service_normal.png";
			bookingButton.Source = "booking_normal.png";
			accountButton.Source = "account_normal.png";
			nurseButton.Source = "findnurse_normal.png";
			if (index == 1)
			{
				serviceButton.Source = "service_selected.png";
			}
			else if (index == 2)
			{
				nurseButton.Source = "findnurse_selected.png";
			}
			else if (index == 3)
			{
				bookingButton.Source = "booking_selected.png";
			}
			else
			{ 
				accountButton.Source = "account_selected.png";
			}
		}

		private void updateScreen() {
			//bodyLayout.WidthConstraint = Constraint.RelativeToParent((parent) =>
			//	{
			//		return (parent.Width / 2) - 20; // center of image (which is 40 wide)
			//	});
		}

		public void OnLogoButtonClicked(object sender, EventArgs e) {
			menuLayout.IsVisible = isMenuVisible;
			isMenuVisible = !isMenuVisible;
			if (isMenuVisible)
			{
				logoButton.Source = ImageSource.FromFile("drip_on.png");
			}
			else 
			{
				logoButton.Source = ImageSource.FromFile("drip_off.png");			
			}
		}

		public void OnUserButtonClicked(object sender, EventArgs e) { 
			
		}

		public void OnServiceButtonClicked(object sender, EventArgs e)
		{
			loadBody(1);
		}

		public void OnNurseButtonClicked(object sender, EventArgs e)
		{
			loadBody(2);
		}

		public void OnBookingButtonClicked(object sender, EventArgs e)
		{
			loadBody(3);
		}

		public void OnAccountButtonClicked(object sender, EventArgs e)
		{
			loadBody(4);
		}

		public override void OnContentViewSelectionChanged()
		{
			Object navRightButton = GetRightNavButton();
			if (navRightButton != null)
			{
				Button rightButton = (Button)navRightButton;
				logoGrid.Children.RemoveAt(2);
				logoGrid.Children.Add(rightButton,2,0);
				rightButton.Clicked += OnRightNavButton;
			}
		}
	}
}
