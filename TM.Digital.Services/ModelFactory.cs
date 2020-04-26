using System;
using System.Collections.Generic;
using TM.Digital.Model.Board;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Corporations;
using TM.Digital.Model.Player;
using TM.Digital.Model.Resources;
using TM.Digital.Model.Tile;
using Action = TM.Digital.Model.Cards.Action;

namespace TM.Digital.Services
{
    public static class ModelFactory
    {

        public static Player NewPlayer(string name, bool test)
        {
            return new Player
            {
                IsReady = false,
                TotalActions = 2,
                RemainingActions = 0,
                PlayerId = Guid.NewGuid(),
                Name = name,
                Resources = new List<ResourceHandler>
                {
                    new ResourceHandler {ResourceType = ResourceType.Money,UnitCount = test?20:0,Production = test?5:0},
                    new ResourceHandler {ResourceType = ResourceType.Steel,UnitCount = test?20:0,Production = test?5:0},
                    new ResourceHandler {ResourceType = ResourceType.Titanium,UnitCount = test?20:0,Production = test?5:0},
                    new ResourceHandler {ResourceType = ResourceType.Plant,UnitCount = test?20:0,Production = test?5:0},
                    new ResourceHandler {ResourceType = ResourceType.Energy,UnitCount = test?20:0,Production = test?5:0},
                    new ResourceHandler {ResourceType = ResourceType.Heat,UnitCount = test?20:0,Production = test?5:0}
                },
                HandCards = new List<Patent>(),
                PlayedCards = new List<Patent>(),
                Corporation = new Corporation(),
                TerraformationLevel = 20,

                BoardActions = new List<BoardAction>
                {
                    new BoardAction
                    {
                        BoardActionType = BoardActionType.PowerPlant,
                        Name = "Centrale",
                        Actions =                     new Action
                        {
                            ActionFrom = new ActionFrom
                            {
                                ResourceType = ResourceType.Money,
                                ResourceKind = ResourceKind.Unit,
                                Amount = -11,
                                ActionTarget = ActionTarget.Self
                            },
                            ActionTo = new List<ActionTo>
                            {
                                new ActionTo
                                {
                                    ResourceKind = ResourceKind.Production,
                                    ResourceType = ResourceType.Energy,
                                    Amount = 1,ActionTarget = ActionTarget.Self
                                }
                            }
                        }
                    },
                    new BoardAction
                    {
                        BoardActionType = BoardActionType.Asteroid,
                        Name = "Astéroïde",
                        Actions =                     new Action
                        {
                            ActionFrom = new ActionFrom
                            {
                                ResourceType = ResourceType.Money,
                                ResourceKind = ResourceKind.Unit,
                                Amount = -14,
                                ActionTarget = ActionTarget.Self
                            },
                            ActionTo = new List<ActionTo>
                            {
                                new ActionTo
                                {
                                    BoardLevelType = BoardLevelType.Temperature,
                                    Amount = 1
                                }
                            }
                        }
                    },
                    new BoardAction
                    {BoardActionType = BoardActionType.Aquifere,
                        Name = "Aquifère",
                        Actions =                     new Action
                        {
                            ActionFrom = new ActionFrom
                            {
                                ResourceType = ResourceType.Money,
                                ResourceKind = ResourceKind.Unit,
                                Amount = -18,
                                ActionTarget = ActionTarget.Self
                            },
                            ActionTo = new List<ActionTo>
                            {new ActionTo()
                                {
                                    TileEffect = new TileEffect
                                    {
                                        Type = TileType.Ocean,
                                        Number = 1,
                                        Constrains = TilePlacementCosntrains.ReservedForOcean
                                    }
                                }
                            }
                        }
                    },
                    new BoardAction
                    {BoardActionType = BoardActionType.Forest,
                        Name = "Forêt",
                        Actions =                     new Action
                        {
                            ActionFrom = new ActionFrom
                            {
                                ResourceType = ResourceType.Money,
                                ResourceKind = ResourceKind.Unit,
                                Amount = -23,
                                ActionTarget = ActionTarget.Self
                            },
                            ActionTo = new List<ActionTo>
                            {
                                new ActionTo
                                {
                                    TileEffect = new TileEffect
                                    {
                                        Type = TileType.Forest,
                                        Number = 1,
                                        Constrains = TilePlacementCosntrains.StandardForest
                                    }
                                }
                            }
                        }
                    },
                    new BoardAction
                    {BoardActionType = BoardActionType.City,
                        Name = "Cité",
                        Actions =                     new Action
                        {
                            ActionFrom = new ActionFrom
                            {
                                ResourceType = ResourceType.Money,
                                ResourceKind = ResourceKind.Unit,
                                Amount = -25,
                                ActionTarget = ActionTarget.Self
                            },
                            ActionTo = new List<ActionTo>
                            {
                                new ActionTo
                                {
                                    TileEffect = new TileEffect
                                    {
                                        Type = TileType.City,
                                        Number = 1,
                                        Constrains = TilePlacementCosntrains.StandardCity
                                    },
                                }, new ActionTo
                                {
                                    Amount = 1,
                                    ResourceType = ResourceType.Money,
                                    ResourceKind = ResourceKind.Production
                                }
                            }
                        }
                    }
                }
            };
        }
    }
}