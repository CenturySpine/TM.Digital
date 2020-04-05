﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using TM.Digital.Cards;
using TM.Digital.Model.Board;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Corporations;
using TM.Digital.Model.Game;
using TM.Digital.Model.Player;
using TM.Digital.Transport.Hubs.Hubs;


namespace TM.Digital.Services
{
    public class GamesService
    {

        public static GamesService Instance { get; } = new GamesService();

        private readonly List<Corporation> _allCorporations = new List<Corporation>
        {
            CorporationsFactory.Arklight(),CorporationsFactory.CheungShingMars(),CorporationsFactory.InterPlanetaryCinematics(), CorporationsFactory.Teractor(), CorporationsFactory.PhobLog(),
        };

        private readonly List<Patent> _allPatents = new List<Patent>
        {
            PatentFactory.BubbleCity(),
            PatentFactory.FusionEnergy(),
            PatentFactory.GiantAsteroid(),
            PatentFactory.IdleGazLiberation(),
            PatentFactory.SolarWindEnergy(),
            PatentFactory.ThreeDimensionalHomePrinting(),
            PatentFactory.ToundraAgriculture(),
            PatentFactory.AdvancedAlliages(),
            PatentFactory.Comet(),
            PatentFactory.ProtectedValley(),
            PatentFactory.GiantIceAsteroid()
        };

        private readonly Dictionary<Guid, GameSession> _currentSessions = new Dictionary<Guid, GameSession>();

        public Guid StartGame(int numberOfPlayer)
        {
            GameSession gs = new GameSession { Id = Guid.NewGuid() };
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

        public GameSetup AddPlayer(Guid gameId, string playerName, bool test)
        {
            try
            {
                if (_currentSessions.TryGetValue(gameId, out var session))
                {
                    GameSetup gameSetup = session.AddPlayer(playerName, test);
                    return gameSetup;
                }

                throw Errors.ErrorGameIdNotFound(gameId);
            }
            catch (Exception e)
            {

                Logger.Log("ERROR", e.ToString());
            }

            return null;
        }

        public Player SetupPlayer(GameSetupSelection selection)
        {
            try
            {
                if (_currentSessions.TryGetValue(selection.GameId, out var session))
                {
                    Player gameSetup = session.SetupPlayer(selection);
                    return gameSetup;
                }

                throw Errors.ErrorGameIdNotFound(selection.GameId);
            }
            catch (Exception e)
            {

                Logger.Log("ERROR", e.ToString());
            }

            return null;
        }

        public async Task Play(Patent card, Guid gameId, Guid playerId, IHubContext<ClientNotificationHub> hubContext)
        {
            if (_currentSessions.TryGetValue(gameId, out var session))
            {

                await session.PlayCard(card, playerId, hubContext);
                return;
            }

            throw Errors.ErrorGameIdNotFound(gameId);
        }

        public async Task PlaceTile(BoardPlace place, Guid gameId, Guid playerId, IHubContext<ClientNotificationHub> hubContext)
        {
            if (_currentSessions.TryGetValue(gameId, out var session))
            {

                await session.PlaceTile(place, playerId, hubContext);
                return;
            }

            throw Errors.ErrorGameIdNotFound(gameId);
        }
    }
}