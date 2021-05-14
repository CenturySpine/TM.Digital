using System;
using TM.Digital.Ui.Resources.ViewModelCore;

namespace TM.Digital.Editor.EditorsModels
{
    public class PropertyDescriptor : NotifierBase
    {
        public object Original { get; set; }
        public string Title { get; set; }
        public Action<object> Setter { get; set; }
    }
}