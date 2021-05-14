using System.Collections.ObjectModel;

namespace TM.Digital.Editor.EditorsModels
{
    public class ObjectDescriptor : PropertyDescriptor
    {
        public ObjectDescriptor()
        {
            PropertiesDescriptors = new ObservableCollection<PropertyDescriptor>();
        }

        public string Name { get; set; }
        public ObservableCollection<PropertyDescriptor> PropertiesDescriptors { get; set; }
        public object Value { get; set; }
    }
}