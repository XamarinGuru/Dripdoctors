using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Dripdoctors;
using Dripdoctors.Droid;

[assembly: ExportRenderer(typeof(CustomButton), typeof(CustomButtonRenderer))]
namespace Dripdoctors.Droid
{
	public class CustomButtonRenderer : ButtonRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
		{
			base.OnElementChanged(e);
			Control?.SetBackgroundColor(Android.Graphics.Color.Transparent);
		}
	}
}
