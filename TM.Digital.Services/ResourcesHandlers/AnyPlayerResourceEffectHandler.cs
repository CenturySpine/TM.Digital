using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using TM.Digital.Model;
using TM.Digital.Model.Board;
using TM.Digital.Model.Effects;
using TM.Digital.Model.Player;

namespace TM.Digital.Services.ResourcesHandlers
{
    class AnyPlayerResourceEffectHandler
    {
        internal  Action<Player, Board> Resolve(ResourceEffect cardResourceEffect, List<Player> allPlayers)
        {
            return async (p, b) =>
            {
                ResourceEffectPlayerChooserList chooser = new ResourceEffectPlayerChooserList
                {
                    ChoicesList = allPlayers.Select(target =>
                    {
                        var targetPlayerREsource = target.Resources
                            .First(r => r.ResourceType == cardResourceEffect.ResourceType);
                        return new ResourceEffectPlayerChooser
                        {
                            TargetPlayerId = target.PlayerId,
                            TargetPlayerName = target.Name,
                            ResourceHandler = cardResourceEffect,
                        };
                    }).ToList()
                };
                chooser.ChoicesList.Add(new ResourceEffectPlayerChooser
                {
                    TargetPlayerId = GameSession._neutralPlayerId,
                    TargetPlayerName = "Neutral Player / Nobody",
                    ResourceHandler = cardResourceEffect,
                });

                await Hubconcentrator.Hub.Clients.All.SendAsync(ServerPushMethods.ResourceEffectForOtherPlayer,
                    $"{p.PlayerId}", JsonSerializer.Serialize(chooser));
            };
        }

    
    }
}