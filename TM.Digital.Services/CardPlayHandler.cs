using System.Collections.Generic;
using System.Linq;
using TM.Digital.Model.Board;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Effects;
using TM.Digital.Model.Player;
using TM.Digital.Model.Tile;

namespace TM.Digital.Services
{
    public static class CardPlayHandler
    {
        public static Choices Play(Patent card, Player player, Board board)
        {
            Logger.Log(player.Name, $"Player '{player.Name}' playing card '{card.Name}'");

            Choices ch = new Choices();
            player.HandCards.Remove(card);
            player.PlayedCards.Add(card);
            //TODO check resources for reductions

            foreach (var cardResourceEffect in card.ResourcesEffects)
            {
                EffectHandler.HandleResourceEffect(player, cardResourceEffect);
            }

            foreach (var boardLevelEffect in card.BoardEffects)
            {
                BoardEffectHandler.HandleBoardEffect(boardLevelEffect, board, player);
            }

            if (card.TileEffects.Any())
            {
                ch.TileEffects = new List<TileEffect>(card.TileEffects);
            }

            return ch;
        }


    }
}