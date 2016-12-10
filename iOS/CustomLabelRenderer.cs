using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using CoreGraphics;
using Foundation;
using Dripdoctors;
using Dripdoctors.iOS;

[assembly: ExportRenderer(typeof(CustomLabel), typeof(CustomLabelRenderer))]
namespace Dripdoctors.iOS
{
	public class CustomLabelRenderer : ViewRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<View> e)
		{
			base.OnElementChanged(e);
			var view = (CustomLabel)Element;
			if (view == null) return;

			UITextView uilabelleftside = new UITextView(new CGRect(0, 0, view.Width, view.Height));
			uilabelleftside.Text = view.Text;
			uilabelleftside.Font = UIFont.SystemFontOfSize((float)view.FontSize);
			uilabelleftside.Editable = false;
			uilabelleftside.TextAlignment = UITextAlignment.Center;
			uilabelleftside.DataDetectorTypes = UIDataDetectorType.All;
			uilabelleftside.BackgroundColor = UIColor.Clear;
			SetNativeControl(uilabelleftside);
		}
	}
}
