using System;
using System.Diagnostics;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Dripdoctors;
using Dripdoctors.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Maps.Android;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using Android.Content;
using Android.Widget;
using Xamarin.Forms.Maps;
using Android.Graphics;


[assembly:ExportRenderer (typeof(TrackMap), typeof(TrackMapRenderer))]
namespace Dripdoctors.Droid
{
	public class TrackMapRenderer : MapRenderer, IOnMapReadyCallback
	{
		Xamarin.Forms.Platform.Android.ElementChangedEventArgs<View> view;
		bool isThread = false;
		List<CustomPin> customPins;
		public TrackMapRenderer() { 
		
		}

		protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<View> e)
		{
			base.OnElementChanged(e);
			Console.WriteLine("OnElementChanged");
			if (e.OldElement != null)
			{
				isThread = false;
				view = null;
			}

			if (e.NewElement != null)
			{
				isThread = true;
				view = e;
				var formsMap = (FindMap)view.NewElement;
				customPins = formsMap.CustomPins;
				Xamarin.Forms.Device.StartTimer(TimeSpan.FromSeconds(3), OnTimer);
			}
		}

		public void OnMapReady(GoogleMap googleMap)
		{
			Console.WriteLine("OnMapReady");
			if (googleMap == null || view == null) {
				return;
			}
			googleMap.Clear();
			var formsMap = (TrackMap)view.NewElement;
			var keys = formsMap.RouteCoordinates.Keys;
			foreach (var key in keys)
			{
				var positionArray = formsMap.RouteCoordinates[key];
				var polylineOptions = new PolylineOptions();
				polylineOptions.InvokeColor(0x66FF0000);
				foreach (var position in positionArray)
				{
					polylineOptions.Add(new LatLng(position.Latitude, position.Longitude));
				}
				googleMap.AddPolyline(polylineOptions);
				var marker = new MarkerOptions();
				var pin = findPin(key);
				marker.SetPosition(new LatLng(pin.Pin.Position.Latitude, pin.Pin.Position.Longitude));
				marker.SetTitle(pin.Pin.Label);
				marker.SetSnippet(pin.Id);
				var bmp = getMarkerFromUrl(pin.Url);
				if (bmp == null) return;
				marker.SetIcon(BitmapDescriptorFactory.FromBitmap(bmp));
				googleMap.AddMarker(marker);
			}
		}

		private Bitmap getMarkerFromResource()
		{

			Bitmap.Config conf = Bitmap.Config.Argb8888;
			Bitmap bmp = Bitmap.CreateBitmap(56, 80, conf);
			Canvas canvas = new Canvas(bmp);

			Bitmap back = BitmapFactory.DecodeResource(Context.Resources, Resource.Drawable.nurse_pin);

			var r0 = new Rect(0, 0, back.Width, back.Height);
			var r1 = new Rect(0, 0, 56, 80);
			canvas.DrawBitmap(back, r0, r1, null);
			return bmp;

		}

		private CustomPin findPin(string id) {
			foreach (CustomPin pin in customPins) {
				if (pin.Id == id)
					return pin;
			}
			return null;
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

		private Bitmap getMarkerFromUrl(string url)
		{

			Bitmap.Config conf = Bitmap.Config.Argb8888;
			Bitmap bmp = Bitmap.CreateBitmap(300, 300, conf);
			Canvas canvas = new Canvas(bmp);

			Bitmap back = BitmapFactory.DecodeResource(Context.Resources, Resource.Drawable.pin2);
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

		private  bool OnTimer() {
			if (!isThread) {
				return false;
			}
			if (((MapView)Control) != null)
			{
				((MapView)Control).GetMapAsync(this);
			}
			return true;
		}
	}
}

