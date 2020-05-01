using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using TM.Digital.Client.Services;
using TM.Digital.Model;
using TM.Digital.Model.Player;
using TM.Digital.Transport;
using TM.Digital.Ui.Resources.ViewModelCore;

namespace TM.Digital.Client.Screens.ActionChoice
{
    public class ActionChoiceViewModel : NotifierBase, IComponentConfigurable
    {
        private readonly IApiProxy _apiProxy;
        private ResourceEffectPlayerChooserList _chooser;
        private bool _isVisible;
        private ResourceEffectPlayerChooser _selectedChoice;

        public ActionChoiceViewModel(IApiProxy apiProxy)
        {
            _apiProxy = apiProxy;
            ValidateResourceEffectChoiceCommand = new RelayCommand(ExecuteValidateChoice, CanExecuteValidateChoice);
        }

        public ResourceEffectPlayerChooserList Chooser
        {
            get => _chooser;
            set
            {
                _chooser = value;
                OnPropertyChanged(nameof(Chooser));
            }
        }

        public bool IsVisible
        {
            get => _isVisible;

            set { _isVisible = value; OnPropertyChanged(); }
        }

        public ResourceEffectPlayerChooser SelectedChoice
        {
            get => _selectedChoice;
            set { _selectedChoice = value; OnPropertyChanged(); }
        }

        public RelayCommand ValidateResourceEffectChoiceCommand { get; set; }

        public void RegisterSubscriptions(HubConnection hubConnection)
        {
            hubConnection.On<string, string>(ServerPushMethods.ResourceEffectForOtherPlayer, (user, message) =>
            {
                if (Guid.Parse(user) == GameData.PlayerId)
                {
                    DisplayResourceEffectChoice(message);
                }
            });
        }

        protected virtual async Task OnChoiceSelected(ResourceEffectPlayerChooser choice)
        {
            Close();

            await _apiProxy.SelectEffectTarget(choice);
        }

        private bool CanExecuteValidateChoice(object arg)
        {
            return SelectedChoice != null;
        }

        private void Close()
        {
            SelectedChoice = null;
            IsVisible = false;
        }

        private void DisplayResourceEffectChoice(string message)
        {
            ResourceEffectPlayerChooserList chooser = JsonConvert.DeserializeObject<ResourceEffectPlayerChooserList>(message);

            Show(chooser);
        }

        private async void ExecuteValidateChoice(object obj)
        {
            if (SelectedChoice != null)
                await OnChoiceSelected(SelectedChoice);
        }

        private void Show(ResourceEffectPlayerChooserList chooser)
        {
            Chooser = chooser;
            IsVisible = true;
        }
    }
}