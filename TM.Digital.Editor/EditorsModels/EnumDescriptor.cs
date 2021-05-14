using System.Collections.ObjectModel;

namespace TM.Digital.Editor.EditorsModels
{
    public class EnumDescriptor : PropertyDescriptor
    {
        private string _selectedValue;
        public ObservableCollection<string> Values { get; set; }

        public string SelectedValue
        {
            get => _selectedValue;
            set
            {
                _selectedValue = value;
                if (Original != null) Setter?.Invoke(Original);
            }
        }
    }
}