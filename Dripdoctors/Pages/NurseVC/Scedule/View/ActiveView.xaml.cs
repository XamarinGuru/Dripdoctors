using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Dripdoctors
{
	public partial class ActiveView : ContentView
	{
		public EventHandler OnActive;
		public ActiveView()
		{
			InitializeComponent();
			activeButton.Clicked += OnActiveClieked;
			cancelButton.Clicked += (sender, e) => {
				Navigation.PopAsync();
			};
		}

		private void OnActiveClieked(object sender, EventArgs e)
		{
			if (OnActive != null) {
				OnActive(this, new EventArgs());
			}
		}
	}
}
