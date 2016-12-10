using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Rg.Plugins.Popup.Services;
using Rg.Plugins.Popup.Pages;

namespace Dripdoctors
{
	public partial class RequestPopupView : PopupPage
	{
		public EventHandler OnDeclineClicked, OnAcceptClicked, OnDetailClicked;

		public RequestPopupView()
		{
			InitializeComponent();
			detailButton.Clicked += (sender, e) => {
				if (OnDetailClicked != null)
				{
					PopupNavigation.PopAsync();
					OnDetailClicked(this, new EventArgs());
				}
			};
			acceptButton.Clicked += (sender, e) =>
			{
				if (OnAcceptClicked != null)
				{
					PopupNavigation.PopAsync();
					OnAcceptClicked(this, new EventArgs());
				}
			};
			declineButton.Clicked += (sender, e) =>
			{
				if (OnDeclineClicked != null)
				{
					PopupNavigation.PopAsync();
					OnDeclineClicked(this, new EventArgs());
				}
			};
		}
	}
}
