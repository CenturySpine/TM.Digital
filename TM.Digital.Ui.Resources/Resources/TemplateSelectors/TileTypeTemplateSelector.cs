using System;
using System.Windows;
using System.Windows.Controls;
using TM.Digital.Model.Tile;

namespace TM.Digital.Ui.Resources.Resources.TemplateSelectors
{
    public class TileTypeTemplateSelector : DataTemplateSelector
    {
        public DataTemplate City { get; set; }
        public DataTemplate Ocean { get; set; }
        public DataTemplate Forest { get; set; }
        public DataTemplate NoMansLand { get; set; }
        public DataTemplate Nuclear { get; set; }


        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item != null)
            {
                TileType t = (TileType)Enum.Parse(typeof(TileType), item.ToString());
                switch (t)
                {
                    case TileType.None:
                        return null;
                    case TileType.City:
                        return City;
                    case TileType.Forest:
                        return Forest;
                    case TileType.Ocean:
                        return Ocean;
                    case TileType.NuclearZone:
                        return Nuclear;
                    case TileType.Capital:
                        break;
                    case TileType.Volcano:
                        break;
                    case TileType.Quarry:
                        break;
                    case TileType.Geothermy:
                        break;
                    case TileType.Factory:
                        break;
                    case TileType.EcologicalArea:
                        break;
                    case TileType.Mall:
                        break;
                    case TileType.NoMansLand:
                        return NoMansLand;
                    case TileType.Colony:
                        break;
                    default:
                        return null;
                }
            }

            return null;
        }
    }
}