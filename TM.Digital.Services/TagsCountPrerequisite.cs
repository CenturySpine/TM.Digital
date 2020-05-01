using System.Collections.Generic;
using System.Linq;
using TM.Digital.Model.Board;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Player;

namespace TM.Digital.Services
{
    /// <summary>
    /// Check that the player owns the required number of target tags among its played cards, NOT INCLUDING EVENT/RED CARDS
    /// </summary>
    public class TagsCountPrerequisite : IPrerequisiteStrategy
    {
        public bool CanPlayCard(Patent inputPatent, Board currentBoardState, Player patentOwner)
        {
            var playerTagsCount = patentOwner.PlayedCards.Concat(new List<Card>{ patentOwner.Corporation }) // players cards + corporation
                .Where(c => c.CardType != CardType.Red)//different from event/red
                .Where(c=>c.Tags!=null && c.Tags.Any())
                .SelectMany(c => c.Tags)//Select ALL tags
                .GroupBy(r => r)//group selection by tag type
                .Select(grp => new { Tag = grp.Key, Count = grp.Count() })//returns object [tag type,count]
                .ToList();

            //for each patent tag prerequisite
            if (inputPatent.Prerequisites?.TagsPrerequisites != null)
                foreach (var patentTagsPrerequisite in inputPatent.Prerequisites.TagsPrerequisites)
                {
                    //get the players number of matching tags
                    var playerTagsMatch = playerTagsCount.FirstOrDefault(t => t.Tag == patentTagsPrerequisite.Tag);

                    //must be a least equal to number prerequisite
                    if (playerTagsMatch == null || playerTagsMatch.Count < patentTagsPrerequisite.Value)
                        return false;
                }

            return true;
        }
    }
}