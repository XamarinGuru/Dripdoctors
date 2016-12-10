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
	public partial class AlertPopup : PopupPage
	{
		private AlertPopup()
		{
			InitializeComponent();
			agreeButton.Clicked += (sender, e) => { 
				PopupNavigation.PopAsync();
			};
		}

		public AlertPopup(string title, string content, string button) : this() {
			titleLabel.Text = title;
			contentLabel.Text = content;
			agreeButton.Text = button;
		}
	}
}
