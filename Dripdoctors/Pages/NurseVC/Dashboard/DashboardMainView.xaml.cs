using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Rg.Plugins.Popup.Extensions;

namespace Dripdoctors
{
	public partial class DashboardMainView : BaseContentView
	{
		APIManager apiManager;

		ListView lstView = null;
		ObservableCollection<CallDataTemplete> veggies;
		List<Call> calls = new List<Call>();
		bool isLoaded = false;
		int waitingTime = 0;
		public DashboardMainView(BaseElementInterface p) : base(p)
		{
			InitializeComponent();
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
					var selectedCall = findCallById(((CallDataTemplete)((CallCell)sender).BindingContext).callId);
					switch (selectedCall.callstep_id) {
						case 1:
							Navigation.PushAsync(new RequestDetailPage(selectedCall));
							break;
						case 2:
							Navigation.PushAsync(new RequestPreviewPage(selectedCall));
							break;
						case 3:
							Navigation.PushAsync(new RequestPreviewPage(selectedCall));
							break;
						case 4:
							Navigation.PushAsync(new CallDetailPage(selectedCall));
							break;
						case 5:
							Navigation.PushAsync(new CallDetailPage(selectedCall));
							break;
						case 6:
							Navigation.PushAsync(new RequestPreviewPage(selectedCall));
							break;
						case 7:
							Navigation.PushAsync(new RequestPreviewPage(selectedCall));
							break;
					}
				};
				return cell;
			});
			veggies = new ObservableCollection<CallDataTemplete>();
			listLayout.Children.Add(lstView);
			startLoader();
			loadBody();
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

		private async void loadBody()
		{
			veggies = new ObservableCollection<CallDataTemplete>();
			int index = 0;
			string[] callstatuses = { "REQUESTS", "MISSED", "DECLINED", "SCHEDULED", "ACTIVE", "COMPLETED", "CANCELLED" };
			if (calls.Count != 0) return;
			var result = await apiManager.getCalls(Singleton.sharedInstance().nurseId);
			if (result is List<Call>)
			{
				calls = (List<Call>)result;
				CallDataTemplete requestCall = null;
				CallDataTemplete scheduleCall = null;
				CallDataTemplete activeCall = null;

				foreach (Call item in calls)
				{
					string typeTitle = "";
					string callStatus = "";

					if (item.callstep_id == 5)
						Singleton.sharedInstance().currentActiveCall = item;
					if (item.callstep_id == 1)
					{
						typeTitle = "NEW REQUEST " + item.booking_type.ToUpper() + " CALL";
					}
					else {
						typeTitle = "IN " + item.booking_type.ToUpper() + " CALL " + callstatuses[item.callstep_id - 1];
					}
					if (item.callstep_id == 1 || item.callstep_id == 4 || item.callstep_id == 5)
					{
						callStatus = Functions.getExpireTime(item.booking_date + " " + item.booking_time);
					}
					if (item.callstep_id == 1)
					{
						if (requestCall == null)
						{
							requestCall = new CallDataTemplete
							{
								callId = item.call_id,
								callType = typeTitle,
								requesteddate = item.requested_date,
								callStatus = callStatus,
								serviceImgUrl = item.serviceInfo.service_img_icon,
								categoryName = item.serviceInfo.category.category_name,
								productName = item.serviceInfo.service_name,
								address = item.clientInfo.address + " " + item.clientInfo.city + ", " + item.clientInfo.state + " " + item.clientInfo.zip
							};
						}
						else {
							if (item.call_id > requestCall.callId) { 
								requestCall = new CallDataTemplete
								{
									callId = item.call_id,
									callType = typeTitle,
									requesteddate = item.requested_date,
									callStatus = callStatus,
									serviceImgUrl = item.serviceInfo.service_img_icon,
									categoryName = item.serviceInfo.category.category_name,
									productName = item.serviceInfo.service_name,
									address = item.clientInfo.address + " " + item.clientInfo.city + ", " + item.clientInfo.state + " " + item.clientInfo.zip
								};
							}
						}
						continue;
					}
					else if (item.callstep_id == 4)
					{
						if (scheduleCall == null)
						{
							scheduleCall = new CallDataTemplete
							{
								callId = item.call_id,
								callType = typeTitle,
								requesteddate = item.requested_date,
								callStatus = callStatus,
								serviceImgUrl = item.serviceInfo.service_img_icon,
								categoryName = item.serviceInfo.category.category_name,
								productName = item.serviceInfo.service_name,
								address = item.clientInfo.address + " " + item.clientInfo.city + ", " + item.clientInfo.state + " " + item.clientInfo.zip
							};
						}
						else {
							if (DateTime.Parse(item.requested_date) > DateTime.Parse(scheduleCall.requesteddate)) { 
								scheduleCall = new CallDataTemplete
								{
									callId = item.call_id,
									callType = typeTitle,
									requesteddate = item.requested_date,
									callStatus = callStatus,
									serviceImgUrl = item.serviceInfo.service_img_icon,
									categoryName = item.serviceInfo.category.category_name,
									productName = item.serviceInfo.service_name,
									address = item.clientInfo.address + " " + item.clientInfo.city + ", " + item.clientInfo.state + " " + item.clientInfo.zip
								};
							}
						}
						continue;
					}
					else if (item.callstep_id == 5){
						activeCall = new CallDataTemplete
						{
							callId = item.call_id,
							callType = typeTitle,
							requesteddate = item.requested_date,
							callStatus = callStatus,
							serviceImgUrl = item.serviceInfo.service_img_icon,
							categoryName = item.serviceInfo.category.category_name,
							productName = item.serviceInfo.service_name,
							address = item.clientInfo.address + " " + item.clientInfo.city + ", " + item.clientInfo.state + " " + item.clientInfo.zip
						};
						continue;
					}
					index++;
				}
				if (index == calls.Count && calls.Count > 0)
					await App.Current.MainPage.DisplayAlert("Warning", "You don't have any calls in this moment.", "OK");
				if (requestCall != null)
				{
					requestCall.requesteddate = Functions.getDateFormatStringByTime(requestCall.requesteddate);
					veggies.Add(requestCall);
				}
				if (scheduleCall != null)
				{
					scheduleCall.requesteddate = Functions.getDateFormatStringByTime(scheduleCall.requesteddate);
					veggies.Add(scheduleCall);
				}
				if (activeCall != null)
				{
					veggies.Add(activeCall);
					activeCall.requesteddate = Functions.getDateFormatStringByTime(activeCall.requesteddate);
				}

				stopLoader();
				lstView.ItemsSource = veggies;
			}
			else if (result == null){
				await App.Current.MainPage.DisplayAlert("Warning", "You don't have any calls in this moment.", "OK");
				stopLoader();
			}
			else
			{
				await App.Current.MainPage.DisplayAlert("Warning", (string)result, "OK");
				stopLoader();
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