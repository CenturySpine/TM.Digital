using System;
using System.Collections.Generic;
using TM.Digital.Model.Effects;
using TM.Digital.Model.Resources;

namespace TM.Digital.Model.Cards
{
    public class Card
    {
        
        public string Name { get; set; }

        public List<Tags> Tags { get; set; } = new List<Tags>();

        public CardType CardType { get; set; }

        public ResourceType ResourceType { get; set; }

        public int ResourcesCount { get; set; }

        public int BaseCost { get; set; }
        public int ModifiedCost { get; set; }

        public ICardVictoryPoints CardVictoryPoints { get; set; }

        public List<IEffect> Effects { get; set; } = new List<IEffect>();
        public List<IAction> Actions { get; set; } = new List<IAction>();


    }

    public interface IAction
    {
        void Execute(Board.Board board, Player.Player player);
    }

    public interface ICardVictoryPoints
    {
        int VictoryPoint(Card card);
    }

    public class StandardVictoryPoint : ICardVictoryPoints
    {
        public int Points { get; set; }
        public int VictoryPoint(Card card)
        {
            return Points;
        }
    }

    public class ResourcesVictoryPoints : ICardVictoryPoints
    {
        public ResourceType ResourceType { get; set; }
        public int VictoryPointRatio { get; set; }

        public int VictoryPoint(Card card)
        {
            return (int)Math.Floor((double)card.ResourcesCount / VictoryPointRatio);
        }
    }
}
