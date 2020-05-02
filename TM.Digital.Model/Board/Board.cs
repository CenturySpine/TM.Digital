using System;
using System.Collections.Generic;
using System.Linq;
using Action = TM.Digital.Model.Cards.Action;

namespace TM.Digital.Model.Board
{
    public class Board
    {
        public string Name { get; set; }
        public List<BoardLine> BoardLines { get; set; }

        public List<BoardPlace> IsolatedPlaces { get; set; }

        public List<BoardParameter> Parameters { get; set; }

        public int Generation { get; set; }

        
        public Board Clone()
        {
            var clone = new Board
            {
                Name = Name,
                Parameters = new List<BoardParameter>(Parameters.Select(p=>p.Clone())),
                Generation = this.Generation,
                IsolatedPlaces = new List<BoardPlace>(this.IsolatedPlaces.Select(p=>p.Clone())),
                BoardLines = new List<BoardLine>(BoardLines.Select(l=>l.Clone()))
            };


            return clone;
        }
    }

    public class BoardAction
    {
        public BoardActionType BoardActionType { get; set; }
        public string Name { get; set; }
        public Action Actions { get; set; }

    }

    public enum BoardActionType
    {
        PatentsSell,
        PowerPlant,
        Asteroid,
        Aquifere,
        Forest,
        City
    }
}