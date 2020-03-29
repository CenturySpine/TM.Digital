using System.Linq;
using TM.Digital.Model.Board;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Resources;

namespace TM.Digital.Model.Effects
{
    public class Effect
    {
    }

    public class ResourceUnitEffect : IEffect
    {
        public ResourceType ResourceType { get; set; }
        public int Amount { get; set; }

        public void Apply(Player.Player player, Board.Board board, Card card)
        {
            var resource = player.PlayerBoard.Resources.FirstOrDefault(r => r.ResourceType == ResourceType);

            if (resource != null)
            {
                resource.UnitCount += Amount;

                //never go below zero for resources units
                if (resource.UnitCount < 0)
                {
                    resource.UnitCount = 0;
                }
            }
        }
    }

    public class TagCardCostEffect : IEffect
    {
        public Tags AffectedTag { get; set; }
        public int CostEffect { get; set; }

        public void Apply(Player.Player player, Board.Board board, Card card)
        {
            var tagCount = card.Tags.Count(t => t == AffectedTag);
            card.ModifiedCost = card.BaseCost += CostEffect * tagCount;
        }
    }

    public class TagPlayRewardEffect : IEffect
    {
        public Tags AffectedTag { get; set; }
        public int RewardAmount { get; set; }
        public ResourceType ResourceType { get; set; }
        public ResourceKind ResourceKind { get; set; }

        public void Apply(Player.Player player, Board.Board board, Card card)
        {
            var tagCount = card.Tags.Count(t => t == AffectedTag);
            var res = player.PlayerBoard.Resources.FirstOrDefault(r => r.ResourceType == ResourceType);
            if (ResourceKind == ResourceKind.Unit)
            {
                if (res != null) res.UnitCount += tagCount * RewardAmount;
            }
            else
            {
                if (res != null) res.Production += tagCount * RewardAmount;
            }
        }
    }
    public class ResourceProductionEffect : IEffect
    {
        public ResourceType ResourceType { get; set; }
        public int Amount { get; set; }

        public void Apply(Player.Player player, Board.Board board, Card card)
        {
            var resource = player.PlayerBoard.Resources.FirstOrDefault(r => r.ResourceType == ResourceType);

            if (resource != null)
            {
                resource.Production += Amount;

                //never go below -5 for money production
                if (resource.ResourceType == ResourceType.Money && resource.Production < -5)
                {
                    resource.Production = -5;
                }
                //never go below zero for other resources production
                else if (resource.Production < 0)
                {
                    resource.Production = 0;
                }
            }
        }
    }

    public class TerraformationLevelEffect : IEffect
    {
        public int Level { get; set; }

        public void Apply(Player.Player player, Board.Board board, Card card)
        {
            player.TerraformationLevel += Level;
        }
    }
    public class GlobalParameterLevelEffect : IEffect
    {
        public GlobalParameterType GlobalParameterType { get; set; }
        public int Level { get; set; }

        public void Apply(Player.Player player, Board.Board board, Card card)
        {
            board.GlobalParameterLevels[GlobalParameterType].Level +=
                Level * board.GlobalParameterLevels[GlobalParameterType].Increment;
        }
    }
    public interface IEffect
    {
        void Apply(Player.Player player, Board.Board board, Card card);
    }
}