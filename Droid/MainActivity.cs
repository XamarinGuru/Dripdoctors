using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using PayPal.Forms;
using PayPal.Forms.Abstractions;
using PayPal.Forms.Abstractions.Enum;

namespace Dripdoctors.Droid
{
	[Activity(Label = "Dripdoctors.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		//static readonly File file = new File(Environment.GetExternalStoragePublicDirectory(Environment.DirectoryPictures), "tmp.jpg");

		protected override void OnCreate(Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;
			base.OnCreate(bundle);

			global::Xamarin.Forms.Forms.Init(this, bundle);
			Xamarin.FormsMaps.Init(this, bundle);
			FFImageLoading.Forms.Droid.CachedImageRenderer.Init();

			CrossPayPalManager.Init(new PayPalConfiguration(PayPalEnvironment.Sandbox, "AU02h6m-zIc4oyBfTgCUSGe1NawxFSoGYUQS6Dz_5YYZXxuQx5A6HEgKTSeZeSrDKbE0ojknO4ezZZF_")
			{
				//If you want to accept credit cards
				AcceptCreditCards = true,
				//Your business name
				MerchantName = "Dripdoctors",
				//Your privacy policy Url
				MerchantPrivacyPolicyUri = "https://www.example.com/privacy",
				//Your user agreement Url
				MerchantUserAgreementUri = "https://www.example.com/legal",

				// OPTIONAL - ShippingAddressOption (Both, None, PayPal, Provided)
				ShippingAddressOption = ShippingAddressOption.Both,

				// OPTIONAL - Language: Default languege for PayPal Plug-In
				Language = "en",

				// OPTIONAL - PhoneCountryCode: Default phone country code for PayPal Plug-In
				PhoneCountryCode = "1",
			}
			);
			LoadApplication(new App());
		}

		protected override void OnActivityResult(int requestCode, Result resultCode, Android.Content.Intent data)
		{
			base.OnActivityResult(requestCode, resultCode, data);
			PayPalManagerImplementation.Manager.OnActivityResult(requestCode, resultCode, data);
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			PayPalManagerImplementation.Manager.Destroy();
		}
	}
}

