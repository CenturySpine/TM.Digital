using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TM.Digital.Model.Board;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Effects;
using TM.Digital.Model.Player;

namespace TM.Digital.Services.ResourcesHandlers
{
    class TargetCardHandler
    {
        public async Task Resolve(Player player, ResourceEffect effect, List<Player> allPlayers,
            Board board, Card card)
        {
            await Task.CompletedTask;
            if (card == null)
                throw new InvalidOperationException("Target card can't be null");

            if (card.ResourceType != effect.ResourceType)
                throw new InvalidOperationException("Effect resource does not match card resource");

            var modifier = ModifierValueHandler.ComputeModifierValue(player, effect, allPlayers, board);

            card.ResourcesCount += effect.Amount * modifier;

        }
    }
}
