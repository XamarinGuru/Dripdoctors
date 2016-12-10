using System;

using Xamarin.Forms;

namespace Dripdoctors
{
	public class BaseContentPage : ContentPage, BaseElementInterface
	{
		public BaseContentPage()
		{
			activeChild = null;
			activeChildIndex = 0;
		}

		public virtual void OnContentViewSelectionChanged() { }

		public virtual Object RightNavButton()
		{
			return null;
		}

		public virtual bool OnRightNavButton_Clicked()
		{
			return true;
		}

		// Implementations of BaseElementInterface
		public void OnChildViewSelectionChanged()
		{
			OnContentViewSelectionChanged();
		}

		public Object GetRightNavButton()
		{
			Object rnb = RightNavButton();
			if (rnb != null)
				return rnb;
			if (activeChild != null && activeChildIndex > 0)
				rnb = activeChild.GetRightNavButton();
			return rnb;
		}

		public void OnRightNavButton(object sender, EventArgs e)
		{
			if (OnRightNavButton_Clicked() == true)
			{
				if (activeChild != null && activeChildIndex > 0)
					activeChild.OnRightNavButton(sender, e);
			}
		}

		protected BaseElementInterface activeChild;
		protected int activeChildIndex;

	}
}

