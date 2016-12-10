using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace Dripdoctors.Carousel
{
	public class HomeViewModel : BaseViewModel
	{
		public HomeViewModel() { }
		public string ID { get; set; }
		public ImageSource Image { get; set; }
		public string Price { get; set; }
		public string Discription { get; set; }
		public double WidthRequest { get; set; }
		public double HeightRequest { get; set; }
	}
}
