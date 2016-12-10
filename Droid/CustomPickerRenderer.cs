using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Dripdoctors.Droid;

[assembly : ExportRenderer(typeof(Picker), typeof(CustomPickerRenderer))]
namespace Dripdoctors.Droid
{
	public class CustomPickerRenderer : PickerRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
		{
			base.OnElementChanged(e);
			Control?.SetBackgroundColor(Android.Graphics.Color.Transparent);
		}
	}
}
