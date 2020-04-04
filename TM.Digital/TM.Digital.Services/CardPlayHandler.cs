using TM.Digital.Model.Board;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Effects;
using TM.Digital.Model.Player;

namespace TM.Digital.Services
{
    public static class CardPlayHandler
    {
        public static void Play(Patent card, Player player, Board board)
        {
            //TODO check resources for reductions

            foreach (var cardResourceEffect in card.ResourcesEffects)
            {
                EffectHandler.HandleResourceEffect(player, cardResourceEffect);
            }

            foreach (var boardLevelEffect in card.BoardEffects)
            {
                BoardEffectHandler.HandleBoardEffect(boardLevelEffect, board, player);
            }

            player.HandCards.Remove(card);
            player.PlayedCards.Add(card);
        }
    }
}