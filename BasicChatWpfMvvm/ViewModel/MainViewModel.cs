using GalaSoft.MvvmLight;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using Microsoft.AspNet.SignalR.Client.Hubs;
using System.Threading.Tasks;
using System.Windows.Threading;
using System;

namespace BasicChatWpfMvvm.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        HubConnection _connection;
        IHubProxy _chat;
        Dispatcher _dispatcher = Dispatcher.CurrentDispatcher;

        private ObservableCollection<string> _messageList;
        public ObservableCollection<string> MessageList
        {
            get { return _messageList; }
            set
            {
                _messageList = value;
                RaisePropertyChanged("MessageList");
            }
        }

        private string _newMessage = string.Empty;
        public string NewMessage
        {
            get
            {
                return _newMessage;
            }

            set
            {
                if (_newMessage == value)
                {
                    return;
                }

                _newMessage = value;
                RaisePropertyChanged("NewMessage");
            }
        }

        public RelayCommand SendMessageCommand
        {
            get;
            private set;
        }

        public MainViewModel()
        {
            MessageList = new ObservableCollection<string>();
            _connection = new HubConnection("http://localhost:44914");
            _chat = _connection.CreateHubProxy("Chat");

            SendMessageCommand = new RelayCommand(() => SendMessage(NewMessage));

            _chat.On<string>("send", MessageReceived);

            _connection.Start();
        }

        private async void SendMessage(string message)
        {
            if (!String.IsNullOrEmpty(message))
            {
                await _chat.Invoke("send", "WPF Client: " + message);
            }
            NewMessage = String.Empty;
        }

        private void MessageReceived(string message)
        {
            _dispatcher.Invoke(() =>
            {
                MessageList.Add(message);
            });
        }
    }
}