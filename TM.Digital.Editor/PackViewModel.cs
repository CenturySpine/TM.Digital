using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using TM.Digital.Model;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Corporations;
using TM.Digital.Ui.Resources.ViewModelCore;

namespace TM.Digital.Editor
{

    public class PackPresenter
    {
        public string Name { get; set; }
        public PackViewModel Content { get; set; }
    }
    public class PackViewModel : NotifierBase
    {

        private object _selectedObject;
        private ExtensionPack _pack;
        private ObservableCollection<Corporation> _corporations;
        private ObservableCollection<Patent> _patents;
        private ObservableCollection<Prelude> _preludes;
        private string _name;
        private string _search;
        private string _destinationPack;


        public PackViewModel(ExtensionPack extensionPack)
        {
            Search = string.Empty;
            Name = extensionPack.Name.ToString();
            Corporations = new ObservableCollection<Corporation>(extensionPack.Corporations);
            CorporationView = CollectionViewSource.GetDefaultView(Corporations);
            CorporationView.Filter = FilterCard;
            CorporationView.SortDescriptions.Add(new SortDescription("OfficialNumberTag", ListSortDirection.Ascending));

            Patents = new ObservableCollection<Patent>(extensionPack.Patents);
            PatentsView = CollectionViewSource.GetDefaultView(Patents);
            PatentsView.Filter = FilterCard;
            PatentsView.SortDescriptions.Add(new SortDescription("OfficialNumberTag", ListSortDirection.Ascending));

            Preludes = new ObservableCollection<Prelude>(extensionPack.Preludes);
            PreludesView = CollectionViewSource.GetDefaultView(Preludes);
            PreludesView.Filter = FilterCard;
            PreludesView.SortDescriptions.Add(new SortDescription("OfficialNumberTag",ListSortDirection.Ascending));

            Refresh = new RelayCommand(ExecuteRefresh);
            AddCorporationCommand = new RelayCommand(ExecuteAddcorporation);
            AddPatentCommand = new RelayCommand(ExecuteAddPatent);
            AddPreludeCommand = new RelayCommand(ExecuteAddPrelude);


            Deletecommand = new RelayCommand(ExecuteDelete);
        }



        private bool FilterCard(object obj)
        {
            if (obj is Card c)
            {
                return string.IsNullOrEmpty(c.Name) && string.IsNullOrEmpty(Search) || !string.IsNullOrEmpty(c.Name) && c.Name.ToUpper().Contains(Search.ToUpper());
            }

            return false;
        }

        public string Search
        {
            get => _search;
            set
            {
                _search = value; OnPropertyChanged();
                CorporationView?.Refresh();
                PatentsView?.Refresh();
                PreludesView?.Refresh();
            }
        }

        public ICollectionView PreludesView { get; set; }

        public ICollectionView PatentsView { get; set; }

        public ICollectionView CorporationView { get; set; }

        private void ExecuteDelete(object obj)
        {

            if (SelectedObject == null)
            {
                return;
            }
            if (SelectedObject is Patent p)
            {
                Patents.Remove(p);
            }
            if (SelectedObject is Corporation c)
            {
                Corporations.Remove(c);
            }
            if (SelectedObject is Prelude pr)
            {
                Preludes.Remove(pr);
            }
        }

        public RelayCommand Deletecommand { get; set; }

        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Corporation> Corporations
        {
            get { return _corporations; }
            set { _corporations = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Patent> Patents
        {
            get { return _patents; }
            set { _patents = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Prelude> Preludes
        {
            get { return _preludes; }
            set { _preludes = value; OnPropertyChanged(); }
        }

        private void ExecuteAddPatent(object obj)
        {
            var patent = new Patent
            {

            };
            Patents.Add(patent);
            PatentsView.Refresh();
            SelectedObject = patent;
        }

        private void ExecuteAddcorporation(object obj)
        {
            var corp = new Corporation
            {

            };
            Corporations.Add(corp);
            CorporationView.Refresh();
            SelectedObject = corp;
        }

        private void ExecuteAddPrelude(object obj)
        {
            var corp = new Prelude();
            {

            };
            Preludes.Add(corp);
            PreludesView.Refresh();
            SelectedObject = corp;
        }

        private void ExecuteRefresh(object obj)
        {

            if (SelectedObject is Prelude)
            {
                PreludesView.Refresh();
            }
            if (SelectedObject is Patent)
            {
                PatentsView.Refresh();
            }
            if (SelectedObject is Corporation)
            {
                CorporationView.Refresh();
            }
   
            
        }


        public object SelectedObject
        {
            get { return _selectedObject; }
            set { _selectedObject = value; OnPropertyChanged(); }
        }

        public RelayCommand AddCorporationCommand { get; set; }
        public RelayCommand AddPatentCommand { get; set; }
        public RelayCommand AddPreludeCommand { get; set; }

        public RelayCommand Refresh { get; }


    }
}