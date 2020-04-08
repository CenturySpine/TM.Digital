namespace TM.Digital.Client.Screens.HandSetup
{
    public class GameBoardViewModel : GameBoardBaseViewModel
    {
        protected override bool CanExecuteSelectCard(object obj)
        {
            if (obj is PatentSelector patent)
            {
                return patent.Patent.CanBePlayed;
            }

            return false;
        }

        protected override void ExecuteSelectCorporation(object obj)
        {
            //TODO
        }
    }
}