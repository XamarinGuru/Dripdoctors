using System;
namespace Dripdoctors
{
	public class Booking
	{
		public string userid { get; set; }
		public string booking_id { get; set; }
		public string booking_type { get; set; }
		public int gps { get; set; }
		public Double gps_latitud { get; set; }
		public Double gps_longitud { get; set; }
		public string client_address { get; set; }
		public string clinic_id { get; set; }

		public int time_type { get; set; }
		public string booking_date { get; set; }
		public string booking_time { get; set; }

		public ServiceItem service_id { get; set; }
		public int process_id { get; set; }
		public int status { get; set; }
		public int canceled { get; set; }
		public string nurse_id_assigned { get; set; }
		public string review_rate { get; set; }
		public string created { get; set; }
		public Nurse nurseInfo { get; set; }
		public Clinic clinicInfo { get; set; }

		public string transCode { get; set; }
		public string transType { get; set; }

		public DateTime bookingdateTime { get; set; }
		public ServiceItem productInfo { get; set; }
		public bool isEmptyProd { get; set; }
		public bool isEmptyService { get; set; }

		public ServiceCategory serviceInfo { get; set; }

		public string nurseId { get; set; }
	}
}

