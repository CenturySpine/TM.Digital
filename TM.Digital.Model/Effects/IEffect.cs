﻿using TM.Digital.Model.Cards;

namespace TM.Digital.Model.Effects
{
    public interface IEffect
    {
        
        void Apply(Player.Player player, Board.Board board, Card card);
    }
}