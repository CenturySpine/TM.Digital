using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;
using Microsoft.AspNetCore.SignalR.Client;
using TM.Digital.Client.Screens.ActionChoice;
using TM.Digital.Model;
using TM.Digital.Ui.Resources.ViewModelCore;

namespace TM.Digital.Client.Screens.Main
{
    public class LoggerViewModel : NotifierBase, IComponentConfigurable
    {
        private ObservableCollection<string> _logs;

        public LoggerViewModel()
        {
            Logs = new ObservableCollection<string>();
        }
        public ObservableCollection<string> Logs
        {
            get { return _logs; }
            set { _logs = value; OnPropertyChanged(); }
        }
        public void Log(string message)
        {

                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => { Logs.Insert(0, message); }));
            
        }

        public void RegisterSubscriptions(HubConnection hubConnection)
        {
            hubConnection.On<string, string>(ServerPushMethods.LogReceived, (user, message) => { Log(message); });
        }
    }
}