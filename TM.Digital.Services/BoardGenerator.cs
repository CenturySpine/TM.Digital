using System.Collections.Generic;
using TM.Digital.Model.Board;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Resources;
using TM.Digital.Model.Tile;

namespace TM.Digital.Services
{
    public class BoardGenerator
    {
        public static BoardGenerator Instance { get; } = new BoardGenerator();

        public Board Original()
        {
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
                    new BoardPlace { Index = new PlaceCoordinates {Y = 0, X=2}, PlacementBonus = new List<BoardPlaceBonus>{new BoardPlaceBonus { BonusType = ResourceType.Steel  }, new BoardPlaceBonus { BonusType = ResourceType.Steel } } },
                    new BoardPlace { Index = new PlaceCoordinates {Y = 0, X=3}, PlacementBonus = new List<BoardPlaceBonus>{new BoardPlaceBonus { BonusType = ResourceType.Steel  },new BoardPlaceBonus { BonusType = ResourceType.Steel } }, Reserved = new BoardPlaceReservedSpace {  ReservedFor = ReservedFor.Ocean } } ,
                    new BoardPlace { Index = new PlaceCoordinates {Y = 0, X=4} },
                    new BoardPlace { Index = new PlaceCoordinates {Y = 0, X=5}, PlacementBonus = new List<BoardPlaceBonus>{new BoardPlaceBonus { BonusType = ResourceType.Card  } }, Reserved = new BoardPlaceReservedSpace {  ReservedFor = ReservedFor.Ocean } } ,
                    new BoardPlace { Index = new PlaceCoordinates {Y = 0, X=6}, Reserved = new BoardPlaceReservedSpace { IsExclusive = true, ReservedFor = ReservedFor.Ocean } }
                        }},
                    new BoardLine
                    {
                        Index = 1,
                        BoardPlaces = new List<BoardPlace>{
                    //line #2
                    new BoardPlace { Index = new PlaceCoordinates {Y = 1, X=1} },
                    new BoardPlace { Index = new PlaceCoordinates {Y = 1, X=2},Name="Tharsis Tholus", PlacementBonus = new List<BoardPlaceBonus>{new BoardPlaceBonus { BonusType = ResourceType.Steel } }, Reserved = new BoardPlaceReservedSpace {IsExclusive=false, ReservedFor = ReservedFor.Volcano } },
                    new BoardPlace { Index = new PlaceCoordinates {Y = 1, X=3}},
                    new BoardPlace { Index = new PlaceCoordinates {Y = 1, X=4} },
                    new BoardPlace { Index = new PlaceCoordinates {Y = 1, X=5} },
                    new BoardPlace { Index = new PlaceCoordinates {Y = 1, X=6}, PlacementBonus = new List<BoardPlaceBonus>{new BoardPlaceBonus { BonusType = ResourceType.Card  },new BoardPlaceBonus { BonusType = ResourceType.Card  } }, Reserved = new BoardPlaceReservedSpace {  ReservedFor = ReservedFor.Ocean } }
                    }},
                    new BoardLine
                    { Index = 2,
                        BoardPlaces = new List<BoardPlace>{
                    //line #3
                    new BoardPlace { Index = new PlaceCoordinates {Y = 2, X=1},Name="Ascareus Mons", PlacementBonus = new List<BoardPlaceBonus>{ new BoardPlaceBonus { BonusType = ResourceType.Card  } } },
                    new BoardPlace { Index = new PlaceCoordinates {Y = 2, X=2} },
                    new BoardPlace { Index = new PlaceCoordinates {Y = 2, X=3} },
                    new BoardPlace { Index = new PlaceCoordinates {Y = 2, X=4} },
                    new BoardPlace { Index = new PlaceCoordinates {Y = 2, X=5} },
                    new BoardPlace { Index = new PlaceCoordinates {Y = 2, X=6} },
                    new BoardPlace { Index = new PlaceCoordinates {Y = 2, X=7}, PlacementBonus =  new List<BoardPlaceBonus>{new BoardPlaceBonus { BonusType = ResourceType.Steel  } } }
                    }},
                    new BoardLine
                    { Index = 3,
                        BoardPlaces = new List<BoardPlace>{
                    //line #4
                    new BoardPlace { Index = new PlaceCoordinates {Y = 3, X=0}, Name="Pavonis Mons",PlacementBonus =  new List<BoardPlaceBonus>{ new BoardPlaceBonus { BonusType = ResourceType.Titanium  },new BoardPlaceBonus { BonusType = ResourceType.Plant  } }, Reserved = new BoardPlaceReservedSpace {IsExclusive=false, ReservedFor = ReservedFor.Volcano } },
                    new BoardPlace { Index = new PlaceCoordinates {Y = 3, X=1}, PlacementBonus = new List<BoardPlaceBonus>{ new BoardPlaceBonus { BonusType = ResourceType.Plant  } } },
                    new BoardPlace { Index = new PlaceCoordinates {Y = 3, X=2}, PlacementBonus = new List<BoardPlaceBonus>{ new BoardPlaceBonus { BonusType = ResourceType.Plant  } } },
                    new BoardPlace { Index = new PlaceCoordinates {Y = 3, X=3}, PlacementBonus = new List<BoardPlaceBonus>{ new BoardPlaceBonus { BonusType = ResourceType.Plant  } } },
                    new BoardPlace { Index = new PlaceCoordinates {Y = 3, X=4}, PlacementBonus = new List<BoardPlaceBonus>{ new BoardPlaceBonus { BonusType = ResourceType.Plant  }, new BoardPlaceBonus { BonusType = ResourceType.Plant } } },
                    new BoardPlace { Index = new PlaceCoordinates {Y = 3, X=5}, PlacementBonus = new List<BoardPlaceBonus>{ new BoardPlaceBonus { BonusType = ResourceType.Plant  } } },
                    new BoardPlace { Index = new PlaceCoordinates {Y = 3, X=6}, PlacementBonus = new List<BoardPlaceBonus>{ new BoardPlaceBonus { BonusType = ResourceType.Plant  } } },
                    new BoardPlace { Index = new PlaceCoordinates {Y = 3, X=7}, PlacementBonus = new List<BoardPlaceBonus>{ new BoardPlaceBonus { BonusType = ResourceType.Plant  },new BoardPlaceBonus { BonusType = ResourceType.Plant  } },Reserved = new BoardPlaceReservedSpace {  ReservedFor = ReservedFor.Ocean } }
                    }},
                    new BoardLine
                    { Index = 4,
                        BoardPlaces = new List<BoardPlace>{
                    //line #5
                    new BoardPlace { Index =new PlaceCoordinates {Y = 4, X=0},Name="Arsia Mons", PlacementBonus =  new List<BoardPlaceBonus>{ new BoardPlaceBonus { BonusType = ResourceType.Plant  },new BoardPlaceBonus { BonusType = ResourceType.Plant  } }, Reserved = new BoardPlaceReservedSpace {IsExclusive=false, ReservedFor = ReservedFor.Volcano } },
                    new BoardPlace { Index =new PlaceCoordinates {Y =4, X=1}, PlacementBonus = new List<BoardPlaceBonus>{ new BoardPlaceBonus { BonusType = ResourceType.Plant  }, new BoardPlaceBonus { BonusType = ResourceType.Plant } } },
                    new BoardPlace { Index =new PlaceCoordinates {Y = 4, X=2},Name="Noctis City", PlacementBonus = new List<BoardPlaceBonus>{ new BoardPlaceBonus { BonusType = ResourceType.Plant  } }, Reserved = new BoardPlaceReservedSpace{IsExclusive=true,ReservedFor=ReservedFor.NoctisCity } },
                    new BoardPlace { Index =new PlaceCoordinates {Y = 4, X=3}, PlacementBonus = new List<BoardPlaceBonus>{ new BoardPlaceBonus { BonusType = ResourceType.Plant  },new BoardPlaceBonus { BonusType = ResourceType.Plant  } },Reserved = new BoardPlaceReservedSpace {  ReservedFor = ReservedFor.Ocean } },
                    new BoardPlace { Index =new PlaceCoordinates {Y =4, X=4}, PlacementBonus = new List<BoardPlaceBonus>{ new BoardPlaceBonus { BonusType = ResourceType.Plant  },new BoardPlaceBonus { BonusType = ResourceType.Plant  } },Reserved = new BoardPlaceReservedSpace {  ReservedFor = ReservedFor.Ocean } },
                    new BoardPlace { Index =new PlaceCoordinates {Y = 4, X=5}, PlacementBonus = new List<BoardPlaceBonus>{ new BoardPlaceBonus { BonusType = ResourceType.Plant  },new BoardPlaceBonus { BonusType = ResourceType.Plant  } },Reserved = new BoardPlaceReservedSpace {  ReservedFor = ReservedFor.Ocean } },
                    new BoardPlace { Index =new PlaceCoordinates {Y = 4, X=6}, PlacementBonus = new List<BoardPlaceBonus>{ new BoardPlaceBonus { BonusType = ResourceType.Plant  }, new BoardPlaceBonus { BonusType = ResourceType.Plant } } },
                    new BoardPlace { Index =new PlaceCoordinates {Y = 4, X=7}, PlacementBonus = new List<BoardPlaceBonus>{ new BoardPlaceBonus { BonusType = ResourceType.Plant  }, new BoardPlaceBonus { BonusType = ResourceType.Plant } } },
                    new BoardPlace { Index =new PlaceCoordinates {Y = 4, X=8}, PlacementBonus = new List<BoardPlaceBonus>{ new BoardPlaceBonus { BonusType = ResourceType.Plant  }, new BoardPlaceBonus { BonusType = ResourceType.Plant } } }
                    }},
                    new BoardLine
                    { Index = 5,
                        BoardPlaces = new List<BoardPlace>{
                    //line #6
                    new BoardPlace { Index = new PlaceCoordinates {Y = 5, X=0}, PlacementBonus = new List<BoardPlaceBonus>{ new BoardPlaceBonus { BonusType = ResourceType.Plant  } } },
                    new BoardPlace { Index = new PlaceCoordinates {Y = 5, X=1}, PlacementBonus = new List<BoardPlaceBonus>{ new BoardPlaceBonus { BonusType = ResourceType.Plant }, new BoardPlaceBonus { BonusType = ResourceType.Plant } } },
                    new BoardPlace { Index = new PlaceCoordinates {Y = 5, X=2}, PlacementBonus = new List<BoardPlaceBonus>{ new BoardPlaceBonus { BonusType = ResourceType.Plant  } } },
                    new BoardPlace { Index = new PlaceCoordinates {Y = 5, X=3}, PlacementBonus = new List<BoardPlaceBonus>{ new BoardPlaceBonus { BonusType = ResourceType.Plant  } } },
                    new BoardPlace { Index = new PlaceCoordinates {Y = 5, X=4}, PlacementBonus = new List<BoardPlaceBonus>{ new BoardPlaceBonus { BonusType = ResourceType.Plant  } } },
                    new BoardPlace { Index = new PlaceCoordinates {Y = 5, X=5}, PlacementBonus = new List<BoardPlaceBonus>{ new BoardPlaceBonus { BonusType = ResourceType.Plant  } },Reserved = new BoardPlaceReservedSpace {  ReservedFor = ReservedFor.Ocean } },
                    new BoardPlace { Index = new PlaceCoordinates {Y = 5, X=6}, PlacementBonus = new List<BoardPlaceBonus>{ new BoardPlaceBonus { BonusType = ResourceType.Plant  } },Reserved = new BoardPlaceReservedSpace {  ReservedFor = ReservedFor.Ocean } },
                    new BoardPlace { Index = new PlaceCoordinates {Y = 5, X=7}, PlacementBonus = new List<BoardPlaceBonus>{ new BoardPlaceBonus { BonusType = ResourceType.Plant  } },Reserved = new BoardPlaceReservedSpace {  ReservedFor = ReservedFor.Ocean } }
                    }},
                    new BoardLine
                    { Index = 6,
                        BoardPlaces = new List<BoardPlace>{
                    //line #7
                    new BoardPlace { Index = new PlaceCoordinates {Y = 6, X=1} },
                    new BoardPlace { Index = new PlaceCoordinates {Y = 6, X=2} },
                    new BoardPlace { Index = new PlaceCoordinates {Y = 6, X=3} },
                    new BoardPlace { Index = new PlaceCoordinates {Y = 6, X=4} },
                    new BoardPlace { Index = new PlaceCoordinates {Y = 6, X=5} },
                    new BoardPlace { Index = new PlaceCoordinates {Y = 6, X=6} ,PlacementBonus = new List<BoardPlaceBonus>{ new BoardPlaceBonus { BonusType = ResourceType.Plant  } }},
                    new BoardPlace { Index = new PlaceCoordinates {Y = 6, X=7} }
                    }},
                    new BoardLine
                    { Index = 7,
                        BoardPlaces = new List<BoardPlace>{
                    //line #8
                    new BoardPlace { Index = new PlaceCoordinates {Y = 7, X=1},PlacementBonus =  new List<BoardPlaceBonus>{new BoardPlaceBonus { BonusType = ResourceType.Steel  },new BoardPlaceBonus { BonusType = ResourceType.Steel  } }},
                    new BoardPlace { Index = new PlaceCoordinates {Y = 7, X=2} },
                    new BoardPlace { Index = new PlaceCoordinates {Y = 7, X=3},PlacementBonus =  new List<BoardPlaceBonus>{new BoardPlaceBonus { BonusType = ResourceType.Card  } } },
                    new BoardPlace { Index = new PlaceCoordinates {Y = 7, X=4} ,PlacementBonus =  new List<BoardPlaceBonus>{new BoardPlaceBonus { BonusType = ResourceType.Card  } }},
                    new BoardPlace { Index = new PlaceCoordinates {Y = 7, X=5} },
                    new BoardPlace { Index = new PlaceCoordinates {Y = 7, X=6} ,PlacementBonus = new List<BoardPlaceBonus>{ new BoardPlaceBonus { BonusType = ResourceType.Titanium  } }}
                    }},
                    new BoardLine
                    { Index = 8,
                        BoardPlaces = new List<BoardPlace>{
                    //line #9
                    new BoardPlace { Index = new PlaceCoordinates {Y = 8, X=2} ,PlacementBonus =  new List<BoardPlaceBonus>{new BoardPlaceBonus { BonusType = ResourceType.Steel  } }},
                    new BoardPlace { Index = new PlaceCoordinates {Y = 8, X=3},PlacementBonus =  new List<BoardPlaceBonus>{new BoardPlaceBonus { BonusType = ResourceType.Steel  }, new BoardPlaceBonus { BonusType = ResourceType.Steel } } },
                    new BoardPlace { Index = new PlaceCoordinates {Y = 8, X=4} },
                    new BoardPlace { Index = new PlaceCoordinates {Y = 8, X=5}},
                    new BoardPlace { Index = new PlaceCoordinates {Y = 8, X=6} ,PlacementBonus = new List<BoardPlaceBonus>{ new BoardPlaceBonus { BonusType = ResourceType.Titanium  },new BoardPlaceBonus { BonusType = ResourceType.Titanium  } },Reserved = new BoardPlaceReservedSpace {  ReservedFor = ReservedFor.Ocean } }
                    }}
                },
                IsolatedPlaces = new List<BoardPlace>
                {
                    new BoardPlace {Index = new PlaceCoordinates {Y = 99, X=0},Name = "Phobos Space Haven",Reserved = new BoardPlaceReservedSpace {ReservedFor = ReservedFor.Phobos}},
                    new BoardPlace {Index = new PlaceCoordinates {Y = 99, X=1} ,Name = "Ganymede Colony",Reserved = new BoardPlaceReservedSpace {ReservedFor = ReservedFor.Ganymede}},
                },

                
            };

            return board;
        }
    }
}