using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TM.Digital.Editor.Board;
using TM.Digital.Model;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Corporations;
using TM.Digital.Services.Common;
using TM.Digital.Ui.Resources.ViewModelCore;

namespace TM.Digital.Editor
{
    public class PackManagerViewModelBase: NotifierBase
    {
        protected const string packs = "packs.json";
        protected void SaveConfig()
        {
            var json = System.Text.Json.JsonSerializer.Serialize(_config);
            using (var sw = new StreamWriter("editorConfig.json", false))
            {
                sw.Write(json);
            }
        }
        protected  void Save(CardReferencesHolder h)
        {
            if (string.IsNullOrEmpty(SavePackageLocation))
            {
                MessageBox.Show("Invalid save location");
                return;
            }
            try
            {
                string path = $"{SavePackageLocation}\\{DateTime.Now.Ticks}_{packs}";
                using (var streamWriter = new StreamWriter(path, false))
                {


                    streamWriter.Write(System.Text.Json.JsonSerializer.Serialize(h));
                }
                MessageBox.Show($"File saved : {path}", "Save successful", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString(), "Save Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        protected async Task<CardReferencesHolder> Load()
        {

            string directory = _config.PackageLocation;

           var all = await PackSerializer.GetPacks(directory);
           return all;
        }
        public string SavePackageLocation
        {
            get => _savePackageLocation;
            set
            {
                _savePackageLocation = value;
                _config.SavePackageLocation = value;
                OnPropertyChanged();
                SaveConfig();
            }
        }
        public async Task Initialize()
        {
            try
            {
                if (File.Exists("editorConfig.json"))
                {
                    _config = System.Text.Json.JsonSerializer.Deserialize<EditorConfig>(
                        await File.ReadAllTextAsync("editorConfig.json"));
                }
                else
                {
                    _config = new EditorConfig { PackageLocation = Environment.CurrentDirectory };
                    SaveConfig();
                }

                _packageLocation = _config.PackageLocation;
                _savePackageLocation = _config.SavePackageLocation;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Load Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public string PackageLocation
        {
            get => _packageLocation;
            set
            {
                _packageLocation = value;
                _config.PackageLocation = value;
                OnPropertyChanged();
                SaveConfig();
            }
        }
        protected string _packageLocation;
        protected EditorConfig _config;
        protected string _savePackageLocation;
    }
    public class EditorConfig
    {
        public string PackageLocation { get; set; }
        public string SavePackageLocation { get; set; }
    }

    public class MainViewModel : PackManagerViewModelBase
    {
        

        private ObservableCollection<PackPresenter> _packs;
        private PackPresenter _destinationPack;
        private BoardViewModel _boardViewModel;




        public ObservableCollection<PackPresenter> Packs
        {
            get => _packs;
            set
            {
                _packs = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand MoveToCommand { get; set; }

        public PackPresenter DestinationPack
        {
            get => _destinationPack;
            set { _destinationPack = value; OnPropertyChanged(); }
        }

        public RelayCommand SaveCommand { get; set; }



        //SelectedObject = all;

        private void SaveObject_OnClick(object sender, RoutedEventArgs e)
        {
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

        public MainViewModel()
        {
            MoveToCommand = new RelayCommand(ExecuteMoveTo);

            SaveCommand = new RelayCommand(ExecuteSave);
            LoadDataCommand = new RelayCommand(ExecuteLoadData);

            SelectBoardPlace = new RelayCommand(ExecuteSelectBoardPlace);

            
        }

        public BoardViewModel BoardViewModel
        {
            get => _boardViewModel;
            set { _boardViewModel = value;OnPropertyChanged(); }
        }

        private void ExecuteSelectBoardPlace(object obj)
        {

            //BoardViewModel.SelectPlace(obj);
        }

        private async void ExecuteLoadData(object obj)
        {

            
            CardReferencesHolder all= await Load(); 

            Packs = new ObservableCollection<PackPresenter>(all.Packs.Select(r => new PackPresenter
            {
                Content = new PackViewModel(r),
                Name = r.Name.ToString(),
            }));
        }

        public RelayCommand LoadDataCommand { get; set; }



        private void ExecuteSave(object obj)
        {
            var h = new CardReferencesHolder
            {
                Packs = Packs.Select(r => new ExtensionPack
                {
                    Name = (Extensions)Enum.Parse(typeof(Extensions), r.Name),
                    Patents = r.Content.Patents.ToList(),
                    Corporations = r.Content.Corporations.ToList(),
                    Preludes = r.Content.Preludes.ToList(),
                }).ToList()
            };
            Save(h);
        }

        public RelayCommand SelectBoardPlace { get; set; }
    }
}