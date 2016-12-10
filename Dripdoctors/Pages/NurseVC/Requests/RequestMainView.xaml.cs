using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Rg.Plugins.Popup.Extensions;

namespace Dripdoctors
{
	public partial class RequestMainView : BaseContentView
	{
		int pageIndex = 1;
		APIManager apiManager;
		ListView lstView = null;
		ObservableCollection<CallDataTemplete> veggies;
		List<Call> calls = null;
		Call selectedCall = null;
		CallDataTemplete selectedTemp = null;
		bool isLoaded = false;
		int waitingTime = 0;
		public RequestMainView(BaseElementInterface p) : base(p)
		{
			InitializeComponent();
			pandingButton.Clicked += (sender, e) => { pageIndex = 1; updateMenu(); };
			historyButton.Clicked += (sender, e) => { pageIndex = 2; updateMenu(); };

			calls = new List<Call>();
			apiManager = new APIManager();
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
					selectedTemp = (CallDataTemplete)((CallCell)sender).BindingContext;
					if (pageIndex == 1)
					{
						var popup = new RequestPopupView();
						popup.OnAcceptClicked += OnAccept;
						popup.OnDeclineClicked += OnDecline;
						popup.OnDetailClicked += OnDetail;
						Navigation.PushPopupAsync(popup);
					}
					else {
						Navigation.PushAsync(new RequestPreviewPage(selectedCall));
					}
				};
				return cell;
			});
			veggies = new ObservableCollection<CallDataTemplete>();
			listLayout.Children.Add(lstView);
			Xamarin.Forms.Device.StartTimer(TimeSpan.FromSeconds(0.02), OnTimer);
			startLoader();
			loadData();
		}

		private bool OnTimer() {
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
				await App.Current.MainPage.DisplayAlert("Warning", "You don't have any calls requests in this moment.", "OK");
			}
			else
			{
				stopLoader();
				await App.Current.MainPage.DisplayAlert("Warning", (string)result, "OK");
			}
		}

		private void updateMenu() { 
			pandingButton.BackgroundColor = Color.FromHex("#c2c2c2");
			historyButton.BackgroundColor = Color.FromHex("#c2c2c2");
			switch (pageIndex)
			{
				case 1:
					pandingButton.BackgroundColor = Color.FromHex("#01b3f0");
					break;
				case 2:
					historyButton.BackgroundColor = Color.FromHex("#01b3f0");
					break;
				default:
					break;
			}
			if (calls.Count > 0)
				updateBody();
		}

		private void updateBody() {
			int index = 0;
			veggies = new ObservableCollection<CallDataTemplete>();
			foreach (Call item in calls)
			{
				if (pageIndex == 1)
				{
					if (item.callstep_id == 1)
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
				else
				{
					if (item.callstep_id == 6 || item.callstep_id == 7||item.callstep_id == 2 || item.callstep_id == 3)
					{
						string[] callstatuses = { "requests","missed","declined","scheduled","active","completed","cancelled"};
						veggies.Add(new CallDataTemplete
						{
							callId = item.call_id,
							callType = "IN " + item.booking_type.ToUpper(),
							requesteddate = callstatuses[item.callstep_id - 1],
							callStatus = Functions.getDateFormatStringByTime(item.requested_date),
							serviceImgUrl = item.serviceInfo.service_img_icon,
							categoryName = item.serviceInfo.category.category_name,
							productName = item.serviceInfo.service_name,
							address = item.clientInfo.address + " " + item.clientInfo.city + ", " + item.clientInfo.state + " " + item.clientInfo.zip
						});
						continue;
					}
				}
				index++;
			}
			if (index == calls.Count && calls.Count != 0)
				App.Current.MainPage.DisplayAlert("Warning", "You don't have any calls requests in this moment.", "OK");
			lstView.ItemsSource = veggies;
		}

		public Call findCallById(int id) 
		{
			foreach (Call item in calls)
				if (id == item.call_id)
					return item;
			return null;
		}

		private async void OnAccept(object sender, EventArgs e)
		{
			selectedCall.callstep_id = 4;
			var result = await apiManager.updateCallStatus(selectedCall.nurse_id, selectedCall.call_id, selectedCall.callstep_id);
			if (((string)result).Contains("success"))
			{
				await Navigation.PushAsync(new RequestAcceptedPage(selectedCall));
			}
			else 
			{
				await App.Current.MainPage.DisplayAlert("Warning",result + "","OK");	
			}
		}

		private async void OnDecline(object sender, EventArgs e)
		{
			selectedCall.callstep_id = 3;
			var result = await apiManager.updateCallStatus(selectedCall.nurse_id, selectedCall.call_id, selectedCall.callstep_id);
			if (((string)result).Contains("success"))
			{
				veggies.Remove(selectedTemp);
				lstView.ItemsSource = veggies;
			}
			else
			{
				await App.Current.MainPage.DisplayAlert("Warning", result + "", "OK");
			}
		}

		private void OnDetail(object sender, EventArgs e)
		{
			Navigation.PushAsync(new RequestDetailPage(selectedCall));
		}
	}
}
