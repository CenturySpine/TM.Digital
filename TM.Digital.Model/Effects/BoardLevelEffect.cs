using TM.Digital.Model.Board;
using TM.Digital.Model.Cards;

namespace TM.Digital.Model.Effects
{
    public class BoardLevelEffect : Effect
    {
        public BoardLevelType BoardLevelType { get; set; }
        public int Level { get; set; }


        public EffectModifier EffectModifier { get; set; }

    }
}