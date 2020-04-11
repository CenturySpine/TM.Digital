using System;
using System.Collections.Generic;
using System.Text;
using TM.Digital.Client.ViewModelCore;
using TM.Digital.Model.Player;

namespace TM.Digital.Client.Screens.ActionChoice
{
    public delegate void SelectedChoiceValidatedEventHandler(ResourceEffectPlayerChooser choice);
    public class ActionChoiceViewModel : NotifierBase
    {
        private ResourceEffectPlayerChooserList _chooser;
        private bool _isVisible;
        private ResourceEffectPlayerChooser _selectedChoice;

        public ActionChoiceViewModel()
        {
            ValidateResourceEffectChoiceCommand = new RelayCommand(ExecuteValidateChoice, CanExecuteValidateChoice);
        }

        public event SelectedChoiceValidatedEventHandler ChoiceSelected;
        private void ExecuteValidateChoice(object obj)
        {
            if (SelectedChoice != null)
                OnChoiceSelected(SelectedChoice);

        }

        private bool CanExecuteValidateChoice(object arg)
        {
            return SelectedChoice != null;
        }

        public bool IsVisible
        {
            get { return _isVisible; }

            set { _isVisible = value; OnPropertyChanged(nameof(IsVisible)); }
        }

        public void Setup(ResourceEffectPlayerChooserList chooser)
        {
            Chooser = chooser;
        }

        public ResourceEffectPlayerChooserList Chooser
        {
            get { return _chooser; }
            set
            {
                _chooser = value;
                OnPropertyChanged(nameof(Chooser));
            }
        }

        public ResourceEffectPlayerChooser SelectedChoice
        {
            get => _selectedChoice;
            set { _selectedChoice = value; OnPropertyChanged(nameof(SelectedChoice)); }
        }

        public RelayCommand ValidateResourceEffectChoiceCommand { get; set; }

        protected virtual void OnChoiceSelected(ResourceEffectPlayerChooser choice)
        {
            ChoiceSelected?.Invoke(choice);
        }
    }
}
