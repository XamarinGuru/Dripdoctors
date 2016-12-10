using System;

namespace Dripdoctors
{
	public class Call
	{
		public int call_id { get; set; }
		public int nurse_id { get; set; }
		public int callstep_id { get; set; }
		public string active_Call { get; set; }
		public string requested_date { get; set; }
		public string userid { get; set; }
		public int booking_id { get; set; }
		public string booking_type { get; set; }
		public string gps { get; set; }
		public double gps_latitud { get; set; }
		public double gps_longitud { get; set; }
		public string client_address { get; set; }
		public int clinic_id { get; set; }
		public string time_type { get; set; }
		public string booking_date { get; set; }
		public string booking_time { get; set; }
		public string amount { get; set; }
		public ServiceItem serviceInfo { get; set; }
		public int process_id { get; set; }
		public string status { get; set; }
		public string canceled { get; set; }
		public string review_rate { get; set;}
		public string created{get; set;}
		public Clinic clinicInfo { get; set;}
		public User clientInfo { get; set; }
	}
}

