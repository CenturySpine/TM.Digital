using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using TM.Digital.Model;
using TM.Digital.Model.Board;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Effects;
using TM.Digital.Model.Player;

namespace TM.Digital.Services.ResourcesHandlers
{
    class OtherCardHandler
    {
        public Action<Player, Board> Resolve(Player player, ResourceEffect effect, List<Player> allPlayers, Board board, Patent card)
        {
            if (allPlayers == null || !allPlayers.Any())
                throw new InvalidOperationException("All player cannot be empty when handling other card resources");

            return async (p, b) =>
            {
                var cardPlayedAcceptingResources =
                        allPlayers
                            .Select(pl => new
                            { PlayerInfo = pl, Cards = pl.PlayedCards.Cast<Card>().Concat(new[] { pl.Corporation }) })
                            

                    ;

                //.Concat(allPlayers.Select(c => c.Corporation).Cast<Card>())
                //.Where(c => c.ResourceType == effect.ResourceType)
                //.ToList();


                ResourceToOtherCardChooserList chooser = new ResourceToOtherCardChooserList
                {
                    ResourceToOtherCardChooser = new List<ResourceToOtherCardChooser>(
                        cardPlayedAcceptingResources.Select(c => new ResourceToOtherCardChooser()
                        {
                            Effect = effect,
                            Card = c,

                        }))
                };

                await Hubconcentrator.Hub.Clients.All.SendAsync(ServerPushMethods.ResourceEffectForOtherCard,
                    $"{p.PlayerId}", JsonSerializer.Serialize(chooser));
            };
        }
    }
}
