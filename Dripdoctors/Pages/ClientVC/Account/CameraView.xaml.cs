using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Dripdoctors
{
	using CameraApplication.Common;
	using CameraApplication.ViewModels;

	using Xamarin.Forms;


	public partial class CameraView : ContentPage
	{
		public CameraView()
		{
			InitializeComponent();
			BindingContext = new TakePictureViewModel(DependencyService.Get<ICameraProvider>());
		}

		private async void initCamera()
		{
			
		}
	}
}
