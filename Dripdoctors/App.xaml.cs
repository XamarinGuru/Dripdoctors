using Xamarin.Forms;
using FFImageLoading.Forms;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace Dripdoctors
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

			MainPage = new NavigationPage(new SignInPage());
		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}

		public static void showClientStoryboard() 
		{
			var navigation = new NavigationPage(new ClientMainPage(1));
			Application.Current.MainPage = navigation;

		}

		public async static void showNurseStroyboard() {


			var result = await Singleton.sharedInstance().getNurseStatus();
			if (result)
			{
				if (Singleton.sharedInstance().nurseId != -1)
				{
					var isThread = true;
					int timeStampValue = 300;
					if (Singleton.sharedInstance().nureseStatus == "Unavailable")
					{
						isThread = false;
					}
					else
					{
						isThread = true;
						if (Singleton.sharedInstance().nureseStatus == "On Call")
						{
							timeStampValue = 5;
						}
					}
					Singleton.sharedInstance().locationManager.setThreadValues(isThread, timeStampValue);
				}

				var navigation = new NavigationPage(new NurseMainPage())
				{
					BarBackgroundColor = Color.FromHex("#01b3f0"),
					BarTextColor = Color.White,
				};
				Application.Current.MainPage = navigation;
			}
		}

		public static void showAdminStroyboard() { 
		
		}
	}
}

