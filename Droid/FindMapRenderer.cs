using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Widget;
using Dripdoctors;
using Dripdoctors.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;
using Android.Graphics;


[assembly:ExportRenderer (typeof(FindMap), typeof(FindMapRenderer))]
namespace Dripdoctors.Droid
{
	public class FindMapRenderer : MapRenderer, GoogleMap.IInfoWindowAdapter, IOnMapReadyCallback
	{
		GoogleMap map;
		List<CustomPin> customPins;
		bool isDrawn = false;
		Xamarin.Forms.Platform.Android.ElementChangedEventArgs<View> view;
		protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<View> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement != null)
			{
				isDrawn = true;
				map.InfoWindowClick -= OnInfoWindowClick;
			}

			if (e.NewElement != null)
			{
				view = e;
				((MapView)Control).GetMapAsync(this);
			}
		}

		public void OnMapReady(GoogleMap googleMap)
		{
			map = googleMap;
			map.InfoWindowClick += OnInfoWindowClick;
			map.SetInfoWindowAdapter(this);
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName.Equals("VisibleRegion") && !isDrawn)
			{
				map.Clear();
				var formsMap = (FindMap)sender;
				customPins = formsMap.CustomPins;
				if (customPins != null)
				{

					foreach (var pin in customPins)
					{
						var marker = new MarkerOptions();
						marker.SetPosition(new LatLng(pin.Pin.Position.Latitude, pin.Pin.Position.Longitude));
						marker.SetTitle(pin.Pin.Label);
						marker.SetSnippet(pin.Id);
						var bmp = getMarkerFromUrl(pin.Url);
						if (bmp == null) return;
						marker.SetIcon(BitmapDescriptorFactory.FromBitmap(bmp));

						map.AddMarker(marker);
					}
					isDrawn = true;
				}
			}
		}

		private Bitmap getBitmapFromUrl(string url)
		{
			if (string.IsNullOrWhiteSpace(url))
				throw new ArgumentException("url must not be null, empty, or whitespace");

			Uri imageUri;
			if (!Uri.TryCreate(url, UriKind.Absolute, out imageUri))
				throw new ArgumentException("Invalid url");

			try
			{
				WebRequest request = HttpWebRequest.Create(imageUri);
				request.Timeout = 10000;

				WebResponse response = request.GetResponse();
				Stream inputStream = response.GetResponseStream();
				Bitmap bmImg = BitmapFactory.DecodeStream(inputStream);
				return bmImg;
			}
			catch (Exception)
			{
				return null;
			}
		}


		private Bitmap getMarkerFromUrl(string url) {
			
			Bitmap.Config conf = Bitmap.Config.Argb8888;
			Bitmap bmp = Bitmap.CreateBitmap(300, 300, conf);
			Canvas canvas = new Canvas(bmp);

			Bitmap back = BitmapFactory.DecodeResource(Context.Resources,Resource.Drawable.pin2);
			//canvas.DrawBitmap(back, 0, 0, null);

			var r0 = new Rect(0, 0, back.Width, back.Height);
			var r1 = new Rect(0, 0, 128, 200);
			canvas.DrawBitmap(back, r0, r1, null);


			var fs = getBitmapFromUrl(url);
			if (fs == null) return null;
			var roud = CreateRoundedBitmap(fs, 5);
			if (roud == null) return null;

			var r2 = new Rect(0, 0, roud.Width, roud.Height);
			var r3 = new Rect(5, 5, 122, 122);
			canvas.DrawBitmap(roud, r2, r3, null);
			return bmp;

		}

		public Bitmap CreateRoundedBitmap(Bitmap bitmap, int padding)
		{
			Bitmap output = Bitmap.CreateBitmap(bitmap.Width, bitmap.Height, Bitmap.Config.Argb8888);
			var canvas = new Canvas(output);

			var paint = new Paint();
			var rect = new Rect(0, 0, bitmap.Width, bitmap.Height);
			var rectF = new RectF(rect);

			paint.AntiAlias = true;
			canvas.DrawARGB(0, 0, 0, 0);
			canvas.DrawOval(rectF, paint);
			paint.SetXfermode(new PorterDuffXfermode(PorterDuff.Mode.SrcIn));

			canvas.DrawBitmap(bitmap, rect, rect, paint);
			return output;
		}

		protected override void OnLayout(bool changed, int l, int t, int r, int b)
		{
			base.OnLayout(changed, l, t, r, b);

			if (changed)
			{
				isDrawn = false;
			}
		}

		void OnInfoWindowClick(object sender, GoogleMap.InfoWindowClickEventArgs e)
		{
			var customPin = GetCustomPin(e.Marker);
		}

		CustomPin findCustomPin(string id)
		{
			var formsMap = (FindMap)view.NewElement;
			foreach (CustomPin item in formsMap.CustomPins)
			{
				if (item.Id == id)
				{
					return item;
				}
			}
			return null;
		}

		public Android.Views.View GetInfoContents(Marker marker)
		{
			var pin = findCustomPin(marker.Snippet	);
			if (pin != null)
			{
				pin.OnPinClicked();
			}
			return null;
		}

		public Android.Views.View GetInfoWindow(Marker marker)
		{
			return null;
		}

		CustomPin GetCustomPin(Marker annotation)
		{
			var position = new Position(annotation.Position.Latitude, annotation.Position.Longitude);
			foreach (var pin in customPins)
			{
				if (pin.Pin.Position == position)
				{
					return pin;
				}
			}
			return null;
		}
	}
}

