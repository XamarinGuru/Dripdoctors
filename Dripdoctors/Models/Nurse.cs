using System;
namespace Dripdoctors
{
	public class Nurse
	{
		public string nurse_id { get; set;}
		public string userid { get; set;}
		public string fname { get; set; }
		public string sname { get; set; }
		public int online { get; set; }
		public int active { get; set; }
		public double last_longitud { get; set;}
		public double last_latitud { get; set;}
		public DateTime last_date_location { get; set;}
		public string review_average { get; set; }
		public int services_completed { get; set; }
		public NurseAvService[] services { get; set; }
		public string img_url { get; set;}
	}
}

