using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using TM.Digital.Model.Cards;

namespace TM.Digital.Ui.Resources.Resources.TemplateSelectors
{
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