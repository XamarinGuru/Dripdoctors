using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Dripdoctors.Droid;

[assembly : ExportRenderer(typeof(Entry), typeof(CustomEntryRenderer))]
namespace Dripdoctors.Droid
{
	public class CustomEntryRenderer : EntryRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
		{
			base.OnElementChanged(e);
			Control?.SetBackgroundColor(Android.Graphics.Color.Transparent);
		}
	}
}
