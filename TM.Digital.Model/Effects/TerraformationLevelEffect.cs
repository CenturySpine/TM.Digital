using TM.Digital.Model.Cards;

namespace TM.Digital.Model.Effects
{
    public class TerraformationLevelEffect : IEffect
    {
        public EffectType Type { get; set; } = EffectType.Terraformation;
        public int Level { get; set; }

        public void Apply(Player.Player player, Board.Board board, Card card)
        {
            player.TerraformationLevel += Level;
        }
    }
}