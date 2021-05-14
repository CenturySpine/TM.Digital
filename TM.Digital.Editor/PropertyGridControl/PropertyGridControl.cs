using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using TM.Digital.Editor.EditorsModels;

namespace TM.Digital.Editor.PropertyGridControl
{
    public class PropertyGridControl : Control
    {
        public object SelectedObject
        {
            get => (object)GetValue(SelectedObjectProperty);
            set => SetValue(SelectedObjectProperty, value);
        }

        // Using a DependencyProperty as the backing store for SelectedObject.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedObjectProperty =
            DependencyProperty.Register("SelectedObject", typeof(object), typeof(PropertyGridControl), new PropertyMetadata(null, OnSelectedChanged));

        private static void OnSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null && d is PropertyGridControl pg)
            {
                pg.Descriptor = pg.Inspect(e.NewValue);
            }
        }

        public ObjectDescriptor Descriptor
        {
            get => (ObjectDescriptor)GetValue(DescriptorProperty);
            set => SetValue(DescriptorProperty, value);
        }

        // Using a DependencyProperty as the backing store for Descriptor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DescriptorProperty =
            DependencyProperty.Register("Descriptor", typeof(ObjectDescriptor), typeof(PropertyGridControl), new PropertyMetadata(null));

        private ObjectDescriptor Inspect(object eNewValue)
        {
            var type = eNewValue.GetType();

            var props = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty);

            var des = new ObjectDescriptor { Name = type.Name };

            foreach (var propertyInfo in props)
            {
                IInspector inspector = null;
                if (propertyInfo.PropertyType.IsEnum)
                {
                    inspector = new EnumInspector();
                }
                else if (propertyInfo.PropertyType == typeof(string))
                {
                    inspector = new StringInspector();
                }
                else if (propertyInfo.PropertyType == typeof(int)
                || propertyInfo.PropertyType == typeof(double)
                || propertyInfo.PropertyType == typeof(decimal)
                || propertyInfo.PropertyType == typeof(float)
                || propertyInfo.PropertyType == typeof(long)
                || propertyInfo.PropertyType == typeof(short))
                {
                    inspector = new NumericInspector<int>();
                }
                else if (propertyInfo.PropertyType.FullName != null
                         && propertyInfo.PropertyType.IsClass 
                         && !(propertyInfo.PropertyType.FullName.ToLowerInvariant().Contains("collections"))
                         && !IsListOrListInherited(propertyInfo.PropertyType, typeof(List<>))
                         && !IsListOrListInherited(propertyInfo.PropertyType, typeof(Collection<>))
                         )
                {
                    inspector = new ObjectInspector(Inspect);
                }

                if (inspector != null)
                    des.PropertiesDescriptors.Add(inspector.Inspect(eNewValue, propertyInfo));
            }

            return des;
        }
        static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur)
                {
                    return true;
                }
                toCheck = toCheck.BaseType;
            }
            return false;
        }
        private bool IsListOrListInherited(Type propertyInfoPropertyType, Type checkAgainst)
        {
            return propertyInfoPropertyType.IsGenericType
                   //&& !(propertyInfoPropertyType.GetGenericTypeDefinition() == checkAgainst)
                   || (IsSubclassOfRawGeneric(propertyInfoPropertyType, checkAgainst));
        }
    }

    public class ObjectInspector : IInspector
    {
        private readonly Func<object, ObjectDescriptor> _inspector;

        public ObjectInspector(Func<object, ObjectDescriptor> inspect)
        {
            _inspector = inspect;
        }

        public PropertyDescriptor Inspect(object original, PropertyInfo property)
        {
            ObjectDescriptor objDesc = new ObjectDescriptor
            {
                Value = _inspector(property.GetValue(original)),
                Title = property.Name,
                Original = original
            };

            return objDesc;
        }
    }
    public class StringInspector : IInspector
    {
        public PropertyDescriptor Inspect(object eNewValue, PropertyInfo propertyInfo)
        {
            StringDescriptor enumDesc = new StringDescriptor
            {
                Value = propertyInfo.GetValue(eNewValue)?.ToString(),
                Title = propertyInfo.Name
            };
            enumDesc.Setter = originalObject =>
            {
                propertyInfo.SetValue(originalObject, enumDesc.Value);
            };

            enumDesc.Original = eNewValue;
            return enumDesc;
        }
    }

    public class EnumInspector : IInspector
    {
        public PropertyDescriptor Inspect(object eNewValue, PropertyInfo propertyInfo)
        {
            EnumDescriptor enumDesc = new EnumDescriptor();
            var values = Enum.GetNames(propertyInfo.PropertyType);
            enumDesc.Values = new ObservableCollection<string>(values);
            enumDesc.Title = propertyInfo.Name;
            enumDesc.Setter = originalObject =>
            {
                if (Enum.TryParse(propertyInfo.PropertyType, enumDesc.SelectedValue, true, out var value))
                {
                    propertyInfo.SetValue(originalObject, value);
                }
            };
            enumDesc.SelectedValue = propertyInfo.GetValue(eNewValue)?.ToString();
            enumDesc.Original = eNewValue;
            return enumDesc;
        }
    }

    public class NumericInspector<T> : IInspector
    {
        public PropertyDescriptor Inspect(object original, PropertyInfo property)
        {
            NumericDescriptor enumDesc = new NumericDescriptor
            {
                Value = property.GetValue(original)?.ToString(),
                Title = property.Name
            };
            enumDesc.Setter = originalObject =>
            {
                if (typeof(T) == typeof(int))
                {
                    if (int.TryParse(enumDesc.Value, out int result))
                        property.SetValue(originalObject, result);
                }
                else if (typeof(T) == typeof(long))
                {
                    if (long.TryParse(enumDesc.Value, out long result))
                        property.SetValue(originalObject, result);
                }
                else if (typeof(T) == typeof(short))
                {
                    if (short.TryParse(enumDesc.Value, out short result))
                        property.SetValue(originalObject, result);
                }
                else if (typeof(T) == typeof(double))
                {
                    if (double.TryParse(enumDesc.Value, out double result))
                        property.SetValue(originalObject, result);
                }
                else if (typeof(T) == typeof(decimal))
                {
                    if (decimal.TryParse(enumDesc.Value, out decimal result))
                        property.SetValue(originalObject, result);
                }
                else if (typeof(T) == typeof(float))
                {
                    if (float.TryParse(enumDesc.Value, out float result))
                        property.SetValue(originalObject, result);
                }
                else
                {
                    throw new NotImplementedException("Type is not supported");
                }
            };

            enumDesc.Original = original;
            return enumDesc;
        }
    }

    public interface IInspector
    {
        PropertyDescriptor Inspect(object original, PropertyInfo property);
    }
}