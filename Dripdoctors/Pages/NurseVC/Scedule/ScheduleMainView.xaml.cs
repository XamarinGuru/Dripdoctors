using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using Xamarin.Forms;
using Rg.Plugins.Popup.Extensions;

namespace Dripdoctors
{
	public partial class ScheduleMainView : BaseContentView
	{
		int pageIndex = 1;
		APIManager apiManager;
		ListView lstView = null;
		ObservableCollection<CallDataTemplete> veggies;
		Call selectedCall = null;
		List<Call> calls = null;
		DateTime today;
		bool isLoaded = false;
		int waitingTime = 0;
		public ScheduleMainView(BaseElementInterface p) : base(p)
		{
			InitializeComponent();
			allButton.Clicked += (sender, e) => { pageIndex = 1; updateMenu(); };
			dayButton.Clicked += (sender, e) => { pageIndex = 2; updateMenu(); };
			weekButton.Clicked += (sender, e) => { pageIndex = 3; updateMenu(); };
			apiManager = new APIManager();
			calls = new List<Call>();
			lstView = new ListView();
			lstView.RowHeight = 150;
			lstView.ItemTapped += (sender, e) =>
			{
				((ListView)sender).SelectedItem = null;
			};
			lstView.ItemSelected += (sender, e) =>
			{
				((ListView)sender).SelectedItem = null;
			};

			lstView.ItemTemplate = new DataTemplate(() =>
			{
				CallCell cell = new CallCell();

				cell.Tapped += (sender, args) =>
				{
					selectedCall = findCallById(((CallDataTemplete)((CallCell)sender).BindingContext).callId);
					Navigation.PushAsync(new CallDetailPage(selectedCall));
				};
				return cell;
			});
			veggies = new ObservableCollection<CallDataTemplete>();
			listLayout.Children.Add(lstView);
			datePicker.BackgroundColor = Color.Transparent;

			dateMarkLabel.Text = Functions.getDateFormatStringByFormat(DateTime.Now.Date + "", '-');
			datePicker.PropertyChanged += (sender, e) => { 
				today = ((DatePicker)sender).Date;
				if (dateMarkLabel.Text != Functions.getDateFormatStringByFormat(today + "", '-'))
				{
					dateMarkLabel.Text = Functions.getDateFormatStringByFormat(today + "", '-');
					updateBody();			
				}
			};
			datePicker.DateSelected += (sender, e) => {

			};
			Xamarin.Forms.Device.StartTimer(TimeSpan.FromSeconds(0.02), OnTimer);
			startLoader();
			loadData();
		}

		private bool OnTimer()
		{
			updateMenu();
			return false;
		}

		public void startLoader()
		{
			isLoaded = false;
			Xamarin.Forms.Device.StartTimer(TimeSpan.FromSeconds(.02), OnLoader);
		}

		public void stopLoader()
		{
			isLoaded = true;
		}

		private bool OnLoader()
		{
			if (progressLayout.IsVisible == false)
				progressLayout.IsVisible = true;
			var progress = (progressControl.Progress + .01);
			if (progress > 1) progress = 0;
			waitingTime++;
			if (waitingTime > 6000)
			{
				App.Current.MainPage.DisplayAlert("Warning", "Please check your internet connection.", "OK");
				progressLayout.IsVisible = false;
				return false;
			}
			progressControl.Progress = progress;
			if (isLoaded)
			{
				progressLayout.IsVisible = false;
				return false;
			}
			return true;
		}

		private void updateMenu() { 
			allButton.BackgroundColor = Color.FromHex("#c2c2c2");
			dayButton.BackgroundColor = Color.FromHex("#c2c2c2");
			weekButton.BackgroundColor = Color.FromHex("#c2c2c2");
			switch (pageIndex)
			{
				case 1:
					calendarLayout.IsVisible = false;
					allButton.BackgroundColor = Color.FromHex("#01b3f0");
					break;
				case 2:
					calendarLayout.IsVisible = true;
					dayButton.BackgroundColor = Color.FromHex("#01b3f0");
					break;
				case 3:
					calendarLayout.IsVisible = false;
					weekButton.BackgroundColor = Color.FromHex("#01b3f0");
					break;
				default:
					break;
			}
			if (calls.Count > 0) {
				updateBody();
			}
		}


		private void updateBody() {
			int index = 0;
			veggies = new ObservableCollection<CallDataTemplete>();
			foreach (Call item in calls)
			{
				if (item.callstep_id != 4) {
					index++;
					continue;
				}
				if (pageIndex == 2)
				{
					var date = item.requested_date.Split(' ');
					if ( today == DateTime.Parse(date[0]).Date)
					{
						veggies.Add(new CallDataTemplete
						{
							callId = item.call_id,
							callType = "IN " + item.booking_type.ToUpper() + " CALL",
							requesteddate = Functions.getDateFormatStringByTime(item.requested_date),
							callStatus = item.requested_date,
							serviceImgUrl = item.serviceInfo.service_img_icon,
							categoryName = item.serviceInfo.category.category_name,
							productName = item.serviceInfo.service_name,
							address = item.clientInfo.address + " " + item.clientInfo.city + ", " + item.clientInfo.state + " " + item.clientInfo.zip
						});
						continue;
					}
				}
				else if (pageIndex == 3)
				{
					DateTime StartOfWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
					DateTime EndOfLastWeek = StartOfWeek.AddDays(6);
					if (StartOfWeek < DateTime.Parse(item.requested_date) && DateTime.Parse(item.requested_date) < EndOfLastWeek)
					{
						veggies.Add(new CallDataTemplete
						{
							callId = item.call_id,
							callType = "IN " + item.booking_type.ToUpper() + " CALL",
							requesteddate = Functions.getDateFormatStringByTime(item.requested_date),
							callStatus = item.requested_date,
							serviceImgUrl = item.serviceInfo.service_img_icon,
							categoryName = item.serviceInfo.category.category_name,
							productName = item.serviceInfo.service_name,
							address = item.clientInfo.address + " " + item.clientInfo.city + ", " + item.clientInfo.state + " " + item.clientInfo.zip

						});
						continue;
					}
				}
				else if(pageIndex == 1){
					veggies.Add(new CallDataTemplete
					{
						callId = item.call_id,
						callType = "IN " + item.booking_type.ToUpper() + " CALL",
						requesteddate = Functions.getDateFormatStringByTime(item.requested_date),
						callStatus = item.requested_date,
						serviceImgUrl = item.serviceInfo.service_img_icon,
						categoryName = item.serviceInfo.category.category_name,
						productName = item.serviceInfo.service_name,
						address = item.clientInfo.address + " " + item.clientInfo.city + ", " + item.clientInfo.state + " " + item.clientInfo.zip
					});
					continue;
				}
				index++;
			}
			if (index == calls.Count && calls.Count != 0)
				App.Current.MainPage.DisplayAlert("Warning", "You don't have any scheduled calls in this moment.", "OK");
			lstView.ItemsSource = veggies;
		}

		private async void loadData()
		{
			var result = await apiManager.getCalls(Singleton.sharedInstance().nurseId);

			if (result is List<Call>)
			{
				calls = (List<Call>)result;
				updateBody();
				stopLoader();
			}
			else if (result == null)
			{
				stopLoader();
				await App.Current.MainPage.DisplayAlert("Warning", "You don't have any scheduled calls in this moment.", "OK");
			}
			else
			{
				stopLoader();
				await App.Current.MainPage.DisplayAlert("Warning", (string)result, "OK");
			}
		}

		public Call findCallById(int id)
		{
			foreach (Call item in calls)
				if (id == item.call_id)
					return item;
			return null;
		}
	}
}
