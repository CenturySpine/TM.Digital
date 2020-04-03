using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using TM.Digital.Cards;
using TM.Digital.Model.Board;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Corporations;
using TM.Digital.Model.Effects;
using TM.Digital.Model.Game;
using TM.Digital.Model.Player;
using TM.Digital.Model.Resources;

namespace TM.Digital.Services
{
    public class GameService
    {
        public static GameService Instance { get; } = new GameService();

        private readonly List<Corporation> _allCorporations = new List<Corporation>
        {
            CorporationsFactory.Arklight(),CorporationsFactory.CheungShingMars(),CorporationsFactory.InterPlanetaryCinematics(), CorporationsFactory.Teractor()
        };

        private readonly List<Patent> _allPatents = new List<Patent>()
        {
            PatentFactory.BubbleCity(),PatentFactory.FusionEnergy(),PatentFactory.GiantAsteroid(),PatentFactory.IdleGazLiberation(),PatentFactory.SolarWindEnergy(),PatentFactory.ThreeDimensionalHomePrinting(),PatentFactory.ToundraAgriculture()
        };

        private readonly Dictionary<Guid, Player> _players = new Dictionary<Guid, Player>();
        private Queue<Corporation> _availableCorporations;
        private Queue<Patent> _availablePatents;
        private Board _board;
        private int _numberOfPlayers;

        public bool StartGame(int numberOfPlayer)
        {
            _numberOfPlayers = numberOfPlayer;
            _board = BoardGenerator.Instance.Original();
            _allCorporations.Shuffle();
            _availableCorporations = new Queue<Corporation>(_allCorporations);
            _allPatents.Shuffle();
            _availablePatents = new Queue<Patent>(_allPatents);
            return true;
        }

        public GameSetup AddPlayer(string playerName)
        {
            var player = new Player { PlayerId = Guid.NewGuid(), Name = playerName };

            _players.Add(player.PlayerId, player);

            GameSetup gs = new GameSetup { PlayerId = player.PlayerId, Corporations = new List<Corporation>(), Patents = new List<Patent>() };
            for (int i = 0; i < 2; i++)
            {
                gs.Corporations.Add(_availableCorporations.Dequeue());
            }
            for (int i = 0; i < 4; i++)
            {
                gs.Patents.Add(_availablePatents.Dequeue());
            }

            return gs;
        }

        public Player AddPlayer(GameSetupSelection selection)
        {
            if (_players.TryGetValue(selection.PlayerId, out var player))
            {
                

                foreach (var corporationEffect in selection.Corporation.ResourcesEffects)
                {
                    EffectHandler.HandleResourceEffect(player, corporationEffect);
                }

                var playersMoney = player.Resources.First(r => r.ResourceType == ResourceType.Money);
                playersMoney.UnitCount = selection.Corporation.StartingMoney;
                foreach (var selectionBoughtCard in selection.BoughtCards)
                {
                    playersMoney.UnitCount -= 3;
                    player.HandCards.Add(selectionBoughtCard);
                }

                return player;
            }
            return null;
        }
    }

    internal static class CardsExtensions
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            int n = list.Count;
            while (n > 1)
            {
                byte[] box = new byte[1];
                do provider.GetBytes(box);
                while (!(box[0] < n * (Byte.MaxValue / n)));
                int k = (box[0] % n);
                n--;
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}