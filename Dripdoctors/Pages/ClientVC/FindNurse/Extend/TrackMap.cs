using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
namespace Dripdoctors
{
	public class TrackMap : FindMap
	{
		public Dictionary<string, List<Position>> RouteCoordinates { get; set; }
		public static readonly BindableProperty CurValueProperty = BindableProperty.Create("RouteCoordinates", typeof(double), typeof(TrackMap), 0d);

		public TrackMap()
		{
			RouteCoordinates = new Dictionary<string, List<Position>>();
		}

		public void addPosition(string nurseId, Position position) {
			if (!RouteCoordinates.ContainsKey(nurseId))
			{
				var positionArray = new List<Position>();
				positionArray.Add(position);
				RouteCoordinates.Add(nurseId, positionArray);
			}
			else {
				if (RouteCoordinates[nurseId].Count > 16) {
					RouteCoordinates[nurseId].RemoveAt(0);
				}
				RouteCoordinates[nurseId].Add(position);
			}
		}
	}
}
