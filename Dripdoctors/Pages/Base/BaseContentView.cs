using System;

using Xamarin.Forms;

namespace Dripdoctors
{
	public abstract class BaseContentView : ContentView, BaseElementInterface
	{
		public BaseContentView(BaseElementInterface p)
		{
			parent = p;
			activeChild = null;
			activeChildIndex = 0;
		}

		public virtual bool OnContentViewSelectionChanged()
		{
			return true;
		}

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
			if (OnContentViewSelectionChanged() == true)
			{
				if (parent != null)
					parent.OnChildViewSelectionChanged();
			}
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

		protected BaseElementInterface parent;
		protected BaseElementInterface activeChild;
		protected int activeChildIndex;
	}
}

