using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Dripdoctors.Droid;

[assembly : ExportRenderer(typeof(TimePicker), typeof(CustomTimePickerRenderer))]
namespace Dripdoctors.Droid
{
	public class CustomTimePickerRenderer : TimePickerRenderer
	{

		protected override void OnElementChanged(ElementChangedEventArgs<TimePicker> e)
		{
			base.OnElementChanged(e);
			Control?.SetBackgroundColor(Android.Graphics.Color.Transparent);
		}
	}
}
