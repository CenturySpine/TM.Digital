using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Effects;
using TM.Digital.Ui.Resources.Resources.TemplateSelectors;

namespace TM.Digital.Ui.Resources.Resources.Converters
{
    public class AltResourceWrapper
    {
        public object Target { get; set; }
        public bool IsLast { get; set; }
    }
    public class AltResourcesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ResourceEffectAlternatives collection)
            {
                List<AltResourceWrapper> alt = new List<AltResourceWrapper>();
                for (int i = 0; i < collection.Count; i++)
                {
                    var initial = collection[i];
                    AltResourceWrapper wrapper=new AltResourceWrapper();
                    wrapper.Target = initial;
                    if (i == collection.Count - 1)
                    {
                        wrapper.IsLast = true;
                    }
                    alt.Add(wrapper);
                }

                return alt;
            }

            return new List<AltResourceWrapper>();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class TagEffectsGroupingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<TagEffectGroup> groups = new List<TagEffectGroup>();
            if (value is TagsEffects tagEffects)
            {

                
                
                //var group = tagEffects.GroupBy(t => t.AffectedTags);
                //foreach (var VARIABLE in group)
                //{
                //    TagEffectGroup teg = new TagEffectGroup();
                //    teg.Tags = VARIABLE.Key;
                //    foreach (var tagEffect in VARIABLE)
                //    {
                //        teg.ResourceEffects.Add(new ResourceEffect()
                //        {
                //            Amount =tagEffect.EffectValue,
                //            ResourceType = tagEffect.ResourceType,
                //            ResourceKind = tagEffect.ResourceKind
                //        });
                //    }
                //}

                //return groups;
            }

            return groups;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}