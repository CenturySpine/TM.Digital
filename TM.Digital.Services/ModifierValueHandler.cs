using System;
using System.Collections.Generic;
using System.Linq;
using TM.Digital.Model.Board;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Effects;
using TM.Digital.Model.Player;
using TM.Digital.Model.Tile;

namespace TM.Digital.Services
{
    public static class ModifierValueHandler
    {
        internal static int ComputeModifierValue(Player player, ResourceEffect effect, List<Player> allPlayers,
            Board board)
        {
            var mod = effect.EffectModifier;
            if (mod != null)
            {
                
                List<BoardPlace> places = new List<BoardPlace>();
                List<Tags> tags = new List<Tags>();
                switch (mod.ModifierFrom)
                {
                    case ActionTarget.Self:
                        
                        places = BoardTilesHandler.GetPlayerTiles(board, player.PlayerId, mod.EffectModifierLocationConstraint);
                        tags = player.AllPlayedCards.SelectMany(c => c.Tags).ToList();
                        break;
                    case ActionTarget.ToCurrentCard:
                        break;
                    case ActionTarget.FromAnyOtherCard:
                        break;
                    case ActionTarget.AnyPlayer:
                        places = allPlayers.Select(p => p.PlayerId).SelectMany(b => BoardTilesHandler.GetPlayerTiles(board, b, mod.EffectModifierLocationConstraint)).ToList();
                        tags = allPlayers.SelectMany(c => c.AllPlayedCards.SelectMany(p => p.Tags)).ToList();
                        break;
                    case ActionTarget.AnyOpponent:
                        places = allPlayers.Except(new List<Player> { player }).Select(p => p.PlayerId).SelectMany(b => BoardTilesHandler.GetPlayerTiles(board, b, mod.EffectModifierLocationConstraint)).ToList();
                        tags = allPlayers.Except(new List<Player> { player }).SelectMany(c => c.AllPlayedCards.SelectMany(p => p.Tags)).ToList();

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                if (mod.TagsModifier != Tags.None)
                {
                    var tagNumber = tags.Count(t => t == mod.TagsModifier);
                    return (int)Math.Floor((double)tagNumber / mod.ModifierRatio);
                }
                if (mod.TileModifier != TileType.None)
                {
                    var tileNumber = places.Count(t => t.PlayedTile.Type == mod.TileModifier);
                    return (int)Math.Floor((double)tileNumber / mod.ModifierRatio);
                }
            }
            return 1;
        }
    }
}