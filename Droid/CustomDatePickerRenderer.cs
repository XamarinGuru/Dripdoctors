using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Dripdoctors.Droid;

[assembly : ExportRenderer(typeof(DatePicker), typeof(CustomDatePickerRenderer))]
namespace Dripdoctors.Droid
{
	public class CustomDatePickerRenderer : DatePickerRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
		{
			base.OnElementChanged(e);
			Control?.SetBackgroundColor(Android.Graphics.Color.Transparent);
		}
	}
}
