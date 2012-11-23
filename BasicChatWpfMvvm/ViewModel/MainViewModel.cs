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
    /// <summary>
    /// This class contains properties and commands that the 
    /// MainWindow View can data bind to.
    /// The Views ListView data binds its ItemsSource to the ObservableCollection MessageList property,
    /// the TextBox data binds its Text property to the NewMessage property
    /// and the Send button data binds its Command to the RelayCommand SendMessageCommand property
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private HubConnection _connection;
        private IHubProxy _chat;
        private Dispatcher _dispatcher = Dispatcher.CurrentDispatcher;

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
            InitializeConnection();
            SendMessageCommand = new RelayCommand(() => SendMessage(NewMessage));
        }

        private async void InitializeConnection()
        {
            _connection = new HubConnection("http://localhost:44914");
            _chat = _connection.CreateHubProxy("Chat");

            _chat.On<string>("send", MessageReceived);

            await _connection.Start();
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