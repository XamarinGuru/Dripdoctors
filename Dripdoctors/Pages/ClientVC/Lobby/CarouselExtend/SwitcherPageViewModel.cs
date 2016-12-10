using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace Dripdoctors.Carousel
{
	public class SwitcherPageViewModel : BaseViewModel
	{
		public SwitcherPageViewModel(List<HomeViewModel> list)
		{
			Pages = list;
			CurrentPage = Pages.First();
		}

		IEnumerable<HomeViewModel> _pages;
		public IEnumerable<HomeViewModel> Pages {
			get {
				return _pages;
			}
			set {
				SetObservableProperty (ref _pages, value);
				CurrentPage = Pages.FirstOrDefault ();
			}
		}

		HomeViewModel _currentPage;
		public HomeViewModel CurrentPage {
			get {
				return _currentPage;
			}
			set {
				SetObservableProperty (ref _currentPage, value);
			}
		}
	}
}

