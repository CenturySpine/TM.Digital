using System;
using System.Collections.Generic;
using System.Linq;
using TM.Digital.Model.Board;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Corporations;
using TM.Digital.Model.Effects;
using TM.Digital.Model.Game;
using TM.Digital.Model.Player;
using TM.Digital.Model.Resources;

namespace TM.Digital.Services
{
    internal class GameSession
    {
        public Guid Id { get; set; }
        public int NumberOfPlayers { get; set; }
        public Board Board { get; set; }
        public Queue<Corporation> AvailableCorporations { get; set; }
        public Queue<Patent> AvailablePatents { get; set; }
        public Dictionary<Guid, Player> Players { get; set; }

        public GameSetup AddPlayer(string playerName)
        {
            var player = new Player
            {
                PlayerId = Guid.NewGuid(),
                Name = playerName,
                Resources = new List<ResourceHandler>
                {
                    new ResourceHandler {ResourceType = ResourceType.Money},
                    new ResourceHandler {ResourceType = ResourceType.Steel},
                    new ResourceHandler {ResourceType = ResourceType.Titanium},
                    new ResourceHandler {ResourceType = ResourceType.Plant},
                    new ResourceHandler {ResourceType = ResourceType.Energy},
                    new ResourceHandler {ResourceType = ResourceType.Heat}
                },
                HandCards = new List<Patent>(),
                PlayedCards = new List<Patent>(),
                TerraformationLevel = 20,
            };
            Players.Add(player.PlayerId, player);
            GameSetup gs = new GameSetup
            {
                PlayerId = player.PlayerId,
                Corporations = new List<Corporation>(),
                Patents = new List<Patent>(),
                GameId = this.Id
            };
            for (int i = 0; i < 2; i++)
            {
                gs.Corporations.Add(AvailableCorporations.Dequeue());
            }
            for (int i = 0; i < 4; i++)
            {
                gs.Patents.Add(AvailablePatents.Dequeue());
            }

            return gs;
        }

        public Player SetupPlayer(GameSetupSelection selection)
        {
            if (Players.TryGetValue(selection.PlayerId, out var player))
            {
                foreach (var corporationEffect in selection.Corporation.ResourcesEffects)
                {
                    EffectHandler.HandleResourceEffect(player, corporationEffect);
                }

                EffectHandler.HandleInitialPatentBuy(player, selection.BoughtCards, selection.Corporation);
                player.Corporation = selection.Corporation;

                EffectHandler.CheckCardsReductions(player);
                PrerequisiteHandler.CanPlayCards(Board, player);

                return player;
            }
            return null;
        }

        public Game PlayCard(Patent card, Guid playerId)
        {
            if (Players.TryGetValue(playerId, out var player))
            {
                CardPlayHandler.Play(card, player, Board);
                EffectHandler.CheckCardsReductions(player);
                PrerequisiteHandler.CanPlayCards(Board, player);
            }

            return new Game
            {
                Board = Board,
                AllPlayers = Players.Select(p => p.Value).ToList(),
            };
        }
    }
}