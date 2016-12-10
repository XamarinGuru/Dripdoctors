using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Dripdoctors
{
	public class Singleton : Object
	{
		/*
		 *	Manager
		*/
		static Singleton sharedManager = null;

		/*
		 *	User
		*/
		public User user = null;

		/*
		 *	Client
		*/
		public List<Clinic> clinics = null;
		public ServiceItem currentRequestedService = null;
		public LocationManager locationManager = null;

		/*
		 *	Nurse
		*/
		public string nureseStatus = "Active";
		public int nurseId = -1;
		public Call currentActiveCall = null;

		////Client Services stringiables
		//public ClientMode clientMode = ClientMode.Service;
		//public ServiceCategory currentCategory = null;
		//public ServiceItem[] currentServices;
		//
		//public Image serviceLogo = null;
		//public Image ServiceItemLogo = null;
		//public bool isClientClinic = true;

		////Client Find Nurse stringiables
		//public Nurse[] nurseItems = null;
		//public Nurse currentSelectedNurse = null;
		//public ServiceCategory currentSelectedNurseCategory = null;
		//public ServiceItem currentSelectedNurseItem = null;
		//public Image currentSelectedNurseCategoryLogo = null;
		//public Image currentSelectedNurseProductLogo = null;

		////Client Booking stringiables
		//public Booking currentClientSelectedBookingItem = null;
		//public string currentCancelId = "1";
		//public Nurse CurrentTrackingNurse = null;

		private Singleton()
		{
			
		}

		public static Singleton sharedInstance() {
			if (sharedManager == null)
				sharedManager = new Singleton();
			return sharedManager;
		}

		public async Task<bool> getNurseStatus()
		{
			var apiManager = new APIManager();
			var result = await apiManager.getNurseLocation(nurseId);
			if (result is List<Nurse>)
			{
				List<Nurse> nurses = (List<Nurse>)result;
				if (nurses == null) return false;

				switch (nurses[0].online) {
					case 1:
						nureseStatus = "Unavailable";
						break;
					case 2:
						nureseStatus = "Active";
						break;
					case 3:
						nureseStatus = "On Call";
						break;
					case 4:
						nureseStatus = "Blocked";
						break;
					case 5:
						nureseStatus = "Offline";
						break;
					default:
						break;
				}
			}
			return true;
		}
	}
}

