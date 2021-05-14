namespace TM.Digital.Editor.EditorsModels
{
    public class NumericDescriptor : PropertyDescriptor
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