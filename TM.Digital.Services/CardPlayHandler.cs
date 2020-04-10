using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TM.Digital.Model.Board;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Effects;
using TM.Digital.Model.Player;
using TM.Digital.Model.Tile;
using TM.Digital.Services.Common;

namespace TM.Digital.Services
{
    public static class CardPlayHandler
    {
        public static async Task<Choices> Play(Patent card, Player player, Board board)
        {
            await Logger.Log(player.Name, $"Player '{player.Name}' playing card '{card.Name}'");

            Choices ch = new Choices();
            player.HandCards.Remove(card);
            player.PlayedCards.Add(card);
            //TODO check resources for reductions

            foreach (var cardResourceEffect in card.ResourcesEffects)
            {
                await EffectHandler.HandleResourceEffect(player, cardResourceEffect);
            }

            foreach (var boardLevelEffect in card.BoardEffects)
            {
                await BoardEffectHandler.HandleBoardEffect(boardLevelEffect, board, player);
            }

            if (card.TileEffects.Any())
            {
                ch.TileEffects = new List<TileEffect>(card.TileEffects);
            }

            return ch;
        }


    }
}