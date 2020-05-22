﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TM.Digital.Model.Board;
using TM.Digital.Model.Effects;
using TM.Digital.Model.Player;
using TM.Digital.Model.Resources;
using TM.Digital.Services.Common;

namespace TM.Digital.Services.ResourcesHandlers
{
    class SelfResourceEffectHandler
    {
        internal async Task Resolve(Player player, ResourceEffect effect, List<Player> allPlayers,
            Board board, CardDrawer cardDrawer)
        {
            if (effect.ResourceType == ResourceType.Card)
            {
                for (int i = 0; i < effect.Amount; i++)
                {
                    player.HandCards.Add(cardDrawer.DrawPatent());
                    await Logger.Log(player.Name, $"Drawing 1 card from deck");
                }
            }
            var resource = player.Resources.FirstOrDefault(r => r.ResourceType == effect.ResourceType);
            if (resource != null)
            {
                var modifierValue = ModifierValueHandler.ComputeModifierValue(player, effect, allPlayers, board);
                if (effect.ResourceKind == ResourceKind.Production)
                {

                    resource.Production += effect.Amount * modifierValue;

                    //never go below -5 for money production
                    if (resource.ResourceType == ResourceType.Money && resource.Production < -5)
                    {
                        resource.Production = -5;
                    }
                    //never go below zero for other resources production
                    else if (resource.ResourceType != ResourceType.Money && resource.Production < 0)
                    {
                        resource.Production = 0;
                    }
                    await Logger.Log(player.Name, $"Resource {resource.ResourceType} Production modified for {effect.Amount}, new value {resource.Production}");
                }
                else
                {
                    resource.UnitCount += effect.Amount * modifierValue;

                    if (resource.UnitCount < 0) resource.UnitCount = 0;
                    await Logger.Log(player.Name, $"Resource {resource.ResourceType} Unit modified for {effect.Amount}, new value {resource.UnitCount}");
                }
            }
        }
    }
}