namespace TM.Digital.Editor.EditorsModels
{
    public class StringDescriptor : PropertyDescriptor
    {
        private string _value;

        public string Value
        {
            get => _value;
            set
            {
                _value = value;
                if (Original != null) Setter?.Invoke(Original);
            }
        }
    }
}