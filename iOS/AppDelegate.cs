using Foundation;
using UIKit;
using PayPal.Forms;
using PayPal.Forms.Abstractions;
using PayPal.Forms.Abstractions.Enum;
namespace Dripdoctors.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			Rg.Plugins.Popup.IOS.Popup.Init(); // Init Popup
			global::Xamarin.Forms.Forms.Init();
			Xamarin.FormsMaps.Init();
			FFImageLoading.Forms.Touch.CachedImageRenderer.Init();

			CrossPayPalManager.Init(new PayPalConfiguration(PayPalEnvironment.Sandbox,"AU02h6m-zIc4oyBfTgCUSGe1NawxFSoGYUQS6Dz_5YYZXxuQx5A6HEgKTSeZeSrDKbE0ojknO4ezZZF_")
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

			return base.FinishedLaunching(app, options);
		}
	}
}

