using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;
using XLabs.Cryptography;

namespace Dripdoctors
{
	public class APIManager
	{
		private HttpClient client;
		private string md5Key;

		public APIManager()
		{
			client = new HttpClient();
			client.MaxResponseContentBufferSize = 256000;
			md5Key = MD5.GetMd5String(Constants.hashKey + "" + DateTime.Today.Year);
		}

		// ------- Login/Logout API -------
		/*
		*	Login
		*	URL : http://uapi.dripdoctors.com/login
		*	Method : Post
		*	Parameters: email, password, key
		*/
		public async Task<object> signInAsync(string email, string pwd)
		{
			var md5pwd = MD5.GetMd5String(pwd);
			User user = null;
			Dictionary<string, string> pairs = new Dictionary<string, string>();
			pairs.Add("email", email);
			pairs.Add("pwd", md5pwd);
			pairs.Add("key", md5Key);
			FormUrlEncodedContent formContent = new FormUrlEncodedContent(pairs);
			var uri = new Uri(string.Format(Constants.logInURL));
			try
			{
				var response = await client.PostAsync(uri, formContent);
				if (response.IsSuccessStatusCode)
				{
					var result = await response.Content.ReadAsStringAsync();
					var json = Newtonsoft.Json.Linq.JObject.Parse(result);
					var state = JsonConvert.SerializeObject(json["status"]);
					if (state.Contains("fail"))
					{
						return "" + json["message"];
					}
					var a = JsonConvert.SerializeObject(json["nurseId"]);
					if(a != "null")
						Singleton.sharedInstance().nurseId = json.Value<int>("nurseId");
					var dicts = JsonConvert.SerializeObject(json["userInfo"]);
					var dict = (List<User>)JsonConvert.DeserializeObject(dicts, typeof(List<User>));
					user = dict[0];
				}
				else {
					Debug.WriteLine(@"Failed.");
					return "Failed";
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"ERROR {0}", ex.Message);
				return ex.Message;
			}
			return user;
		}

		/*
		*	Register user
		*	URL : http://uapi.dripdoctors.com/register
		*	Method : Post
		*	Parameters: email, password, key
		*/
		public async Task<object> signUpAsync(string email, string pwd)
		{
			User user = null;
			var md5pwd = MD5.GetMd5String(pwd);
			Dictionary<string, string> pairs = new Dictionary<string, string>();
			pairs.Add("email", email);
			pairs.Add("pwd", md5pwd);
			pairs.Add("key", md5Key);
			FormUrlEncodedContent formContent = new FormUrlEncodedContent(pairs);
			var uri = new Uri(string.Format(Constants.registerURL));
			try
			{
				var response = await client.PostAsync(uri, formContent);
				if (response.IsSuccessStatusCode)
				{
					var result = await response.Content.ReadAsStringAsync();
					var json = Newtonsoft.Json.Linq.JObject.Parse(result);
					var state = JsonConvert.SerializeObject(json["status"]);
					if (state.Contains("validationFail"))
					{
						return "" + json["message"];
					}
					var dicts = JsonConvert.SerializeObject(json["userInfo"]);
					var dict = (List<User>)JsonConvert.DeserializeObject(dicts, typeof(List<User>));
					user = dict[0];
				}
				else {
					Debug.WriteLine(@"Failed.");
					return "Failed";
				}

			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"ERROR {0}", ex.Message);
				return ex.Message;
			}
			return user;
		}


		// ------- Service API -------
		/*
		*	Get Service Categories
		*	URL : http://uapi.dripdoctors.com/getCategories
		*	Method : Post
		*	Parameters: key
		*/
		public async Task<object> getServiceCategory()
		{
			var dict = new List<ServiceCategory>();
			Dictionary<string, string> pairs = new Dictionary<string, string>();
			pairs.Add("key", md5Key);
			FormUrlEncodedContent formContent = new FormUrlEncodedContent(pairs);
			var uri = new Uri(string.Format(Constants.getCategoryURL));
			try
			{
				var response = await client.PostAsync(uri, formContent);
				if (response.IsSuccessStatusCode)
				{
					var result = await response.Content.ReadAsStringAsync();
					var json = Newtonsoft.Json.Linq.JObject.Parse(result);
					var dicts = JsonConvert.SerializeObject(json["categories"]);
					dict = (List<ServiceCategory>)JsonConvert.DeserializeObject(dicts, typeof(List<ServiceCategory>));
				}
				else {
					Debug.WriteLine(@"Failed.");
					return "Failed";
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"ERROR {0}", ex.Message);
				return ex.Message;
			}
			return dict;
		}


		/*
		*	Get Service Products
		*	URL : http://uapi.dripdoctors.com/getServices
		*	Method : Post
		*	Parameters: key, category_id
		*/
		public async Task<object> getServiceProducts(string category_id)
		{
			var dict = new List<ServiceItem>();
			Dictionary<string, string> pairs = new Dictionary<string, string>();
			pairs.Add("key", md5Key);
			pairs.Add("category_id", category_id);
			FormUrlEncodedContent formContent = new FormUrlEncodedContent(pairs);
			var uri = new Uri(string.Format(Constants.getProductsURL));
			try
			{
				var response = await client.PostAsync(uri, formContent);
				if (response.IsSuccessStatusCode)
				{
					var result = await response.Content.ReadAsStringAsync();
					var json = Newtonsoft.Json.Linq.JObject.Parse(result);
					var dicts = JsonConvert.SerializeObject(json["categories"]);
					dict = (List<ServiceItem>)JsonConvert.DeserializeObject(dicts, typeof(List<ServiceItem>));
				}
				else {
					Debug.WriteLine(@"Failed.");
					return "Failed";
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"ERROR {0}", ex.Message);
				return ex.Message;
			}
			return dict;
		}

		/*
		*	GET Get Clinicst
		*	URL : http://uapi.dripdoctors.com/getClinics
		*	Method : Post
		*	Parameters: clinicId				
		*/
		public async Task<object> getClinics(string clinicId = null)
		{
			var dict = new List<Clinic>();
			Dictionary<string, string> pairs = new Dictionary<string, string>();
			pairs.Add("key", md5Key);
			if (clinicId != null)
			{
				pairs.Add("clinicId", clinicId);
			}

			FormUrlEncodedContent formContent = new FormUrlEncodedContent(pairs);
			var uri = new Uri(string.Format(Constants.getClinicURL));
			try
			{
				var response = await client.PostAsync(uri, formContent);
				if (response.IsSuccessStatusCode)
				{
					var result = await response.Content.ReadAsStringAsync();
					var json = Newtonsoft.Json.Linq.JObject.Parse(result);
					var dicts = JsonConvert.SerializeObject(json["clinics"]);
					dict = (List<Clinic>)JsonConvert.DeserializeObject(dicts, typeof(List<Clinic>));
				}
				else {
					Debug.WriteLine(@"Failed.");
					return "Failed";
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"ERROR {0}", ex.Message);
				return ex.Message;
			}
			return dict;
		}

		/*
		*	GET Send Booking Request
		*	URL : http://uapi.dripdoctors.com/sendBooking
		*	Method : Post
		*	Parameters: 				
		*/
		public async Task<object> sendBookingRequest(string bookingType, bool isGPS, double lat, double longi
													 , string address, string clinicId, int timeType, DateTime date, string serviceId
		                                             , int userId, int transactionType, string transactionCode, string amount, int payTypeId)
		{
			Dictionary<string, string> pairs = new Dictionary<string, string>();
			var bookingDate = Functions.getDateFormat(date);
			var bookingTime = Functions.getTimeFormat(date);

			pairs.Add("key", md5Key);
			pairs.Add("timeType", "" + timeType);
			pairs.Add("bookingDate", bookingDate);
			pairs.Add("bookingTime", bookingTime);
			pairs.Add("serviceId", serviceId);
			pairs.Add("userId", ""+userId);
			pairs.Add("transactionType", "" + transactionType);
			pairs.Add("transactionCode", transactionCode);
			pairs.Add("amount", amount);
			pairs.Add("paytypeId", "" + payTypeId);

			if (bookingType == "clinic" && isGPS == true)
			{
				pairs.Add("bookingType", bookingType);
				pairs.Add("gps", "1");
				pairs.Add("gpsLatitud", "" + lat);
				pairs.Add("gpsLongitud", "" + longi);
				pairs.Add("clinicId", clinicId);
			}
			else if (bookingType == "clinic" && isGPS == false)
			{

				pairs.Add("bookingType", bookingType);
				pairs.Add("gps", "0");
				pairs.Add("gpsLatitud", "" + lat);
				pairs.Add("clientAddress", "" + address);
			}
			else if (bookingType == "house" && isGPS == true)
			{

				pairs.Add("bookingType", bookingType);
				pairs.Add("gps", "1");
				pairs.Add("gpsLatitud", "" + lat);
				pairs.Add("gpsLongitud", "" + longi);
			}
			else {

				pairs.Add("bookingType", bookingType);
				pairs.Add("gps", "0");
				pairs.Add("clientAddress", address);
			}

			FormUrlEncodedContent formContent = new FormUrlEncodedContent(pairs);
			var uri = new Uri(string.Format(Constants.sendBookingRequestURL));
			try
			{
				var response = await client.PostAsync(uri, formContent);
				if (response.IsSuccessStatusCode)
				{
					var result = await response.Content.ReadAsStringAsync();
					var json = Newtonsoft.Json.Linq.JObject.Parse(result);
					var state = JsonConvert.SerializeObject(json["status"]);
					if (state.Contains("fail"))
					{
						return "" + json["message"];
					}
				}
				else {
					Debug.WriteLine(@"Failed.");
					return "Failed";
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"ERROR {0}", ex.Message);
				return ex.Message;
			}
			return true;
		}

		// ------- Account API -------
		/*
		*	Update user info
		*	URL : http://uapi.dripdoctors.com/updateUserInfo
		*	Method : Post
		*	Parameters: key, userId, email, fname, sname, mobilenum, address, city, zip, state, country, gender, 
		*				date_of_birthday, imgName, imgExt, imgBase64, imgUpdate				
		*/
		public async Task<object> updateUserInfo(User user)
		{

			Dictionary<string, string> pairs = new Dictionary<string, string>();
			pairs.Add("key", md5Key);
			pairs.Add("userId", ""+user.userid);
			pairs.Add("email", user.email);
			pairs.Add("fname", user.fname);
			pairs.Add("sname", user.sname);
			pairs.Add("mobilenum", user.mobilenum);
			pairs.Add("address", user.address);
			pairs.Add("city", user.city);
			pairs.Add("zip", user.zip);
			pairs.Add("state", user.state);
			pairs.Add("country", user.country);
			pairs.Add("gender", user.gender);
			pairs.Add("date_of_birth", user.date_of_birth);
			if (user.profileImage != null)
			{
				pairs.Add("imgName", "imgProfile");
				pairs.Add("imgExt", "JPG");
				pairs.Add("imgBase64", user.profileImage);
				pairs.Add("imgUpdate", "YES");
			}
			else {
				pairs.Add("imgUpdate", "NO");
			}

			FormUrlEncodedContent formContent = new FormUrlEncodedContent(pairs);
			var uri = new Uri(string.Format(Constants.updateUserInfoURL));
			try
			{
				var response = await client.PostAsync(uri, formContent);
				if (response.IsSuccessStatusCode)
				{
					var result = await response.Content.ReadAsStringAsync();
					var json = Newtonsoft.Json.Linq.JObject.Parse(result);
					var dicts = JsonConvert.SerializeObject(json["userInfo"]);
					var state = JsonConvert.SerializeObject(json["status"]);
					if (state.Contains("fail"))
					{
						return "" + json["message"];
					}
					Singleton.sharedInstance().user = (User)JsonConvert.DeserializeObject(dicts, typeof(User));
				}
				else {
					Debug.WriteLine(@"Failed.");
					return "Failed";
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"ERROR {0}", ex.Message);
				return ex.Message;
			}
			return true;
		}


		/*
		*	Update account password
		*	URL : http://uapi.dripdoctors.com/userUpdatePassword
		*	Method : Post
		*	Parameters: key, userId, userName, newPasswork, newPasswordConfirm, actualPassword
		*/
		public async Task<object> updateUserPassword(User user, string newPassword, string newPasswordConfirm)
		{
			Dictionary<string, string> pairs = new Dictionary<string, string>();
			pairs.Add("key", md5Key);
			pairs.Add("userId", ""+user.userid);
			pairs.Add("userName", user.username);
			pairs.Add("newPassword", newPassword);
			pairs.Add("newPasswordConfirm", newPasswordConfirm);
			pairs.Add("actualPassword", user.password);

			FormUrlEncodedContent formContent = new FormUrlEncodedContent(pairs);
			var uri = new Uri(string.Format(Constants.updateUserPasswordURL));
			try
			{
				var response = await client.PostAsync(uri, formContent);
				if (response.IsSuccessStatusCode)
				{
					var result = await response.Content.ReadAsStringAsync();
					var json = Newtonsoft.Json.Linq.JObject.Parse(result);
					var dicts = JsonConvert.SerializeObject(json["userInfo"]);
					Singleton.sharedInstance().user = (User)JsonConvert.DeserializeObject(dicts, typeof(User));
				}
				else {
					Debug.WriteLine(@"Failed.");
					return "Failed";
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"ERROR {0}", ex.Message);
				return ex.Message;
			}
			return true;
		}


		// ------- Location API -------
		/*
		*	GET Geocode
		*	URL : https://maps.googleapis.com/maps/api/geocode/json?
		*	Method : Get
		*	Parameters: latlng				
		*/

		public async Task<object> getGeoCode(double longuitude, double latitude)
		{
			string reValue = null;
			try
			{
				var response = await client.GetAsync(Constants.locationRUL + "latlng=" + latitude + "," + longuitude);
				if (response.IsSuccessStatusCode)
				{
					var result = await response.Content.ReadAsStringAsync();
					var json = Newtonsoft.Json.Linq.JObject.Parse(result);
					reValue = "" + json["results"][0]["formatted_address"];
				}
				else {
					Debug.WriteLine(@"Failed.");
					return "Failed";
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"ERROR {0}", ex.Message);
				return ex.Message;
			}
			return reValue;
		}


		// ------- Nurse API -------
		/*
		*	Get Nurse
		*	URL : http://uapi.dripdoctors.com/getNurses
		*	Method : Post
		*	Parameters: key
		*/

		public async Task<object> getNurse(string nurseId = null)
		{
			List<Nurse> dict = new List<Nurse>();
			Dictionary<string, string> pairs = new Dictionary<string, string>();
			pairs.Add("key", md5Key);
			if (nurseId != null)
			{
				pairs.Add("nurseId", nurseId);
			}
			FormUrlEncodedContent formContent = new FormUrlEncodedContent(pairs);
			var uri = new Uri(string.Format(Constants.getNurseURL));
			try
			{
				var response = await client.PostAsync(uri, formContent);
				if (response.IsSuccessStatusCode)
				{
					var result = await response.Content.ReadAsStringAsync();
					var json = Newtonsoft.Json.Linq.JObject.Parse(result);
					var dicts = JsonConvert.SerializeObject(json["nurses"]);
					dict = (List<Nurse>)JsonConvert.DeserializeObject(dicts, typeof(List<Nurse>));
				}
				else {
					Debug.WriteLine(@"Failed.");
					return "Failed";
				}

			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"ERROR {0}", ex.Message);
				return ex.Message;
			}
			return dict;
		}

		/*
		*	Get Nurse
		*	URL : http://uapi.dripdoctors.com/getNurseLocationsss
		*	Method : Post
		*	Parameters: key, nurseId
		*/
		public async Task<object> getNurseLocation(int nurseId)
		{
			var dict = new List<Nurse>();
			Dictionary<string, string> pairs = new Dictionary<string, string>();
			pairs.Add("key", md5Key);
			pairs.Add("nurseId", ""+nurseId);
			FormUrlEncodedContent formContent = new FormUrlEncodedContent(pairs);
			var uri = new Uri(string.Format(Constants.getNurseLocationURL));
			try
			{
				var response = await client.PostAsync(uri, formContent);
				if (response.IsSuccessStatusCode)
				{
					var result = await response.Content.ReadAsStringAsync();
					var json = Newtonsoft.Json.Linq.JObject.Parse(result);
					var dicts = JsonConvert.SerializeObject(json["nurseLocation"]);
					dict = (List<Nurse>)JsonConvert.DeserializeObject(dicts, typeof(List<Nurse>));
				}
				else {
					Debug.WriteLine(@"Failed.");
					return "Failed";
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"ERROR {0}", ex.Message);
				return ex.Message;
			}
			return dict;
		}

		/*
		*	Send Nurse Status
		*	URL : http://uapi.dripdoctors.com/getNurseLocationsss
		*	Method : Post
		*	Parameters: key, nurseId
		*/
		public async Task<object> sendNurseStatus(int nurseId, int statusId)
		{
			Dictionary<string, string> pairs = new Dictionary<string, string>();
			pairs.Add("key", md5Key);
			pairs.Add("nurseId", ""+nurseId);
			pairs.Add("statusId", ""+statusId);
			
			FormUrlEncodedContent formContent = new FormUrlEncodedContent(pairs);
			var uri = new Uri(string.Format(Constants.sendNurseStatusURL));
			try
			{
				var response = await client.PostAsync(uri, formContent);
				if (response.IsSuccessStatusCode)
				{
					var result = await response.Content.ReadAsStringAsync();
					var json = Newtonsoft.Json.Linq.JObject.Parse(result);
					return JsonConvert.SerializeObject(json["status"]);

				}
				else {
					Debug.WriteLine(@"Failed.");
					return "Failed";
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"ERROR {0}", ex.Message);
				return ex.Message;
			}
		}


		/*
		*	Send Nurse Location
		*	URL : http://uapi.dripdoctors.com/getNurseLocationsss
		*	Method : Post
		*	Parameters: key, nurseId
		*/
		public async Task<object> sendNurseLocation(int nurseId, double latitude, double longitude)
		{
			Dictionary<string, string> pairs = new Dictionary<string, string>();
			pairs.Add("key", md5Key);
			pairs.Add("nurseId", "" + nurseId);
			pairs.Add("gpsLatitud", "" + latitude);
			pairs.Add("gpsLongitud", "" + longitude);

			FormUrlEncodedContent formContent = new FormUrlEncodedContent(pairs);
			var uri = new Uri(string.Format(Constants.sendNurseLocationURL));
			try
			{
				var response = await client.PostAsync(uri, formContent);
				if (response.IsSuccessStatusCode)
				{
					var result = await response.Content.ReadAsStringAsync();
					var json = Newtonsoft.Json.Linq.JObject.Parse(result);
					return JsonConvert.SerializeObject(json["status"]);

				}
				else {
					Debug.WriteLine(@"Failed.");
					return "Failed";
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"ERROR {0}", ex.Message);
				return ex.Message;
			}
		}


// ------- Booking API -------
		/*
		*	Get Booking
		*	URL : http://uapi.dripdoctors.com/getNurses
		*	Method : Post
		*	Parameters: key
		*/

		public async Task<object> getBookingItems(int userId, string bookingId = null){
			List<Booking> dict = new List<Booking>();
			Dictionary<string, string> pairs = new Dictionary<string, string>();
			pairs.Add("key", md5Key);
			pairs.Add("userId", ""+userId);
			if (bookingId != null)
			{
				pairs.Add("bookingId", bookingId);
			}
			FormUrlEncodedContent formContent = new FormUrlEncodedContent(pairs);
			var uri = new Uri(string.Format(Constants.getBookingURL));
			try
			{
				var response = await client.PostAsync(uri, formContent);
				if (response.IsSuccessStatusCode)
				{
					var result = await response.Content.ReadAsStringAsync();
					var json = Newtonsoft.Json.Linq.JObject.Parse(result);
					var dicts = JsonConvert.SerializeObject(json["bookings"]);
					dict = (List<Booking>)JsonConvert.DeserializeObject(dicts, typeof(List<Booking>));
				}
				else {
					Debug.WriteLine(@"Failed.");
					return "Failed";
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"ERROR {0}", ex.Message);
				return ex.Message;
			}
			return dict;
		}

		/*
		*	Cancel Booking
		*	URL : http://uapi.dripdoctors.com/cancelBooking
		*	Method : Post
		*	Parameters: key,bookingId,canceledId
		*/
		public async Task<object> cancelBooking(string bookingId, string cancelId = "1") { 
			Dictionary<string, string> pairs = new Dictionary<string, string>();
			pairs.Add("key", md5Key);
			pairs.Add("bookingId", bookingId);
			pairs.Add("canceledId", cancelId);

			FormUrlEncodedContent formContent = new FormUrlEncodedContent(pairs);
			var uri = new Uri(string.Format(Constants.cancelBookingURL));
			try
			{
				var response = await client.PostAsync(uri, formContent);
				if (response.IsSuccessStatusCode)
				{
					var result = await response.Content.ReadAsStringAsync();
					var json = Newtonsoft.Json.Linq.JObject.Parse(result);
					var state = JsonConvert.SerializeObject(json["status"]);
					if (state.Contains("fail"))
					{
						return "" + json["message"];
					}
				}
				else {
					Debug.WriteLine(@"Failed.");
					return "Failed";
				}

			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"ERROR {0}", ex.Message);
				return ex.Message;
			}
			return true;
		}

		/*
		*	Remove Booking
		*	URL : http://uapi.dripdoctors.com/sendRemoveBooking
		*	Method : Post
		*	Parameters: key,bookingId
		*/
		public async Task<object> removeBooking(string bookingId, string cancelId = null)
		{
			Dictionary<string, string> pairs = new Dictionary<string, string>();
			pairs.Add("key", md5Key);
			pairs.Add("bookingId", bookingId);

			FormUrlEncodedContent formContent = new FormUrlEncodedContent(pairs);
			var uri = new Uri(string.Format(Constants.sendRemoveBookingURL));
			try
			{
				var response = await client.PostAsync(uri, formContent);
				if (response.IsSuccessStatusCode)
				{
					var result = await response.Content.ReadAsStringAsync();
					var json = Newtonsoft.Json.Linq.JObject.Parse(result);
					var state = JsonConvert.SerializeObject(json["status"]);
					if (state.Contains("fail"))
					{
						return "" + json["message"];
					}
				}
				else {
					Debug.WriteLine(@"Failed.");
					return "Failed";
				}

			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"ERROR {0}", ex.Message);
				return ex.Message;
			}
			return true;
		}

		/*
		*	Remove All Bookings
		*	URL : http://uapi.dripdoctors.com/sendRemoveBooking
		*	Method : Post
		*	Parameters: key
		*/
		public async Task<object> removeAllBookings(string bookingId, string cancelId = null)
		{
			Dictionary<string, string> pairs = new Dictionary<string, string>();
			pairs.Add("key", md5Key);
			pairs.Add("bookingId", bookingId);

			FormUrlEncodedContent formContent = new FormUrlEncodedContent(pairs);
			var uri = new Uri(string.Format(Constants.sendRemoveBookingURL));
			try
			{
				var response = await client.PostAsync(uri, formContent);
				if (response.IsSuccessStatusCode)
				{
					var result = await response.Content.ReadAsStringAsync();
					var json = Newtonsoft.Json.Linq.JObject.Parse(result);
					var state = JsonConvert.SerializeObject(json["status"]);
					if (state.Contains("fail"))
					{
						return "" + json["message"];
					}
				}
				else {
					Debug.WriteLine(@"Failed.");
					return "Failed";
				}

			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"ERROR {0}", ex.Message);
				return ex.Message;
			}
			return true;
		}

		/*
		*	Get AvaliableBooking Nurse
		*	URL : http://uapi.dripdoctors.com/sendRemoveBooking
		*	Method : Post
		*	Parameters: key
		*/
		public async Task<object> getNurseAvailable4Booking(string bookingId)
		{
			List<Nurse> dict = new List<Nurse>();
			Dictionary<string, string> pairs = new Dictionary<string, string>();
			pairs.Add("key", md5Key);
			pairs.Add("bookingId", bookingId);

			FormUrlEncodedContent formContent = new FormUrlEncodedContent(pairs);
			var uri = new Uri(string.Format(Constants.getNurseAvailable4Booking));
			try
			{
				var response = await client.PostAsync(uri, formContent);
				if (response.IsSuccessStatusCode)
				{
					var result = await response.Content.ReadAsStringAsync();
					var json = Newtonsoft.Json.Linq.JObject.Parse(result);
					var dicts = JsonConvert.SerializeObject(json["nurses"]);
					dict = (List<Nurse>)JsonConvert.DeserializeObject(dicts, typeof(List<Nurse>));
				}
				else {
					Debug.WriteLine(@"Failed.");
					return "Failed";
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"ERROR {0}", ex.Message);
				return ex.Message;
			}
			return dict;
		}

		/*
		*	Get All Calls for Nurse
		*	URL : http://uapi.dripdoctors.com/sendRemoveBooking
		*	Method : Post
		*	Parameters: key
		*/
		public async Task<object> getCalls(int nurseId)
		{
			List<Call> dict = new List<Call>();
			Dictionary<string, string> pairs = new Dictionary<string, string>();
			pairs.Add("key", md5Key);
			pairs.Add("nurseId", ""+nurseId);

			FormUrlEncodedContent formContent = new FormUrlEncodedContent(pairs);
			var uri = new Uri(string.Format(Constants.getAllNurseCallsURL));
			try
			{
				var response = await client.PostAsync(uri, formContent);
				if (response.IsSuccessStatusCode)
				{
					var result = await response.Content.ReadAsStringAsync();
					var json = Newtonsoft.Json.Linq.JObject.Parse(result);
					var dicts = JsonConvert.SerializeObject(json["dashboard"]["calls"]);
					dict = (List<Call>)JsonConvert.DeserializeObject(dicts, typeof(List<Call>));
				}
				else {
					Debug.WriteLine(@"Failed.");
					return "Failed";
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"ERROR {0}", ex.Message);
				return ex.Message;
			}
			return dict;
		}

		public async Task<object> updateCallStatus(int nurseId, int callId, int statusId)
		{
			Dictionary<string, string> pairs = new Dictionary<string, string>();
			pairs.Add("key", md5Key);
			pairs.Add("nurseId", ""+nurseId);
			pairs.Add("callId", ""+callId);
			pairs.Add("statusId", ""+statusId);
			string status = "";

			FormUrlEncodedContent formContent = new FormUrlEncodedContent(pairs);
			var uri = new Uri(string.Format(Constants.updateCallStatusURL));
			try
			{
				var response = await client.PostAsync(uri, formContent);
				if (response.IsSuccessStatusCode)
				{
					var result = await response.Content.ReadAsStringAsync();
					var json = Newtonsoft.Json.Linq.JObject.Parse(result);
					status = JsonConvert.SerializeObject(json["status"]);
					if (((string)result).Contains("success"))
						status = "success";
					else
						status = JsonConvert.SerializeObject(json["message"]);
				}
				else {
					Debug.WriteLine(@"Failed.");
					return "Failed";
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"ERROR {0}", ex.Message);
				return ex.Message;
			}
			return status;
		}
	}
}

