using System;
using Xamarin.Forms;

namespace Dripdoctors
{
	public class MyTemplateSelector : DataTemplateSelector
	{
		private readonly DataTemplate templateOne;

		public MyTemplateSelector ()
		{
			this.templateOne = new DataTemplate (typeof(FirstView));
		}

		protected override DataTemplate OnSelectTemplate (object item, BindableObject container)
		{
			return templateOne;
		}
	}
}

