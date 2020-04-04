using System;
using System.Collections.Generic;
using TM.Digital.Cards;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Corporations;
using TM.Digital.Model.Game;
using TM.Digital.Model.Player;

namespace TM.Digital.Services
{
    public class GamesService
    {
        public static GamesService Instance { get; } = new GamesService();

        private readonly List<Corporation> _allCorporations = new List<Corporation>
        {
            CorporationsFactory.Arklight(),CorporationsFactory.CheungShingMars(),CorporationsFactory.InterPlanetaryCinematics(), CorporationsFactory.Teractor()
        };

        private readonly List<Patent> _allPatents = new List<Patent>()
        {
            PatentFactory.BubbleCity(),PatentFactory.FusionEnergy(),PatentFactory.GiantAsteroid(),PatentFactory.IdleGazLiberation(),PatentFactory.SolarWindEnergy(),PatentFactory.ThreeDimensionalHomePrinting(),PatentFactory.ToundraAgriculture()
        };

        private readonly Dictionary<Guid, GameSession> _currentSessions = new Dictionary<Guid, GameSession>();

        public Guid StartGame(int numberOfPlayer)
        {
            GameSession gs = new GameSession() { Id = Guid.NewGuid() };
            _allCorporations.Shuffle();
            _allPatents.Shuffle();
            gs.NumberOfPlayers = numberOfPlayer;
            gs.Board = BoardGenerator.Instance.Original();
            gs.AvailableCorporations = new Queue<Corporation>(_allCorporations);
            gs.AvailablePatents = new Queue<Patent>(_allPatents);
            gs.Players = new Dictionary<Guid, Player>();
            _currentSessions.Add(gs.Id, gs);

            return gs.Id;
        }

        public GameSetup AddPlayer(Guid gameId, string playerName)
        {
            if (_currentSessions.TryGetValue(gameId, out var session))
            {
                GameSetup gameSetup = session.AddPlayer(playerName);
                return gameSetup;
            }

            throw Errors.ErrorGameIdNotFound(gameId);
        }

        public Player SetupPlayer(GameSetupSelection selection)
        {
            if (_currentSessions.TryGetValue(selection.GameId, out var session))
            {
                Player gameSetup = session.SetupPlayer(selection);
                return gameSetup;
            }

            throw Errors.ErrorGameIdNotFound(selection.GameId);
        }

        public void Play(Patent card, Guid gameId, Guid playerId)
        {
            if (_currentSessions.TryGetValue(gameId, out var session))
            {
                session.PlayCard(card, playerId);
                return;
            }

            throw Errors.ErrorGameIdNotFound(gameId);
        }
    }
}