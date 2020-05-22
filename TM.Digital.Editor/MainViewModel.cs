using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using TM.Digital.Editor.Board;
using TM.Digital.Model;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Corporations;
using TM.Digital.Ui.Resources.ViewModelCore;

namespace TM.Digital.Editor
{
    public class MainViewModel : NotifierBase
    {
        private BoardViewModel _boardViewModel;
        private PackPresenter _destinationPack;
        private ObservableCollection<PackPresenter> _packs;
        public MainViewModel(PackManager pm)
        {
            Pm = pm;
            MoveToCommand = new RelayCommand(ExecuteMoveTo);

            SaveCommand = new RelayCommand(ExecuteSave);
            LoadDataCommand = new RelayCommand(ExecuteLoadData);

            SelectBoardPlace = new RelayCommand(ExecuteSelectBoardPlace);
        }

        public BoardViewModel BoardViewModel
        {
            get => _boardViewModel;
            set { _boardViewModel = value; OnPropertyChanged(); }
        }

        public PackPresenter DestinationPack
        {
            get => _destinationPack;
            set { _destinationPack = value; OnPropertyChanged(); }
        }

        public RelayCommand LoadDataCommand { get; set; }
        public RelayCommand MoveToCommand { get; set; }
        public ObservableCollection<PackPresenter> Packs
        {
            get => _packs;
            set
            {
                _packs = value;
                OnPropertyChanged();
            }
        }

        public PackManager Pm { get; }
        public RelayCommand SaveCommand { get; set; }

        //SelectedObject = all;

        public RelayCommand SelectBoardPlace { get; set; }

        private async void ExecuteLoadData(object obj)
        {
            await Pm.Load();
            CardReferencesHolder all = Pm.AllPack;
            BoardViewModel.LoadCommand.Execute(null);
            Packs = new ObservableCollection<PackPresenter>(all.Packs.Select(r => new PackPresenter
            {
                Content = new PackViewModel(r),
                Name = r.Name.ToString(),
            }));
        }

        private void ExecuteMoveTo(object obj)
        {
            if (obj != null && DestinationPack != null)
            {
                if (obj is Patent patent)
                {
                    var currentPack = Packs.FirstOrDefault(r => r.Content.Patents.Contains(patent));
                    if (currentPack != null)
                    {
                        currentPack.Content.Patents.Remove(patent);
                        DestinationPack.Content.Patents.Add(patent);
                    }
                }
                if (obj is Corporation corporation)
                {
                    var currentPack = Packs.FirstOrDefault(r => r.Content.Corporations.Contains(corporation));
                    if (currentPack != null)
                    {
                        currentPack.Content.Corporations.Remove(corporation);
                        DestinationPack.Content.Corporations.Add(corporation);
                    }
                }
                if (obj is Prelude prelude)
                {
                    var currentPack = Packs.FirstOrDefault(r => r.Content.Preludes.Contains(prelude));
                    if (currentPack != null)
                    {
                        currentPack.Content.Preludes.Remove(prelude);
                        DestinationPack.Content.Preludes.Add(prelude);
                    }
                }
            }
        }

        private void ExecuteSave(object obj)
        {
            //var h = new CardReferencesHolder
            //{
            //    Packs = Packs.Select(r => new ExtensionPack
            //    {
            //        Name = (Extensions)Enum.Parse(typeof(Extensions), r.Name),
            //        Patents = r.Content.Patents.ToList(),
            //        Corporations = r.Content.Corporations.ToList(),
            //        Preludes = r.Content.Preludes.ToList(),
            //    }).ToList()
            //};

            Pm.AllPack.Packs = Packs.Select(r => new ExtensionPack
            {
                Name = (Extensions)Enum.Parse(typeof(Extensions), r.Name),
                Patents = r.Content.Patents.ToList(),
                Corporations = r.Content.Corporations.ToList(),
                Preludes = r.Content.Preludes.ToList(),
            }).ToList();
            Pm.Save();
        }

        private void ExecuteSelectBoardPlace(object obj)
        {
            //BoardViewModel.SelectPlace(obj);
        }

        private void SaveObject_OnClick(object sender, RoutedEventArgs e)
        {
        }
    }
}