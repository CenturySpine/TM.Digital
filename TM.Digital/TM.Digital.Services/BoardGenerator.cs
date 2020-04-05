using System.Collections.Generic;
using System.Drawing;
using TM.Digital.Model.Board;
using TM.Digital.Model.Resources;

namespace TM.Digital.Services
{
    public class BoardGenerator
    {
        public static BoardGenerator Instance { get; } = new BoardGenerator();

        public Board Original()
        {
            int index = 0;
            var board = new Board
            {
                Parameters = new List<BoardParameter>
                {
                    new BoardParameter
                    {
                        Type = BoardLevelType.Temperature,
                        GlobalParameterLevel = new GlobalParameterLevel {BoardLevelType = BoardLevelType.Temperature,Level = -30,Min=-30,Increment = 2, Max = 8}

                    },
                    new BoardParameter
                    {
                        Type = BoardLevelType.Oxygen,
                        GlobalParameterLevel = new GlobalParameterLevel {BoardLevelType = BoardLevelType.Oxygen,Level = 0, Min=0,Increment = 1, Max = 14}

                    },
                    new BoardParameter
                    {
                        Type = BoardLevelType.Oceans,
                        GlobalParameterLevel = new GlobalParameterLevel {BoardLevelType = BoardLevelType.Oceans,Level = 0,Min=0,Increment = 1, Max = 9}

                    },
                },
                BoardLines = new List<BoardLine>

                {
                    //line #1
                    new BoardLine
                    {
                        Index = 0,
                        BoardPlaces = new List<BoardPlace>{
                    new BoardPlace { Index = new Point(0,0), PlacementBonus = new List<BoardPlaceBonus>{new BoardPlaceBonus { BonusType = ResourceType.Steel  }, new BoardPlaceBonus { BonusType = ResourceType.Steel } } },
                    new BoardPlace { Index = new Point(0,1), PlacementBonus = new List<BoardPlaceBonus>{new BoardPlaceBonus { BonusType = ResourceType.Steel  },new BoardPlaceBonus { BonusType = ResourceType.Steel } }, Reserved = new BoardPlaceReservedSpace {  ReservedFor = ReservedFor.Ocean } } ,
                    new BoardPlace { Index = new Point(0,2) },
                    new BoardPlace { Index = new Point(0,3), PlacementBonus = new List<BoardPlaceBonus>{new BoardPlaceBonus { BonusType = ResourceType.Card  } }, Reserved = new BoardPlaceReservedSpace {  ReservedFor = ReservedFor.Ocean } } ,
                    new BoardPlace { Index = new Point(0,4), Reserved = new BoardPlaceReservedSpace { IsExclusive = true, ReservedFor = ReservedFor.Ocean } }
                        }},
                    new BoardLine
                    {
                        Index = 1,
                        BoardPlaces = new List<BoardPlace>{
                    //line #2
                    new BoardPlace { Index = new Point(1,0) },
                    new BoardPlace { Index = new Point(1,1),Name="Tharsis Tholus", PlacementBonus = new List<BoardPlaceBonus>{new BoardPlaceBonus { BonusType = ResourceType.Steel } }, Reserved = new BoardPlaceReservedSpace {IsExclusive=false, ReservedFor = ReservedFor.Volcano } },
                    new BoardPlace { Index = new Point(1, 2)},
                    new BoardPlace { Index = new Point(1,3) },
                    new BoardPlace { Index = new Point(1,4) },
                    new BoardPlace { Index = new Point(1,5), PlacementBonus = new List<BoardPlaceBonus>{new BoardPlaceBonus { BonusType = ResourceType.Card  },new BoardPlaceBonus { BonusType = ResourceType.Card  } }, Reserved = new BoardPlaceReservedSpace {  ReservedFor = ReservedFor.Ocean } }
                    }},
                    new BoardLine
                    { Index = 2,
                        BoardPlaces = new List<BoardPlace>{
                    //line #3
                    new BoardPlace { Index = new Point(2,0),Name="Ascareus Mons", PlacementBonus = new List<BoardPlaceBonus>{ new BoardPlaceBonus { BonusType = ResourceType.Card  } } },
                    new BoardPlace { Index = new Point(2,1) },
                    new BoardPlace { Index = new Point(2,2) },
                    new BoardPlace { Index = new Point(2,3) },
                    new BoardPlace { Index = new Point(2,4) },
                    new BoardPlace { Index = new Point(2,5) },
                    new BoardPlace { Index = new Point(2,6), PlacementBonus =  new List<BoardPlaceBonus>{new BoardPlaceBonus { BonusType = ResourceType.Steel  } } }
                    }},
                    new BoardLine
                    { Index = 3,
                        BoardPlaces = new List<BoardPlace>{
                    //line #4
                    new BoardPlace { Index = new Point(3,0), Name="Pavonis Mons",PlacementBonus =  new List<BoardPlaceBonus>{ new BoardPlaceBonus { BonusType = ResourceType.Titanium  },new BoardPlaceBonus { BonusType = ResourceType.Plant  } }, Reserved = new BoardPlaceReservedSpace {IsExclusive=false, ReservedFor = ReservedFor.Volcano } },
                    new BoardPlace { Index = new Point(03,1), PlacementBonus = new List<BoardPlaceBonus>{ new BoardPlaceBonus { BonusType = ResourceType.Plant  } } },
                    new BoardPlace { Index = new Point(03,2), PlacementBonus = new List<BoardPlaceBonus>{ new BoardPlaceBonus { BonusType = ResourceType.Plant  } } },
                    new BoardPlace { Index = new Point(03,3), PlacementBonus = new List<BoardPlaceBonus>{ new BoardPlaceBonus { BonusType = ResourceType.Plant  } } },
                    new BoardPlace { Index = new Point(03,4), PlacementBonus = new List<BoardPlaceBonus>{ new BoardPlaceBonus { BonusType = ResourceType.Plant  }, new BoardPlaceBonus { BonusType = ResourceType.Plant } } },
                    new BoardPlace { Index = new Point(03,5), PlacementBonus = new List<BoardPlaceBonus>{ new BoardPlaceBonus { BonusType = ResourceType.Plant  } } },
                    new BoardPlace { Index = new Point(03,6), PlacementBonus = new List<BoardPlaceBonus>{ new BoardPlaceBonus { BonusType = ResourceType.Plant  } } },
                    new BoardPlace { Index = new Point(03,7), PlacementBonus = new List<BoardPlaceBonus>{ new BoardPlaceBonus { BonusType = ResourceType.Plant  },new BoardPlaceBonus { BonusType = ResourceType.Plant  } },Reserved = new BoardPlaceReservedSpace {  ReservedFor = ReservedFor.Ocean } }
                    }},
                    new BoardLine
                    { Index = 4,
                        BoardPlaces = new List<BoardPlace>{
                    //line #5
                    new BoardPlace { Index = new Point(4,0),Name="Arsia Mons", PlacementBonus =  new List<BoardPlaceBonus>{ new BoardPlaceBonus { BonusType = ResourceType.Plant  },new BoardPlaceBonus { BonusType = ResourceType.Plant  } }, Reserved = new BoardPlaceReservedSpace {IsExclusive=false, ReservedFor = ReservedFor.Volcano } },
                    new BoardPlace { Index = new Point(04,1), PlacementBonus = new List<BoardPlaceBonus>{ new BoardPlaceBonus { BonusType = ResourceType.Plant  }, new BoardPlaceBonus { BonusType = ResourceType.Plant } } },
                    new BoardPlace { Index = new Point(04,2),Name="Noctis City", PlacementBonus = new List<BoardPlaceBonus>{ new BoardPlaceBonus { BonusType = ResourceType.Plant  } }, Reserved = new BoardPlaceReservedSpace{IsExclusive=true,ReservedFor=ReservedFor.NoctisCity } },
                    new BoardPlace { Index = new Point(04,3), PlacementBonus = new List<BoardPlaceBonus>{ new BoardPlaceBonus { BonusType = ResourceType.Plant  },new BoardPlaceBonus { BonusType = ResourceType.Plant  } },Reserved = new BoardPlaceReservedSpace {  ReservedFor = ReservedFor.Ocean } },
                    new BoardPlace { Index = new Point(04,4), PlacementBonus = new List<BoardPlaceBonus>{ new BoardPlaceBonus { BonusType = ResourceType.Plant  },new BoardPlaceBonus { BonusType = ResourceType.Plant  } },Reserved = new BoardPlaceReservedSpace {  ReservedFor = ReservedFor.Ocean } },
                    new BoardPlace { Index = new Point(04,5), PlacementBonus = new List<BoardPlaceBonus>{ new BoardPlaceBonus { BonusType = ResourceType.Plant  },new BoardPlaceBonus { BonusType = ResourceType.Plant  } },Reserved = new BoardPlaceReservedSpace {  ReservedFor = ReservedFor.Ocean } },
                    new BoardPlace { Index = new Point(04,6), PlacementBonus = new List<BoardPlaceBonus>{ new BoardPlaceBonus { BonusType = ResourceType.Plant  }, new BoardPlaceBonus { BonusType = ResourceType.Plant } } },
                    new BoardPlace { Index = new Point(04,7), PlacementBonus = new List<BoardPlaceBonus>{ new BoardPlaceBonus { BonusType = ResourceType.Plant  }, new BoardPlaceBonus { BonusType = ResourceType.Plant } } },
                    new BoardPlace { Index = new Point(04,8), PlacementBonus = new List<BoardPlaceBonus>{ new BoardPlaceBonus { BonusType = ResourceType.Plant  }, new BoardPlaceBonus { BonusType = ResourceType.Plant } } }
                    }},
                    new BoardLine
                    { Index = 5,
                        BoardPlaces = new List<BoardPlace>{
                    //line #6
                    new BoardPlace { Index = new Point(5,0), PlacementBonus = new List<BoardPlaceBonus>{ new BoardPlaceBonus { BonusType = ResourceType.Plant  } } },
                    new BoardPlace { Index = new Point(05,1), PlacementBonus = new List<BoardPlaceBonus>{ new BoardPlaceBonus { BonusType = ResourceType.Plant }, new BoardPlaceBonus { BonusType = ResourceType.Plant } } },
                    new BoardPlace { Index = new Point(05,2), PlacementBonus = new List<BoardPlaceBonus>{ new BoardPlaceBonus { BonusType = ResourceType.Plant  } } },
                    new BoardPlace { Index = new Point(05,3), PlacementBonus = new List<BoardPlaceBonus>{ new BoardPlaceBonus { BonusType = ResourceType.Plant  } } },
                    new BoardPlace { Index = new Point(05,4), PlacementBonus = new List<BoardPlaceBonus>{ new BoardPlaceBonus { BonusType = ResourceType.Plant  } } },
                    new BoardPlace { Index = new Point(05,5), PlacementBonus = new List<BoardPlaceBonus>{ new BoardPlaceBonus { BonusType = ResourceType.Plant  } },Reserved = new BoardPlaceReservedSpace {  ReservedFor = ReservedFor.Ocean } },
                    new BoardPlace { Index = new Point(05,6), PlacementBonus = new List<BoardPlaceBonus>{ new BoardPlaceBonus { BonusType = ResourceType.Plant  } },Reserved = new BoardPlaceReservedSpace {  ReservedFor = ReservedFor.Ocean } },
                    new BoardPlace { Index = new Point(05,7), PlacementBonus = new List<BoardPlaceBonus>{ new BoardPlaceBonus { BonusType = ResourceType.Plant  } },Reserved = new BoardPlaceReservedSpace {  ReservedFor = ReservedFor.Ocean } }
                    }},
                    new BoardLine
                    { Index = 6,
                        BoardPlaces = new List<BoardPlace>{
                    //line #7
                    new BoardPlace { Index = new Point(6,0) },
                    new BoardPlace { Index = new Point(6,1) },
                    new BoardPlace { Index = new Point(6,2) },
                    new BoardPlace { Index = new Point(6,3) },
                    new BoardPlace { Index = new Point(6,4) },
                    new BoardPlace { Index = new Point(6,5) ,PlacementBonus = new List<BoardPlaceBonus>{ new BoardPlaceBonus { BonusType = ResourceType.Plant  } }},
                    new BoardPlace { Index = new Point(6,6) }
                    }},
                    new BoardLine
                    { Index = 7,
                        BoardPlaces = new List<BoardPlace>{
                    //line #8
                    new BoardPlace { Index = new Point(7,0) ,PlacementBonus =  new List<BoardPlaceBonus>{new BoardPlaceBonus { BonusType = ResourceType.Steel  },new BoardPlaceBonus { BonusType = ResourceType.Steel  } }},
                    new BoardPlace { Index = new Point(07,1) },
                    new BoardPlace { Index = new Point(07,2),PlacementBonus =  new List<BoardPlaceBonus>{new BoardPlaceBonus { BonusType = ResourceType.Card  } } },
                    new BoardPlace { Index = new Point(07,3) ,PlacementBonus =  new List<BoardPlaceBonus>{new BoardPlaceBonus { BonusType = ResourceType.Card  } }},
                    new BoardPlace { Index = new Point(07,4) },
                    new BoardPlace { Index = new Point(07,5) ,PlacementBonus = new List<BoardPlaceBonus>{ new BoardPlaceBonus { BonusType = ResourceType.Titanium  } }}
                    }},
                    new BoardLine
                    { Index = 8,
                        BoardPlaces = new List<BoardPlace>{
                    //line #9
                    new BoardPlace { Index = new Point(8,0) ,PlacementBonus =  new List<BoardPlaceBonus>{new BoardPlaceBonus { BonusType = ResourceType.Steel  } }},
                    new BoardPlace { Index = new Point(8,1),PlacementBonus =  new List<BoardPlaceBonus>{new BoardPlaceBonus { BonusType = ResourceType.Steel  }, new BoardPlaceBonus { BonusType = ResourceType.Steel } } },
                    new BoardPlace { Index = new Point(8,2) },
                    new BoardPlace { Index = new Point(8,3)},

                    new BoardPlace { Index = new Point(8,4) ,PlacementBonus = new List<BoardPlaceBonus>{ new BoardPlaceBonus { BonusType = ResourceType.Titanium  },new BoardPlaceBonus { BonusType = ResourceType.Titanium  } },Reserved = new BoardPlaceReservedSpace {  ReservedFor = ReservedFor.Ocean } }
                    }}
                },
                IsolatedPlaces = new List<BoardPlace>
                {
                    new BoardPlace {Index = new Point(99,0),Name = "Phobos Space Haven",Reserved = new BoardPlaceReservedSpace {ReservedFor = ReservedFor.Phobos}},
                    new BoardPlace {Index = new Point(99,1) ,Name = "Ganymede Colony",Reserved = new BoardPlaceReservedSpace {ReservedFor = ReservedFor.Ganymede}},
                }

            };


            return board;
        }
    }
}