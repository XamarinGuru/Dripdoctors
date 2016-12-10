using MapKit;
using UIKit;
using CoreGraphics;
using Foundation;
using System.Net.Http;
using System.Threading.Tasks;

namespace Dripdoctors.iOS
{
	public class CustomMKAnnotationView : MKAnnotationView
	{
		public string Id { get; set; }
		public string Url { get; set; }
		UIImageView profileImage;
		public CustomMKAnnotationView(IMKAnnotation annotation, string id): base(annotation, id) 
		{ 
			
		}

		public CustomMKAnnotationView(IMKAnnotation annotation, string id, string url = null)
			: base(annotation, id)
		{
			profileImage = new UIImageView();

			if (url != null && url.Trim() != "")
			{
				setProfile(url);
			}
			profileImage.Frame = new CGRect(3, 3, 60, 60);

		}

		async void setProfile(string url)
		{ 
			profileImage.Image = await FromUrl1(url);
			profileImage.Layer.CornerRadius = 30;
			profileImage.ClipsToBounds = true;
			this.Image = UIImage.FromFile("pin2.png");
			this.Frame = new CGRect(0,0,65,101);
			this.AddSubview(profileImage);
		}

		UIImage FromUrl(string uri)
		{
			var url = new NSUrl(uri);
			var data = NSData.FromUrl(url);
			if (data == null)
			{
				return null;
			}
			return UIImage.LoadFromData(data);
		}
		public async Task<UIImage> FromUrl1(string imageUrl)
		{
			var httpClient = new HttpClient();
			var contents = await httpClient.GetByteArrayAsync(imageUrl);
			return UIImage.LoadFromData(NSData.FromArray(contents));
		}
	}
}
