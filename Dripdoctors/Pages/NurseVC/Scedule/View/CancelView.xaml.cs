using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Dripdoctors
{
	public partial class CancelView : ContentView
	{
		public EventHandler OnCancel;
		public CancelView()
		{
			InitializeComponent();
			cancelCallButton.Clicked += (sender, e) =>
			{
				if (OnCancel != null)
				{
					OnCancel(this, new EventArgs());
				}
			};
			cancelButton.Clicked += (sender, e) => { 
				Navigation.PopAsync();
			};
		}
	}
}
