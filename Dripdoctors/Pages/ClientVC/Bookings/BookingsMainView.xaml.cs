using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace Dripdoctors
{
	public partial class BookingsMainView : BaseContentView
	{
		private APIManager apiManager;
		private List<Booking> bookings;
		private int tabIndex = 1;
		private bool isLoaded = false;
		private int waitingTime = 0;
		ListView lstView = null;
		ObservableCollection<Booking> veggies;
		int start_index = 0;
		public BookingsMainView(BaseElementInterface p) : base(p)
		{
			InitializeComponent();

			allButton.Clicked += (sender, e) => { updateMenu(1);};
			confirmButton.Clicked += (sender, e) => { updateMenu(2);};
			pendingButton.Clicked += (sender, e) => { updateMenu(3);};
			declineButton.Clicked += (sender, e) => { updateMenu(4);};

			apiManager = new APIManager();
			bookings = new List<Booking>();


			lstView = new ListView();
			lstView.RowHeight = 240;
			lstView.ItemTapped += (sender, e) => { 
				((ListView)sender).SelectedItem = null;
			};
			lstView.ItemSelected += (sender, e) => { 
				((ListView)sender).SelectedItem = null;
			};
			lstView.IsVisible = false;

			lstView.ItemTemplate = new DataTemplate(() =>
			{
				CellView cell = new CellView();
				cell.OnTrackNurseButtonClicked += OnTrakNurseClicked;
				cell.OnOptionButtonClicked += OnOptionClicked;
				cell.OnApplyAgainButtonClicked += OnApplayBookingClicked;
				cell.OnEditBookingButtonClicked += OnEditBookingClicked;
				cell.OnCancelBookingButtonClicked += OnCancelBookingClicked;
				cell.OnRemoveBookingButtonClicked += OnCancelBookingClicked;
				return cell;
			});
			veggies = new ObservableCollection<Booking>();
			lstView.ItemAppearing += (sender, e) =>
			{
				var cell = (Booking)e.Item;
				if (bookings.Count == 0)
					return;
				var selected_index = findBookingById(cell.booking_id);
				if (selected_index == bookings.Count - 1)
					return;
				if (selected_index == veggies.Count - 1)
				{
					veggies.RemoveAt(selected_index - 2);
					veggies.Add(bookings[selected_index + 1]);
					lstView.ItemsSource = veggies;
				}
			};

			loadBooking();

			Xamarin.Forms.Device.StartTimer(TimeSpan.FromSeconds(.02), OnTimer2);
			Xamarin.Forms.Device.StartTimer(TimeSpan.FromSeconds(.02), OnTimer1);
		}

		private bool OnTimer1()
		{
			var progress = (progressControl.Progress + .01);
			if (progress > 1) progress = 0;
			waitingTime ++;
			if (waitingTime > 1000) {	
				Navigation.PushPopupAsync(new AlertPopup("Warning","Please check your internet connection.","OK"));
				progressLayout.IsVisible = false;
				return false;
			}
			progressControl.Progress = progress;
			if (isLoaded) { 
				return false; 
			}
			return true;
		}

		private bool OnTimer2() {
			updateMenu(1);
			return false;
		}

		private async void loadBooking() 
		{
			var result = await apiManager.getBookingItems(Singleton.sharedInstance().user.userid);
			if (!(result is List<Booking>)) {
				await Navigation.PushPopupAsync(new AlertPopup("Warning",(string)result,"OK"));
				return;
			}

			bookings = (List<Booking>)result;
			progressLayout.IsVisible = false;
			isLoaded = true;
			updateBody();
		}

		private void updateBody() {
			lstView.ItemsSource = null;
			lstView.IsVisible = false;
			int i = 0;
			foreach (Booking item in bookings)
			{
				//if (i > 2) break;
				switch (tabIndex) {
					case 1:
						veggies.Add(item);
						i++;
						break;
					case 2:
						if (item.status == 2)
						{
							veggies.Add(item);
						}
						i++;
						break;
					case 3:
						if (item.status == 1)
						{
							veggies.Add(item);
						}
						i++;
						break;
					case 4:
						if (item.status == 3)
						{
							veggies.Add(item);
						}
						i++;
						break;
					default:
						break;
				}
			}
			lstView.ItemsSource = veggies;
			if(veggies.Count > 0)
				lstView.IsVisible = true;
			bodyLayout.Children.Add(lstView);
		}

		private void updateMenu(int index)
		{
			allButton.BackgroundColor = Color.FromHex("#c2c2c2");
			confirmButton.BackgroundColor = Color.FromHex("#c2c2c2");
			pendingButton.BackgroundColor = Color.FromHex("#c2c2c2");
			declineButton.BackgroundColor = Color.FromHex("#c2c2c2");
			if (index == 1)
			{
				allButton.BackgroundColor = Color.FromHex("#01b3f0");
			}
			else if (index == 2)
			{
				confirmButton.BackgroundColor = Color.FromHex("#01b3f0");
			}
			else if (index == 3)
			{
				pendingButton.BackgroundColor = Color.FromHex("#01b3f0");
			}
			else { 
				declineButton.BackgroundColor = Color.FromHex("#01b3f0");
			}
			tabIndex = index;
			updateBody();
		}

		private void ListViewProperties_ItemAppearing(object sender, ItemVisibilityEventArgs e)
		{
			
		} 

		private int findBookingById(String Id)
		{
			int i = 0;
			foreach (Booking item in bookings)
			{
				if (item.booking_id == Id)
				{
					return i;
				}
				i++;
			}
			return -1;
		}

		private void OnOptionClicked(object sender, EventArgs e) {
			var cell = (CellView)sender;
			cell.showPopup(tabIndex);
		}

		private void OnApplayBookingClicked(object sender, EventArgs e) { 
			var cell = (BaseCellView)sender;
			var booking = (Booking)cell.BindingContext;
			App.Current.MainPage.Navigation.PushAsync(new EditBookingPage(booking));
		}

		public static async void OnTrakNurseClicked(object sender, EventArgs e) 
		{
			var cell = (BaseCellView)sender;
			var booking = (Booking)cell.BindingContext;
			var result = await TrackNursePage.getNurses(booking.booking_id);
			if (result)
			{
				await App.Current.MainPage.Navigation.PushAsync(new TrackNursePage(booking));
			}
		}

		private void OnEditBookingClicked(object sender, EventArgs e)
		{
			var cell = (BaseCellView)sender;
			var booking = (Booking)cell.BindingContext;
			App.Current.MainPage.Navigation.PushAsync(new EditBookingPage(booking));
		}

		private void OnSendMessageClicked(object sender, EventArgs e) 
		{ 
			var cell = (BaseCellView)sender;
			
		}

		private void OnCancelBookingClicked(object sender, EventArgs e) 
		{
			var cell = (BaseCellView)sender;
			var booking = (Booking)cell.BindingContext;
			Navigation.PushPopupAsync(new CancelPopup(booking.booking_id));
		}

		private void OnRemoveBookingClicked(object sender, EventArgs e) 
		{
			var cell = (BaseCellView)sender;
			//var result = await apiManager.removeBooking(cell.booking.booking_id);
			//if (result == "SUCCESS")
			//{
			//	bookings.Remove(findBookingById(cell.booking.booking_id));
			//	updateBody();
			//}
			//else { 
			
			//}
		}

		public override object RightNavButton()
		{
			return new CustomButton
			{
				TextColor = Color.White,
				HorizontalOptions = LayoutOptions.EndAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Text = "History"
			};
		}

		public override bool OnRightNavButton_Clicked()
		{
			return false;
		}
	}
}

