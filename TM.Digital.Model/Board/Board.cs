using System;
using System.Collections.Generic;

namespace TM.Digital.Model.Board
{
    public class Board
    {
        public List<BoardLine> BoardLines { get; set; }

        public List<BoardPlace> IsolatedPlaces { get; set; }

        public List<BoardParameter> Parameters { get; set; }

        public int Generation { get; set; }

    }
}