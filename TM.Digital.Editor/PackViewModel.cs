using System;
using System.Collections.ObjectModel;
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

        public PackViewModel(ExtensionPack extensionPack)
        {
            Name = extensionPack.Name.ToString();
            Corporations = new ObservableCollection<Corporation>(extensionPack.Corporations);
            Patents = new ObservableCollection<Patent>(extensionPack.Patents);
            Preludes = new ObservableCollection<Prelude>(extensionPack.Preludes);
            Refresh = new RelayCommand(ExecuteRefresh);
            AddCorporationCommand = new RelayCommand(ExecuteAddcorporation);
            AddPatentCommand = new RelayCommand(ExecuteAddPatent);
            AddPreludeCommand = new RelayCommand(ExecuteAddPrelude);

            Deletecommand = new RelayCommand(ExecuteDelete);
        }

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
            var patent = new Patent()
            {

            };
            Patents.Add(patent);
            SelectedObject = patent;
        }

        private void ExecuteAddcorporation(object obj)
        {
            var corp = new Corporation()
            {

            };
            Corporations.Add(corp);
            SelectedObject = corp;
        }

        private void ExecuteAddPrelude(object obj)
        {
            var corp = new Prelude();
            {

            };
            Preludes.Add(corp);
            SelectedObject = corp;
        }

        private void ExecuteRefresh(object obj)
        {
            if (obj == null)
            {
                Corporations = new ObservableCollection<Corporation>(_corporations);
                Patents = new ObservableCollection<Patent>(_patents);
                Preludes = new ObservableCollection<Prelude>(_preludes);


            }
            else if (SelectedObject != null)
            {
                if (SelectedObject is Patent)
                {
                    OnPropertyChanged(nameof(Patents));
                }
                if (SelectedObject is Corporation)
                {
                    OnPropertyChanged(nameof(Corporations));
                }
                if (SelectedObject is Prelude)
                {
                    OnPropertyChanged(nameof(Preludes));
                }
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