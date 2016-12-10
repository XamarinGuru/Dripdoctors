using System;
using Xamarin.Forms;

namespace Dripdoctors
{
	public partial class AccountProfileView : BaseContentView
	{
		public AccountProfileView(BaseElementInterface p) : base(p)
		{
			InitializeComponent();
			initBody();
		}

		private void initBody() {
			if (Singleton.sharedInstance().user != null)
			{
				lblName.Text = Singleton.sharedInstance().user.sname;
				lblPhone.Text = Singleton.sharedInstance().user.mobilenum;
				lblZip.Text = Singleton.sharedInstance().user.zip;
				lblAge.Text = "" + Singleton.sharedInstance().user.age;
				lblCity.Text = Singleton.sharedInstance().user.city;
				lblGender.Text = Singleton.sharedInstance().user.gender;
				lblCountry.Text = Singleton.sharedInstance().user.country;
				lblAddress.Text = Singleton.sharedInstance().user.address;
				if (Singleton.sharedInstance().user.img_url != null)
				{
					imgProfile.Source = ImageSource.FromUri(new Uri(Singleton.sharedInstance().user.img_url));
				}
			}
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
			Navigation.PushAsync(new AccountProfileEditPage());
			return false;
		}
	}
}
