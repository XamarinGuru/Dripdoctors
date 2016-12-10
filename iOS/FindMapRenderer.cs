using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using CoreGraphics;
using Dripdoctors;
using Dripdoctors.iOS;
using MapKit;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.iOS;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(FindMap), typeof(FindMapRenderer))]
namespace Dripdoctors.iOS
{
	public class FindMapRenderer : MapRenderer
	{
		ElementChangedEventArgs<View> view;
		List<CustomPin> customPins;
		bool isThread = true;
		protected override void OnElementChanged(ElementChangedEventArgs<View> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement != null)
			{
				var nativeMap = Control as MKMapView;
				nativeMap.GetViewForAnnotation = null;
				nativeMap.CalloutAccessoryControlTapped -= OnCalloutAccessoryControlTapped;
				nativeMap.DidSelectAnnotationView -= OnDidSelectAnnotationView;
				nativeMap.DidDeselectAnnotationView -= OnDidDeselectAnnotationView;
			}

			if (e.NewElement != null)
			{
				view = e;
				new System.Threading.Thread(new System.Threading.ThreadStart(() =>
				{
					InvokeOnMainThread(() =>
					{
						OnTimer();
					});
				})).Start();
			}
		}


		public async void OnTimer()
		{
			while (isThread)
			{
				var formsMap = (FindMap)view.NewElement;
				var nativeMap = Control as MKMapView;
				customPins = formsMap.CustomPins;
				if (customPins != null)
				{
					nativeMap.GetViewForAnnotation = GetViewForAnnotation;
					nativeMap.CalloutAccessoryControlTapped += OnCalloutAccessoryControlTapped;
					nativeMap.DidSelectAnnotationView += OnDidSelectAnnotationView;
					nativeMap.DidDeselectAnnotationView += OnDidDeselectAnnotationView;
					isThread = false;
				}
				await Task.Delay(200);
			}
		}

		MKAnnotationView GetViewForAnnotation(MKMapView mapView, IMKAnnotation annotation)
		{
			MKAnnotationView annotationView = null;

			if (annotation is MKUserLocation)
				return null;

			var anno = annotation as MKPointAnnotation;
			var customPin = GetCustomPin(anno);
			if (customPin == null)
			{
				return null;
			}

			annotationView = mapView.DequeueReusableAnnotation(customPin.Id);
			if (annotationView == null)
			{
				annotationView = new CustomMKAnnotationView(annotation, customPin.Id, customPin.Url);
				((CustomMKAnnotationView)annotationView).Id = customPin.Id;
			}
			annotationView.CanShowCallout = true;

			return annotationView;
		}

		CustomPin GetCustomPin(MKPointAnnotation annotation)
		{
			var position = new Position(annotation.Coordinate.Latitude, annotation.Coordinate.Longitude);
			foreach (var pin in customPins)
			{
				if (pin.Pin.Position == position)
				{
					return pin;
				}
			}
			return null;
		}

		void OnCalloutAccessoryControlTapped(object sender, MKMapViewAccessoryTappedEventArgs e)
		{
			var customView = e.View as CustomMKAnnotationView;
			if (!string.IsNullOrWhiteSpace(customView.Url))
			{
				UIApplication.SharedApplication.OpenUrl(new Foundation.NSUrl(customView.Url));
			}
		}

		void OnDidSelectAnnotationView(object sender, MKAnnotationViewEventArgs e)
		{
			var customView = e.View as CustomMKAnnotationView;
			if (customView == null) return;
			var formsMap = (FindMap)view.NewElement;
			var pin = findCustomPin(customView.Id);
			pin.OnPinClicked();
		}

		CustomPin findCustomPin(string id)
		{
			var formsMap = (FindMap)view.NewElement;
			foreach (CustomPin item in formsMap.CustomPins) {
				if (item.Id == id) {
					return item;
				}
			}
			return null;
		}

		void OnDidDeselectAnnotationView(object sender, MKAnnotationViewEventArgs e)
		{
			if (!e.View.Selected)
			{
				
			}
		}
	}
}
