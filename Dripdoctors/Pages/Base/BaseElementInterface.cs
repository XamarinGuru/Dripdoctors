using System;
namespace Dripdoctors
{
	public interface BaseElementInterface
	{
		void OnChildViewSelectionChanged();
		Object GetRightNavButton();
		void OnRightNavButton(object sender, EventArgs e);
	}
}
