using System;

namespace TM.Digital.Model.Player
{
    public class Player
    {
        public Player()
        {
            TerraformationLevel = 20;
            PlayerBoard = new PlayerBoard();
        }
        public Guid PlayerId { get; set; }
        public string Name { get; set; }
        public PlayerBoard PlayerBoard { get; set; }
        public int TerraformationLevel { get; set; }
    }
}