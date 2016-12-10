using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;

namespace Dripdoctors
{
	public partial class AccountMainView : BaseContentView
	{
		public AccountMainView(BaseElementInterface p) : base(p)
		{
			InitializeComponent();
			btnProfile.Clicked += OnProfileClicked;
			btnAccount.Clicked += OnAccountClicked;
			btnPayments.Clicked += OnPaymentsClicked;
			Xamarin.Forms.Device.StartTimer(TimeSpan.FromSeconds(.02), OnTimer);

		}

		private bool OnTimer() {
			updateMenu(1);
			loadBody(1);
			return false;
		}

		private void updateMenu(int index) {
			btnProfile.BackgroundColor = Color.FromHex("#c2c2c2");
			btnAccount.BackgroundColor = Color.FromHex("#c2c2c2");
			btnPayments.BackgroundColor = Color.FromHex("#c2c2c2");
			if (index == 1)
			{
				btnProfile.BackgroundColor = Color.FromHex("#01b3f0");
			}
			else if (index == 2)
			{
				btnAccount.BackgroundColor = Color.FromHex("#01b3f0");
			}
			else
			{
				btnPayments.BackgroundColor = Color.FromHex("#01b3f0");
			}
		}

		public void OnProfileClicked(object sender, EventArgs e)
		{
			loadBody(1);
			updateMenu(1);

		}

		public void OnAccountClicked(object sender, EventArgs e)
		{
			loadBody(2);
			updateMenu(2);
		}

		public void OnPaymentsClicked(object sender, EventArgs e)
		{
			updateMenu(3);
		}

		public void loadBody(int param) { 
			accountBodyLayout.Children.Clear();
			switch (param) {
				case 1:
					AccountProfileView apv = new AccountProfileView(this);
					activeChild = apv;
					activeChildIndex = 1;
					OnChildViewSelectionChanged();
					accountBodyLayout.Children.Add(apv);
					break;
				case 2:
					AccountDetailView adv = new AccountDetailView(this);
					activeChild = adv;
					activeChildIndex = 2;
					OnChildViewSelectionChanged();
					accountBodyLayout.Children.Add(adv);
					break;
				default:
					break;
			}
		}
	}
}

