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
    public class MainViewModel : NotifierBase
    {
        private const string packs = "packs.json";

        private ObservableCollection<PackPresenter> _packs;
        private PackPresenter _destinationPack;
        private string _packageLocation;
        private EditorConfig _config;
        private string _savePackageLocation;

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

        private void SaveConfig()
        {
            var json = System.Text.Json.JsonSerializer.Serialize(_config);
            using (var sw = new StreamWriter("editorConfig.json", false))
            {
                sw.Write(json);
            }
        }

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

            BoardViewModel = new BoardTesterViewModel();
        }

        public BoardTesterViewModel BoardViewModel { get; set; }

        private void ExecuteSelectBoardPlace(object obj)
        {

            BoardViewModel.SelectPlace(obj);
        }

        private async void ExecuteLoadData(object obj)
        {

            CardReferencesHolder all;
            string directory = _config.PackageLocation;

            all = await PackSerializer.GtPacks(directory);
            Packs = new ObservableCollection<PackPresenter>(all.Packs.Select(r => new PackPresenter()
            {
                Content = new PackViewModel(r),
                Name = r.Name.ToString(),
            }));
        }

        public RelayCommand LoadDataCommand { get; set; }

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
                    _config = new EditorConfig() { PackageLocation = Environment.CurrentDirectory };
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

        private void ExecuteSave(object obj)
        {
            if (string.IsNullOrEmpty(SavePackageLocation))
            {
                MessageBox.Show("Invalid save location");
                return;
            }
            try
            {
                using (var streamWriter = new StreamWriter($"{SavePackageLocation}\\{DateTime.Now.Ticks}_{packs}", false))
                {
                    var h = new CardReferencesHolder()
                    {
                        Packs = Packs.Select(r => new ExtensionPack()
                        {
                            Name = (Extensions)Enum.Parse(typeof(Extensions), r.Name),
                            Patents = r.Content.Patents.ToList(),
                            Corporations = r.Content.Corporations.ToList(),
                            Preludes = r.Content.Preludes.ToList(),
                        }).ToList()
                    };

                    streamWriter.Write(System.Text.Json.JsonSerializer.Serialize(h));
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString(), "Save Error",MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }

        public RelayCommand SelectBoardPlace { get; set; }
    }
}