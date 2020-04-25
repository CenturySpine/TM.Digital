using System;
using System.Collections.Generic;
using System.Linq;

namespace TM.Digital.Model.Board
{
    public class Board
    {
        public List<BoardLine> BoardLines { get; set; }

        public List<BoardPlace> IsolatedPlaces { get; set; }

        public List<BoardParameter> Parameters { get; set; }

        public int Generation { get; set; }

        public Board Clone()
        {
            var clone = new Board
            {
                Parameters = new List<BoardParameter>(Parameters.Select(p=>p.Clone())),
                Generation = this.Generation,
                IsolatedPlaces = new List<BoardPlace>(this.IsolatedPlaces.Select(p=>p.Clone())),
                BoardLines = new List<BoardLine>(BoardLines.Select(l=>l.Clone()))
            };


            return clone;
        }
    }
}