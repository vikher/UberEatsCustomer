using ClubersCustomerMobile.Prism.Models;
using System;
using Xamarin.Forms;

namespace ClubersCustomerMobile.Prism.Views
{
    public partial class MessagesPage : ContentPage
    {
        SignalRService signalR;
        public MessagesPage()
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "");

            signalR = new SignalRService();
            signalR.Connected += SignalR_ConnectionChanged;
            signalR.ConnectionFailed += SignalR_ConnectionChanged;
            signalR.NewMessageReceived += SignalR_NewMessageReceived;

            ConnectButton_ClickedAsync(connectButton, EventArgs.Empty);
        }

        void AddMessage(string message)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Label label = new Label
                {
                    Text = message,
                    HorizontalOptions = LayoutOptions.Start,
                    VerticalOptions = LayoutOptions.Start
                };

                messageList.Children.Add(label);
            });
        }

        void SignalR_NewMessageReceived(object sender, Message message)
        {
            string msg = $"{message.Name} ({message.TimeReceived}) - {message.Text}";
            AddMessage(msg);
        }

        void SignalR_ConnectionChanged(object sender, bool success, string message)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                connectButton.Text = "Connect";
                connectButton.IsEnabled = !success;
                sendButton.IsEnabled = success;
                AddMessage($"Server connection changed: {message}");
            });
        }

        async void ConnectButton_ClickedAsync(object sender, EventArgs e)
        {
            connectButton.Text = "Connecting...";
            connectButton.IsEnabled = false;
            await signalR.ConnectAsync();
        }

        async void SendButton_ClickedAsync(object sender, EventArgs e)
        {
            await signalR.SendMessageAsync(Constants.Username, messageEntry.Text);
            messageEntry.Text = "";
        }
    }
}
