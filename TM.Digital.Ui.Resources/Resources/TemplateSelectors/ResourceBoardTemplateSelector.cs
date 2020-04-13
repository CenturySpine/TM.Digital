using System;
using System.Windows;
using System.Windows.Controls;
using TM.Digital.Model.Board;
using TM.Digital.Model.Resources;
using TM.Digital.Model.Tile;

namespace TM.Digital.Ui.Resources.Resources.TemplateSelectors
{
    public class ResourceBoardTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Steel { get; set; }
        public DataTemplate Titanium { get; set; }
        public DataTemplate Plant { get; set; }
        public DataTemplate Money { get; set; }
        public DataTemplate Heat { get; set; }
        public DataTemplate Energy { get; set; }
        public DataTemplate Card { get; set; }
        public DataTemplate Animal { get; set; }
        public DataTemplate Microbe { get; set; }
        public DataTemplate Science { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item != null)
            {
                ResourceType t = (ResourceType)Enum.Parse(typeof(ResourceType), item.ToString());
                switch (t)
                {
                    case ResourceType.None:
                        break;

                    case ResourceType.Steel:
                        return Steel;

                    case ResourceType.Titanium:
                        return Titanium;

                    case ResourceType.Card:
                        return Card;

                    case ResourceType.Plant:
                        return Plant;

                    case ResourceType.Heat:
                        return Heat;

                    case ResourceType.Money:
                        return Money;

                    case ResourceType.Energy:
                        return Energy;

                    case ResourceType.Animal:
                        return Animal;

                    case ResourceType.Microbe:
                        return Microbe;

                    case ResourceType.Fleet:
                        break;

                    case ResourceType.Science:
                        return Science;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return null;
        }
    }
    public class GolbalParameterTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Temprature { get; set; }
        public DataTemplate Oxygen { get; set; }
        public DataTemplate TerraformationLevel { get; set; }
        public DataTemplate Ocean { get; set; }


        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item != null)
            {
                BoardLevelType t = (BoardLevelType)Enum.Parse(typeof(BoardLevelType), item.ToString());
                switch (t)
                {
                    case BoardLevelType.Temperature:
                        return Temprature;

                    case BoardLevelType.Oxygen:
                        return Oxygen;
                    case BoardLevelType.Terraformation:
                        return TerraformationLevel;
                    case BoardLevelType.Oceans:
                        return Ocean;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return null;
        }
    }

    public class TileTypeTemplateSelector : DataTemplateSelector
    {
        public DataTemplate City { get; set; }
        public DataTemplate Ocean { get; set; }
        public DataTemplate Forest { get; set; }


        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item != null)
            {
                TileType t = (TileType)Enum.Parse(typeof(TileType), item.ToString());
                switch (t)
                {
                    case TileType.City:
                        return City;
                    case TileType.Forest:
                        return Forest;
                    case TileType.Ocean:
                        return Ocean;
                    case TileType.NuclearZone:
                        break;
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
                        break;
                    case TileType.Colony:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return null;
        }
    }



}