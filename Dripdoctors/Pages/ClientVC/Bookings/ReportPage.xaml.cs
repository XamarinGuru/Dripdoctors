using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Dripdoctors
{
	public partial class ReportPage : ContentPage
	{
		public ReportPage()
		{
			InitializeComponent();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			NavigationPage.SetHasNavigationBar(this, false);
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();
		}
	}
}
