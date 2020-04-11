﻿using System;
using System.Collections.Generic;
using System.Linq;
using TM.Digital.Cards;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Corporations;

namespace TM.Digital.Services
{
    public class CardDrawer
    {
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

        public CardDrawer()
        {
            _allCorporations.Shuffle();
            _allPatents.Shuffle();

            DiscardPile = new List<Patent>();
            AvailableCorporations = new Queue<Corporation>(_allCorporations);
            AvailablePatents = new Queue<Patent>(_allPatents);
        }

        private Queue<Corporation> AvailableCorporations { get; }

        private Queue<Patent> AvailablePatents { get; set; }

        private List<Patent> DiscardPile { get; }

        public Corporation DrawCorporation()
        {
            return AvailableCorporations.Dequeue();
        }

        public Patent DrawPatent()
        {
            if (AvailablePatents.Count > 0)
                return AvailablePatents.Dequeue();

            ReShuffle();
            return DrawPatent();
        }

        private void ReShuffle()
        {
            DiscardPile.Shuffle();

            AvailablePatents = new Queue<Patent>(DiscardPile);
            DiscardPile.Clear();

        }

        public Corporation PickCorporation(Guid parse)
        {
            return _allCorporations.FirstOrDefault(c => c.Guid == parse);
        }

        public List<Patent> DispatchPatents(Dictionary<string, bool> selectionBoughtCards)
        {
            List<Patent> pat = new List<Patent>();

            foreach (var selectionBoughtCard in selectionBoughtCards)
            {
                var patetn = _allPatents.FirstOrDefault(r => r.Guid == Guid.Parse(selectionBoughtCard.Key));
                if (selectionBoughtCard.Value)
                {

                    if (patetn != null)
                    {
                        pat.Add(patetn);
                    }
                }
                else
                {
                    DiscardPile.Add(patetn);
                }
            }
            return pat;
        }
    }
}