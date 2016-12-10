using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dripdoctors;
using Dripdoctors.iOS;
using MapKit;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.iOS;
using Xamarin.Forms.Platform.iOS;
using CoreLocation;


[assembly: ExportRenderer(typeof(TrackMap), typeof(TrackMapRenderer))]
namespace Dripdoctors.iOS
{
	public class TrackMapRenderer : MapRenderer
	{
		List<CustomPin> customPins;
		MKPolylineRenderer polylineRenderer;
		ElementChangedEventArgs<View> view;
		bool isThread = false;
		MKMapView nativeMap;
		protected override void OnElementChanged(ElementChangedEventArgs<View> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement != null)
			{
				nativeMap = Control as MKMapView;
				nativeMap.OverlayRenderer = null;
				isThread = false;
			}
			if (e.NewElement != null)
			{
				view = e;
				nativeMap = Control as MKMapView;
				var formsMap = (TrackMap)view.NewElement;
				var keys = formsMap.RouteCoordinates.Keys;

				customPins = formsMap.CustomPins;
				foreach (var key in keys)
				{
					int index = 0;
					var positionArray = formsMap.RouteCoordinates[key];
					CLLocationCoordinate2D[] coords = new CLLocationCoordinate2D[formsMap.RouteCoordinates[key].Count];
					foreach (var position in positionArray)
					{
						coords[index] = new CLLocationCoordinate2D(position.Latitude, position.Longitude);
						index++;
					}
					var routeOverlay = MKPolyline.FromCoordinates(coords);
					nativeMap.AddOverlay(routeOverlay);
				};

				nativeMap.GetViewForAnnotation = GetViewForAnnotation;
				nativeMap.OverlayRenderer = GetOverlayRender;
				Xamarin.Forms.Device.StartTimer(TimeSpan.FromSeconds(3),OnTimer);
				isThread = true;
			}
		}

		public bool OnTimer() {
			if (!isThread) return false;
			var formsMap = (TrackMap)view.NewElement;
			Console.WriteLine("RendererThread:");

			var keys = formsMap.RouteCoordinates.Keys;

			customPins = formsMap.CustomPins;

			foreach (var key in keys)
			{
				int index = 0;
				var positionArray = formsMap.RouteCoordinates[key];
				CLLocationCoordinate2D[] coords = new CLLocationCoordinate2D[formsMap.RouteCoordinates[key].Count];
				foreach (var position in positionArray)
				{
					coords[index] = new CLLocationCoordinate2D(position.Latitude, position.Longitude);
					index++;
				}
				var routeOverlay = MKPolyline.FromCoordinates(coords);
				nativeMap.AddOverlay(routeOverlay);
			};
			nativeMap.OverlayRenderer = GetOverlayRender;
			if (nativeMap.OverlayRenderer == null)
				return false;
			return true;
		}

		MKOverlayRenderer GetOverlayRender(MKMapView mapView, IMKOverlay overlay) { 
			
			if (polylineRenderer == null)
			{
				MKPolyline polyLine = overlay as MKPolyline;
				if (polyLine == null) {
					return null;
				}
				polylineRenderer = new MKPolylineRenderer(polyLine);
				polylineRenderer.FillColor = UIColor.Blue;
				polylineRenderer.StrokeColor = UIColor.Red;
				polylineRenderer.LineWidth = 3;
				polylineRenderer.Alpha = 0.4f;
			}
			return polylineRenderer;
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
			if (customPins == null)
				return null;
			foreach (var pin in customPins)
			{
				bool latIsEqual = false, lonIsEqual = false;
				if (Math.Round(position.Latitude * 1000).Equals(Math.Round(pin.Pin.Position.Latitude * 1000)))
					latIsEqual = true;
				if (Math.Round(position.Longitude * 1000).Equals(Math.Round(pin.Pin.Position.Longitude * 1000)))
					lonIsEqual = true;	
				if (latIsEqual && lonIsEqual)
				{
					return pin;
				}
			}
			return null;
		}
	}
}
