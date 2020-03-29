using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using TM.Digital.Cards;
using TM.Digital.Model.Board;
using TM.Digital.Model.Corporations;
using TM.Digital.Model.Game;
using TM.Digital.Model.Player;

namespace TM.Digital.Services
{
    public class GameService
    {
        public static GameService Instance { get; } = new GameService();

        private readonly List<Corporation> _allCorporations = new List<Corporation>()
        {
            new Arklight(),new CheungShingMars(),new InterPlanetaryCinematics(), new Teractor()
        };
        Dictionary<Guid, Player> _players = new Dictionary<Guid, Player>();
        private Queue<Corporation> _availableCorporations;
        private Board _board;


        public void StartGame()
        {
            _board = BoardGenerator.Instance.Original();
            _allCorporations.Shuffle();
            _availableCorporations = new Queue<Corporation>(_allCorporations);
        }

        public GameSetup AddPlayer()
        {
            var player = new Player { PlayerId = Guid.NewGuid() };

            _players.Add(player.PlayerId, player);

            GameSetup gs = new GameSetup() { PlayerId = player.PlayerId };
            for (int i = 0; i < 2; i++)
            {
                gs.Corporations.Add(_availableCorporations.Dequeue());
            }

            return gs;
        }

        public Player AddPlayer(GameSetupSelection selection)
        {

            if (_players.TryGetValue(selection.PlayerId, out var player))
            {
                foreach (var corporationEffect in selection.Corporation.Effects)
                {
                    corporationEffect.Apply(player, _board, selection.Corporation);
                }

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
