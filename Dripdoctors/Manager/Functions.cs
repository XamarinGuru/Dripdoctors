using System;
using System.IO;
using System.Globalization;
using Xamarin.Forms;


namespace Dripdoctors
{
	public class Functions
	{
		public Functions()
		{
			
		}

		// ------ convert image to string
		public static string ImageToBase64(System.Drawing.Image image)
		{
			using (MemoryStream ms = new MemoryStream())
			{
				image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
				byte[] imageBytes = ms.ToArray();
				string base64string = Convert.ToBase64String(imageBytes);
				return base64string;
			}
		}

		// ------ convert string to image
		public static ImageSource Base64ToImage(string base64string)
		{
			byte[] imageBytes = Convert.FromBase64String(base64string);
			MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
			ms.Write(imageBytes, 0, imageBytes.Length);
			var image = ImageSource.FromStream(() => ms);
			return image;
		}

		// ------ convert date to year
		public static int getYear(DateTime date) {
			return date.Year;
		}

		// ------ convert date to dateformat
		public static string getDateFormat(DateTime date) {
			string month = "" + date.Month;
			string day = "" + date.Day;
			if (date.Month < 10)
				month = "0" + date.Month;
			if (date.Day < 10)
				day = "0" + date.Day;
			return date.Year + "-" + month + "-" + day;
		}

		// ------ convert date to timeformat 
		public static string getTimeFormat(DateTime date) {
			string hour = "" + date.Hour;
			string min = "" + date.Minute;
			string sec = "" + date.Second;
			if (date.Hour < 10) {
				hour = "0" + date.Hour;
			}
			if (date.Minute < 10)
			{
				min = "0" + date.Minute;
			}
			if (date.Second < 10)
			{
				sec = "0" + date.Second;
			}
			return hour + ":" + min + ":" + sec;
		}

		// ------ convert date to timeformat : January 10
		public static string getDateFormatByString(string date) {
			string[] monthName = {"January","February","March","April","May","June","July","August","September","October","November","December"};
			char[] arr = {'-'};
			var splitArr = date.Split(arr);
			int monthInt = Int32.Parse(splitArr[1]);
			int dayInt = Int32.Parse(splitArr[2]);
			return monthName[monthInt - 1] + " " + dayInt;
		}

		// ------ Format date string : Friedy 16 Oct, 2016
		public static string getDateFormatStringByFormat(string date, char format) {
			var day = DateTime.Parse(date).DayOfWeek;
			string[] monthNames = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
			return day + " " + monthNames[DateTime.Parse(date).Month - 1] + ". " + DateTime.Parse(date).Day + ", " + DateTime.Parse(date).Year;  
		}

		// ------ Format date string : Today 2:30 PM
		public static string getDateFormatStringByTime(string date)
		{
			string[] monthNames = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
			int hour = DateTime.Parse(date).TimeOfDay.Hours;
			int min = DateTime.Parse(date).TimeOfDay.Minutes;
			string reStr = "";
			hour = hour > 12 ? (hour - 12) : hour;
			if (hour < 12)
			{
				reStr = hour + ":" + min + " AM";
			}
			else
				reStr = hour + ":" + min + " PM";
			return monthNames[DateTime.Parse(date).Month - 1] + " " +DateTime.Parse(date).Day + ", " + reStr;
		}

		//----- Get Expire Time -----
		public static string getExpireTime(string bookingDate) 
		{
			string hour = "", min = "", sec = "";
			var curdate = DateTime.Now;
			TimeSpan duration = DateTime.Parse(bookingDate).Subtract(curdate);
			if (duration.TotalHours > 0)
				hour = Math.Round(duration.TotalHours) + " hours ";
			if ((duration.TotalMinutes % 60) > 0)
				min = Math.Round(duration.TotalMinutes % 60) + " min ";
			if ((duration.TotalSeconds % 3600) % 60 > 0)
				sec = Math.Round((duration.TotalSeconds % 3600) % 60) + " secs";
			if (hour == "" && min == "" && sec == "")
				return "Expired";
			else
				return "Expires in "+ hour+""+min+""+sec;
		}
	}
}
