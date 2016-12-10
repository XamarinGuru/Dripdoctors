using System;
namespace Dripdoctors
{
	public class ServiceItem : Object
	{
		public string service_id { get; set; }
		public string service_name { get; set; }
		public string category_id { get; set; }
		public string service_description { get; set; }
		public string price { get; set; }
		public string service_img { get; set; }
		public string service_img_icon { get; set; }
		public string mobile_description { get; set; }
		public int inhouse_call { get; set; }
		public int van_call { get; set;}
		public string discount { get; set; }
		public string delivery_fee { get; set; }
		public ServiceCategory category { get; set; }
	}
}

