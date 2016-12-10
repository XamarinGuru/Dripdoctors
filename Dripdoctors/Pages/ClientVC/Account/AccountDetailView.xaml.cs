using System;
using Xamarin.Forms;

namespace Dripdoctors
{
	public partial class AccountDetailView : BaseContentView
	{
		public AccountDetailView(BaseElementInterface p) : base(p)
		{
			InitializeComponent();
			initBody();
		}

		private void initBody() {
			txtEmail.Text = Singleton.sharedInstance().user.email;
			txtPassword.Text = Singleton.sharedInstance().user.password;
		}

		public override Object RightNavButton()
		{
			return new CustomButton
			{
				TextColor = Color.White,
				HorizontalOptions = LayoutOptions.EndAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Text = "Edit"
			};
		}


		public override bool OnRightNavButton_Clicked()
		{
			Navigation.PushAsync(new AccountDetailEditPage());
			return false;
		}

	}
}
