using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Plugin.Messaging;

namespace Dripdoctors
{
	public partial class ContactView : ContentView
	{
		public EventHandler OnCall;
		public EventHandler OnMessage;
		public ContactView()
		{
			InitializeComponent();

			var callTapGesture = new TapGestureRecognizer();
			callTapGesture.Tapped += OnCallButtonClicked;
			callButton.GestureRecognizers.Add(callTapGesture);

			var messageTapGesture = new TapGestureRecognizer();
			messageTapGesture.Tapped += OnMessageButtonClicked;
			messageButton.GestureRecognizers.Add(messageTapGesture);


		}

		private void OnCallButtonClicked(object sender, EventArgs e)
		{
			if (OnCall != null) 
			{
				OnCall(this, new EventArgs());
			}
		}

		private void OnMessageButtonClicked(object sender, EventArgs e)
		{
			if (OnMessage != null) 
			{
				OnMessage(this, new EventArgs());
			}
		}
	}
}
