using System;
using Xamarin.Forms;
using FFImageLoading.Forms;


namespace Dripdoctors.Carousel
{
	public class HomeView : ContentView
	{
		public event EventHandler OnClick;
		public event EventHandler OnInfoClick;

		public HomeView()
		{
			VerticalOptions = LayoutOptions.FillAndExpand;
			HorizontalOptions = LayoutOptions.FillAndExpand;
			var icon = new CachedImage
			{
				HorizontalOptions = LayoutOptions.End,
				WidthRequest = 30,
				HeightRequest = 30,
				Source = ImageSource.FromFile("info_icon.png")
			};
			var image = new CachedImage {
				
			};

			var label = new Label
			{
				TextColor = new Color(0, 0, 0),
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				FontSize = 30,
				FontAttributes = FontAttributes.Bold
			};
			image.SetBinding(CachedImage.SourceProperty, "Image");
			image.SetBinding(CachedImage.WidthRequestProperty, "WidthRequest");
			image.SetBinding(CachedImage.HeightRequestProperty,"HeightRequest");
			label.SetBinding(Label.TextProperty, "Price");

			var tapGestureRecognizer1 = new TapGestureRecognizer();
			tapGestureRecognizer1.Tapped += OnClicked1;
			image.GestureRecognizers.Add(tapGestureRecognizer1);

			var tapGestureRecognizer2 = new TapGestureRecognizer();
			tapGestureRecognizer2.Tapped += OnClicked2;
			icon.GestureRecognizers.Add(tapGestureRecognizer2);

			var layout = new StackLayout {
				Margin = 10,
				BackgroundColor = new Color(255, 255, 255),
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Padding = 5,
				Children = {
					icon,
					image,
					label
				}
			};
			//layout.SetBinding(StackLayout.WidthRequestProperty,"WidthRequest");
			//layout.SetBinding(StackLayout.HeightRequestProperty, "HeightRequest");
			Content = layout;
		}

		private void OnClicked1(object sender, EventArgs e) {
			if (OnClick != null)
			{
				OnClick(this, new EventArgs());
			}
		}

		private void OnClicked2(object sender, EventArgs e) {
			if (OnInfoClick != null) {
				OnInfoClick(this, new EventArgs());
			}
		}
	}
}

